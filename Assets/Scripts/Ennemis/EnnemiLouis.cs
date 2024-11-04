using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiLouis : EtresVivants
{
    [SerializeField] private float _forceSaut; // Force du saut du personnage
    [SerializeField] ParticleSystem _partMort;
    [SerializeField] ParticleSystem _partDeplacement;
    [SerializeField] Retroaction _retroModele;
    [SerializeField] int points = 50; // Points attribués à l'ennemi.
    Rigidbody2D _rb;
    SpriteRenderer _sr;
    Animator _anim; // Animator de l'ennemi.
    Perso _perso; // Personnage joueur.
    bool _peutSauter = true; // Indique si le personnage peut bouger.
    int _axeHorizontal; // Axe horizontal de l'ennemi.
    float _posDebut;
    float _posFin;
    private int _rot = 0; // Rotation du personnage
    private int _vitesseRot = 10; // Vitesse de rotation du personnage
    public int axeHorizontal { get => _axeHorizontal; set => _axeHorizontal = value; }

    // Start is called before the first frame update
    void Start()
    {

        Perso.instance.donneesPerso.changementGravite.AddListener(InverserSaut);
        axeHorizontal = Random.Range(0, 2) * 2 - 1;
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        StartCoroutine(CoroutineDeterminerSiEnnemiBouge());

        if(Physics2D.gravity.y > 0) graviteEstInversee = true;
        if(graviteEstInversee){
            _sr.flipY = true;
            _forceSaut *= -1;
        }
    }

    protected override void FixedUpdate()
    {
        DeplacerEnnemi();
        _anim.SetFloat("VelociteY", _rb.velocity.y);
    }

    public void RegarderJoueur(float posJoueurX)
    {
        if (posJoueurX > transform.position.x + .1f) axeHorizontal = 1; // Récupération de la valeur de l'axe horizontal.
        else if (posJoueurX < transform.position.x - .1f) axeHorizontal = -1;
    }

    public void Sauter()
    {
        if (_peutSauter) _rb.AddForce(Vector2.up * _forceSaut, ForceMode2D.Impulse);
        _peutSauter = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Perso"))
        {
            other.gameObject.GetComponent<Perso>().PerdreVie(degats, other.gameObject);
            Mourir();
        }

        if (other.gameObject.CompareTag("TuileSol"))
        {
            _peutSauter = true;
        }
    }

    void DeplacerEnnemi()
    {
        if (graviteEstInversee) _sr.flipX = (axeHorizontal < 0) ? true : (axeHorizontal > 0) ? false : _sr.flipX;
        else _sr.flipX = (axeHorizontal < 0) ? true : (axeHorizontal > 0) ? false : _sr.flipX;

        _rb.velocity = new Vector2(axeHorizontal * vitesseBase, _rb.velocity.y);
    }

    IEnumerator CoroutineDeterminerSiEnnemiBouge()
    {
        while (true)
        {
            _posDebut = transform.position.x;
            yield return new WaitForSeconds(.5f);
            _posFin = transform.position.x;
            if (_posDebut == _posFin) axeHorizontal *= -1;
        }
    }

    public void Mourir()
    {
        Perso.instance.AjouterPts(points);
        Vector3 pos = transform.position;
        pos.y += 1;
        Retroaction retro = Instantiate(_retroModele, pos, Quaternion.identity, transform.parent); // instancie le gameobject retroaction
        retro.ChangerTexte($"+ {points}pts"); // affiche le texte
        Instantiate(_partMort, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void InverserSaut()
    {
        _sr.flipY = !_sr.flipY;
        _forceSaut *= -1;
    }
}