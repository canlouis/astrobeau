using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ScriptableObject représentant un objet disponible dans la boutique.
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>
[CreateAssetMenu(fileName = "Objet", menuName = "Objet boutique")]
public class SOObjet : ScriptableObject
{
    [Header("LES DONNÉES")]
    [SerializeField] string _nom = "Trèfle"; // Le nom de l'objet.
    [SerializeField][Tooltip("Icône de l'objet à afficher dans la boutique")] Sprite _sprite; // L'icône de l'objet.
    [SerializeField][Range(0, 200)] int _prixDeBase = 30; // Le prix de base de l'objet.
    [SerializeField][Range(1, 5)] int _niveauRequis = 1; // Le niveau requis pour acheter l'objet.
    [SerializeField][TextArea] string _description; // La description de l'objet.
    [SerializeField][Tooltip("Cet objet donne-t-il droit au rabais?")] bool _donneDroitRabais = false; // Indique si l'objet donne droit à un rabais.



    /// <summary>
    /// Propriété pour accéder et définir le nom de l'objet.
    /// </summary>
    public string nom { get => _nom; set => _nom = value; }

    /// <summary>
    /// Propriété pour accéder et définir l'icône de l'objet.
    /// </summary>
    public Sprite sprite { get => _sprite; set => _sprite = value; }

    /// <summary>
    /// Propriété pour accéder au prix de l'objet, ajusté selon le facteur de prix du joueur s'il existe.
    /// </summary>
    public int prix
    {
        get
        {
            float facteurPrix = 1f;
            if (Boutique.instance != null) facteurPrix = Boutique.instance.donneesPerso.facteurPrix; // Si une instance de la boutique existe, utilise le facteur de prix du joueur.
            int prix = Mathf.RoundToInt(_prixDeBase * facteurPrix); // Calcule le prix en tenant compte du facteur de prix.
            return prix;
        }
    }

    /// <summary>
    /// Propriété pour accéder et définir le niveau requis pour acheter l'objet.
    /// </summary>
    public int niveauRequis { get => _niveauRequis; set => _niveauRequis = Mathf.Clamp(value, 0, int.MaxValue); }

    /// <summary>
    /// Propriété pour accéder et définir la description de l'objet.
    /// </summary>
    public string description { get => _description; set => _description = value; }

    /// <summary>
    /// Propriété pour accéder et définir si l'objet donne droit à un rabais.
    /// </summary>
    public bool donneDroitRabais { get => _donneDroitRabais; set => _donneDroitRabais = value; }
}
