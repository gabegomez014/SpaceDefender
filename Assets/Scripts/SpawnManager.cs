using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;

    [SerializeField]
    private Transform _enemyHolder;

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
    }
}
