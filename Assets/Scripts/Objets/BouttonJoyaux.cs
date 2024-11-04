using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class BouttonJoyaux : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _bc; // #synthese Olivier Référence à un modèle de rétroaction pour l'affichage de texte.
    [SerializeField] private SOJoyaux _donneesJoyaux; // #synthese Olivier Référence à un modèle de rétroaction pour l'affichage de texte.
    [SerializeField] Light2D _lumiere; // #synthese Olivier Référence à une lumière pour l'effet visuel.
    [SerializeField] Retroaction _retroModele; // #synhtese Olivier Référence à un modèle de rétroaction pour l'affichage de texte.


    private Animator _animator; // #synthese Olivier Référence à l'animator de l'activateur.
    void Start()
    {
        _lumiere.enabled = true; // #synthese Olivier Active la lumière du joyaux.
        _bc = GetComponent<BoxCollider2D>(); // #synthese Olivier Récupération du composant BoxCollider2D.
        _animator = GetComponent<Animator>(); // #synthese Olivier Récupération du composant Animator.
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Perso")) // #synthese Olivier si il collisionne avec le joueur 
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z); // #synthese Olivier déplace l'activateur
            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent); // instancie le gameobject retroaction
            retro.ChangerTexte("Joyaux Tombent"); // affiche le texte
            transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z); // #synthese Olivier déplace l'activateur
            _bc.enabled = false; // #synthese Olivier Désactive le collider du bouton joyaux.
            _donneesJoyaux.joyauxTombe.Invoke(); // #synthese Olivier déclenche l'événement de mise à jour des données.
            //animation
            _animator.SetTrigger("Actif"); // #synthese Olivier Déclenche l'animation de chute.
            _lumiere.enabled = false; // #synthese Olivier Désactive la lumière du joyaux.
        }
    }
}
