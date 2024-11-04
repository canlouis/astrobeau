using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GestAudio : MonoBehaviour
{
    [SerializeField] float _volumeMusicalRef = 0.5f; // #TP4 Olivier Volume de la musique
    public float volumeMusicalRef => _volumeMusicalRef; // #TP4 Olivier Propriété pour accéder au volume de la musique
    PisteMusicale[] _tPistes;   // #TP4 Olivier Tableau des pistes musicales
    public PisteMusicale[] tPistes => _tPistes; // #TP4 Olivier Propriété pour accéder au tableau des pistes musicales
    AudioSource _sourceEffetsSonores; // #TP4 Olivier Source des effets sonores
    static GestAudio _instance; // #TP4 Olivier Instance de la classe
    public static GestAudio instance => _instance; // #TP4 Olivier Propriété pour accéder à l'instance de la classe


    void Awake()
    {
        if (!DevenirSingleton()) // #TP4 Olivier Si l'instance est déjà créée, détruit le gameObject
        {
            Destroy(gameObject); // #TP4 Olivier Détruit le gameObject
            return; // #TP4 Olivier Retourne
        }
        DontDestroyOnLoad(gameObject); // #TP4 Olivier Ne détruit pas le gameObject lors du changement de scène
        _tPistes = GetComponentsInChildren<PisteMusicale>(); // #TP4 Olivier Récupère les pistes musicales
        _sourceEffetsSonores = gameObject.AddComponent<AudioSource>(); // #TP4 Olivier Ajoute un composant AudioSource
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">La musique de l'évenement A ou de l'évennement B</param>
    /// <param name="volumeFin">Volume que l'on veux que la musique soit jouer.</param>
    /// <param name="debut">Détermine si c'est le dégrader du debut(true) ou si c'est le dégrader de fin(false)</param>
    public void DemarerCoroutineMusical(TypePiste type, float volumeFin, bool debut) // #TP4 Olivier Coroutine pour démarrer la musique
    {
        StartCoroutine(CoroutineDegraderMusique(type, volumeFin, debut)); // #TP4 Olivier Démarre la coroutine
    }

    /// <summary>
    /// Coroutine pour commencer la musique de l'évennement A ou B avec un dégrader de volume au deubut et à la fin
    /// </summary>
    /// <param name="type">La musique de l'évenement A ou de l'évennement B</param>
    /// <param name="volumeFin">Volume que l'on veux que la musique soit jouer.</param>
    /// <param name="debut">Détermine si c'est le dégrader du debut(true) ou si c'est le dégrader de fin(false)</param>
    /// <returns></returns>
    IEnumerator CoroutineDegraderMusique(TypePiste type, float volumeFin, bool debut)
    {
        PisteMusicale pisteUtilisee = null; // #TP4 Olivier Piste musicale utilisée
        float duration = 1.5f;  // #TP4 Olivier Durée de la transition
        float VolumeIni = 0; // #TP4 Olivier Volume initial
        if (debut) // #TP4 Olivier Si c'est le dégrader du début
        {
            foreach (PisteMusicale piste in _tPistes) // #TP4 Olivier Pour chaque piste musicale dans le tableau de pistes musicales
            {
                if (piste.type == type) // #TP4 Olivier Si le type de la piste est égal au type
                {
                    pisteUtilisee = piste; // #TP4 Olivier Piste utilisée est égale à la piste
                    break;  // #TP4 Olivier Sort de la boucle
                }
            }
            if (pisteUtilisee != null) // #TP4 Olivier Si la piste utilisée n'est pas nulle
            {
                pisteUtilisee.source.volume = VolumeIni; // #TP4 Olivier Volume de la piste utilisée est égal au volume initial

                while (pisteUtilisee.source.volume < volumeFin) // #TP4 Olivier Tant que le volume de la piste utilisée est inférieur au volume final
                {
                    pisteUtilisee.source.volume += Time.deltaTime / duration; // #TP4 Olivier Augmente le volume de la piste utilisée selon la durée
                    yield return null; // #TP4 Olivier Retourne rien
                }
                pisteUtilisee.source.volume = volumeFin; // #TP4 Olivier Volume de la piste utilisée est égal au volume final
            }
        }
        else
        {
            foreach (PisteMusicale piste in _tPistes) // #TP4 Olivier Pour chaque piste musicale dans le tableau de pistes musicales
            {
                if (piste.type == type)   // #TP4 Olivier Si le type de la piste est égal au type
                {
                    pisteUtilisee = piste; // #TP4 Olivier Piste utilisée est égale à la piste
                    break;  // #TP4 Olivier Sort de la boucle
                }
            }

            if (pisteUtilisee != null) // #TP4 Olivier Si la piste utilisée n'est pas nulle
            {
                float volumeDebut = pisteUtilisee.source.volume;    // #TP4 Olivier Volume de début est égal au volume de la piste utilisée

                while (pisteUtilisee.source.volume > 0.0f) // #TP4 Olivier Tant que le volume de la piste utilisée est supérieur à 0
                {
                    pisteUtilisee.source.volume -= volumeDebut * (Time.deltaTime / duration); // #TP4 Olivier Diminue le volume de la piste utilisée selon la durée

                    yield return null; // #TP4 Olivier Retourne rien
                }

                pisteUtilisee.source.volume = 0f; // #TP4 Olivier Volume de la piste utilisée est égal à 0
            }
        }
    }

    /// <summary>
    /// Joue un son seulement une fois
    /// </summary>
    /// <param name="clip">AudioClip du son que l'on veux faire jouer</param>
    public void JouerSon(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("AudioClip is null");
        }
        else if (_sourceEffetsSonores == null)
        {
            Debug.LogError("_sourceEffetsSonores is null");
        }
        else
        {
            _sourceEffetsSonores.PlayOneShot(clip);
        }
    }


    /// <summary>
    /// Permet au gestionnaire autoi de devenir un singleton
    /// </summary>
    /// <returns>Le singleton à été créé ou non</returns>
    bool DevenirSingleton() // #TP4 Olivier Permet au gestionnaire audio de devenir un singleton
    {
        if (_instance == null) // #TP4 Olivier Si l'instance est nulle
        {
            _instance = this; // #TP4 Olivier L'instance est égale à cette instance
            return true; // #TP4 Olivier Retourne vrai
        }
        return false; // #TP4 Olivier Retourne faux
    }
}


