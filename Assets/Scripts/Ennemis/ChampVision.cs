using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampVision : MonoBehaviour
{
    [SerializeField] EnnemiLouis _ennemiLouis;
    [SerializeField] float _rayonChampVision;
    CircleCollider2D _col;
    bool _peutRegarder = true;
    float _tempDeReaction = .5f;
    void Awake()
    {
        _col = gameObject.GetComponent<CircleCollider2D>();
        _col.radius = _rayonChampVision;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Perso")){
            if (_peutRegarder)
            {
                _ennemiLouis.RegarderJoueur(other.transform.position.x);
                _peutRegarder = false;
                StartCoroutine(CoroutineRegarder());
            }
        }
    }

    IEnumerator CoroutineRegarder()
    {
        yield return new WaitForSeconds(_tempDeReaction);
        _peutRegarder = true;
    }
}
