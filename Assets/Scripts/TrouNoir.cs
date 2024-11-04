using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TrouNoir : MonoBehaviour
{
    [SerializeField] GameObject[] _tabObjetsGrossir;
    [SerializeField] Light2D _lumiere;
    [SerializeField] SOTemps _temps;
    [SerializeField] float _acceleration = 50;
    [SerializeField] float _rayonMax = 0.1f;
    [SerializeField] float _rayonMin = 5000;
    [SerializeField] float _multiplicateurTailleObjets = 2f; // Multiplicateur de taille
    [SerializeField] float _exponentielFactor = 3f; // Facteur d'exponentiation
    [SerializeField] float _momentAcceleration = 2f; // Moment d'accélération (en secondes)
    
    Material _mat;
    List<Vector2> _rayonObjetMin = new List<Vector2>();
    float _rayonExtLumiereMin;

    void Start()
    {
        _mat = GetComponent<Renderer>().material;
        _rayonExtLumiereMin = _lumiere.pointLightOuterRadius;

        for (int i = 0; i < _tabObjetsGrossir.Length; i++)
        {
            _rayonObjetMin.Add(_tabObjetsGrossir[i].transform.localScale);
        }

        StartCoroutine(CoroutineTempsTrouNoir());
        StartCoroutine(CoroutineTrouNoir());
    }

    IEnumerator CoroutineTrouNoir()
    {
        float tempsInitial = _temps.tempsTrouNoir; // Temps total initial
        float elapsedTime = 0f; // Temps écoulé

        // Calculer la taille finale du pointLightOuterRadius
        float tailleFinaleLumiere = _rayonExtLumiereMin * _multiplicateurTailleObjets;

        while (elapsedTime < tempsInitial)
        {
            elapsedTime += Time.deltaTime;

            // Normaliser le temps pour obtenir une valeur entre 0 et 1
            float normalizedTime = Mathf.Clamp01(elapsedTime / tempsInitial);
            float rayonCourant = Mathf.Lerp(_rayonMin, _rayonMax, normalizedTime);
            
            // Mettre à jour le rayon de la lumière avec exponentiation
            float exponentielleValue = Mathf.Pow(normalizedTime, _exponentielFactor);
            float easeOutValue = 1 - Mathf.Pow(1 - normalizedTime, 2); // Quadratique "ease-out"
            float finalLightScaleValue;

            if (elapsedTime < _momentAcceleration)
            {
                // Avant le moment d'accélération
                finalLightScaleValue = exponentielleValue * easeOutValue;
            }
            else
            {
                // Après le moment d'accélération, on augmente la rapidité de la mise à l'échelle
                float accelerationTime = elapsedTime - _momentAcceleration;
                float acceleratedNormalizedTime = Mathf.Clamp01(accelerationTime / (tempsInitial - _momentAcceleration));
                float acceleratedEaseOutValue = 1 - Mathf.Pow(1 - acceleratedNormalizedTime, 2);
                finalLightScaleValue = exponentielleValue * acceleratedEaseOutValue;
            }

            // Mettre à jour le rayon de la lumière en utilisant le facteur de mise à l'échelle final
            _lumiere.pointLightOuterRadius = Mathf.Lerp(_rayonExtLumiereMin, tailleFinaleLumiere, finalLightScaleValue);

            for (int i = 0; i < _tabObjetsGrossir.Length; i++)
            {
                // Appliquer la logique d'exponentiation et d'accélération pour les objets
                float finalScaleValue;

                if (elapsedTime < _momentAcceleration)
                {
                    // Avant le moment d'accélération
                    finalScaleValue = exponentielleValue * easeOutValue;
                }
                else
                {
                    // Après le moment d'accélération
                    float accelerationTime = elapsedTime - _momentAcceleration;
                    float acceleratedNormalizedTime = Mathf.Clamp01(accelerationTime / (tempsInitial - _momentAcceleration));
                    float acceleratedEaseOutValue = 1 - Mathf.Pow(1 - acceleratedNormalizedTime, 2);
                    finalScaleValue = exponentielleValue * acceleratedEaseOutValue;
                }

                // Multiplier la taille initiale par le multiplicateur à la fin
                Vector2 tailleFinale = Vector2.Lerp(_rayonObjetMin[i], new Vector2(_rayonObjetMin[i].x * _multiplicateurTailleObjets, _rayonObjetMin[i].y * _multiplicateurTailleObjets), finalScaleValue);
                _tabObjetsGrossir[i].transform.localScale = tailleFinale;
            }

            _mat.SetFloat("_Rayon", rayonCourant);

            yield return null; // Attendre le prochain frame
        }

        // Assurer que tout est à sa valeur maximale à la fin
        _lumiere.pointLightOuterRadius = tailleFinaleLumiere; // Ajuster la lumière à la taille maximale
        _mat.SetFloat("_Rayon", _rayonMax);
        for (int i = 0; i < _tabObjetsGrossir.Length; i++)
        {
            _tabObjetsGrossir[i].transform.localScale = new Vector2(_rayonObjetMin[i].x * _multiplicateurTailleObjets, _rayonObjetMin[i].y * _multiplicateurTailleObjets); // Appliquer la taille maximale
        }
    }

    IEnumerator CoroutineTempsTrouNoir()
    {
        _temps.tempsTrouNoir = _temps.temps;
        while (_temps.tempsTrouNoir > 0)
        {
            float vitesse = _temps.tempsTrouNoir / _acceleration;
            _temps.tempsTrouNoir -= vitesse;
            yield return new WaitForSeconds(vitesse);
        }
    }
}
