using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FondFin : MonoBehaviour
{
    [SerializeField] SONavigation _navigation; // Référence au script de navigation vers la prochaine scène.
    SpriteRenderer _sr;
    float _vitessseApparition = 0.03f;
    float _vitessseRotMax = 3f;
    float _vitessseRotMin = 1f;
    float _vitesseRotRandom;
    int _directionRotation = 0;
    float _reductionVelocitePerso = .95f;
    float _ratio = .01f;
    float _tremblement = 0.6f;
    void Awake()
    {
        _vitesseRotRandom = Random.Range(_vitessseRotMin, _vitessseRotMax); // vitesse de rotation des tresors
        _directionRotation = Random.Range(0, 2); // direction de rotation des tresors
        if (_directionRotation == 0) _vitesseRotRandom = -_vitesseRotRandom; // si la direction de rotation est 0, la rotation est negative
        _sr = GetComponent<SpriteRenderer>();
        _sr.color = new(0, 0, 0, 0);
    }

    public void ApparaitreFond(CinemachineBasicMultiChannelPerlin perlin)
    {
        Physics2D.gravity = new Vector2(0, 0);
        StartCoroutine(ApparaitreFondCoroutine(perlin));
    }

    IEnumerator ApparaitreFondCoroutine(CinemachineBasicMultiChannelPerlin perlin)
    {
        for (float i = 0; i < 1; i += _ratio)
        {
            perlin.m_AmplitudeGain += _ratio;
            perlin.m_FrequencyGain += _ratio;
            Perso.instance.FinirPartie(_vitesseRotRandom, _reductionVelocitePerso);
            _sr.color = new(0, 0, 0, i);
            yield return new WaitForSeconds(_vitessseApparition);
        }
        _navigation.AllerPanneauFin();
    }
}
