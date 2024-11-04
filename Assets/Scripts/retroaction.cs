using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Retroaction : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _champPoints;
    [SerializeField] TextMeshProUGUI _tempsAimant;

    public void ChangerTexte(string texte)
    {
        _champPoints.text = texte;
    }
    public void ChangerTexteAimantActif(string texte)
    {
        _tempsAimant.color = new Color(1, 0, 0, 1); // #TP3 Olivier le texte est de couleur rouge
        _tempsAimant.text = texte;
    }
    public void ChangerTexteAimantInactif(string texte)
    {
        _tempsAimant.color = new Color(0, 1, 0, 1); // #TP3 Olivier la couleur du texte est vert
        _tempsAimant.text = texte;
    }

    public void Detruire()
    {
        Destroy(gameObject);
    }
}
