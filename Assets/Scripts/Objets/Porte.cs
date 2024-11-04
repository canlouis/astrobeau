using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Classe contrôlant le comportement d'une porte.
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>
public class Porte : MonoBehaviour
{
    SpriteRenderer _sr; // Référence au composant SpriteRenderer attaché à cet objet.
    [SerializeField] SONavigation _maNavigation; // Référence au script de navigation vers la prochaine scène.
    [SerializeField] ParticleSystem _part; // Référence à un système de particules pour l'effet visuel.
    [SerializeField] Light2D _lumiere; // #synthese Louis Référence à une lumière pour l'effet visuel.
    [SerializeField] AudioClip _sonPorte; // #TP4 Olivier Référence à un son pour la porte.
Animator _anim; // #syntheseLouis Référence à l'Animator attaché à cet objet.

    void Start()
    {
        // #synthese Louis
        _lumiere.enabled = false; // Désactive la lumière du bonus
        _anim = GetComponent<Animator>(); // Récupération du composant Animator attaché à cet objet.
        //

        // #tp4 Louis
        Perso.instance.donneesPerso.ouvrirPorte.AddListener(OuvrirPorte); // Ajoute la méthode MettreAJourInfos à l'événement de mise à jour des données du personnage.
        _sr = GetComponent<SpriteRenderer>(); // Récupération du composant SpriteRenderer attaché à cet objet.
        _sr.color = new Color(.3f, .3f, .3f); // Définition de la couleur grise pour la porte.
    }

    /// <summary>
    /// Méthode appelée lorsqu'un autre collider entre en collision avec cet objet.
    /// </summary>
    /// <param name="other">Collider entrant en collision avec cet objet</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Perso")) // Vérifie si l'objet entrant en collision est de type "Perso".
        {
            if (Perso.instance.aCle == true) // Vérifie si le personnage possède la clé nécessaire.
            {
                GestAudio.instance.JouerSon(_sonPorte); // #TP4 Olivier Joue le son de la porte.
                _maNavigation.AllerSceneSuivante(); // Appelle la méthode pour passer à la prochaine scène.
                Perso.instance.donneesPerso.ViderInventaire(); // Efface les objets collectés pour passer au niveau suivant.
            }
        }
    }

    // #tp4 Louis
    void OuvrirPorte()
    {
        _lumiere.enabled = true; // #synthese Louis Active la lumière du bonus
        _anim.SetBool("estOuverte", true); // Déclenche l'animation d'ouverture de la porte.
        _sr.color = Color.white; // Change la couleur de la porte pour indiquer qu'elle est déverrouillée.
        // Instancie le système de particules au même emplacement que la porte.
        Instantiate(_part, transform.position, Quaternion.identity, transform);
    }
}
