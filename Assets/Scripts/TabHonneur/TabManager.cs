using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Classe contrôlant le tableau d'honneur.
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>
public class TabManager : MonoBehaviour
{
    [SerializeField] SOSauvegarde _sauvegarde; 
    [SerializeField] SOScore _score; 
    [SerializeField] LigneTabHonneur _ligne; // Prefab d'une ligne de tableau

    int _nbLigne = 5;

    void Awake()
    {
        // Boucle pour créer les lignes de tableau
        for (int i = 0; i < _nbLigne; i++)
        {
            // Définit le numéro de la ligne
            _ligne.numero.text = (i + 1).ToString();

            // Définit le score de la ligne
            _ligne.score.text = _score.score.ToString();

            // Définit le pseudo de la ligne
            _ligne.pseudo.text = (i + 1).ToString();

            // Crée une instance de la ligne
            Instantiate(_ligne, transform.position, Quaternion.identity, transform);
        }
        _sauvegarde.LireFichier(_ligne.pseudo, _ligne.score, 0);
    }
}