using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DonneesJoyaux", menuName = "Joyaux")]
public class SOJoyaux : ScriptableObject
{
    UnityEvent _joyauxTombe = new UnityEvent(); // Événement déclenché lors de la mise à jour des données.
    public UnityEvent joyauxTombe => _joyauxTombe; // Propriété d'accès à l'événement de mise à jour.

}
