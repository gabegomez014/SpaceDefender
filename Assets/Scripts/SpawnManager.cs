﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject[] powerups;

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
            GameObject powerupToSpawn;
            int powerupType = Random.Range(0, 3);

            powerupToSpawn = powerups[powerupType];

            Instantiate(powerupToSpawn, spawnPos, Quaternion.identity, _powerupHolder);
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator SpawnEnemies() 
    {
        yield return new WaitForSeconds(0.25f);

        while (_keepSpawning)
        {
            Vector3 spawnPos = new Vector3(Random.Range(_leftBound, _rightBound), _topBound);
            Instantiate(_enemy, spawnPos, Quaternion.identity, _enemyHolder);
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnPowerups());
        StartCoroutine(SpawnEnemies());
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
    }
}