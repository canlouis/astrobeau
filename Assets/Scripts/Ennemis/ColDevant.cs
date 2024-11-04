using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColDevant : MonoBehaviour
{
    [SerializeField] EnnemiLouis _ennemi;
    BoxCollider2D _col;
    float _offset;
    float _compteurColMur;
    float _compteurColEnnemi;
    // Start is called before the first frame update
    void Start()
    {
        _col = GetComponent<BoxCollider2D>();
        if (_ennemi.axeHorizontal < 0) _offset = _col.offset.x * -1;
        else if (_ennemi.axeHorizontal > 0) _offset = _col.offset.x;
        _col.offset = new Vector2(_offset, _col.offset.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TuileSol"))
        {
            if (_compteurColEnnemi > 0)
            {
                _compteurColEnnemi = 0;
                _compteurColMur = 0;
            }

            if (_compteurColMur > 1)
            {
                _ennemi.Sauter();
            }

            _ennemi.axeHorizontal *= -1;
            _offset *= -1;
            _col.offset = new Vector2(_offset, _col.offset.y);
            _compteurColMur++;
        }

        if (other.CompareTag("Ennemis"))
        {
            if (_compteurColMur > 0)
            {
                _compteurColEnnemi = 0;
                _compteurColMur = 0;
            }

            if (_compteurColEnnemi > 1)
            {
                _ennemi.Sauter();
            }

            _ennemi.axeHorizontal *= -1;
            _offset *= -1;
            _col.offset = new Vector2(_offset, _col.offset.y);
            _compteurColEnnemi++;
        }
    }
}
