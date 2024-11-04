using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Classe faisant l'animation des points bonus.
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>
public class PanneauFin : MonoBehaviour
{
    [SerializeField] TMP_Text _champPointsTotaux; // Champ de texte pour afficher les points totaux des joueurs.
    [SerializeField] SOScore _score; // Les données de score.
    [SerializeField] SOSauvegarde _sauvegarde; // Les données de sauvegarde.
    [SerializeField] Button _btn; // Le bouton interactif.
    int _pointsTotauxIni = 0; // Points totaux initiaux.
    int _vitessePoints; // La vitesse de la progression des points.
    float _tempsEntreEtapes = 1.5f; // Le temps entre chaque étape de l'animation.
    int _ratioVitessePoints = 500; // Le ratio de vitesse de progression des points.

    void Awake()
    {
        _btn.interactable = false; // Désactive le bouton interactif.
        StartCoroutine(CoroutinePoints()); // Démarre la coroutine pour l'animation des points.
    }

    /// <summary>
    /// Coroutine pour l'animation des points.
    /// </summary>
    IEnumerator CoroutinePoints()
    {
        yield return new WaitForSeconds(_tempsEntreEtapes); // Attend un certain temps avant de commencer.
        if (_ratioVitessePoints > _score.score) _ratioVitessePoints = _score.score; // Ajuste le ratio de vitesse de progression des points.
        if (_score.score > 0)
        {
            _vitessePoints = Mathf.FloorToInt(_score.score / _ratioVitessePoints); // Ajuste la vitesse de progression des points.
            for (int i = 0; i < _score.score / _vitessePoints; i++)
            {
                yield return new WaitForSeconds(1 / _score.score);
                _pointsTotauxIni += _vitessePoints; // Incrémente les points totaux initiaux.
                _champPointsTotaux.text = $"{_pointsTotauxIni}"; // Met à jour le champ de texte des points totaux.
            }
        }
        else
        {
            _champPointsTotaux.text = "0";
        }
        yield return new WaitForSeconds(_tempsEntreEtapes);
        _btn.interactable = true; // Active le bouton interactif.
    }
}
