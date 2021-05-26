using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileBehavior : ProjectileBehavior
{
    private Enemy _trackedEnemy;
    private Transform _enemyHolder;
    private Rigidbody2D _rb;
    private bool _enemyFound = false;

    private static GameObject[] _allTargetedEnemies;    // static because I want all homing missile's to be referencing the same list

    void Awake()
    {
        _enemyHolder = GameObject.Find("EnemyHolder").transform;
        _rb = GetComponent<Rigidbody2D>();

        if (_enemyHolder == null)
        {
            Debug.LogWarning("Could not find the EnemyHolder transform");
        }

        if (_rb == null)
        {
            Debug.LogWarning("Could not find the RigidBody2D");
        }

        _shotDirection = Vector2.up;
        _rb.velocity = new Vector2(0, _speed); ;

        FindNearestEnemy();
    }

    public override void CalculateMovement()
    {

        if (!_enemyFound) { FindNearestEnemy(); return; }

        if (_trackedEnemy == null) { Destroy(this.gameObject); }

        _rb.velocity = _shotDirection * _speed;

        _shotDirection = (_trackedEnemy.transform.position - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, _shotDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _speed * 90);

        if (transform.position.y >= _topBound || transform.position.y <= _bottomBound || transform.position.x <= _leftBound || transform.position.x >= _rightBound)
        {
            _trackedEnemy.SetTrackedStatus(false);

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }

    }

    private void FindNearestEnemy()
    {
        if (_enemyHolder.childCount <= 0)
        {
            // Can't do anything in this case
            return;
        }

        Enemy closestEnemy = _enemyHolder.GetChild(0).GetComponent<Enemy>();  // Defaulting to first entry for now
        float closestDistance = 1000000; // Defaulting to a large number for the first iteration

        foreach (Transform child in _enemyHolder)
        {
            Enemy childEnemy = child.GetComponent<Enemy>();

            float calculatedDistance = Vector2.Distance(transform.position, child.position);
            if (calculatedDistance < closestDistance && child.position.y >= -2 && !childEnemy.GetTrackedStatus())
            {
                closestDistance = calculatedDistance;
                closestEnemy = childEnemy;
            }
        }

        _trackedEnemy = closestEnemy;
        _trackedEnemy.SetTrackedStatus(true);
        _enemyFound = true;
        _shotDirection = (_trackedEnemy.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, _shotDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _speed * 90);
    }
}
