using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemies;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject[] _rarePowerups;
    [SerializeField]
    private GameObject[] _planets;

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

    IEnumerator SpawnPowerups()
    {
        yield return new WaitForSeconds(0.5f);

        while (_keepSpawning)
        {
            float waitTime = Random.Range(3, 7);
            Vector3 spawnPos = new Vector3(Random.Range(_leftBound, _rightBound), _topBound);

            //Checking to see if we should drop a rare power-up (Currently set to a 5% probability)
            if (Random.value <= 0.05f)
            {
                GameObject rarePowerupToSpawn;
                int powerupType = Random.Range(0, _rarePowerups.Length);
                rarePowerupToSpawn = _rarePowerups[powerupType];

                Instantiate(rarePowerupToSpawn, spawnPos, Quaternion.identity, _powerupHolder);
            }

            else
            {
                GameObject powerupToSpawn;
                int powerupType = Random.Range(0, _powerups.Length);
                powerupToSpawn = _powerups[powerupType];

                Instantiate(powerupToSpawn, spawnPos, Quaternion.identity, _powerupHolder);
            }

            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator SpawnEnemies() 
    {
        yield return new WaitForSeconds(0.25f);

        while (_keepSpawning)
        {
            Vector3 spawnPos = new Vector3(Random.Range(_leftBound, _rightBound), _topBound);
            GameObject enemyToSpawn;
            int enemyType = Random.Range(0, _enemies.Length);
            enemyToSpawn = _enemies[enemyType];
            Instantiate(enemyToSpawn, spawnPos, Quaternion.identity, _enemyHolder);
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    IEnumerator SpawnPlanets()
    {
        while (_keepSpawning)
        {
            Vector3 spawnPos = new Vector3(Random.Range(_leftBound, _rightBound), _topBound + 6);
            GameObject planetToSpawn;
            int planet = Random.Range(0, _planets.Length);
            planetToSpawn = _planets[planet];

            Instantiate(planetToSpawn, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5, 15));
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnPowerups());
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPlanets());
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

    public void GameRestarted()
    {
        _keepSpawning = true;
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerups());
        StartCoroutine(SpawnPlanets());
    }
}
