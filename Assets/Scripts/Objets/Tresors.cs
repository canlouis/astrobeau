using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Classe générant un niveau composé de salles disposées en grille avec des bordures.
/// Auteurs du code: Louis Cantin et Olivier Dion
/// Auteurs des commentaires: Louis Cantin et Olivier Dion
/// </summary>

public class Tresors : MonoBehaviour
{
    [SerializeField] Retroaction _retroModele;
    [SerializeField] SOJoyaux _donneesJoyaux; // #synthese Olivier Référence à un scriptable object contenant les données des joyaux.
    [SerializeField] AudioClip _sonTresor; // #synthese Olivier Son joué lorsqu'un trésor est ramassé.

    // #tp4 Louis
    [SerializeField] int _valeurArgent; // valeur des tresors en argent
    [SerializeField] int _valeurScore = 30; // valeur des tresors en points
    private Rigidbody2D _rb; // #synthese Olivier rigidbody des tresors
    float _vitessseRotMax = 1.5f;
    float _vitessseRotMin = .5f;
    float _vitesseRotRandom;
    int _directionRotation = 0;
    bool _bonusArgentEstActif = false;
    bool _estDansZoneEffector = false;
    private CircleCollider2D _cc; // #synthese Olivier collider des tresors

    // #tp4 Louis
    void Start()
    {
        _donneesJoyaux.joyauxTombe.AddListener(FaireTomber); // #synthese Olivier Quand le joyaux tombe, le tresor est détruit
        Perso.instance.donneesPerso.bonusArgentDebut.AddListener(ActiverBonusArgent); // Ajoute la méthode MettreAJourInfos à l'événement de mise à jour des données du personnage.
        Perso.instance.donneesPerso.bonusArgentFin.AddListener(DesactiverBonusArgent); // Ajoute la méthode MettreAJourInfos à l'événement de mise à jour des données du personnage.
        _vitesseRotRandom = Random.Range(_vitessseRotMin, _vitessseRotMax); // vitesse de rotation des tresors
        _directionRotation = Random.Range(0, 2); // direction de rotation des tresors
        if (_directionRotation == 0) _vitesseRotRandom = -_vitesseRotRandom; // si la direction de rotation est 0, la rotation est negative
        _rb = GetComponent<Rigidbody2D>(); //#synthese Olivier récupère le rigidbody des tresors
        _cc = GetComponent<CircleCollider2D>(); //#synthese Olivier récupère le collider des tresors
    }

    private void FaireTomber()
    {
        if (_estDansZoneEffector)
        {
            _rb.gravityScale = 0.1f; // #synthese Olivier active la gravité des tresors
        }
        else { return; } // #synthese Olivier si les tresors ne sont pas dans la zone de collision, ils ne tombent pas
    }

    // #tp4 Louis
    void FixedUpdate()
    {

        transform.Rotate(0, 0, _vitesseRotRandom); // fait tourner les tresors
    }

    void OnTriggerEnter2D(Collider2D other) // quand il entre en collision avec les autres objets
    {
        if (other.gameObject.tag == "Perso") // entre en collision avec les joueurs
        {
            // #tp4 Louis
            if (_bonusArgentEstActif) _valeurArgent *= 2; // si le bonus d'argent est actif, la valeur des tresors est doublee
            Perso.instance.AjouterArgent(_valeurArgent);  // ajoute la valeur des tresors a l'argent du personnage
            Perso.instance.AjouterPts(_valeurScore); // ajoute la valeur des tresors au score du personnage
            GestAudio.instance.JouerSon(_sonTresor); // joue le son des tresors
            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent); // instancie le gameobject retroaction
            retro.ChangerTexte(_valeurArgent + ""); // affiche le texte
            Destroy(gameObject); // detruit le gameobject des tresors.
        }
        if (other.gameObject.tag == "EffectorJoyaux") //synthese Olivier entre en collision avec l'effector des joyaux
        {
            _estDansZoneEffector = true; //#synthese olivier les tresors sont dans la zone de collision
            _cc.isTrigger = false; // #synthese Olivier active le collider des tresors
        }
    }


    void OnCollisionEnter2D(Collision2D other) // quand il entre en collision avec les autres objets
    {
        if (other.gameObject.tag == "Perso") //synthese Olivier entre en collision avec l'effector des joyaux
        {
            if (_bonusArgentEstActif) _valeurArgent *= 2; // si le bonus d'argent est actif, la valeur des tresors est doublee
            Perso.instance.AjouterArgent(_valeurArgent);  // ajoute la valeur des tresors a l'argent du personnage
            Perso.instance.AjouterPts(_valeurScore); // ajoute la valeur des tresors au score du personnage
            GestAudio.instance.JouerSon(_sonTresor); // joue le son des tresors
            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent); // instancie le gameobject retroaction
            retro.ChangerTexte(_valeurArgent + ""); // affiche le texte
            Destroy(gameObject); // detruit le gameobject des tresors.
        }
    }

    // #tp4 Louis
    void ActiverBonusArgent()
    {
        _bonusArgentEstActif = true; // active le bonus d'argent
    }
    void DesactiverBonusArgent()
    {
        _bonusArgentEstActif = false; // Désactive le bonus d'argent
    }
}
