using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #synthese olivier Classe de l'ennemi Olivier.
public class EnnemiOlivier : EtresVivants
{
    [SerializeField] ParticleSystem _partMort; // Particules de mort de l'ennemi.
    [SerializeField] ParticleSystem _partDeplacement; // Particules de déplacement de l'ennemi.
    [SerializeField] Retroaction _retroModele; // Modèle de rétroaction.
    [SerializeField] int points = 150; // Points attribués à l'ennemi.

    [SerializeField] Perso _perso; // Personnage joueur.

    Rigidbody2D _rb; // Rigidbody de l'ennemi.
    SpriteRenderer _sr; // SpriteRenderer de l'ennemi.
    bool _peutBouger = false; // Indique si le personnage peut bouger.
    int _axeHorizontal; // Axe horizontal de l'ennemi.

    public int axeHorizontal { get => _axeHorizontal; set => _axeHorizontal = value; }
    public bool peutBouger { get => _peutBouger; set => _peutBouger = value; } // Propriété de _peutBouger.

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); // Récupération du Rigidbody de l'ennemi.
        _sr = GetComponent<SpriteRenderer>(); // Récupération du SpriteRenderer de l'ennemi.
    }


    protected override void FixedUpdate()
    {
        if (_peutBouger) DeplacerEnnemi(); // Si l'ennemi peut bouger, il se déplace.
        if (Physics2D.gravity.y > 0) graviteEstInversee = true; // Si la gravité est inversée, la valeur de graviteEstInversee est égale à true.
        else graviteEstInversee = false; // Si la gravité n'est pas inversée, la valeur de graviteEstInversee est égale à false.
        if (graviteEstInversee) // Si la gravité est inversée.
        {
            _sr.flipY = true; // Si la gravité est inversée, le sprite se retourne.
        }
        else if (graviteEstInversee == false) // Si la gravité n'est pas inversée, le sprite ne se retourne pas.
        {
            _sr.flipY = false; // Si la gravité n'est pas inversée, le sprite ne se retourne pas.
        }
    }

    /// <summary>
    /// Fonction qui permet à l'ennemi de regarder le joueur.
    /// </summary>
    /// <param name="posJoueurX"></param>
    public void RegarderJoueur(float posJoueurX)
    {
        if (posJoueurX > transform.position.x + .1f) _axeHorizontal = 1; // Récupération de la valeur de l'axe horizontal.
        else if (posJoueurX < transform.position.x - .1f) _axeHorizontal = -1; // Récupération de la valeur de l'axe horizontal.
        else _axeHorizontal = 0; // Si la valeur est égale à 0, l'ennemi ne bouge pas.
    }

    /// <summary>
    /// Fonction qui permet à l'ennemi de perdre de la vie.
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Perso")) // Si l'ennemi entre en collision avec le joueur.
        {
            other.gameObject.GetComponent<Perso>().PerdreVie(degats, other.gameObject); // Perte de vie du joueur.
        }
    }

    /// <summary>
    /// Fonction qui permet à l'ennemi de se déplacer.
    /// </summary>
    void DeplacerEnnemi()
    {
        if (graviteEstInversee) _sr.flipX = (axeHorizontal < 0) ? true : (axeHorizontal > 0) ? false : _sr.flipX; // Si la gravité est inversée, le sprite se retourne.
        else _sr.flipX = (axeHorizontal < 0) ? true : (axeHorizontal > 0) ? false : _sr.flipX; // Si la gravité n'est pas inversée, le sprite ne se retourne pas.
        _rb.velocity = new Vector2(_axeHorizontal * vitesseBase, _rb.velocity.y); // Déplacement de l'ennemi.
        if (_rb.velocity.x > 0) _sr.flipX = false; // Si la vitesse est supérieure à 0, le sprite ne se retourne pas.
        else if (_rb.velocity.x < 0) _sr.flipX = true; // Si la vitesse est inférieure à 0, le sprite se retourne.
    }

    public void Mourir()
    {
        _perso.AjouterPts(points); // Ajout des points au joueur.
        Vector3 pos = transform.position; // Position de l'ennemi.
        pos.y += 1; // Position de l'ennemi.
        Retroaction retro = Instantiate(_retroModele, pos, Quaternion.identity, transform.parent); // instancie le gameobject retroaction
        retro.ChangerTexte($"+ {points}pts"); // affiche le texte
        Instantiate(_partMort, transform.position, Quaternion.identity);
        Destroy(gameObject); // Destruction de l'ennemi.
    }
}