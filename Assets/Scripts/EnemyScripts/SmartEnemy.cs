using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : Enemy
{
    [SerializeField]
    private GameObject _multiDirectionShot;

    private Vector2 _directionToShoot;

    public override void ScanEnvironment()
    {
        LayerMask layer = LayerMask.GetMask("Player");

        RaycastHit2D downHit = Physics2D.Raycast(transform.position, Vector2.down, 10, layer);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, 10, layer);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, 10, layer);
        RaycastHit2D topHit = Physics2D.Raycast(transform.position, Vector2.up, 10, layer);  // This one checks if the player is behind technically

        if (downHit.collider != null )
        {
            _directionToShoot = Vector2.down;
        }

        else if (rightHit.collider != null)
        {
            _directionToShoot = Vector2.right;
        }

        else if (leftHit.collider != null)
        {
            _directionToShoot = Vector2.left;
        }

        else if (topHit.collider != null)
        {
            _directionToShoot = Vector2.up;
        }

        if (_directionToShoot.magnitude > 0 && _currentShotCoolDownTimer <= 0)
        {
            Shoot();
        }

        base.ScanEnvironment();
    }

    public override void Shoot()
    {  
        if (_directionToShoot.magnitude == 0)
        {
            base.Shoot();
        }

        else
        {
            ProjectileBehavior laser = Instantiate(_multiDirectionShot, transform.position, Quaternion.identity).GetComponent<ProjectileBehavior>();
            laser.SetShotDirection(_directionToShoot);
            _audioSource.PlayOneShot(_laserSFX, 0.75f);

            _currentShotCoolDownTimer = _shotCooldownTime;
            _directionToShoot = new Vector2(0, 0);
        }
    }
}
