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
public class PanneauPointsBonus : MonoBehaviour
{
    [SerializeField] TMP_Text _champPointsTemps; // Champ de texte pour afficher les points temps des joueurs.
    [SerializeField] TMP_Text _champBonusPoints; // Champ de texte pour afficher les points bonus des joueurs.
    [SerializeField] TMP_Text _champPointsTotaux; // Champ de texte pour afficher les points totaux des joueurs.
    [SerializeField] SOScore _score; // Les données de score.
    [SerializeField] SOSauvegarde _sauvegarde; // Les données de sauvegarde.
    [SerializeField] SOTemps _temps; // Les données de temps.
    [SerializeField] Button _btn; // Le bouton interactif.
    [SerializeField] int _constantePointsTemps = 50; // La constante de points pour le temps.
    [SerializeField] int _constantePointsBonus = 200; // La constante de points pour le bonus.
    int _pointsTemps; // Points obtenus à partir du temps.
    int _pointsTempsIni = 0; // Points de temps initiaux.
    int _pointsBonus = 100; // Points obtenus à partir du bonus.
    int _pointsBonusIni = 0; // Points de bonus initiaux.
    int _pointsTotaux; // Points totaux.
    int _pointsTotauxIni = 0; // Points totaux initiaux.
    int _vitessePoints; // La vitesse de la progression des points.
    float _tempsEntreEtapes = .5f; // Le temps entre chaque étape de l'animation.
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

        _champPointsTemps.text = $"{_temps.tempsRestant} s"; // Affiche le temps restant.

        yield return new WaitForSeconds(_tempsEntreEtapes);

        _champPointsTemps.text = $"{_temps.tempsRestant} s   X";

        yield return new WaitForSeconds(_tempsEntreEtapes);

        // Affiche les éléments nécessaires pour calculer les points obtenus à partir du temps.
        _champPointsTemps.text = $"{_temps.tempsRestant} s   X   {_constantePointsTemps} :  ";

        yield return new WaitForSeconds(_tempsEntreEtapes);

        _pointsTemps = _temps.tempsRestant * _constantePointsTemps; // Calcule les points obtenus à partir du temps.
        if (_ratioVitessePoints > _pointsTemps) _ratioVitessePoints = _pointsTemps; // Ajuste le ratio de vitesse de progression des points.

        _vitessePoints = Mathf.FloorToInt(_pointsTemps / _ratioVitessePoints); // Ajuste la vitesse de progression des points.
        for (int i = 0; i < _pointsTemps / _vitessePoints; i++)
        {
            yield return new WaitForSeconds(1 / _pointsTemps);
            _pointsTempsIni += _vitessePoints; // Incrémente les points de temps initiaux.
            _champPointsTemps.text = $"{_temps.tempsRestant} s   X   {_constantePointsTemps}:   {_pointsTempsIni}"; // Met à jour le champ de texte.
        }
        if (_pointsTemps == 0) _champPointsTemps.text = $"{_temps.tempsRestant} s   X   {_constantePointsTemps} :   0"; // Si aucun point n'est obtenu à partir du temps, affiche 0.

        yield return new WaitForSeconds(_tempsEntreEtapes);

        _champBonusPoints.text = $"{_score.nbBonusRecoltes}"; // Affiche le nombre de bonus récoltés.

        yield return new WaitForSeconds(_tempsEntreEtapes);

        _champBonusPoints.text = $"{_score.nbBonusRecoltes}   X"; // Affiche le nombre de bonus récoltés avec 'X'.

        yield return new WaitForSeconds(_tempsEntreEtapes);

        _champBonusPoints.text = $"{_score.nbBonusRecoltes}   X   {_constantePointsBonus} :  "; // Affiche les éléments nécessaires pour calculer les points obtenus à partir des bonus.

        yield return new WaitForSeconds(_tempsEntreEtapes);
        _pointsBonus = _score.nbBonusRecoltes * _constantePointsBonus; // Calcule les points obtenus à partir des bonus.
        if (_pointsBonus > 0)
        {
            if (_ratioVitessePoints > _pointsBonus) _ratioVitessePoints = _pointsBonus; // Ajuste le ratio de vitesse de progression des points.
            _vitessePoints = Mathf.FloorToInt(_pointsBonus / _ratioVitessePoints); // Ajuste la vitesse de progression des points.
        for (int i = 0; i < _pointsBonus / _vitessePoints; i++)
        {
            yield return new WaitForSeconds(1 / _pointsBonus);
            _pointsBonusIni += _vitessePoints; // Incrémente les points de bonus initiaux.
            _champBonusPoints.text = $"{_score.nbBonusRecoltes}   X   {_constantePointsBonus} :   {_pointsBonusIni}"; // Met à jour le champ de texte.
        }
        }
        else
        {
            _champBonusPoints.text = $"{_score.nbBonusRecoltes}   X   {_constantePointsBonus} :   0";
        }

        yield return new WaitForSeconds(_tempsEntreEtapes);

        _pointsTotaux = _pointsTemps + _pointsBonus; // Calcule les points totaux.
        if (_ratioVitessePoints > _pointsTotaux) _ratioVitessePoints = _pointsTotaux; // Ajuste le ratio de vitesse de progression des points.

        _vitessePoints = Mathf.FloorToInt(_pointsTotaux / _ratioVitessePoints); // Ajuste la vitesse de progression des points.
        for (int i = 0; i < _pointsTotaux / _vitessePoints; i++)
        {
            yield return new WaitForSeconds(1 / _pointsTotaux);
            _pointsTotauxIni += _vitessePoints; // Incrémente les points totaux initiaux.
            _champPointsTotaux.text = $"{_pointsTotauxIni}"; // Met à jour le champ de texte des points totaux.
        }
        if (_pointsTotaux == 0) _champPointsTotaux.text = $"0"; // Si aucun point total n'est obtenu, affiche 0.
        _score.score += _pointsTotaux; // Ajoute les points totaux au score.

        yield return new WaitForSeconds(_tempsEntreEtapes);
        _btn.interactable = true; // Active le bouton interactif.
    }
}
