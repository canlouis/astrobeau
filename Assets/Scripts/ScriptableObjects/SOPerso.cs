using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ScriptableObject représentant les données du personnage
/// Auteur du code: Louis Cantin
/// Auteur des commentaires: Louis Cantin
/// </summary>

// ScriptableObject contenant les données d'un personnage joueur.
[CreateAssetMenu(fileName = "DonneesPerso", menuName = "Perso")]
public class SOPerso : ScriptableObject
{
    // #synthese Louis
    UnityEvent _permettreTirer = new UnityEvent(); // Événement déclenché lors de la mise à jour des données.
    public UnityEvent permettreTirer => _permettreTirer; // Propriété d'accès à l'événement de mise à jour.
    UnityEvent _changementGravite = new UnityEvent(); // Événement déclenché lors de la mise à jour des données.
    public UnityEvent changementGravite => _changementGravite; // Propriété d'accès à l'événement de mise à jour.
    //

    // #tp4 Louis
    UnityEvent _evenementMiseAJour = new UnityEvent(); // Événement déclenché lors de la mise à jour des données.
    public UnityEvent evenementMiseAJour => _evenementMiseAJour; // Propriété d'accès à l'événement de mise à jour.
    UnityEvent _bonusArgentDebut = new UnityEvent(); // Événement déclenché lors de l'ajout d'argent.
    public UnityEvent bonusArgentDebut => _bonusArgentDebut; // Propriété d'accès à l'événement de bonus d'argent.
    UnityEvent _bonusArgentFin = new UnityEvent(); // Événement déclenché lors de la fin de l'ajout d'argent.
    public UnityEvent bonusArgentFin => _bonusArgentFin; // Propriété d'accès à l'événement de fin de bonus d'argent.
    UnityEvent _activerBonus = new UnityEvent(); // Événement déclenché lors de l'activation d'un bonus.
    public UnityEvent activerBonus => _activerBonus; // Propriété d'accès à l'événement d'activation de boSnus.

    UnityEvent _desactiverBonus = new UnityEvent(); // Événement déclenché lors de la désactivation d'un bonus.
    public UnityEvent desactiverBonus => _desactiverBonus; // Propriété d'accès à l'événement de désactivation de bonus.
    UnityEvent _bonusBoostDebut = new UnityEvent(); // Événement déclenché lors de l'ajout de boost.
    public UnityEvent bonusBoostDebut => _bonusBoostDebut; // Propriété d'accès à l'événement de bonus de boost.
    UnityEvent _ouvrirPorte = new UnityEvent(); // Événement déclenché lors de l'ouverture d'une porte.
    public UnityEvent ouvrirPorte => _ouvrirPorte; // Propriété d'accès à l'événement d'ouverture de porte.

    // Les valeurs initiales du personnage.
    [Header("Les valeurs initiales: ")]
    [SerializeField][Range(1, 5)] int _niveauIni = 1; // Le niveau initial du personnage.
    [SerializeField][Range(0, 500)] int _argentIni = 0; // L'argent initial du personnage.

    // Les valeurs actuelles du personnage.
    [Header("Les valeurs actuelles: ")]
    [SerializeField][Range(1, 5)] int _niveau = 1; // Le niveau actuel du personnage.
    [SerializeField][Range(0, 500)] int _argent = 0; // L'argent actuel du personnage.
    [SerializeField] int _tempsAimant = 5; // #TP4 Olivier Temps restant de l'aimant
    [SerializeField] bool _aAimant = false; // #synthese Olivier intique si l'aimant est acheté ou non
    [SerializeField] bool _aPouvoirGravite = false; // #synthese Olivier intique si le pouvoir de gravité est acheté ou non
    [SerializeField] bool _aDoubleSaut = false; // #synthese Olivier intique si le double saut est acheté ou non

    [SerializeField] bool _activateurActif = false; // #TP4 olivier liens avec l'activateur dans l'inspecteur



    // Propriétés d'accès aux valeurs actuelles du personnage.
    public int niveau
    {
        get => _niveau;
        set
        {
            _niveau = Mathf.Clamp(value, 1, int.MaxValue); // Assure que le niveau reste dans les bornes.
            _evenementMiseAJour.Invoke(); // Déclenche l'événement de mise à jour.
        }
    }

    public int argent
    {
        get => _argent;
        set
        {
            _argent = Mathf.Clamp(value, 0, int.MaxValue); // Assure que l'argent reste dans les bornes.
            _evenementMiseAJour.Invoke(); // Déclenche l'événement de mise à jour.
        }
    }

    public int tempsAimant // #TP4 Olivier Temps restant de l'aimant
    {
        get => _tempsAimant; // #TP4 Olivier recupere le temps restant de l'aimant
        set
        {
            _tempsAimant = Mathf.Clamp(value, 0, int.MaxValue); // #TP4 Olivier Assure que le temps restant de l'aimant reste dans les bornes.
            _evenementMiseAJour.Invoke(); // #TP4 Olivier Déclenche l'événement de mise à jour.
        }
    }

    public bool activateurActif    //tp4 olivier
    {
        get => _activateurActif;    //tp4 olivier recupere l'activateur

        set => _activateurActif = value;    //tp4 olivier donne l'activateur
    }


    // public List<SOObjet> _lesObjets = new List<SOObjet>(); // Liste des objets possédés par le personnage.
    Dictionary<SOObjet, int> _dObjets = new Dictionary<SOObjet, int>();
    public Dictionary<SOObjet, int> dObjets { get => _dObjets; set => _dObjets = value; }

    float _facteurPrixIni = 1; // Facteur de prix initial.
    float _facteurPrix = 1f; // Facteur de prix actuel du joueur.
    public float facteurPrix
    {
        get => _facteurPrix;
        set
        {
            _facteurPrix = Mathf.Clamp(value, 0, int.MaxValue); // Assure que le facteur de prix reste dans les bornes.
            _evenementMiseAJour.Invoke(); // Déclenche l'événement de mise à jour.
        }
    }
    public bool aAimant
    {
        get => _aAimant;
        set => _aAimant = value;
    }
    public bool aPouvoirGravite
    {
        get => _aPouvoirGravite;
        set => _aPouvoirGravite = value;
    }
    public bool aDoubleSaut
    {
        get => _aDoubleSaut;
        set => _aDoubleSaut = value;
    }

    float _facteurPrixSiRabais = .9f; // Facteur de prix en cas de rabais.

    // Initialise les valeurs du personnage.
    public void Initialiser()
    {
        _niveau = _niveauIni; // Réinitialise le niveau.
        _argent = _argentIni; // Réinitialise l'argent.
        ViderInventaire(); // Vide l'inventaire.
        _facteurPrix = _facteurPrixIni; // Réinitialise le facteur de prix.
    }

    public void ViderInventaire()
    {
        _dObjets.Clear();
        aAimant = false;
        aPouvoirGravite = false;
        aDoubleSaut = false;
    }

    public void GererInventaire()
    {
        // Réinitialisation des pouvoirs du personnage en fonction des objets collectés.
        foreach (KeyValuePair<SOObjet, int> objet in dObjets)
        {
            if (objet.Key.name == "ObjGravite") aPouvoirGravite = true;
            if (objet.Key.name == "ObjDoubleSaut") aDoubleSaut = true;
            if (objet.Key.name == "ObjAimant") aAimant = true;
        }
        // _lesObjets.Clear(); // Efface la liste des objets possédés.
        _dObjets.Clear();
        _facteurPrix = _facteurPrixIni; // Réinitialise le facteur de prix.
    }

    // Permet au personnage d'acheter un objet.
    public void Acheter(SOObjet donneesObjet)
    {
        // Vérifie si le personnage a le niveau et l'argent nécessaires pour acheter l'objet.
        if (_niveau >= donneesObjet.niveauRequis && _argent >= donneesObjet.prix)
        {
            argent -= donneesObjet.prix; // Déduit le prix de l'objet de l'argent du personnage.
            if (donneesObjet.donneDroitRabais) facteurPrix = _facteurPrixSiRabais; // Applique un rabais si nécessaire.
            // _lesObjets.Add(donneesObjet); // Ajoute l'objet à la liste des objets possédés.
            if (_dObjets.ContainsKey(donneesObjet)) _dObjets[donneesObjet]++;
            else _dObjets.Add(donneesObjet, 1);
            // _lesObjets.Sort((x,y) => x.nom.CompareTo(y.nom)); // Place les objets par ordre alphabétique
            AfficherInventaire(); // Affiche l'inventaire mis à jour.
        }
    }

/// <summary>
/// Retire un objet de l'inventaire.
/// </summary>
/// <param name="donneesObjet"></param>
    public void RetirerObjet(SOObjet donneesObjet)
    {
        _dObjets.Remove(donneesObjet); //#Tp synthese OlivierRetire l'objet de la liste des objets possédés.
        AfficherInventaire(); //#Tp synthese Olivier Affiche l'inventaire mis à jour.
    }

    // Affiche l'inventaire du personnage dans la console.
    public void AfficherInventaire()
    {
        if (Boutique.instance == null) return;
        Boutique.instance.panneauInventaire.Vider();
        PanneauVignette panneauVignettePrecedente = null;
        // foreach (SOObjet objet in _lesObjets)
        foreach (KeyValuePair<SOObjet, int> objet in _dObjets)
        {
            panneauVignettePrecedente = Boutique.instance.panneauInventaire.Ajouter(objet.Key); // Ajoute un objet à l'inventaire.
            panneauVignettePrecedente.nb = objet.Value; // Affiche le nombre d'objets possédés.
        }
    }

    // Appelée lors de la validation des données dans l'éditeur Unity.
    void OnValidate()
    {
        _evenementMiseAJour.Invoke(); // Déclenche l'événement de mise à jour.
    }
}
