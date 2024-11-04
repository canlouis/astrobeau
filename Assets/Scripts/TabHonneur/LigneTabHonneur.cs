using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Classe contrôlant une ligne du tableau d'honneur.
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>
public class LigneTabHonneur : MonoBehaviour
{
    [SerializeField] SOSauvegarde _sauvegarde; // Référence à l'objet de sauvegarde.
    [SerializeField] TMPro.TMP_Text _pseudo; // Champ de texte pour le pseudo du joueur.
    [SerializeField] TMPro.TMP_Text _score; // Champ de texte pour le score du joueur.
    [SerializeField] TMPro.TMP_Text _numero; // Champ de texte pour le numéro du joueur dans le tableau.
    [SerializeField] TMP_InputField _pseudoInput; // Champ de texte pour entrer le pseudo du joueur.
    [SerializeField] Button _button; // Bouton pour enregistrer le pseudo du joueur.

    // Permet d'accéder aux champs de texte d'une ligne.
    public TMP_Text numero { get => _numero; set => _numero = value; }
    public TMP_Text score { get => _score; set => _score = value; }
    public TMP_Text pseudo { get => _pseudo; set => _pseudo = value; }

}
