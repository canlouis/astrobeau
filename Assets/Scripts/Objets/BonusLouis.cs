using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Classe qui contrôle mon bonus
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>
public class BonusLouis : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particules; // Référence à un système de particules pour l'effet visuel.
    [SerializeField] SOPerso _donneesPerso; // #Synthese Olivier Référence à un scriptable object contenant les données du personnage.
    [SerializeField] Light2D _lumiere; // #Synthese Louis Référence à une lumière pour l'effet visuel.

    //# tp4 Louis
    [SerializeField] Retroaction _retroModele; // Référence à un modèle de rétroaction pour l'affichage de texte.
    [SerializeField] SOScore _score;
    [SerializeField] int _tempsBonus = 10; // temps du bonus d'argent
    private Animator _animator; //#synthese Olivier Récupere le composant animator

    float _couleur = 0; // couleur des particules
    float _ratioCouleur = .1f;
    float _vitessseRotMax = 1.5f;
    float _vitessseRotMin = .5f;
    float _vitesseRotRandom;
    int _directionRotation = 0;
    bool _estActive = false; // #synthese Louis Si le bonus est actif

    // #tp4 Louis
    void Start()
    {
        _animator = GetComponent<Animator>(); // synthese Olivier Récupération du composant Animator.
        _lumiere.enabled = false; // #Synthese Louis Désactive la lumière du bonus
        _vitesseRotRandom = Random.Range(_vitessseRotMin, _vitessseRotMax); // vitesse de rotation du bonus
        _directionRotation = Random.Range(0, 2);
        if (_directionRotation == 0) _vitesseRotRandom = -_vitesseRotRandom; // direction de rotation du bonus
        _donneesPerso.activerBonus.AddListener(ActiverArgent); // #synthese Olivier Quand le bonus est activer, il peut être récupéré
        _donneesPerso.desactiverBonus.AddListener(DesactiverArgent); // #TP4 Olivier Quand le bonus est activer, il peut être récupéré
        GetComponent<Collider2D>().enabled = false; // #synthese Olivier Désactive le collider du bonus

    }

    // #tp4 Louis
    void FixedUpdate()
    {
        // #synthese Louis Si le bonus est actif
        if (_estActive) transform.Rotate(0, 0, _vitesseRotRandom); // fait tourner le bonus
    }


    void ActiverArgent()
    {
        _animator.SetTrigger("BonusActif"); // #TP4 olivier active l'animation quand le bonus est disponnible
        _lumiere.enabled = true; // #Synthese Louis Active la lumière du bonus
        _estActive = true; // #synthese Louis Si le bonus est actif
        GetComponent<Collider2D>().enabled = true;
    }
    void DesactiverArgent()
    {
        _estActive = false; // #synthese Louis Si le bonus est actif
        _lumiere.enabled = false; // #Synthese Louis Désactive la lumière du bonus
        _animator.SetTrigger("BonusInactif"); // #synthese olivier active l'animation quand le bonus est disponnible
        GetComponent<Collider2D>().enabled = false; // #synthese Olivier Désactive le collider du bonus
    }

    /// <summary>
    /// Méthode appelée lorsqu'un autre collider entre en collision avec le collider de cet objet.
    /// </summary>
    /// <param name="other">Le collider de l'autre objet avec lequel cet objet est entré en collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si l'objet en collision est le joueur.
        if (other.CompareTag("Perso"))
        {
            // #tp4 Louis
            SOPerso _donneesPerso = other.GetComponent<Perso>().donneesPerso; // recupere les donnees du personnage
            _donneesPerso.bonusArgentDebut.Invoke(); // déclenche l'événement de début de bonus d'argent
            StartCoroutine(CoroutineBonusArgent(_donneesPerso, other.GetComponent<Perso>()));

            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent); // instancie le gameobject retroaction
            retro.ChangerTexte("Argent X 2"); // affiche le texte

            _score.nbBonusRecoltes++; // Incrémente le nombre de bonus récoltés par le joueur.

            // Instantie un effet de particules à la position du bonus.
            Instantiate(_particules, transform.position, Quaternion.identity);

            this.GetComponent<SpriteRenderer>().enabled = false; // Désactive le sprite du bonus.
            this.GetComponent<Collider2D>().enabled = false; // Désactive le sprite du bonus.
        }
    }

    // #tp4 Louis
    IEnumerator CoroutineBonusArgent(SOPerso _donneesPerso, Perso perso)
    {
        _ratioCouleur = 1f / _tempsBonus;
        for (int i = 0; i < _tempsBonus; i++)
        {
            _couleur += _ratioCouleur;
            // Appelle la méthode du joueur pour changer la couleur des particules.
            perso.ChangerCouleurParticules(_couleur);
            yield return new WaitForSeconds(1);
        }
        _donneesPerso.bonusArgentFin.Invoke(); // déclenche l'événement de début de bonus d'argent
        // Détruit ce bonus après qu'il a été collecté.
        Destroy(gameObject);
    }
}
