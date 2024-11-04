using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GestTexte : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _texteArgents;  // #TP4 Olivier Champ de texte affichant le temps restant de l'aimant.
    [SerializeField] TextMeshProUGUI _texteScore;  // #TP4 Olivier Champ de texte affichant le temps restant de l'aimant.
    [SerializeField] TextMeshProUGUI _texteTemps;  // #synthese Olivier Champ de texte affichant le temp
    static GestTexte _instance; // #TP4 Olivier Instance de la classe
    public static GestTexte instance => _instance; // #TP4 Olivier Propriété pour accéder à l'instance de la classe
    [SerializeField] SOPerso _donneesPerso; // Données du personnage.
    [SerializeField] SOScore _donneesScore; // Données du personnage.
    [SerializeField] SOTemps _donneesTemps; // Données du temps.



    void Awake()
    {
        if (!DevenirSingleton()) // #TP4 Olivier Si l'instance est déjà créée, détruit le gameObject
        {
            Destroy(gameObject); // #TP4 Olivier Détruit le gameObject
            return; // #TP4 Olivier Retourne
        }
    }
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        _texteArgents.text = _donneesPerso.argent.ToString();  // #TP4 Olivier Change le texte du champ de texte de l'argent
        _texteScore.text = _donneesScore.score.ToString();  // #TP4 Olivier Change le texte du champ de texte du score
        _texteTemps.text = _donneesTemps.tempsRestant.ToString();  // #synthese Olivier Change le texte du champ de texte du temps
    }


    /// <summary>
    /// Fonction qui permet de devenir un singleton
    /// </summary>
    /// <returns>Le singleton à été créé ou non</returns>
    bool DevenirSingleton()
    {
        if (_instance == null) // #TP4 Olivier Si l'instance est null
        {
            _instance = this; // #TP4 Olivier L'instance est égale à cette instance
            return true; // #TP4 Olivier Retourne vrai
        }
        return false; // #TP4 Olivier Retourne faux
    }
}
