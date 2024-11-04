using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// #TP3 Olivier
public class Aimant : MonoBehaviour
{
    [SerializeField] SOObjet _donnees; //#Tp synthese Olivier Données de l'objet représenté par le panneau.
    public SOObjet donnees => _donnees; //#Tp synthese Olivier Propriété permettant d'accéder aux données de l'objet.
    private SpriteRenderer _sr; // #TP3 Olivier Composant SpriteRenderer du personnage.
    [SerializeField] SOPerso _donneesPerso; // Données du personnage.

    private PointEffector2D _pe; // #TP3 Olivier Composant SpriteRenderer du personnage.
    bool _estDisponible = true; // #TP3 Olivier Indique si l'aimant est disponible pour être 

    public SOPerso donneesPerso { get => _donneesPerso; set => _donneesPerso = value; }

    void Start()
    {
        _pe = GetComponent<PointEffector2D>(); // #TP3 Olivier Récupération du composant PointEffector.
        _sr = GetComponent<SpriteRenderer>(); // #TP3 Olivier Récupération du composant SpriteRenderer.
        _pe.enabled = false;    // #TP3 Olivier le PointEffector n'est pas afficher quand le jeu start.
        _sr.color = new Color(1, 1, 1, 0);  // #TP3 Olivier la couleur du sprite renderer est blanc et l'opacité a 0%.
    }
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (_donneesPerso.aAimant && _estDisponible) // #TP3 Olivier Si l'aimant est acheté
        {
            ActiverAimant(); // #TP3 Olivier Active l'aimant
        }
    }


    private void ActiverAimant()
    {
        if (Input.GetKey(KeyCode.E))
        {
            StartCoroutine(CoroutineActiver()); // #TP3 Olivier Start la coroutine pour activé l'aimant
        }
    }

    /// <summary>
    /// Coroutine qui active l'aimant.
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineActiver()
    {
        Debug.Log("AIMANT ACTIVÉ");
        _pe.enabled = true; // #TP3 Olivier Composant pointEffector du personnage est activé.
        _sr.color = new Color(1, 1, 1, 0.4f); // #TP3 Olivier La couleur est blanc et est de 0.4 d'opacité.
        _estDisponible = false; // #TP3 Olivierl'aimant est indisponible.

        StartCoroutine(CoroutineCompteur(true)); // #TP3 Olivier start la coroutine pour commencer le compteur de 5 secondes pour déterminer quand l'aimant doit être désactivé.
        yield return new WaitForSeconds(5f); // #TP3 Olivier attend 5 secondes avant de faire le reste

        Debug.Log("AIMANT DÉSACTIVÉ");
        _pe.enabled = false;    // #TP3 Olivier Composant pointEffector du personnage est désactivé.
        _sr.color = new Color(1, 1, 1, 0); // #TP3 Olivier La couleur est blanc et est de 0 d'opacité. (invisible)

        StartCoroutine(CoroutineCompteur(false)); // #TP3 Olivierstart la coroutine pour commancer le compteur de 10 secondes pour déterminer quand l'aimant peut etre réactivé.
    }

    /// <summary>
    /// Coroutine du compteur de 5 ou de 10 secondes selon si l'aimant est activer ou désactivé.
    /// </summary>
    /// <param name="estActive"></param>
    /// <returns></returns>
    IEnumerator CoroutineCompteur(bool estActive)
    {
        if (estActive) // #TP3 Olivier Si l'aimant est activé
        {
            Boutique.instance.donneesPerso.RetirerObjet(_donnees); //#Tp synthese Olivier Appelle la méthode d'achat de l'objet dans les données du personnage de la boutique.
            int compteur = 5; // #TP3 Olivier Nombre de secondes du compteur 
            while (compteur > 0) // #TP3 Olivier Quand le compteur est plus quand que 0
            {
                _donneesPerso.tempsAimant = compteur; // #TP4 Olivier Donne le nombre de secondes restantes de l'aimant
                yield return new WaitForSeconds(1); // #TP3 Olivier attend 1 seconde avant de continuer
                compteur--; // #TP3 Olivier reduit le nombre de 1
                if (compteur == 0) // #TP3 Olivier si le compteur est de 0
                {
                    _donneesPerso.tempsAimant = 0; // #TP4 Olivier Donne le nombre de secondes restantes de l'aimant
                    _estDisponible = false; // #TP3 Olivier l'aimant n'est pas disponnible
                }
            }
        }
    }


}
