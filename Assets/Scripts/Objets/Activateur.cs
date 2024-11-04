using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;


// #TP3 Olivier
public class Activateur : MonoBehaviour
{
    [SerializeField] Retroaction _retroModele; // #TP4 Olivier Référence à un modèle de rétroaction pour l'affichage de texte.
    [SerializeField] Light2D _lumiere; // #synthese Louis Référence à une lumière pour l'effet visuel.

    private Animator _animator; // #TP4 olivierrecupère l'animateur de l'activateur
    private BoxCollider2D _bc; // #TP4 Olivier Composant BoxCollider2D de l'activateur.

    void Awake()
    {
        _lumiere.enabled = true; // #Synthese Louis Active la lumière du bonus
        _bc = GetComponent<BoxCollider2D>(); // #TP4 Olivier Récupération du composant BoxCollider2D.
        _animator = GetComponent<Animator>(); // Initialize _animator here
        _bc.isTrigger = true; // #TP3 Olivier l'activateur peut collisioné avec autre object
    }

    void OnTriggerEnter2D(Collider2D other) // #TP3 Olivier quand il entre en collision evec un autre object
    {
        if (other.gameObject.tag == "Perso")   // #TP3 Olivier si il est en collision avec le joueur, faire ceci...
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z); // #synthese Olivier déplace l'activateur
            Perso perso = other.GetComponent<Perso>(); // #TP4 olivier récupère le script Perso du personnage.
            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent); // instancie le gameobject retroaction
            retro.ChangerTexte("BonusActivé"); // affiche le texte
            transform.position = new Vector3(transform.position.x, transform.position.y -1, transform.position.z); // #synthese Olivier déplace l'activateur
            _lumiere.enabled = false; // #Synthese Louis Désactive la lumière du bonus
            StartCoroutine(CoroutineActiverBonus(perso)); // #TP4 olivier lance la coroutine pour activer le bonus
        }
    }


    IEnumerator CoroutineActiverBonus(Perso perso)
    {
        _animator.SetTrigger("Actif"); // #synthese olivier active l'animation de l'activateur actif
        perso.donneesPerso.activerBonus.Invoke(); // #synthese olivier déclenche l'événement d'activation de bonus.
        GetComponent<Collider2D>().enabled = false; // #synthese Olivier Désactive le collider de l'activateur.

        yield return new WaitForSeconds(10); // #Synthese Olivier attend 10 secondes

        _animator.SetTrigger("Inactif"); // #synthese Olivier active l'animation de l'activateur inactif
        perso.donneesPerso.desactiverBonus.Invoke(); // #synthese olivier déclenche l'événement d'activation de bonus.
        GetComponent<Collider2D>().enabled = true; // #synthese Olivier Désactive le collider de l'activateur.
    }
}