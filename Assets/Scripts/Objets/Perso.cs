using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.Animations;
using UnityEngine.UIElements;

/// <summary>
/// Classe qui contrôle les déplacements du personnage.
/// Auteurs du code: Louis Cantin et Olivier Dion
/// Auteur des commentaires: Louis Cantin
/// </summary>

public class Perso : EtresVivants
{
    // # synthese Louis
    [SerializeField] BrasTirer _bras; // Référence au script de tir du personnage.
    [SerializeField] BrasTirer _brasIdle; // Référence au script de tir du personnage.
    [SerializeField] SONavigation _maNavigation; // Référence au script de navigation vers la prochaine scène.
    [SerializeField] ParticleSystem _partMort; // Référence au script de navigation vers la prochaine scène.
    [SerializeField] AnimatorOverrideController _animCont;
    public static Perso instance; // Instance statique de la classe.
    bool _peutBouger = true; // Indique si le personnage peut bouger.
    bool _aCle = false; // Indique si le personnage peut bouger.
    bool _estMort = false; // Indique si le personnage est mort.
    //

    [SerializeField] AudioClip _sonSaut; // #TP4 olivier Référence à un son pour la clé.
    [SerializeField] private float _vitesseBoost = 11f; // Vitesse de déplacement de base du personnage.
    [SerializeField] private float _forceSautBase = 120f; // Force du saut de base.
    [SerializeField] private int _nbFramesMax = 10; // Nombre maximal de frames pour le saut.
    [SerializeField] SOPerso _donneesPerso; // Données du personnage.
    private bool _veutSauter = false; // Indique si le joueur veut sauter.
    private bool _peutResauter = false; // Indique si le personnage peut effectuer un double saut.
    private bool _veutResauter = false; // Indique si le personnage veut effectuer un double saut.
    private int _nbFramesRestants = 0; // Nombre de frames restantes pour le double saut.
    private float _axeHorizontal; // Valeur de l'axe horizontal pour le déplacement.
    private Rigidbody2D _rb; // Composant Rigidbody2D du personnage.
    private SpriteRenderer _sr; // Composant SpriteRenderer du personnage.

    // #tp3 Louis
    [SerializeField] private float _gravite = -20f; // Valeur de gravité de base.
    [SerializeField] ParticleSystem _part; // Système de particules du personnage.
    private bool _peutChangerGravite = true; // Indique si le personnage a le droit de changer la gravité actuellement.
    private float _vitesse; // Vitesse du personnage
    private float _forceSaut; // Force du saut du personnage
    private int _rot = 0; // Rotation du personnage
    private int _vitesseRot = 10; // Vitesse de rotation du personnage
    private Animator _anim; // Composant Animator du personnage.
    ParticleSystem.MainModule _main; // Module principal du système de particules.

    // #tp4 Louis
    [SerializeField] SOScore _donneesScore; // Données du personnage.
    Transform _conteneurPart; // Conteneur des particules de déplacement.
    bool _aToucheSol = false;
    float _velociteY;
    public bool aToucheSol { get => _aToucheSol; set => _aToucheSol = value; }
    public bool veutSauter { get => _veutSauter; set => _veutSauter = value; }
    public float forceSaut { get => _forceSaut; set => _forceSaut = value; }
    public float forceSautBase { get => _forceSautBase; set => _forceSautBase = value; }
    public SOPerso donneesPerso { get => _donneesPerso; set => _donneesPerso = value; }
    public bool peutBouger { get => _peutBouger; set => _peutBouger = value; }
    public float vitesse { get => _vitesse; set => _vitesse = value; }
    public float axeHorizontal { get => _axeHorizontal; set => _axeHorizontal = value; }
    public bool aCle { get => _aCle; set => _aCle = value; }
    public SOScore donneesScore { get => _donneesScore; set => _donneesScore = value; }

    bool aJouerSonSaut = false; // #TP4 Olivier Indique si le son du saut a été joué.


    private void Awake()
    {
        // # synthese Louis
        if (instance == null) instance = this; // Affectation de l'instance si elle est nulle.
        else Destroy(gameObject); // Destruction de l'objet si l'instance existe déjà.

        vitesse = vitesseBase; // #synthese Olivier Ajuste la vitesse pour la gravité normale
        forceSaut = _forceSautBase; // #synthese Olivier Ajuste la force de saut pour la gravité normale

        // #tp4 Louis
        _conteneurPart = new GameObject("ParticulesDeplacement").transform;
        // #tp3 Louis
        _main = _part.main; // Récupération du module principal du système de particules.
        _anim = GetComponent<Animator>(); // Récupération du composant Animator.

        _rb = GetComponent<Rigidbody2D>(); // Récupération du composant Rigidbody2D.
        _sr = GetComponent<SpriteRenderer>(); // Récupération du composant SpriteRenderer.

        // #tp3 Louis
        ReinitialiserNiveau(); // Appel de la méthode pour réinitialiser le niveau.
    }

    /// <summary>
    /// Permet de déplacer le personnage horizontalement lorsqu'une touche de déplacement est appuyée.
    /// </summary>
    /// <param name="value">Boutons de déplacement appuyé ou pas.</param>
    private void OnMove(InputValue value)
    {
        _axeHorizontal = value.Get<Vector2>().x; // Récupération de la valeur de l'axe horizontal.
        // # synthese Louis
        Vector3 scale = transform.localScale;
        if (graviteEstInversee)
        {
            if (_axeHorizontal > 0)
            {
                if (scale.x < 0) transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
                else transform.localScale = scale;
    }
            else if (_axeHorizontal < 0)
            {
                if (scale.x < 0) transform.localScale = scale;
                else transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
            }
        }
        else
        {
            if (_axeHorizontal < 0)
            {
                if (scale.x < 0) transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
                else transform.localScale = scale;
            }
            else if (_axeHorizontal > 0)
            {
                if (scale.x < 0) transform.localScale = scale;
                else transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
            }
        }
        //
    }

    protected override void FixedUpdate()
    {
        // # synthese Louis
        if (_aCle)
        {
            if (_rb.velocity.x == 0 && _rb.velocity.y == 0)
            {
                _brasIdle.GetComponent<SpriteRenderer>().enabled = true;
                _bras.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                _brasIdle.GetComponent<SpriteRenderer>().enabled = false;
                _bras.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else
        {
            _brasIdle.GetComponent<SpriteRenderer>().enabled = false;
            _bras.GetComponent<SpriteRenderer>().enabled = false;
        }
        //

        // #tp4 Louis
        if (graviteEstInversee)
        {
            _velociteY = -_rb.velocity.y;
        }
        else
        {
            _velociteY = _rb.velocity.y;
        }

        // #tp3 Louis
        _anim.SetFloat("vitesseX", _rb.velocity.x); // Mise à jour de l'animation de déplacement.
        _anim.SetFloat("vitesseY", _velociteY); // Mise à jour de l'animation de saut / chute.
        _anim.SetBool("estAuSol", _estAuSol); // Mise à jour de l'animation d'état au sol.
        if (_rb.velocity.x > .1 && _estAuSol || _rb.velocity.x < -.1 && _estAuSol) FaireApparaitreParticules();

        base.FixedUpdate(); // Appel de la méthode FixedUpdate de la classe de base.

        // # synthese Louis
        // Mise à jour de la vélocité du Rigidbody en fonction de l'axe horizontal.
        if (_peutBouger) _rb.velocity = new Vector2(_axeHorizontal * _vitesse, _rb.velocity.y);


        // Vérification de l'état du personnage par rapport au sol et au saut.
        if (_estAuSol && !_veutSauter)
        {
            _nbFramesRestants = _nbFramesMax; // Réinitialisation du nombre de frames restantes pour le saut.
            _peutResauter = true; // Le personnage peut effectuer un double saut.

            // #tp3 Louis
            _peutChangerGravite = true; // Le personnage peut changer la gravité.

            // #tp4 Louis
            _aToucheSol = true;

            aJouerSonSaut = false; // #TP4 Olivier Réinitialisation de la variable pour jouer le son du saut.

        }

        if (!_veutSauter && !_estAuSol)
        {
            if (!_donneesPerso.aDoubleSaut) _nbFramesRestants = 0;

            if (_peutResauter && _donneesPerso.aDoubleSaut)
            {
                _veutResauter = true;
                _nbFramesRestants = _nbFramesMax; // Réinitialisation du nombre de frames restantes pour le double saut.
                _peutResauter = false; // Le personnage ne peut plus effectuer de double saut immédiat.
            }
        }

        if (_veutSauter && _peutBouger)
        {
            if (_veutResauter)
            {
                aJouerSonSaut = false; // Réinitialisation de la variable pour jouer le son du saut.
                _rb.velocity = new Vector2(0, 0); // Annule la vélocité du personnage pour le double saut.
                _veutResauter = false; // Empêche la vélocité d'être annulée plusieurs fois lors du double saut.
            }
            Sauter(); // Appel de la méthode de saut.
        }

        // #tp3 Louis
        if (_donneesPerso.aPouvoirGravite)
        {
            GererGravite(); // Gestion du changement de gravité si le personnage a ce pouvoir.
        }
    }

    /// <summary>
    /// Lorsque le bouton pour faire sauter le personnage (barre d'espacement / bouton du sud sur manette) est appuyé.
    /// </summary>
    /// <param name="value">Bouton de saut appuyé ou pas.</param>
    private void OnJump(InputValue value) => _veutSauter = value.isPressed; // Met à jour l'état de saut en fonction de l'input.

    /// <summary>
    /// Effectue un saut en appliquant une force proportionnelle au nombre de frames restantes.
    /// </summary>
    private void Sauter()
    {
        if (!aJouerSonSaut)
        {
            GestAudio.instance.JouerSon(_sonSaut); // #TP4 Olivier son quand le personnage saute.
            aJouerSonSaut = true; // #TP4 Olivier Empêche de jouer le son du saut plusieurs fois.
        }
        float fractionForce = (float)_nbFramesRestants / _nbFramesMax; // Calcul de la fraction de force pour le saut.
        Vector2 vecteurForce = Vector2.up * _forceSaut * fractionForce; // Calcul du vecteur de force pour le saut.
        _rb.AddForce(vecteurForce); // Application de la force au Rigidbody.

        _nbFramesRestants = (_nbFramesRestants > 0) ? _nbFramesRestants - 1 : 0; // Décrémentation des frames restantes pour le double saut.
    }

    // Méthodes pour gérer le changement de gravité du personnage.
    // #tp3 Louis
    private void GererGravite()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            ChangerGravite(); // Appel de la méthode pour changer la gravité si la touche Q est enfoncée.
        }
        FaireRotationGravite(); // Appel de la méthode pour faire la rotation liée à la gravité.
    }

    // Méthode permettant de changer la gravité
    // #tp3 Louis
    private void ChangerGravite()
    {
        if (_peutChangerGravite)
        {
            _gravite = -_gravite; // Inversion de la gravité.
            _distanceSol = -_distanceSol; // Inversion de la distance entre l'objet et le sol.
            graviteEstInversee = !graviteEstInversee; // Inversion du statut de la gravité inversée.

            // #synthese Louis
            _donneesPerso.changementGravite.Invoke();
            //

            // Inversion de la vitesse et de la force de saut si la gravité est inversée.
            if (graviteEstInversee) { _forceSaut = -_forceSaut; }

            // Réinitialisation des variables si la gravité n'est pas inversée.
            else ReinitialiserVariables();

            _peutChangerGravite = false; // Empêche de changer la gravité plusieurs fois d'affilée.
        }
        Physics2D.gravity = new(0, _gravite); // Application de la gravité modifiée.
    }

    // Méthode permettant de réinitialiser le niveau
    // #tp3 Louis
    private void ReinitialiserNiveau()
    {
        _donneesPerso.GererInventaire(); // Gestion de l'inventaire du personnage.
        _main.startColor = new Color(1, 1, 1); // Réinitialisation de la couleur des particules.
        Physics2D.gravity = new(0, _gravite); // Réinitialisation de la gravité.
        ReinitialiserVariables(); // Réinitialisation des variables de vitesse et de force de saut.
    }

    // Méthode permettant de faire la rotation du personnage lors du changement gravité
    // #tp3 Louis
    private void FaireRotationGravite()
    {
        // Fait tourner le personnage lorsqu'il y a inversion de gravité.
        if (graviteEstInversee && _rot <= 180)
        {
            transform.rotation = Quaternion.Euler(0, 0, _rot);
            _rot += _vitesseRot;
        }
        else if (!graviteEstInversee && _rot >= 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, _rot);
            _rot -= _vitesseRot;
        }
    }

    // Méthode permettant de réinitialiser les variables de vitesse et de force de saut
    // #tp3 Louis
    private void ReinitialiserVariables()
    {
        _vitesse = vitesseBase; // Réinitialisation de la vitesse.
        _forceSaut = _forceSautBase; // Réinitialisation de la force de saut.
    }

    void OnApplicationQuit()
    {
        _donneesPerso.Initialiser(); // Réinitialisation des données du personnage lorsque l'application se ferme.
    }

    // Méthode permettant de faire apparaitre les particules lors du déplacement du personnage
    // #tp3 Louis
    void FaireApparaitreParticules()
    {
        // Fait apparaître des particules lorsque le personnage est en mouvement.
        Vector3 posPart = transform.position;
        posPart.y -= _distanceSol;
        // posPart.z = transform.position.z - 1;
        Instantiate(_part, posPart, Quaternion.identity, _conteneurPart.transform);
    }

    // #tp4 Louis
    /// <summary>
    /// Change la couleur des particules lorsque le personnage utilise un pouvoir.
    /// </summary>
    /// <param name="couleur">Couleur des particules</param>
    public void ChangerCouleurParticules(float couleur)
    {
        // Change la couleur des particules lorsque le personnage utilise un pouvoir.
        _main.startColor = new Color(couleur, couleur, couleur);
    }

    public void DebutBoost()
    {
        StartCoroutine(CoroutineAjusterVitesse()); // #TP4 Olivier Start la coroutine pour activer le boost
    }

    /// <summary>
    /// compteur de 5 seconde pour activer et désactiver le boost de vitesse et de saut
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineAjusterVitesse()
    {
        Debug.Log("Bonus récupéré"); // #TP3 Olivier affiche un message dans la console
        GestAudio.instance.DemarerCoroutineMusical(TypePiste.MusiqueEventA, 0.2f, true); // #TP4 Olivier Joue la musique d'événement A
        _vitesse = _vitesseBoost; // #TP3 Olivier Ajuste la vitesse pour la gravité normale

        yield return new WaitForSeconds(5); // #TP3 Olivier Attend 5 secondes

        Debug.Log("Bonus terminé"); // #TP3 Olivier affiche un message dans la console
        GestAudio.instance.DemarerCoroutineMusical(TypePiste.MusiqueEventA, 0.3f, false); // #TP4 Olivier Joue la musique d'événement A
        _vitesse = vitesseBase; // #TP3 Olivier Ajuste la vitesse après le compteur pour la gravité normale
        StopCoroutine(CoroutineAjusterVitesse()); // #TP3 Olivier Arrête la coroutine
    }

    // #synthese Louis
    public void FinirPartie(float vitesseRotRandom, float reductionVelocitePerso)
    {
        _peutBouger = false;
        _rb.velocity *= reductionVelocitePerso;
        transform.Rotate(0, 0, vitesseRotRandom);
    }

    public void Mourir()
    {
        float vitessseRotMax = 10f;
        float vitessseRotMin = 5f;
        float vitesseRotRandom;
        int directionRotation;
        float reductionVelocitePerso = .95f;

        vitesseRotRandom = Random.Range(vitessseRotMin, vitessseRotMax); // vitesse de rotation des tresors
        directionRotation = Random.Range(0, 2); // direction de rotation des tresors
        if (directionRotation == 0) vitesseRotRandom = -vitesseRotRandom; // si la direction de rotation est 0, la rotation est negative
        _rb.velocity = new Vector2(0, 0);
        _rb.gravityScale = 0;
        _rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        StartCoroutine(CoroutineMourir(reductionVelocitePerso, vitesseRotRandom));
    }

    IEnumerator CoroutineMourir(float reductionVelocitePerso, float vitesseRotRandom)
    {
        _peutBouger = false;
        float duree = 30;
        Physics2D.gravity = new Vector2(0, 0);

        for (int i = 0; i < duree; i++)
        {
            _rb.velocity *= reductionVelocitePerso;
            transform.Rotate(0, 0, vitesseRotRandom);
            yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(1);
        Instantiate(_partMort, transform.position, Quaternion.identity);
        _sr.enabled = false;
        _bras.GetComponent<SpriteRenderer>().enabled = false;
        _brasIdle.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2);
        _maNavigation.AllerPanneauFin();
    }

    public void ChangerAnim()
    {
        _anim.runtimeAnimatorController = _animCont;
        aCle = true;
    }

    public void AjouterPts(int points)
    {
        donneesScore.score += points;
    }

    public void AjouterArgent(int argent)
    {
        donneesPerso.argent += argent;
    }

    public void ActiverInvulnerabilite()
    {
        StartCoroutine(CoroutineInvulnerabilite());
    }

    IEnumerator CoroutineInvulnerabilite()
    {
        float duree = 1.5f;

        tag = "PersoInvincible";
        _sr.color = new Color(1, 1, 1, .5f);
        yield return new WaitForSeconds(duree);
        tag = "Perso";
        _sr.color = new Color(1, 1, 1, 1);
    }
    //
}
