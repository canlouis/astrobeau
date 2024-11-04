using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui détecte la présence du sol sous le personnage
/// et fournit une indication de la présence au script dérivé.
/// Auteurs du code: Louis Cantin et Olivier Dion
/// Auteur des commentaires: Louis Cantin/// 
/// </summary>
public class DetecteurSol : MonoBehaviour
{
    [SerializeField] protected float _distanceSol = 0.4f; // Distance entre le centre du personnage et le sol.
    [SerializeField] private Vector2 _tailleBoite = new Vector2(0.3f, 0.1f); // Taille de la boîte de détection du sol.
    [SerializeField] private LayerMask _layerMask; // Masque de calques pour définir quels calques représentent le sol.
    [SerializeField] AudioClip _sonChute; // #TP4 Olivier Référence à un son pour la chute.


    bool aJouerSon = false; // #TP4 Olivier Variable pour jouer le son une seule fois.
    protected bool _estAuSol = false; // Indique si le personnage est actuellement en contact avec le sol.

    protected virtual void FixedUpdate()
    {
        VerifierSol();
    }

    /// <summary>
    /// Vérifie la présence du sol sous le personnage en utilisant une boîte de détection.
    /// </summary>
    private void VerifierSol()
    {
        // Calcul de la position du centre de la boîte de détection par rapport à la position de le personnage.
        Vector2 posCentreBoite = (Vector2)transform.position + new Vector2(0, -_distanceSol);

        // Effectue une détection de collision en utilisant une boîte pour vérifier la présence du sol.
        Collider2D col = Physics2D.OverlapBox(posCentreBoite, _tailleBoite, 0, _layerMask);

        // Met à jour la variable '_estAuSol' en fonction de la détection de collision.
        _estAuSol = col != null;
        if (_estAuSol && !aJouerSon)
        {
            GestAudio.instance.JouerSon(_sonChute);
            aJouerSon = true;
        }
        else if (!_estAuSol)
        {
            aJouerSon = false;
        }
    }

    /// <summary>
    /// Dessine une boîte de détection du sol avec un gizmo sous les pieds du personnage pour une visualisation dans l'éditeur Unity.
    /// </summary>
    private void OnDrawGizmos()
    {
        // Permet de changer la couleur du gizmo même lorsque le jeu est à l'arrêt
        if (!Application.isPlaying) VerifierSol();

        // Définit la couleur du gizmo en vert si le personnage est au sol, sinon en rouge.
        Gizmos.color = _estAuSol ? Color.green : Color.red;

        // Calcul de la position du centre du gizmo par rapport à la position du personnage.
        Vector2 posCentreGizmo = (Vector2)transform.position + new Vector2(0, -_distanceSol);

        // Dessine une boîte de détection du sol avec le gizmo.
        Gizmos.DrawWireCube(posCentreGizmo, _tailleBoite);
    }
}