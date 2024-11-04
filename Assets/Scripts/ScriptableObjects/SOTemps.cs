using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject pour le temps.
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>
[CreateAssetMenu(fileName = "Temps", menuName = "Temps")]
public class SOTemps : ScriptableObject
{
    [SerializeField] int _tempsBase = 90;
    int _temps;
    int _tempsRestant;
    float _tempsTrouNoir;
    public int temps { get => _temps; set => _temps = value;}
    public int tempsRestant { get => _tempsRestant; set => _tempsRestant = value; }
    public float tempsTrouNoir { get => _tempsTrouNoir; set => _tempsTrouNoir = value; }

    public void ReinitialiserTemps(){
        _temps = _tempsBase;
    }
}
