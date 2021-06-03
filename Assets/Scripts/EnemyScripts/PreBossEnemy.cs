using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBossEnemy : Enemy
{
    [SerializeField]
    private GameObject _laserCharge;
    [SerializeField]
    private GameObject _lasers;

    private GameObject _currentlyInstantiatedGameObject;

    private Coroutine _laserCoroutine;

    private bool _attackingPlayer = false;

    private float _originalSpeed;

    private void Start()
    {
        _originalSpeed = _speed;
    }

    public override void CalculateMovment()
    {
        base.CalculateMovment();

        if (transform.position.y <= _bottomBound && _attackingPlayer)
        {
            _attackingPlayer = false;
            StopCoroutine(_laserCoroutine);
            Destroy(_currentlyInstantiatedGameObject);
            _laserCoroutine = null;
            _speed = _originalSpeed;
        }

    }


    public override void ScanEnvironment()
    {
        LayerMask layer = LayerMask.GetMask("Player");
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1.5f, Vector2.down, 10, layer);

        if (hit.collider != null && !_attackingPlayer)
        {
            _attackingPlayer = true;
            Shoot();
        }

        base.ScanEnvironment();
    }

    public override void Shoot()
    {
        if (_attackingPlayer)
        {
            _laserCoroutine = StartCoroutine(LaserCoroutine());
        }

        else
        {
            base.Shoot();
        }
    }

    private IEnumerator LaserCoroutine()
    {
        _originalSpeed = _speed;
        _speed = 1;
        _currentlyInstantiatedGameObject = Instantiate(_laserCharge, transform);
        yield return new WaitForSeconds(1.5f);
        Destroy(_currentlyInstantiatedGameObject);
        _speed = 2;
        _currentlyInstantiatedGameObject = Instantiate(_lasers, transform);
        yield return new WaitForSeconds(2);
        Destroy(_currentlyInstantiatedGameObject);
        _speed = _originalSpeed;
        _attackingPlayer = false;

    }
}
