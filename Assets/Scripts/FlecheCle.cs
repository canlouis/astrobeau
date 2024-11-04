using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlecheCle : MonoBehaviour
{
    Vector2 posCle = new Vector2(0, 0);
    Vector3 posPerso = new Vector3(0, 0, 0);
    Perso _perso;
    static FlecheCle _instance;
    float _decalageBase = 1.5f;
    float _decalage = 1.5f;
    public static FlecheCle instance { get => _instance; set => _instance = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Cle cle = FindObjectOfType<Cle>();
        posCle = cle.transform.position;
        _perso = FindObjectOfType<Perso>();
        _perso.donneesPerso.changementGravite.AddListener(InverserDecalage);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        posPerso = _perso.transform.position;
        transform.position = new(posPerso.x, posPerso.y + _decalage, posPerso.z);
        float angle = TrouverAngle(transform.position, posCle);
        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
    }

    private float TrouverAngle(Vector2 v1, Vector2 v2)
    {
        Vector2 v = v2 - v1;
        return Mathf.Atan2(v.y, v.x);
    }

    public void Detruire()
    {
        Destroy(gameObject);
    }

    void InverserDecalage()
    {
        _decalage *= -1;
    }
}
