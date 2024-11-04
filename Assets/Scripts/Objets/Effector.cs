using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TP4 - Louis
/// Classe permettant de gérer les trapolines (effectors)
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>
public class Effector : DetecteurSol
{
    [SerializeField] ParticleSystem _part;
    AreaEffector2D _effector;
    Perso _perso;
    Rigidbody2D _rbPerso;
    Animator _anim;
    int _retourTrampolineBase = 10; // Force de retour du trampoline
    int _retourTrampoline = 10; // Force de retour du trampoline
    int _factionForceRebond = 3; // Facteur de réduction de la force de rebond
    float _velociteMinRebond = -6f; // Vélocité minimale pour déclencher un rebond
    float _multiplicateurSautTrampoline = 2.2f; // Multiplicateur de la force de saut du personnage
    float _distanceParticules = .5f; // Distance entre les particules et le sol


    void Start()
    {
        _anim = GetComponent<Animator>();
        _effector = GetComponent<AreaEffector2D>();
        _effector.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        _perso = other.GetComponent<Perso>(); // Récupération du composant "Perso" attaché à l'objet entrant
        _rbPerso = other.GetComponent<Rigidbody2D>(); // Récupération du composant Rigidbody2D attaché à l'objet entrant

        if (other.CompareTag("Perso")) // Vérifie si l'objet entrant a le tag "Perso"
        {
            if (_rbPerso.velocity.y < _velociteMinRebond) // Vérifie si la vélocité verticale de l'objet est inférieure à la vélocité minimale pour déclencher un rebond
            {
                _anim.SetTrigger("Rebond"); // Déclenchement de l'animation de rebond
                _perso.forceSaut = 0; // Réinitialisation de la force de saut du personnage à zéro
                Vector2 pos = other.transform.position; // Position de l'objet entrant
                pos.y -= _distanceSol + _distanceParticules; // Ajustement de la position pour placer les particules sous l'objet
                Instantiate(_part, pos, Quaternion.identity); // Instanciation du système de particules à la position ajustée
                _effector.enabled = true; // Activation de l'effet de zone
                if (_perso.aToucheSol) // Vérifie si le personnage touche déjà le sol
                {
                    _effector.forceMagnitude = _rbPerso.velocity.y * -_retourTrampoline; // Applique une force de retour du trampoline basée sur la vélocité verticale de l'objet entrant
                    _perso.aToucheSol = !_perso.aToucheSol; // Indique que le personnage n'est plus en contact avec le sol
                }
                if (_perso.veutSauter)
                { // Vérifie si le personnage veut sauter
                    _effector.forceMagnitude = _rbPerso.velocity.y * -_retourTrampoline * _multiplicateurSautTrampoline; // Applique une force de rebond avec un multiplicateur si le personnage veut sauter
                }
                else _effector.forceMagnitude -= _effector.forceMagnitude / _factionForceRebond; // Réduit la force de rebond s'il n'y a pas de saut demandé
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Perso"))
        { // Vérifie si l'objet sortant a le tag "Perso"
            _perso = other.GetComponent<Perso>();
            _perso.forceSaut = _perso.forceSautBase; // Réinitialisation de la force de saut du personnage à sa valeur de base
            _effector.enabled = false; // Désactivation de l'effet de zone
        }
    }
}
