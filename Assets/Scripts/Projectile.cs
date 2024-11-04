using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] ParticleSystem _part;
    [SerializeField] AudioClip _sonTir;
    [SerializeField] float _vitesse = 20f;
    Rigidbody2D _rb;

    // Start is called before the first frame update

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        // GestAudio.instance.JouerSon(_sonTir);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * _vitesse * Time.deltaTime, Space.Self);
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(_part, transform.position, Quaternion.identity);
        Destroy(gameObject);


        if (other.CompareTag("Ennemis"))
        {
            other.GetComponent<EtresVivants>().PerdreVie(Perso.instance.degats, other.gameObject);
        }

    }
}
