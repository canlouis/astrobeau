using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanneauInventaire : MonoBehaviour
{
    [SerializeField] PanneauVignette _panneauVignetteModele; // Modèle de panneau vignette pour les objets dans l'inventaire

    void Start()
    {
        // Affiche l'inventaire lorsque la scène démarre, si l'instance de la boutique existe
        if (Boutique.instance != null) Boutique.instance.donneesPerso.AfficherInventaire();
    }

    /// <summary>
    /// Ajoute un objet à l'inventaire sous forme de panneau vignette.
    /// </summary>
    /// <param name="donneesObjet">Les données de l'objet à ajouter.</param>
    /// <returns>Le panneau vignette représentant l'objet ajouté.</returns>
    public PanneauVignette Ajouter(SOObjet donneesObjet)
    {
        // Instancie un nouveau panneau vignette à partir du modèle et l'ajoute comme enfant du panneau inventaire
        PanneauVignette panneauVignette = Instantiate(_panneauVignetteModele, transform);
        // Définit les données de l'objet pour le panneau vignette
        panneauVignette.donneesObjet = donneesObjet;
        // Retourne le panneau vignette nouvellement créé
        return panneauVignette;
    }

    /// <summary>
    /// Vide l'inventaire en supprimant tous les panneaux vignettes enfants.
    /// </summary>
    public void Vider()
    {
        // Parcourt tous les enfants du panneau inventaire
        foreach (Transform enfant in transform)
        {
            // Détruit l'objet enfant (le panneau vignette)
            Destroy(enfant.gameObject);
        }
    }
}
