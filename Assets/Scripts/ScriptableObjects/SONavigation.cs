using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ScriptableObject pour la navigation entre les scènes du jeu.
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>
[CreateAssetMenu(fileName = "MaNavigation", menuName = "Navigation")]
public class SONavigation : ScriptableObject
{
    [SerializeField] SOPerso _donneesPerso; // Référence aux données du personnage.
    [SerializeField] SOScore _score; // Référence aux données du personnage.
    [SerializeField] SOTemps _temps; // Référence aux données du personnage.

    /// <summary>
    /// Initialise les données du personnage et charge la scène suivante.
    /// </summary>
    public void Jouer()
    {
        // #tp4 Louis
        _score.score = 0; // Initialise le score du personnage.
        _score.nbBonusRecoltes = 0; // Initialise le score du personnage.
        _temps.ReinitialiserTemps(); // Réinitialise le chronomètre.
        SceneManager.LoadScene("Jeu"); // Charger scène du jeu.
    }

    /// <summary>
    /// Incrémente le niveau du personnage et charge la scène précédente.
    /// </summary>
    public void SortirBoutique()
    {
        _donneesPerso.niveau++; // Incrémente le niveau du personnage.
        // #tp4 Louis
        SceneManager.LoadScene("Jeu"); // Charger scène du jeu.
    }

    /// <summary>
    /// Charge la scène suivante dans l'ordre de la build.
    /// </summary>
    public void AllerSceneSuivante()
    {
        GestAudio.instance.DemarerCoroutineMusical(TypePiste.MusiqueEventB, 0.2f, false); //#Tp synthese Olivier arrête la musique de l'événement B
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Charge la scène suivante dans l'ordre de la build.
    }

    /// <summary>
    /// Charge la scène précédente dans l'ordre de la build.
    /// </summary>
    public void AllerScenePrecedente()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // Charge la scène précédente dans l'ordre de la build.
    }

    // #tp4 Louis
    public void AllerDerniereScene()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1); // Charge la scène précédente dans l'ordre de la build.
    }

    // #tp4 Louis
    public void AllerPremiereScene()
    {
        SceneManager.LoadScene(0); // Charge la scène précédente dans l'ordre de la build.
    }

    // #synthese Louis
    public void AllerTabHonneur()
    {
        SceneManager.LoadScene("TableauHonneur"); // Charge la scène précédente dans l'ordre de la build.
    }
    // #synthese Louis
    public void AllerPanneauFin()
    {
        SceneManager.LoadScene("PanneauFin"); // Charge la scène précédente dans l'ordre de la build.
    }
    // #synthese Olivier
    public void AllerInterfaceExplicative()
    {
        SceneManager.LoadScene("InterfaceExplicative"); // Charge la scène précédente dans l'ordre de la build.
    }
    public void AllerInterfaceTouche()
    {
        SceneManager.LoadScene("Scene_Touches"); // Charge la scène précédente dans l'ordre de la build.
    }
}
