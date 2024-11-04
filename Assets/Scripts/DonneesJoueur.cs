using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe permettant de sauvegarder les données du joueur
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>

// Attribut pour rendre cette classe sérialisable, ce qui permet de l'enregistrer et de la charger
[System.Serializable]
public class DonneesJoueur
{
    public string pseudo;

    public int score;

    public bool estScoreActuel; // #synthese olivier Indique si le score est celui du joueur actuel
}