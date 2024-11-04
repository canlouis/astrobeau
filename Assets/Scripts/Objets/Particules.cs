using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe contrôlant le comportement des particules.
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>
public class Particules : MonoBehaviour
{
    ParticleSystem _par; // Référence au composant ParticleSystem attaché à cet objet.
    ParticleSystem.EmissionModule _emission; // Module de gestion des émissions de particules.
    ParticleSystem.MainModule _main; // Module principal de gestion des particules.

    void Start()
    {
        _par = GetComponent<ParticleSystem>(); // Récupération du composant ParticleSystem attaché à cet objet.
        _emission = _par.emission; // Récupération du module de gestion des émissions de particules.
    }

    /// <summary>
    /// Méthode appelée lorsqu'un autre collider entre en collision avec cet objet.
    /// </summary>
    /// <param name="other">Collider entrant en collision avec cet objet</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Perso")) // Vérifie si l'objet entrant en collision est de type "Perso".
        {
            _emission.enabled = false; // Désactive les émissions de particules.
        }
    }
}
