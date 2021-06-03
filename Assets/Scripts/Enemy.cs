using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected GameObject _laser;
    [SerializeField]
    private GameObject _shieldPrefab;
    [SerializeField]
    protected AudioClip _laserSFX;
    [SerializeField]
    protected float _speed = 5;
    [SerializeField]
    protected AudioClip _explosionSFX;
    [SerializeField]
    public GameObject _explosionVFX;
    [SerializeField]
    private GameObject _ammoCollectible;

    [SerializeField]
    protected float _shotCooldownTime = 0.3f;
    protected float _currentShotCoolDownTimer = 0;

    protected float _bottomBound = -5.5f;
    protected float _topBound = 7;
    protected float _leftBound = -9.3f;
    protected float _rightBound = 9.3f;
    protected float _moveCoolDown = 0;  // Left and right movement on a cooldown so the enemy does not constantly move left and right
    protected float _timeMoving = 1f;   // To keep track off how long the enemy will move to the left or right

    protected bool _moving = false;
    protected bool _dead = false;
    private bool _beingTracked = false;
    private bool _shieldActivated = false;

    private GameObject _currentShield;

    protected AudioSource _audioSource;

    protected Vector3 _currentMoveDir;


    private void Start()
    {

        // Doing this twice because I got a weird error a few times where the audio source is not found, but no warning is thrown
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();

            if (_audioSource == null)
            {
                Debug.LogWarning("Could not find Audio Source component");
            }
        }

        // Check to see if our enemy gets a shield
        if (Random.value <= 0.25f)
        {
            _currentShield = Instantiate(_shieldPrefab, transform.position,Quaternion.identity, transform);
            if (transform.name == "TheDodgerEnemy(Clone)")
            {
                _currentShield.transform.localPosition = new Vector3(0, 2.1f, 0);
            }
            _shieldActivated = true;
        }
    }

    private void OnEnable()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();

            if (_audioSource == null)
            {
                Debug.LogWarning("Could not find Audio Source component");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovment();
        ScanEnvironment();

        if (_currentShotCoolDownTimer > 0)
        {
            _currentShotCoolDownTimer -= Time.deltaTime;
        }
    }

    public bool GetTrackedStatus()
    {
        return _beingTracked;
    }

    public void SetTrackedStatus(bool status)
    {
        _beingTracked = status;
    }

    public virtual void CalculateMovment()
    {
        if (_dead) { return; }

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
            _currentMoveDir = new Vector3(0,0,0);
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

    public virtual void ScanEnvironment()
    {
        LayerMask mask = LayerMask.GetMask("Powerup");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10, mask);

        if (hit.collider != null && hit.collider.tag == "Powerup")
        {
            if (_dead)
            {
                //This is for when an enemy drops an ammo collectible on death so they cannot destroy that collectible.
                return;
            }

            if (_currentShotCoolDownTimer <= 0)
            {
                Shoot();
            }
        }
    }

    public virtual void TakeDamage()
    {
        if (_shieldActivated)
        {
            _shieldActivated = false;
            Destroy(_currentShield);
            return;
        }

        _dead = true;
        _speed = 0;

        if (Random.value <= 0.2)
        {
            Instantiate(_ammoCollectible, transform.position, Quaternion.identity);
        }

        GameObject explosion = Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        _audioSource.PlayOneShot(_explosionSFX);
        Destroy(explosion, 2.5f);
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 0.5f);
    }

    public virtual void Shoot()
    {
        Instantiate(_laser, transform.position, Quaternion.identity);
        _audioSource.PlayOneShot(_laserSFX);
        _currentShotCoolDownTimer = 3;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            ProjectileBehavior laser = other.GetComponent<ProjectileBehavior>();

            laser.EnemyHit();

            TakeDamage();
        }

        else if (other.tag == "HeatedShot")
        {
            HeatedShotControl shot = other.GetComponent<HeatedShotControl>();
            shot.Explode();

            TakeDamage();
        }

        else if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (_shieldActivated)
            {
                _shieldActivated = false;
                Destroy(_currentShield);
                player.HitByEnemy();
                return;
            }

            player.HitByEnemy();
            _dead = true;
            _speed = 0;
            GameObject explosion = Instantiate(_explosionVFX, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_explosionSFX);
            Destroy(explosion, 2.5f);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.5f);
        }

    }
}
