using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private GameObject _explosion;

    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
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
            Destroy(explosion, 2.5f);
            Destroy(this.gameObject, 0.25f);
            _spawnManager.StartSpawning();

        }
    }
}
