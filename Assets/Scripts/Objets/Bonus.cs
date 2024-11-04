using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// #TP3 Olivier
public class Boost : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; // #TP4 Olivier Référence à un scriptable object contenant les données du personnage.
    [SerializeField] Retroaction _retroModele; // #TP4 Olivier Référence à un modèle de rétroaction pour l'affichage de texte.
    [SerializeField] Light2D _lumiere; // #synthese Louis Référence à une lumière pour l'effet visuel.


    private Animator _animator; //#TP4 Olivier Récupere le composant animator
    private CircleCollider2D _cc; // #TP4 OlivierCircle collider du bonus


    float _vitessseRotMax = 1.5f;
    float _vitessseRotMin = .5f;
    float _vitesseRotRandom;
    int _directionRotation = 0;
    bool _estActive = false; // #synthese Louis Si le bonus est actif

    void Awake()
    {
        _animator = GetComponent<Animator>(); // #TP4 Olivier Récupération du composant Animator.
        _cc = GetComponent<CircleCollider2D>(); // #TP3 Olivier Récupération du composant SpriteRenderer.
    }
    void Start()
    {
        _lumiere.enabled = false; // #Synthese Louis Désactive la lumière du bonus
        _vitesseRotRandom = Random.Range(_vitessseRotMin, _vitessseRotMax); // vitesse de rotation du bonus
        _directionRotation = Random.Range(0, 2); // direction de rotation du bonus
        if (_directionRotation == 0) _vitesseRotRandom = -_vitesseRotRandom; // direction de rotation du bonus
        _donneesPerso.activerBonus.AddListener(ActiverBoost); // #TP4 Olivier Quand le bonus est activer, il peut être récupéré
        _donneesPerso.desactiverBonus.AddListener(DesactiverBoost); // #TP4 Olivier Quand le bonus est activer, il peut être récupéré
        GetComponent<Collider2D>().enabled = false; // #synthese Olivier Désactive le collider du bonus
        _cc.isTrigger = true; // #TP4 Olivier les bonus ne peuvent pas collisioné avec autre object
    }


    void FixedUpdate()
    {
        if (_estActive) // #synthese Louis Si le bonus est actif
        {
            if (_cc.isTrigger == false) return; // #TP4 Olivier Si l'activateur n'as pas été activé, les bonus ne son pas être récupéré
            transform.Rotate(0, 0, _vitesseRotRandom); // fait tourner le bonus
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Perso")) // #TP3 Oliviersi il collisionne avec le joueur 
        {
            Perso perso = other.GetComponent<Perso>(); // #TP3 Olivier Récupère le composant Perso de l'objet entrant
            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent); // instancie le gameobject retroaction
            retro.ChangerTexte("Boost"); // affiche le texte
            perso.DebutBoost(); // #synthese Olivier Lance la coroutine du compteur
            Destroy(gameObject); // #Synthese Olivier détruit le bonus
        }
    }

    void ActiverBoost()
    {
        _animator.SetTrigger("BonusActif"); // #synthese olivier active l'animation quand le bonus est disponnible
        _lumiere.enabled = true; // #synthese Louis Active la lumière du bonus
        _estActive = true; // #synthese Louis Si le bonus est actif
        GetComponent<Collider2D>().enabled = true; // #synthese Olivier Désactive le collider du bonus
    }

    void DesactiverBoost()
    {
        _animator.SetTrigger("BonusInactif"); // #synthese olivier active l'animation quand le bonus est disponnible
        _lumiere.enabled = false; // #Synthese Louis Désactive la lumière du bonus
        _estActive = false; // #synthese Louis Si le bonus est actif
        GetComponent<Collider2D>().enabled = false; // #synthese Olivier Désactive le collider du bonus
    }
}
