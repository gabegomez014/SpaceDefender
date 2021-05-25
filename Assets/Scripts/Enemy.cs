﻿using System.Collections;
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
    private AudioClip _explosionSFX;
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
    private bool _shieldActivated = false;

    private GameObject _currentShield;

    protected AudioSource _audioSource;

    protected Vector3 _currentMoveDir;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogWarning("Could not find Audio Source component");
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

    // Update is called once per frame
    void Update()
    {
        CalculateMovment();
        ScanEnvironment();
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
        // Do nothing for this main class at the moment
    }

    public virtual void Shoot()
    {
        //Do nothing for this main class at the moment
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            if (_shieldActivated)
            {
                _shieldActivated = false;
                Destroy(_currentShield);
                return;
            }

            ProjectileBehavior laser = other.GetComponent<ProjectileBehavior>();

            laser.EnemyHit();
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

        else if (other.tag == "HeatedShot")
        {
            if (_shieldActivated)
            {
                _shieldActivated = false;
                Destroy(_currentShield);
                return;
            }

            HeatedShotControl shot = other.GetComponent<HeatedShotControl>();
            shot.Explode();
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
