using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _tripleShotPowerup;
    [SerializeField]
    private GameObject _speedPowerup;

    [SerializeField]
    private Transform _enemyHolder;
    [SerializeField]
    private Transform _powerupHolder;

    [SerializeField]
    private float _spawnRate = 3;

    private float _topBound = 7;
    private float _leftBound = -9.3f;
    private float _rightBound = 9.3f;

    private bool _keepSpawning = true;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerups());
    }

    IEnumerator SpawnPowerups()
    {
        while (_keepSpawning)
        {
            float waitTime = Random.Range(3, 7);
            Vector3 spawnPos = new Vector3(Random.Range(_leftBound, _rightBound), _topBound);
            GameObject powerupToSpawn;
            int powerupType = Random.Range(0, 2);

            switch (powerupType)
            {
                case (0):
                    powerupToSpawn = _speedPowerup;
                    break;

                case (1):
                    powerupToSpawn = _tripleShotPowerup;
                    break;

                default:
                    powerupToSpawn = _tripleShotPowerup;
                    break;

            }

            Instantiate(powerupToSpawn, spawnPos, Quaternion.identity, _powerupHolder);
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator SpawnEnemies() 
    {
        while(_keepSpawning)
        {
            Vector3 spawnPos = new Vector3(Random.Range(_leftBound, _rightBound), _topBound);
            Instantiate(_enemy, spawnPos, Quaternion.identity, _enemyHolder);
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    public void StopSpawning()
    {
        _keepSpawning = false;
        foreach (Transform child in _enemyHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in _powerupHolder)
        {
            Destroy(child.gameObject);
        }
    }
}
