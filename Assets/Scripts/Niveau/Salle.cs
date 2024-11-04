using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe permettant de définir les dimensions des salles et d'afficher ces dimensions.
/// Auteurs du code: Louis Cantin et Olivier Dion
/// Auteur des commentaires: Louis Cantin
/// </summary>
public class Salle : MonoBehaviour
{
    // #tp3 Louis
    [SerializeField] Transform[] _reperesPorte; // Références aux repères pour placer les portes.
    [SerializeField] Transform[] _reperesActivateur; // Références aux repères pour placer les activateurs.
    [SerializeField] Transform[] _reperesClee; // Références aux repères pour placer les clés.
    [SerializeField] Transform[] _reperesEffectorP; // Références aux repères pour placer les petits effectors.
    [SerializeField] Transform[] _reperesEffectorM; // Références aux repères pour placer les moyens effectors.
    [SerializeField] Transform[] _reperesEffectorG; // Références aux repères pour les gros effectors.
    [SerializeField] Transform[] _reperesNids; // Références aux repères pour placer les nids.
    [SerializeField] Transform[] _reperesBoutonJoyaux; // Références aux repères pour placer les clés.
    [SerializeField] Transform[] _reperesEnnemiOlivier; // Références aux repères pour placer les grosse araignée.
    [SerializeField] Transform _reperePerso; // Référence au repère pour placer le personnage.

    [SerializeField] private Vector2Int _tailleBoite = new Vector2Int(24, 24); // Taille de la boîte de la salle.
    // Taille de la salle avec bordures (statique pour être accessible depuis d'autres classes)
    static Vector2Int _tailleAvecBordures = new Vector2Int(24, 24);
    static public Vector2Int tailleAvecBordures => _tailleAvecBordures;

    // #tp3 Louis
    /// <summary>
    /// Place un modèle d'objet sur un repère de la salle et retourne sa position.
    /// </summary>
    /// <param name="modele">Modèle d'objet à placer</param>
    /// <returns>Position du modèle d'objet</returns>
    public Vector2Int PlacerSurRepere(GameObject modele)
    {
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity; // #synthese Louis
        if (modele.name == "Porte") // Vérifie si le modèle est une porte.
        {
            int emplacementAlea = ChoisirEmplacementAleatoire(_reperesPorte.Length); // Choix aléatoire d'un repère pour la porte.
            pos = _reperesPorte[emplacementAlea].position; // Position du repère pour la porte.
            pos.y += 1; // Ajustement de la hauteur pour la porte.
            rot = _reperesPorte[emplacementAlea].rotation; // #synthese Louis
        }
        else if (modele.name == "Activateur") // Vérifie si le modèle est un activateur.
        {
            int emplacementAlea = ChoisirEmplacementAleatoire(_reperesActivateur.Length); // Choix aléatoire d'un repère pour l'activateur.
            pos = _reperesActivateur[emplacementAlea].position; // Position du repère pour l'activateur.
            rot = _reperesActivateur[emplacementAlea].rotation; // #synthese Louis
        }
        else if (modele.name == "Cle") // Vérifie si le modèle est une clé.
        {
            int emplacementAlea = ChoisirEmplacementAleatoire(_reperesClee.Length); // Choix aléatoire d'un repère pour la clé.
            pos = _reperesClee[emplacementAlea].position; // Position du repère pour la clé.
            rot = _reperesClee[emplacementAlea].rotation; // #synthese Louis
        }
        else if (modele.name == "EffectorP") // Vérifie si le modèle est un petit effector.
        {
            if (_reperesEffectorP.Length != 0)
            {
                int emplacementAlea = ChoisirEmplacementAleatoire(_reperesEffectorP.Length); // Choix aléatoire d'un repère pour la clé.
                pos = _reperesEffectorP[emplacementAlea].position; // Position du repère pour la clé.
                rot = _reperesEffectorP[emplacementAlea].rotation; // #synthese Louis
            }
        }
        else if (modele.name == "EffectorM") // Vérifie si le modèle est un moyen effector.
        {
            if (_reperesEffectorM.Length != 0)
            {
                int emplacementAlea = ChoisirEmplacementAleatoire(_reperesEffectorM.Length); // Choix aléatoire d'un repère pour la clé.
                pos = _reperesEffectorM[emplacementAlea].position; // Position du repère pour la clé.
                rot = _reperesEffectorM[emplacementAlea].rotation; // #synthese Louis
            }
        }
        else if (modele.name == "EffectorG") // Vérifie si le modèle est un gros effector.
        {
            if (_reperesEffectorG.Length != 0)
            {
                int emplacementAlea = ChoisirEmplacementAleatoire(_reperesEffectorG.Length); // Choix aléatoire d'un repère pour la clé.
                pos = _reperesEffectorG[emplacementAlea].position; // Position du repère pour la clé.
                rot = _reperesEffectorG[emplacementAlea].rotation; // #synthese Louis
            }
        }
        else if (modele.name == "BoutonJoyaux") // #synthese olivier Vérifie si le modèle est une clé.
        {
            // #synthese olivier Vérifie si il y a des repères pour les clés.
            if (_reperesBoutonJoyaux.Length != 0)
            {
                int emplacementAlea = ChoisirEmplacementAleatoire(_reperesBoutonJoyaux.Length); // #synthese olivier Choix aléatoire d'un repère pour la clé.
                pos = _reperesBoutonJoyaux[emplacementAlea].position; // #synthese olivier Position du repère pour la clé.
                rot = _reperesBoutonJoyaux[emplacementAlea].rotation; // #synthese olivier Position du repère pour la clé.
            }
        }
        else if (modele.name == "EnnemiOlivier") // #synthese olivier Vérifie si le modèle est une araignee.
        {
            if (_reperesEnnemiOlivier.Length != 0) // #synthese olivier Vérifie si il y a des repères pour les araignées.
            {
                int emplacementAlea = ChoisirEmplacementAleatoire(_reperesEnnemiOlivier.Length);  // #synthese olivier Choix aléatoire d'un repère pour l'araignée.
                pos = _reperesEnnemiOlivier[emplacementAlea].position;  // #synthese olivier Position du repère pour l'araignée.
                rot = _reperesEnnemiOlivier[emplacementAlea].rotation;  // #synthese olivier Position du repère pour l'araignée.
            }
        }
        // #synthese Louis
        else if (modele.name == "Nid") // Vérifie si le modèle est une clé.
        {
            if (_reperesNids.Length != 0)
            {
                int emplacementAlea = ChoisirEmplacementAleatoire(_reperesNids.Length); // Choix aléatoire d'un repère pour la clé.
                pos = _reperesNids[emplacementAlea].position; // Position du repère pour la clé.
                rot = _reperesNids[emplacementAlea].rotation;
            }
        }
        //
        else
        {
            pos = _reperePerso.position; // Position par défaut pour le modèle.
            rot = _reperePerso.rotation; // #synthese Louis
        }


        if (pos != Vector3.zero)
        {
            Instantiate(modele, pos, rot, transform.parent); // Instancie le modèle à la position déterminée.
        }
        return Vector2Int.FloorToInt(pos); // Retourne la position du modèle en entiers.
    }

    /// <summary>
    /// Dessine une boîte centrée sur la position de la salle avec ses dimensions.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, (Vector3Int)_tailleBoite); // Dessine une boîte représentant la salle.
    }

    /// <summary>
    /// Choix aléatoire d'un emplacement parmi une quantité donnée.
    /// </summary>
    /// <param name="emplacements">Nombre d'emplacements disponibles</param>
    /// <returns>Indice de l'emplacement choisi</returns>
    int ChoisirEmplacementAleatoire(int emplacements)
    {
        int emplacementAlea = Random.Range(0, emplacements); // Génère un indice aléatoire.
        return emplacementAlea; // Retourne l'indice aléatoire.
    }
}
