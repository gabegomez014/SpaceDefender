using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : Enemy
{
    private bool _enemySighted = false;

    public override void CalculateMovment()
    {
        //if (_enemySighted) { return; } // Will be running the Ram Coroutine at this time
        if (_enemySighted)
        {
            _currentMoveDir = Vector2.down;
        }

        base.CalculateMovment();
    }

    public override void ScanEnvironment()
    {
        LayerMask layer = LayerMask.GetMask("Player");

        RaycastHit2D downHit = Physics2D.Raycast(transform.position, Vector2.down, 10, layer);

        if (downHit.collider != null && !_enemySighted)
        {
            _enemySighted = true;
            StartCoroutine(RamAction());
        }

        base.ScanEnvironment();
    }

    private IEnumerator RamAction()
    {
        float tempSpeed = _speed;
        _speed = -1f;
        yield return new WaitForSeconds(0.3f);
        _speed = tempSpeed * 3;
        yield return new WaitForSeconds(0.25f); // Only want to let the ram action happen for a second (mostly so it is not ramming indefinitely)
        _speed = tempSpeed;
        _enemySighted = false;
    }
}
