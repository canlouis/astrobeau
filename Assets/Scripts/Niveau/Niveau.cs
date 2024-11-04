using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Classe générant un niveau composé de salles disposées en grille avec des bordures.
/// Auteurs du code: Louis Cantin et Olivier Dion
/// Auteurs des commentaires: Louis Cantin et Olivier Dion
/// </summary>
public class Niveau : MonoBehaviour
{
    // synthese Louis
    [SerializeField] FondFin _fondFin; // Le Tilemap pour afficher le niveau.

    //#tp4 Louis
    static Niveau _instance; // Instance statique du niveau.
    static public Niveau instance => _instance; // Propriété permettant d'accéder à l'instance du niveau.

    [SerializeField] SOPerso _donneesPerso; // Le Tilemap pour afficher le niveau.
    [SerializeField] Tilemap _tilemap; // Le Tilemap pour afficher le niveau.
    [SerializeField] TileBase _tuileModele; // Modèle de tuile pour les bordures du niveau.
    [SerializeField] Salle[] _tSallesModele; // Modèles de salles à instancier.
    [SerializeField] Vector2Int _taille = new(2, 2); // Taille du niveau en nombre de salles.
    [SerializeField] CinemachineVirtualCamera _camera; // Caméra du perso.
    [SerializeField] int _nbTresorsParSalle = 20; // #TP3 Olivier nombre de joyaux par salle.
    [SerializeField] Tresors[] _tTresorsModeles; // #TP3 Olivier tableau pour les joyaux


    List<Vector2Int> _lesPosLibres = new List<Vector2Int>(); // #TP3 Olivier liste des position libre dans le niveau

    [SerializeField] GameObject[] _tBonusModeles; // #TP3 Olivier tableau pour les bonus

    [SerializeField] int _nbBonusParSalle = 1;  // #TP3 Olivier nombre de Bonus par salle

    int tailleYBase = 3;
    int tailleXBase = 2;


    // #tp3 Louis
    [SerializeField] GameObject[] _objetsNiveau; // Objets du niveau.
    List<Vector2Int> _lesPosSurReperes = new(); // Liste des positions sur les repères.

    // #synthese Louis
    [SerializeField] GameObject[] _effectors; // Tableau des effectors
    [SerializeField] GameObject _nid; // Nid pour les ennemis 
    List<Vector2Int> _emplacementObjetsSalle = new(); // Emplacements des objets dans chaque salle.
    List<Vector2Int> _emplacementEffectorsSalle = new(); // Emplacements des objets dans chaque salle.
    List<Vector2Int> _emplacementNidsSalle = new(); // Emplacements des ennemis dans chaque salle.
    List<Vector2Int> _posObjets = new(); // Emplacements des objets dans chaque salle.
    List<Vector2Int> _posEffectors = new(); // Emplacements des effectors dans chaque salle.
    List<Vector2Int> _posNids = new(); // Emplacements des nids dans chaque salle.
    CinemachineBasicMultiChannelPerlin _perlin;
    //

    // #tp4 Louis
    [SerializeField] SOTemps _temps; // Objets du niveau.
    PolygonCollider2D _col; // Collider du niveau.
    Vector2Int min; // Coin inférieur gauche du niveau.
    Vector2Int max; // Coin inférieur gauche du niveau.
    int _tempsAdditionnel = 30;

    /// <summary>
    /// Propriété pour accéder au Tilemap depuis l'extérieur.
    /// </summary>
    public Tilemap tilemap => _tilemap;

    void Awake()
    {
        // #synthese Louis
        _perlin = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        for (int i = 0; i < _objetsNiveau.Length; i++)
        {
            _emplacementObjetsSalle.Add(Vector2Int.zero);
            _posObjets.Add(Vector2Int.zero);
        }
        //

        // #tp4 Louis
        if (_donneesPerso.niveau > 1) _temps.temps = _temps.tempsRestant + _tempsAdditionnel * (_donneesPerso.niveau - 1); // Ajoute du temps en fonction du niveau.
        _temps.tempsRestant = _temps.temps; // Initialise le temps restant.
        StartCoroutine(CoroutineTemps()); // Début de la coroutine pour le temps restant
        _col = GetComponent<PolygonCollider2D>(); // Récupère le collider du niveau.

        GererTailleNiveau();

        // #tp3 Louis
        PlacerObjetsSalle(); // Placement des objets dans les salles.

        Vector2Int tailleSalleAvecUneBordure = InstancierSalles();
        CalculerDimensionsTilemap(tailleSalleAvecUneBordure);

        // #tp4 Louis
        GererTailleCollider();
        PlacerBordures();
        _camera.Follow = GameObject.Find("Perso(Clone)").transform; // Fait suivre la caméra du personnage.

        TrouverPosLibre(); // #TP3 Olivier Trouve les positions libres dans le niveau


        StartCoroutine(CoroutinePlacerLesTresors()); // #TP3 Olivier start la coroutine pour placer les Joyaux

        StartCoroutine(CoroutinePlacerLesBonus()); // #TP3 Olivier start la coroutine pour placer les Bonus
    }


    private Vector2Int InstancierSalles()
    {
        // Calcul de la taille des salles avec une seule bordure.
        Vector2Int tailleSalleAvecUneBordure = Salle.tailleAvecBordures - Vector2Int.one;

        // Instanciation des salles dans une grille.
        for (int y = 0; y < _taille.y; y++) // Parcours vertical pour l'instanciation des salles.
        {
            for (int x = 0; x < _taille.x; x++) // Parcours horizontal pour l'instanciation des salles.
            {

                Vector2Int placementSalle = new(x, y); // Emplacement de la salle dans la grille.
                int salleRand = Random.Range(0, _tSallesModele.Length); // Choix aléatoire d'un modèle de salle.

                // Calcul de la position de la salle dans le monde.
                Vector2 pos = new(x * tailleSalleAvecUneBordure.x, y * tailleSalleAvecUneBordure.y);

                // Instanciation de la salle dans le niveau à l'emplacement désigné.
                Salle salle = Instantiate(_tSallesModele[salleRand], pos, Quaternion.identity, transform);

                salle.name = $"Salle_{x}_{y}"; // Renomme chacune des salles selon sa rangée et sa colonne.

                // Vérification des emplacements des objets pour chaque salle.
                for (int i = 0; i < _emplacementObjetsSalle.Count; i++)
                {
                    if (_emplacementObjetsSalle[i] == placementSalle)
                    {
                        // Placement des objets dans la salle.
                        Vector2Int decalage = Vector2Int.CeilToInt(_tilemap.transform.position);
                        Vector2Int posRep = salle.PlacerSurRepere(_objetsNiveau[i]) - decalage;
                        _lesPosSurReperes.Add(posRep);
                    }
                }

                // #synthese Louis
                // Vérification des emplacements des objets pour chaque salle.
                for (int i = 0; i < _emplacementEffectorsSalle.Count; i++)
                {
                    if (_emplacementEffectorsSalle[i] == placementSalle)
                    {
                        int effectorRand = Random.Range(0, _effectors.Length); // Choix aléatoire d'un modèle de salle.
                        // Placement des objets dans la salle.
                        Vector2Int decalage = Vector2Int.CeilToInt(_tilemap.transform.position);
                        Vector2Int posRep = salle.PlacerSurRepere(_effectors[effectorRand]) - decalage;
                        _lesPosSurReperes.Add(posRep);
                    }
                }

                for (int i = 0; i < _emplacementNidsSalle.Count; i++)
                {
                    if (_emplacementNidsSalle[i] == placementSalle)
                    {
                        // Placement des objets dans la salle.
                        Vector2Int decalage = Vector2Int.CeilToInt(_tilemap.transform.position);
                        Vector2Int posRep = salle.PlacerSurRepere(_nid) - decalage;
                        _lesPosSurReperes.Add(posRep);
                    }
                }
                //
            }
        }

        return tailleSalleAvecUneBordure;
    }

    private void CalculerDimensionsTilemap(Vector2Int tailleSalleAvecUneBordure)
    {
        // Calcul des dimensions du Tilemap.
        Vector2Int tailleTable = new Vector2Int(_taille.x, _taille.y) * tailleSalleAvecUneBordure;
        min = Vector2Int.zero - Salle.tailleAvecBordures / 2; // Coin inférieur gauche du niveau.
        max = min + tailleTable; // Coin supérieur droit du niveau.
    }

    private void PlacerBordures()
    {
        // Placement des bordures autour du Tilemap.
        for (int y = min.y; y <= max.y; y++) // Placement des bordures verticales.
        {
            for (int x = min.x; x <= max.x; x++) // Placement des bordures horizontales.
            {
                Vector3Int pos = new(x, y); // Position à boucher.

                // Ajout des bordures en haut et en bas.
                if (y == min.y || y == max.y) tilemap.SetTile(pos, _tuileModele);

                // Ajout des bordures sur les côtés.
                if (x == min.x || x == max.x) tilemap.SetTile(pos, _tuileModele);
            }
        }
    }

    // #tp3 Louis
    /// <summary>
    /// Place les objets dans les salles en fonction de leurs types.
    /// </summary>
    private void PlacerObjetsSalle()
    {
        for (int i = 0; i < _objetsNiveau.Length; i++)
        {
            // Placement des clés dans une salle aléatoire.
            if (_objetsNiveau[i].name == "Cle") FaireApparaitreCle(i);
            // Placement des portes dans une salle aléatoire.
            else if (_objetsNiveau[i].name == "Porte") FaireApparaitrePorte(i);
            // Placement du personnage dans une salle adjacente à la clé.
            else if (_objetsNiveau[i].name == "Perso") FaireApparaitrePerso(i);
            else if (_objetsNiveau[i].name == "BoutonJoyaux") FaireApparaitreBoutonJoyaux(i); // #synthese olivier Placement des boutons de joyaux dans des salles distinctes.
            else if (_objetsNiveau[i].name == "EnnemiOlivier") FaireApparaitreEnnemiOlivier(i); // #synthese olivier Placement des ennemis Olivier dans des salles distinctes.
            // Placement des interrupteurs dans des salles distinctes.
            else FaireApparaitreInterrupteur(i);

        }

        // #synthese Louis
        FaireApparaitreEffectors();
        FaireApparaitreNids();
        //
    }

    // #tp synthese olivier
    /// <summary>
    /// Fait apparaître un ennemi Olivier dans une salle aléatoire.
    /// </summary>
    /// <param name="i"></param>
    private void FaireApparaitreEnnemiOlivier(int i)
    {
        // Génération de coordonnées aléatoires pour l'emplacement de la grosse araignée.
        int emplacementAleaX = Random.Range(0, _taille.x);
        int emplacementAleaY = Random.Range(0, _taille.y);
        // Enregistrement de l'emplacement du bouton dans le tableau des emplacements d'objets.
        _emplacementObjetsSalle[i] = new(emplacementAleaX, emplacementAleaY);
    }
    // #tp synthese olivier
    /// <summary>
    /// Fait apparaître un bouton de joyaux dans une salle aléatoire.
    /// </summary>
    /// <param name="i"></param>
    private void FaireApparaitreBoutonJoyaux(int i)
    {
        // Génération de coordonnées aléatoires pour l'emplacement du bouton.
        int emplacementAleaX = Random.Range(0, _taille.x);
        int emplacementAleaY = Random.Range(0, _taille.y);
        // Enregistrement de l'emplacement du bouton dans le tableau des emplacements d'objets.
        _emplacementObjetsSalle[i] = new(emplacementAleaX, emplacementAleaY);



    }

    // #tp3 Louis
    /// <summary>
    /// Fait apparaître une clé dans une salle aléatoire.
    /// </summary>
    private void FaireApparaitreCle(int i)
    {
        // Génération de coordonnées aléatoires pour l'emplacement de la clé.
        int emplacementAleaX = Random.Range(0, 2);
        int emplacementAleaY = Random.Range(0, 2);
        // Correction pour placer la clé sur le bord opposé si nécessaire.
        if (emplacementAleaX == 1) emplacementAleaX = _taille.x - 1;
        if (emplacementAleaY == 1) emplacementAleaY = _taille.y - 1;
        // Enregistrement de l'emplacement de la clé dans le tableau des emplacements d'objets.
        _emplacementObjetsSalle[i] = new(emplacementAleaX, emplacementAleaY);
        _posObjets[i] = _emplacementObjetsSalle[i];
    }

    // #tp3 Louis
    /// <summary>
    /// Fait apparaître une porte dans une salle aléatoire.
    /// </summary>
    private void FaireApparaitrePorte(int i)
    {
        int emplacementAleaX;
        // Choix aléatoire de la coordonnée Y pour l'emplacement de la porte.
        int emplacementAleaY = Random.Range(0, _taille.y);
        // Correction pour placer la porte sur le bord opposé si nécessaire.
        if (_posObjets[0].x == _taille.x - 1) emplacementAleaX = 0;
        else emplacementAleaX = _taille.x - 1;
        // Enregistrement de l'emplacement de la porte dans le tableau des emplacements d'objets.
        _emplacementObjetsSalle[i] = new(emplacementAleaX, emplacementAleaY);
        _posObjets[i] = _emplacementObjetsSalle[i];
    }

    // #tp3 Louis
    /// <summary>
    /// Fait apparaître le personnage dans une salle adjacente à la clé.
    /// </summary>
    private void FaireApparaitrePerso(int i)
    {
        // Choix aléatoire de la direction de décalage (horizontale ou verticale).
        int decalageDirection = Random.Range(0, 2);
        if (decalageDirection == 0)
        {
            do
            {
                // Calcul aléatoire du décalage horizontal et positionnement du personnage.
                int decalage = Random.Range(0, _taille.x);
                _emplacementObjetsSalle[i] = new(_posObjets[1].x + decalage, _posObjets[1].y);
            } while (_emplacementObjetsSalle[i].x > _taille.x - 1);
        }
        else
        {
            do
            {
                // Calcul aléatoire du décalage vertical et positionnement du personnage.
                int decalage = Random.Range(0, _taille.y);
                _emplacementObjetsSalle[i] = new(_posObjets[1].x, _posObjets[1].y + decalage);
            } while (_emplacementObjetsSalle[i].y > _taille.y - 1);
        }
        // Enregistrement de l'emplacement du personnage dans le tableau des emplacements d'objets.
        _posObjets[i] = _emplacementObjetsSalle[i];
    }

    // #tp3 Louis
    /// <summary>
    /// Fait apparaître un effector dans une salle aléatoire.
    /// </summary>
    private void FaireApparaitreEffectors()
    {
        // #sythese Louis
        for (int i = 0; i < _donneesPerso.niveau; i++)
        {
            for (int j = 0; j < _effectors.Length; j++)
            {
                // Génération de coordonnées aléatoires pour l'emplacement des effectors.
                int emplacementAleaX = Random.Range(0, _taille.x);
                int emplacementAleaY = Random.Range(0, _taille.y);
                Vector2Int emplacementEffector = new(emplacementAleaX, emplacementAleaY);
                while (_posEffectors.Contains(emplacementEffector))
                {
                    emplacementAleaX = Random.Range(0, _taille.x);
                    emplacementAleaY = Random.Range(0, _taille.y);
                    emplacementEffector = new(emplacementAleaX, emplacementAleaY);
                }

                // Enregistrement de l'emplacement de la porte dans le tableau des emplacements d'objets.
                _emplacementEffectorsSalle.Add(emplacementEffector);
                _posEffectors.Add(emplacementEffector);
            }
        }

        //
    }

    // #synthese Louis
    /// <summary>
    /// Fait apparaître un ennemi dans une salle aléatoire.
    /// </summary>
    private void FaireApparaitreNids()
    {
        for (int i = 0; i < _donneesPerso.niveau; i++)
        {
            int emplacementAleaX = Random.Range(0, _taille.x);
            int emplacementAleaY = Random.Range(0, _taille.y);
            Vector2Int emplacementNid = new(emplacementAleaX, emplacementAleaY);
            while (_posNids.Contains(emplacementNid))
            {
                emplacementAleaX = Random.Range(0, _taille.x);
                emplacementAleaY = Random.Range(0, _taille.y);
                emplacementNid = new(emplacementAleaX, emplacementAleaY);
            }

            // Enregistrement de l'emplacement de la porte dans le tableau des emplacements d'objets.
            _emplacementNidsSalle.Add(emplacementNid);
            _posNids.Add(emplacementNid);
        }
    }
    //

    // #tp3 Louis
    /// <summary>
    /// Fait apparaître un interrupteur dans une salle aléatoire.
    /// </summary>
    private void FaireApparaitreInterrupteur(int i)
    {
        // Génération de coordonnées aléatoires pour l'emplacement de l'interrupteur.
        do
        {
            int emplacementAleaX = Random.Range(0, _taille.x);
            int emplacementAleaY = Random.Range(0, _taille.y);
            _emplacementObjetsSalle[i] = new Vector2Int(emplacementAleaX, emplacementAleaY);
        } while (_emplacementObjetsSalle[i] == _posObjets[0] || _emplacementObjetsSalle[i] == _posObjets[1] || _emplacementObjetsSalle[i] == _posObjets[2]);
        // Enregistrement de l'emplacement de l'interrupteur dans le tableau des emplacements d'objets.
    }


    IEnumerator CoroutinePlacerLesTresors()
    {
        Transform conteneur = new GameObject("tresors").transform; // #TP3 Olivier Création du conteneur des trésors
        conteneur.parent = transform; // #TP3 Olivier Attribue ce conteneur au transform du GameObject courant
        int nbTresors = _nbTresorsParSalle; // #TP3 Olivier Calcule le nombre total de trésors à générer
        for (int i = 0; i < nbTresors; i++) // #TP3 Olivier Boucle de placement des trésors
        {
            yield return new WaitForSeconds(0f); // #TP3 Olivier Attente facultative (pour l'instant immédiate)
            int indexTresors = Random.Range(0, _tTresorsModeles.Length); // #TP3 Olivier Sélection aléatoire d'un modèle de trésor
            Tresors tresorsModele = _tTresorsModeles[indexTresors]; // #TP3 Olivier Récupération du modèle de trésor

            Vector2Int pos = ObtenirUnePosLibre(); // #TP3 Olivier Obtention d'une position libre

            Vector3 pos3 = (Vector3)(Vector2)pos + _tilemap.transform.position + _tilemap.tileAnchor; // #TP3 Olivier Conversion et ajustement de la position en 3D
            Instantiate(tresorsModele, pos3, Quaternion.identity, conteneur); // #TP3 Olivier Instanciation du trésor

            if (_lesPosLibres.Count == 0) { Debug.LogWarning("Aucun espace libre"); break; } // #TP3 Olivier Vérification de l'espace disponible
        }
    }

    /// <summary>
    ///  Place les bonus à des entroids aléatoires et dans des positions libres dans le niveau
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutinePlacerLesBonus()
    {
        Transform conteneur = new GameObject("bonus").transform; // #TP3 Olivier Création du conteneur des bonus
        conteneur.parent = transform; // #TP3 Olivier Attribue ce conteneur au transform du GameObject couran
        int _nbSalles = tailleXBase * tailleYBase; // #TP4 Olivier Calcule le nombre de salle
        int nbBonus = _nbBonusParSalle * _nbSalles; // #TP3 Olivier Calcule le nombre total de bonus à générer
        for (int i = 0; i < nbBonus; i++) // #TP3 Olivier Boucle de placement des bonus
        {
            yield return new WaitForSeconds(0f); // #TP3 Olivier Attente facultative (pour l'instant immédiate)
            int indexBonus = Random.Range(0, _tBonusModeles.Length); // #TP3 Olivier Sélection aléatoire d'un modèle de bonus
            Boost bonusModele = _tBonusModeles[indexBonus].GetComponent<Boost>();
            BonusLouis bonusModeleLouis = _tBonusModeles[indexBonus].GetComponent<BonusLouis>();

            Vector2Int pos = ObtenirUnePosLibre(); // #TP3 Olivier Obtention d'une position libre

            Vector3 pos3 = (Vector3)(Vector2)pos + _tilemap.transform.position + _tilemap.tileAnchor; // #TP3 Olivier Conversion et ajustement de la position en 3D

            if (bonusModele != null)
            {
                Instantiate(bonusModele, pos3, Quaternion.identity, conteneur);
            }
            else if (bonusModeleLouis != null)
            {
                Instantiate(bonusModeleLouis, pos3, Quaternion.identity, conteneur);
            }
            else
            {
                Debug.LogError("No Bonus or BonusLouis component found");
            }

            if (_lesPosLibres.Count == 0)
            {
                Debug.LogError("Aucun espace libre");
                break;
            } // #TP3 Olivier Vérification de l'espace disponible
        }
    }

    Vector2Int ObtenirUnePosLibre()
    {
        int indexPosLibre = Random.Range(0, _lesPosLibres.Count); // #TP3 Olivier Sélection aléatoire d'une position libre
        Vector2Int pos = _lesPosLibres[indexPosLibre]; // #TP3 Olivier Récupération de la position
        _lesPosLibres.RemoveAt(indexPosLibre); // #TP3 Olivier Retrait de la position de la liste pour éviter sa réutilisation
        return pos; // #TP3 Olivier Retourne la position libre sélectionnée
    }

    // #tp4 Louis
    /// <summary>
    /// Gère la taille du niveau en fonction du niveau actuel.
    /// </summary>
    private void GererTailleNiveau()
    {
        int tailleYBase = 3;
        int tailleXBase = 2;
        if (_donneesPerso.niveau <= 2)
        {
            _taille.x = tailleXBase; // Ajuste la taille en X.
            _taille.y = _donneesPerso.niveau + 1; // Ajuste la taille en Y.
        }
        else if (_donneesPerso.niveau > 2)
        {
            _taille.x = _donneesPerso.niveau;
            _taille.y = tailleYBase;
        }
    }

    // #tp4 Louis
    /// <summary>
    /// Positionne le collider du niveau en fonction de sa taille.
    private void GererTailleCollider()
    {
        Vector2[] points = new Vector2[4];
        points[0] = new Vector2(max.x + 1, max.y + 1); // Coin supérieur droit.
        points[1] = new Vector2(min.x, max.y + 1); // Coin supérieur gauche.
        points[2] = new Vector2(min.x, min.y); // Coin inférieur gauche.
        points[3] = new Vector2(max.x + 1, min.y); // Coin inférieur droit.
        _col.points = points; // Affecte les points au collider.
    }

    void TrouverPosLibre()
    {
        BoundsInt bornes = _tilemap.cellBounds; // Obtention des limites du tilemap cible
        for (int x = bornes.xMin + 4; x < bornes.xMax; x++) // Parcours des colonnes dans les limites
        {
            for (int y = bornes.yMin; y < bornes.yMax; y++) // Parcours des lignes dans les limites
            {
                Vector2Int posTuile = new Vector2Int(x, y); // Création de la position de la tuile
                TileBase tuile = _tilemap.GetTile((Vector3Int)posTuile); // Récupération de la tuile à la position donnée
                if (tuile == null) _lesPosLibres.Add(posTuile); // Si pas de tuile, ajoute la position comme libre
            }
        }
    }

    // #tp4 Louis
    /// <summary>
    /// Coroutine pour gérer le temps restant et passer à la scène du tableau d'honneur si le temps est écoulé.
    /// </summary>
    IEnumerator CoroutineTemps()
    {
        for (int i = 0; i < _temps.temps; i++)
        {
            yield return new WaitForSeconds(1);
            _temps.tempsRestant--;
            if (_temps.tempsRestant == 30) GestAudio.instance.DemarerCoroutineMusical(TypePiste.MusiqueEventB, 0.2f, true); // #TP synthese Olivier Démarre la musique de l'événement B

            if (_temps.tempsRestant == 0)
            {
                _fondFin.ApparaitreFond(_perlin); // #synthese Louis
                GestAudio.instance.DemarerCoroutineMusical(TypePiste.MusiqueEventB, 0.2f, false); // #TP synthese Olivier arrête la musique de l'événement B
            }
        }
    }
}
