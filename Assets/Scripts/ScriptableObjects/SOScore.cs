using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject représentant le score du joueur.
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>

// ScriptableObject contenant les données d'un personnage joueur.
[CreateAssetMenu(fileName = "Score", menuName = "Score")]
public class SOScore : ScriptableObject
{
    [SerializeField][Range(0, 100000)] int _score = 0; // Le score actuel du personnage.
    int _nbBonusRecoltes = 0; // Le nombre de bonus récoltés par le joueur.
    public int nbBonusRecoltes { get => _nbBonusRecoltes; set => _nbBonusRecoltes = value; }

    public int score
    {
        get => _score;
        set
        {
            _score = Mathf.Clamp(value, 0, int.MaxValue); // Assure que l'argent reste dans les bornes.
        }
    }

}
