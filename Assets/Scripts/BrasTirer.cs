using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrasTirer : MonoBehaviour
{
    [SerializeField] Projectile _projectile;
    [SerializeField] Transform _bout;
    SpriteRenderer _sr;
    bool _peutTirer = false;
    float _delaiTirer = .2f;
    // public bool peutTirer { get => _peutTirer; set => _peutTirer = value; }

    // Start is called before the first frame update
    void Start()
    {
        Perso.instance.donneesPerso.permettreTirer.AddListener(PermettreTirer);
        _sr = GetComponent<SpriteRenderer>();
        _sr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_peutTirer && _sr.enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Tirer();
                _peutTirer = !_peutTirer;
                StartCoroutine(CoroutineDelaiTirer());
            }
        }

        Vector2 posSourie = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 scale = transform.localScale;
        if (Perso.instance.transform.localScale.x < 0)
        {
            if (scale.x < 0) transform.localScale = new Vector3(-scale.x, -scale.y, scale.z);
            else transform.localScale = scale;
        }
        else if (Perso.instance.transform.localScale.x > 0)
        {
            if (scale.x < 0) transform.localScale = scale;
            else transform.localScale = new Vector3(-scale.x, -scale.y, scale.z);
        }

        float angle = TrouverAngle(transform.position, posSourie);
        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
    }

    private float TrouverAngle(Vector2 v1, Vector2 v2)
    {
        Vector2 v = v2 - v1;
        return Mathf.Atan2(v.y, v.x);
    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        if (_peutTirer)
        {
            Tirer();
            _peutTirer = !_peutTirer;
            StartCoroutine(CoroutineDelaiTirer());
        }
    }

    private void Tirer()
    {
        Projectile instance = Instantiate(_projectile, _bout.position, transform.rotation);
        instance.transform.right = transform.right;
    }

    void PermettreTirer()
    {
        _peutTirer = !_peutTirer;
    }

    IEnumerator CoroutineDelaiTirer()
    {
        yield return new WaitForSeconds(_delaiTirer);
        _peutTirer = !_peutTirer;
    }
}
