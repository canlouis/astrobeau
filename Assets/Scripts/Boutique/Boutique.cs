using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Classe représentant une boutique dans le jeu.
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>
public class Boutique : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; // Données du personnage utilisées pour la boutique.
    public SOPerso donneesPerso => _donneesPerso; // Propriété permettant d'accéder aux données du personnage.

    [SerializeField] TextMeshProUGUI _champNiveau; // Champ d'affichage du niveau du personnage.
    [SerializeField] TextMeshProUGUI _champArgent; // Champ d'affichage de l'argent du personnage.
    [SerializeField] PanneauInventaire _panneauInventaire;
    public PanneauInventaire panneauInventaire => _panneauInventaire;

    static Boutique _instance; // Instance statique de la boutique.
    static public Boutique instance => _instance; // Propriété permettant d'accéder à l'instance de la boutique.

    void Awake()
    {
        // Empêche la création d'une nouvelle instance de la boutique si une existe déjà.
        if (_instance != null) { Destroy(gameObject); return; }
        _instance = this; // Enregistre l'instance actuelle de la boutique.
        MettreAJourInfos(); // Met à jour les informations affichées dans la boutique.
        _donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfos); // Ajoute la méthode MettreAJourInfos à l'événement de mise à jour des données du personnage.
    }

    /// <summary>
    /// Met à jour les informations affichées dans la boutique.
    /// </summary>
    private void MettreAJourInfos()
    {
        _champNiveau.text = "Niveau " + _donneesPerso.niveau; // Affiche le niveau du personnage.
        _champArgent.text = _donneesPerso.argent + ""; // Affiche l'argent du personnage.
    }

    bool _estEnPlay = true; // Indique si le jeu est en cours.

    void OnApplicationQuit()
    {
        _donneesPerso.Initialiser(); // Réinitialise les données du personnage lors de la fermeture du jeu.
        _estEnPlay = false; // Le jeu n'est plus en cours.
    }

    void OnDestroy()
    {
        _donneesPerso.evenementMiseAJour.RemoveAllListeners(); // Supprime tous les écouteurs de l'événement de mise à jour des données du personnage.
        // if(_estEnPlay) _donneesPerso.niveau++; // Incrémente le niveau du personnage si le jeu est en cours lors de la destruction de la boutique.
    }
}
