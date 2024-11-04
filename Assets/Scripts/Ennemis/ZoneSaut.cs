using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSaut : MonoBehaviour
{
    [SerializeField] float _rayonZoneSaut;
    [SerializeField] EnnemiLouis _ennemiLouis;
    CircleCollider2D _col;

    void Awake()
    {
        _col = gameObject.GetComponent<CircleCollider2D>();
        _col.radius = _rayonZoneSaut;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Perso")){
            _ennemiLouis.Sauter();
        }
    }
}
