using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanneauVignette : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _champ;
    SOObjet _donneesObjet;

    public SOObjet donneesObjet { get => _donneesObjet; set => _donneesObjet = value; }
    public int nb
    {
        get => _nb;
        set
        {
            _nb = value;
            _champ.text = "" + _nb;
        }
    }

    int _nb = 1;
    // Start is called before the first frame update
    void Start()
    {
        _image.sprite = _donneesObjet.sprite;
        // _champ.text = "" + _nb;
        nb = nb;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
