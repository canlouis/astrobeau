using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarreVie : MonoBehaviour
{
    RectTransform _scaleBarre;
    static BarreVie _instance;

    public static BarreVie instance { get => _instance; set => _instance = value; }

    void Start()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
        _scaleBarre = GetComponent<RectTransform>();
    }

    public void AfficherVieRestante(int nbVies, float nbViesBase)
    {
        float longueurBarre = nbVies / nbViesBase;
        transform.localScale = new Vector3(_scaleBarre.localScale.x * longueurBarre, _scaleBarre.localScale.y, _scaleBarre.localScale.z);
    }
}
