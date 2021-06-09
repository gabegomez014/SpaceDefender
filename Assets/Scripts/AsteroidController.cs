using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private AudioClip _explosionSFX;

    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.forward * Time.deltaTime * _speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_explosionSFX);
            Destroy(explosion, 2.5f);
            Destroy(this.gameObject, 0.5f);
            Destroy(GetComponent<Collider2D>());
            _spawnManager.StartSpawning();

        }
    }
}
