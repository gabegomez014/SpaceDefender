using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveStruct
{
    [SerializeField]
    private GameObject[] _enemies;

    public List<GameObject> GetEnemies()
    {
        return _enemies.ToList();
    }

    public void SetEnemies(GameObject[] enemies)
    {
        _enemies = enemies;
    }
}
