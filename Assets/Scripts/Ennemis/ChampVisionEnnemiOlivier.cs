using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #synthese Olivier Classe du champ de vision de l'ennemi Olivier.
public class ChampVisionEnnemiOlivier : MonoBehaviour
{
    [SerializeField] EnnemiOlivier _ennemiOlivier; // #synthese Olivier Référence à l'ennemi.
    [SerializeField] float _rayonChampVision; // #synthese Olivier Rayon du champ de vision.
    CircleCollider2D _cc; // #synthese Olivier Collider du champ de vision.
    bool _peutRegarder = true; // #synthese Olivier Indique si l'ennemi peut regarder.
    float _tempDeReaction = .5f; // #synthese Olivier Temps de réaction de l'ennemi.





    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _cc = gameObject.GetComponent<CircleCollider2D>(); // #synthese Olivier Récupère le collider du champ de vision.
        _cc.radius = _rayonChampVision; // #synthese Olivier Ajuste le rayon du champ de vision.

    }

    /// <summary>
    /// Fonction qui permet de détecter si le joueur est dans le champ de vision.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Perso")) // #synthese Olivier Si le joueur est dans le champ de vision.
        {
            _ennemiOlivier.peutBouger = true; // #synthese Olivier L'ennemi peut bouger.
        }

    }

    /// <summary>
    /// Fonction qui permet à l'ennemi de regarder le joueur.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay2D(Collider2D other)
    {
        // #synthese Olivier Si le joueur est dans le champ de vision.
        if (other.CompareTag("Perso"))
        {
            // #synthese Olivier Si l'ennemi peut regarder.
            if (_peutRegarder)
            {
                _ennemiOlivier.RegarderJoueur(other.transform.position.x); // #synthese Olivier L'ennemi regarde le joueur.
                _peutRegarder = false; // #synthese Olivier L'ennemi ne peut plus regarder.
                StartCoroutine(CoroutineRegarder()); // #synthese Olivier Coroutine pour permettre à l'ennemi de regarder.
            }
        }

    }

    /// <summary>
    /// Fonction qui permet de détecter si le joueur n'est plus dans le champ de vision.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {
        // #synthese Olivier Si le joueur n'est plus dans le champ de vision.
        if (other.CompareTag("Perso"))
        {
            _ennemiOlivier.peutBouger = false; // #synthese Olivier L'ennemi ne peut plus bouger.
        }
    }

    /// <summary>
    /// Coroutine pour permettre à l'ennemi de regarder.
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineRegarder()
    {
        yield return new WaitForSeconds(_tempDeReaction); // #synthese Olivier Attend le temps de réaction.
        _peutRegarder = true; // #synthese Olivier L'ennemi peut regarder.
    }

}
