using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtresVivants : DetecteurSol
{
    [SerializeField] private int _nbViesBase;
    [SerializeField] private float _vitesseBase;
    [SerializeField] private int _degats;
    int _nbVies;
    float _couleurDegats = .7f;
    bool _debut = true;
    static bool _graviteEstInversee = false;
    public int degats { get => _degats; set => _degats = value; }
    protected float vitesseBase { get => _vitesseBase; set => _vitesseBase = value; }
    protected int nbViesBase { get => _nbViesBase; set => _nbViesBase = value; }
    public static bool graviteEstInversee { get => _graviteEstInversee; set => _graviteEstInversee = value; }
    public int nbVies { get => _nbVies; set => _nbVies = value; }

    public virtual void PerdreVie(int degats, GameObject other)
    {
        if (_debut)
        {
            _nbVies = _nbViesBase;
            _debut = false;
        }

        bool estMort = false;
        _nbVies -= degats;

        if (other.CompareTag("Perso"))
        {
            BarreVie.instance.AfficherVieRestante(_nbVies, _nbViesBase);
            if (_nbVies > 0)
            {
                other.GetComponent<Perso>().ActiverInvulnerabilite();
            }
        }

        if (_nbVies <= 0 & !estMort)
        {
            other.GetComponent<Collider2D>().enabled = false;
            string nomObjet = other.name;
            nomObjet = nomObjet.Replace("(Clone)", "");
            Component component = other.GetComponent(nomObjet);
            component.GetType().GetMethod("Mourir").Invoke(component, null);
        }
        else
        {
            StartCoroutine(ChangerCouleurDegats(other));
        }
    }

    IEnumerator ChangerCouleurDegats(GameObject other)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(_couleurDegats, _couleurDegats, _couleurDegats);
        yield return new WaitForSeconds(.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
