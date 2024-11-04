using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
//

/// <summary>
/// Classe représentant un panneau d'objet dans l'interface utilisateur.
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>
public class PanneauObjet : MonoBehaviour
{
    [Header("LES DONNÉES")]
    [SerializeField] SOObjet _donnees; // Données de l'objet représenté par le panneau.
    public SOObjet donnees => _donnees; // Propriété permettant d'accéder aux données de l'objet.

    [Header("LES CONTENEURS")]
    [SerializeField] TextMeshProUGUI _champNom; // Champ d'affichage du nom de l'objet.
    [SerializeField] TextMeshProUGUI _champPrix; // Champ d'affichage du prix de l'objet.
    [SerializeField] TextMeshProUGUI _champDescription; // Champ d'affichage de la description de l'objet.
    [SerializeField] Image _image; // Image représentant l'objet.
    [SerializeField] CanvasGroup _canvasGroup; // Groupe de canvas pour gérer l'interaction avec l'objet.

    void Start()
    {
        MettreAJourInfos(); // Met à jour les informations affichées dans le panneau.
        Boutique.instance.donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfos); // Ajoute la méthode MettreAJourInfos à l'événement de mise à jour des données du personnage de la boutique.
    }

    /// <summary>
    /// Met à jour les informations affichées dans le panneau en fonction des données de l'objet.
    /// </summary>
    private void MettreAJourInfos()
    {
        _champNom.text = _donnees.nom; // Affiche le nom de l'objet.
        _champPrix.text = _donnees.prix + "$"; // Affiche le prix de l'objet.
        _champDescription.text = _donnees.description; // Affiche la description de l'objet.
        _image.sprite = _donnees.sprite; // Affiche l'image représentant l'objet.
        GererDispo(); // Gère la disponibilité de l'objet dans le panneau.
    }

    /// <summary>
    /// Gère la disponibilité de l'objet dans le panneau en fonction du niveau requis et de l'argent disponible du personnage.
    /// </summary>
    void GererDispo()
    {
        bool aNiveauRequis = Boutique.instance.donneesPerso.niveau >= _donnees.niveauRequis; // Vérifie si le personnage a le niveau requis pour acheter l'objet.
        bool aAssezArgent = Boutique.instance.donneesPerso.argent >= _donnees.prix; // Vérifie si le personnage a assez d'argent pour acheter l'objet.
        
        // Si le personnage a le niveau requis et assez d'argent, l'objet est disponible à l'achat.
        if(aNiveauRequis && aAssezArgent)
        {
            _canvasGroup.interactable = true; // Active l'interaction avec le panneau.
            _canvasGroup.alpha = 1; // Affiche le panneau avec une opacité à 100%.
        } 
        else // Sinon, l'objet n'est pas disponible à l'achat.
        {
            _canvasGroup.interactable = false; // Désactive l'interaction avec le panneau.
            _canvasGroup.alpha = .5f; // Affiche le panneau avec une opacité à 50%.
        }
    }

    /// <summary>
    /// Méthode appelée lors de l'achat de l'objet.
    /// </summary>
    public void Acheter()
    {
        Boutique.instance.donneesPerso.Acheter(_donnees); // Appelle la méthode d'achat de l'objet dans les données du personnage de la boutique.
    }
}
