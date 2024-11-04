using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nid : EtresVivants
{
    [SerializeField] EnnemiLouis _ennemi;
    [SerializeField] GameObject _sortie;
    [SerializeField] ParticleSystem _part;
    [SerializeField] ParticleSystem _partMort;
    [SerializeField] Retroaction _retroModele;
    [SerializeField] int _nbEnnemisMax = 5;
    [SerializeField]int _tempsApparitionEnnemis = 3;
    [SerializeField] int points = 200;
    Transform _conteneurEnnemisNid;
    float _forcePropulsion = 5;
    int _nbEnnemis = 0;

    // Start is called before the first frame update
    void Start()
    {
        _conteneurEnnemisNid = new GameObject("ParticulesDeplacement").transform;
        StartCoroutine(CoroutineApparitonEnnemi());
    }

    IEnumerator CoroutineApparitonEnnemi()
    {
        while (true)
        {
            _nbEnnemis = _conteneurEnnemisNid.transform.childCount;
            if (_nbEnnemis == _nbEnnemisMax)
            {
                yield return new WaitForSeconds(1);
                continue;
            }
            else
            {
                yield return new WaitForSeconds(_tempsApparitionEnnemis);
                Instantiate(_part, _sortie.transform.position, Quaternion.identity);
                EnnemiLouis instance = Instantiate(_ennemi, _sortie.transform.position, Quaternion.identity, _conteneurEnnemisNid.transform);
                Vector2 directionPropulsion = (_sortie.transform.position - transform.position).normalized;
                instance.GetComponent<Rigidbody2D>().AddForce( directionPropulsion * _forcePropulsion, ForceMode2D.Impulse);
            }
        }
    }

    public void Mourir()
    {
        Perso.instance.AjouterPts(points);
        Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent); // instancie le gameobject retroaction
        retro.ChangerTexte($"+ {points}pts"); // affiche le texte
        Instantiate(_partMort, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
