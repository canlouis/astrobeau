using UnityEngine;

/// <summary>
/// Classe qui contrôle la clé.
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>
public class Cle : MonoBehaviour
{
    [SerializeField] ParticleSystem _part; // Référence à un système de particules pour l'effet visuel.
    [SerializeField] AudioClip _sonCle; // #TP4 Olivier Référence à un son pour la clé.
    // #tp4 Louis
    [SerializeField] Retroaction _retroModele;
    float _vitessseRotMax = 1.5f;
    float _vitessseRotMin = .5f;
    float _vitesseRotRandom;
    int _directionRotation = 0;

    void Start()
    {
        // Instancie le système de particules au même emplacement que la clé.
        Instantiate(_part, transform.position, Quaternion.identity, transform.parent);
        _vitesseRotRandom = Random.Range(_vitessseRotMin, _vitessseRotMax); // vitesse de rotation de la clé
        _directionRotation = Random.Range(0, 2);
        if (_directionRotation == 0) _vitesseRotRandom = -_vitesseRotRandom; // direction de rotation de la clé
    }

    // #tp4 Louis
    void FixedUpdate()
    {
        transform.Rotate(0, 0, _vitesseRotRandom); // fait tourner la clé
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Perso"))
        {
            // #tp4 Louis
            Perso perso = other.GetComponent<Perso>(); // Récupère le script Perso du personnage.
            perso.donneesPerso.ouvrirPorte.Invoke(); // Déclenche l'événement d'ouverture de porte.
            perso.donneesPerso.permettreTirer.Invoke(); // #synthese Louis Déclenche l'événement d'ouverture de porte.
            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent); // instancie le gameobject retroaction
            retro.ChangerTexte("Portail ouvert"); // affiche le texte

            // #synthese Louis
            perso.ChangerAnim();
            FlecheCle.instance.Detruire(); // Détruit la flèche de la clé.
            //

            GestAudio.instance.JouerSon(_sonCle); // #TP4 Olivier Joue le son de la clé.
            Destroy(gameObject); // Détruit la clé après qu'elle a été ramassée
        }
    }
}

