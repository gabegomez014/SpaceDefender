using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDodger : Enemy
{ 
    [SerializeField]
    private float _dodgeSpeed = 4;

    public override void CalculateMovment()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1.5f, Vector2.down, 10);

        if (hit.collider != null && (hit.collider.tag == "Laser" || hit.collider.tag == "HeatedShot"))
        {
            Vector3 projectileLocation = hit.point;
            if (transform.position.x <= projectileLocation.x)
            {
                _currentMoveDir = Vector3.left * _dodgeSpeed;
            }

            else
            {
                _currentMoveDir = Vector3.right * _dodgeSpeed;
            }

            _moving = true;
            //_currentMoveDir = (Random.value < 0.5) ? Vector3.left * 5 : Vector3.right * 5;
            _timeMoving = 0.25f;
        }

        Vector3 translationDir = Vector3.down + _currentMoveDir;

        // 25% chance enemy moves left
        if (_moveCoolDown <= 0 && Random.value <= 0.25f)
        {
            _moveCoolDown += 1;
            _moving = true;
            _currentMoveDir += Vector3.left;
        }

        //25% chance enemy moves right
        else if (_moveCoolDown <= 0 && Random.value >= 0.75f)
        {
            _moveCoolDown += 1;
            _moving = true;
            _currentMoveDir += Vector3.right;
        }

        if (_timeMoving > 0)
        {
            _timeMoving -= Time.deltaTime;
        }

        else if (_timeMoving <= 0)
        {
            _currentMoveDir = new Vector3(0, 0, 0);
            _moving = false;
            _timeMoving = Random.Range(0.5f, 1);
        }

        transform.Translate(translationDir * Time.deltaTime * _speed);

        if (transform.position.y <= _bottomBound)
        {
            Vector3 respawnPos = new Vector3(Random.Range(_leftBound, _rightBound), _topBound);
            transform.position = respawnPos;
        }

        if (!_moving && _moveCoolDown > 0)
        {
            _moveCoolDown -= Time.deltaTime;
        }

        if (transform.position.x <= _leftBound || transform.position.x >= _rightBound)
        {
            _moving = false;
            _timeMoving = 0;
        }
    }
}
