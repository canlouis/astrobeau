using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Runtime.InteropServices;
/// <summary>
/// Classe gérant la sauvegarde des données.
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>

[CreateAssetMenu(menuName = "TIM/Sauvegarde", fileName = "Sauvegarde")]
public class SOSauvegarde : ScriptableObject
{
    [SerializeField] SOScore _score;
    [SerializeField] string _fichier = "Score.tim"; // Nom du fichier de sauvegarde.

    // Liste de données sérialisées de joueurs
    List<DonneesJoueur> _donneesJoueurs = new List<DonneesJoueur>();

    [DllImport("__Internal")]  // Importation de la fonction WebGL pour la synchronisation.
    private static extern void SynchroniserWebGL();

    /// <summary>
    /// Fonction pour lire le fichier de sauvegarde et mettre à jour les champs de pseudo et de score dans l'interface utilisateur. 
    /// </summary>
    /// <param name="champPseudo">Champ de texte pour afficher le pseudo du joueur.</param>
    /// <param name="champScore">Champ de texte pour afficher le score du joueur.</param>
    /// <param name="i">Indice du joueur dans la liste des données.</param>
    public void LireFichier(TMP_Text champPseudo, TMP_Text champScore, int i)
    {
        string fichierEtChemin = Application.dataPath + "/" + _fichier;  // Chemin complet du fichier de sauvegarde.
        if (File.Exists(fichierEtChemin))  // Vérifie si le fichier existe.
        {
            string contenu = File.ReadAllText(fichierEtChemin);  // Lit le contenu du fichier.
            JsonUtility.FromJsonOverwrite(contenu, this);  // Met à jour les données avec celles du fichier.
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            #endif
            champPseudo.text = _donneesJoueurs[i].pseudo + "";  // Met à jour le champ de pseudo dans l'interface utilisateur.
            champScore.text = _donneesJoueurs[i].score + "";  // Met à jour le champ de score dans l'interface utilisateur.
        }
        else Debug.LogWarning("Fichier inexistant");  // Affiche un avertissement si le fichier n'existe pas.
    }

    // Fonction pour écrire les données dans le fichier de sauvegarde.
    public void EcrireFichier()
    {
        string fichierEtChemin = Application.dataPath + "/" + _fichier;  // Chemin complet du fichier de sauvegarde.
        string contenu = JsonUtility.ToJson(this);  // Convertit les données en format JSON.
        File.WriteAllText(fichierEtChemin, contenu);  // Écrit le contenu JSON dans le fichier.

        if(Application.platform == RuntimePlatform.WebGLPlayer)  // Vérifie si l'application est WebGL.
        {
            SynchroniserWebGL();  // Appelle la fonction de synchronisation WebGL.
            Debug.Log("WebGL synchronisé");  // Affiche un message de confirmation dans la console.
        }
        Debug.Log("Fichier écrit");  // Affiche un message de confirmation dans la console.
        
    }

    // Fonction pour trier les scores des joueurs en ordre décroissant.
    public void TrierLesScores(){
        _donneesJoueurs.Sort((a, b) => b.score.CompareTo(a.score));
    }

    // Fonction pour ajouter un nouveau score à la liste des données.
    // Paramètres :
    // - pseudo : Pseudo du joueur dont le score doit être ajouté.
    // - score : Score du joueur à ajouter.
    public void AjouterScore(string pseudo, int score){
        _donneesJoueurs.Add(new DonneesJoueur(){pseudo = pseudo, score = score});  // Ajoute un nouveau joueur à la liste.
        if(_donneesJoueurs.Count > 5){  // Si la liste dépasse 5 éléments...
            _donneesJoueurs.RemoveAt(5);  // ...supprime le dernier élément.
        }
        if(_donneesJoueurs.Count > 1){  // Si la liste contient plus d'un élément...
            TrierLesScores();  // ...trie les scores.
        }
    }
}


