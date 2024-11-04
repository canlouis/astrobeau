using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PisteMusicale : MonoBehaviour
{
    [SerializeField] TypePiste _type;
    public TypePiste type => _type;
    [SerializeField] bool _estActifParDefaut;
    [SerializeField] bool _estActif;
    public bool estActif
    {
        get => _estActif;
        set
        {
            _estActif = value;
            AjusterVolume();
        }
    }

    AudioSource _source;
    public AudioSource source => _source;


    void Awake()
    {
        _source = GetComponent<AudioSource>();
        _estActif = _estActifParDefaut;
        _source.loop = true;
        _source.playOnAwake = true;
    }

    void Start()
    {
        AjusterVolume();
    }

    private void AjusterVolume()
    {
        if (estActif) _source.volume = GestAudio.instance.volumeMusicalRef;
        else _source.volume = 0;
    }
}
