using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Script pour copier des tuiles d'un Tilemap à un autre avec une probabilité spécifiée.
/// Auteurs du code: Louis Cantin et Olivier Dion
/// Auteur des commentaires: Louis Cantin
/// </summary>
public class TilemapScript : MonoBehaviour
{
    [SerializeField][Range(0, 100)] int _probabilites; // Probabilité (en pourcentage) de copier un Tilemap

    void Awake()
    {
        Tilemap tilemap = GetComponent<Tilemap>(); // Récupération du Tilemap sur lequel le script est attaché

        BoundsInt bounds = tilemap.cellBounds; // Récupération des limites du Tilemap
        
        Niveau niveau = GetComponentInParent<Niveau>(); // Récupération du composant Niveau dans le parent

        Vector3Int decalage = Vector3Int.FloorToInt(transform.position); // Calcul du décalage à appliquer lors de la copie des tuiles

        int nbRand = Random.Range(0, 101); // Génération d'un nombre aléatoire entre 0 et 100 pour comparer avec la probabilité

        // Parcours de toutes les cellules du Tilemap
        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                // Vérification si la probabilité est atteinte pour copier la tuile
                if (_probabilites >= nbRand)
                {
                    TraiterUneTuile(tilemap, niveau, y, x, decalage);
                }
            }
        }

        // Désactivation de l'objet portant ce script après le traitement
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Copie une tuile d'un Tilemap à un autre, ajustée avec un décalage.
    /// </summary>
    /// /// <param name="tilemap">Composant tilemap de la tilemap actuelle</param>
    /// /// <param name="niveau">Endroit où les tuiles seront déposées</param>
    /// /// <param name="x">Position x d'une tuile</param>
    /// /// <param name="y">Position y d'une tuile</param>
    /// /// <param name="decalage">Décalage à appliquer lors de la copie des tuiles</param>
    void TraiterUneTuile(Tilemap tilemap, Niveau niveau, int y, int x, Vector3Int decalage)
    {
        Vector3Int pos = new Vector3Int(x, y); // Position de la tuile à copier
        TileBase tuile = tilemap.GetTile(pos); // Sélectionne la tuile à l'emplacment désiré

        // Si la tuile existe, elle est copiée dans le Tilemap du niveau avec le décalage
        if (tuile != null) niveau.tilemap.SetTile(pos + decalage, tuile);
    }
}

