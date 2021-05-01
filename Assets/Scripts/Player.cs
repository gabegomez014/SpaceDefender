using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Directions
{
    UP,
    LEFT,
    RIGHT,
    DOWN,
    CLEAR
}

public class Player : MonoBehaviour
{
    public float speed = 2;
    public float boostMultiplier = 5;

    public GameObject projectile;
    public GameObject tripleShotPrefab;
    public GameObject shieldPrefab;

    [SerializeField]
    private float speedPowerupMultiplier = 5;

    [SerializeField]
    private float _cooldownTime = 0.1f;
    private float _currentCoolDownTimer = 0;

    private int _lives = 3;

    private float _topBounds = 0;
    private float _bottomBounds = -4;
    private float _rightBounds = 9.3f;
    private float _leftBounds = -9.3f;

    private Directions _horizontalFlag;
    private Directions _verticalFlag;

    private bool _boostActivated = false;
    private bool _tripleShotActivated = false;
    private bool _speedPowerupActivated = false;
    private bool _shieldActivated = false;

    private SpawnManager _spawnManager;

    private UIManager _uiManager;

    private GameObject _currentActivatedShield;

    void Start()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Code to switch boostActivated boolean
        if ( Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) )
        {
            _boostActivated = true;
        }

        else { _boostActivated = false; }

        // Checking all cooldown related aspects for shooting projectiles
        if (_currentCoolDownTimer > 0)
        {
            _currentCoolDownTimer -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space)) { Shoot(); }

        // Calculate Player movements given User Input
        CalculatePlayerMovement();
    }

    void Shoot()
    {
        if (_currentCoolDownTimer <= 0)
        {
            Vector3 spawnLocation = transform.position;
            spawnLocation.y += 1.05f;

            if (_tripleShotActivated)
            {
                // Instantiate triple shot
                spawnLocation.x = spawnLocation.x - 0.5f;
                Instantiate(tripleShotPrefab, spawnLocation, Quaternion.identity);
            }

            else
            {
                Instantiate(projectile, spawnLocation, Quaternion.identity);
            }

            _currentCoolDownTimer += _cooldownTime;
        }
    }

    void CalculatePlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // Move the GameObject up or down based off User Input if we are not at the bounds already
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_boostActivated)
        {
            direction = direction * boostMultiplier;
        } 

        if (_speedPowerupActivated)
        {
            direction = direction * speedPowerupMultiplier;
        }

        if (_horizontalFlag == Directions.RIGHT && horizontalInput > 0) { direction.x = 0; }
        else if (_horizontalFlag == Directions.LEFT && horizontalInput < 0) { direction.x = 0; }
        if (_verticalFlag == Directions.UP && verticalInput > 0) { direction.y = 0; }
        else if (_verticalFlag == Directions.DOWN && verticalInput < 0) { direction.y = 0; }

        transform.Translate(direction * Time.deltaTime * speed);

        if (transform.position.y >= _topBounds) { _verticalFlag = Directions.UP; }

        else if (transform.position.y <= _bottomBounds) { _verticalFlag = Directions.DOWN; }

        else { _verticalFlag = Directions.CLEAR; }

        if (transform.position.x >= _rightBounds) {   _horizontalFlag = Directions.RIGHT; }

        else if (transform.position.x <= _leftBounds) { _horizontalFlag = Directions.LEFT; }

        else { _horizontalFlag = Directions.CLEAR; }
    }

    public void HitByEnemy()
    {
        if (_shieldActivated)
        {
            _shieldActivated = false;
            Destroy(_currentActivatedShield);
            return;
        }

        _lives -= 1;
        _uiManager.DecreaseLife(_lives);

        if (_lives <= 0)
        {
            _spawnManager.StopSpawning();
            Destroy(this.gameObject);
        }
    }

    public void PowerupCollected(PowerupType powerupType)
    {
        if (powerupType == PowerupType.TRIPLESHOT)
        {
            _tripleShotActivated = true;
            StartCoroutine(TripleShotPowerDownRoutine());
        }

        else if (powerupType == PowerupType.SPEED)
        {
            _speedPowerupActivated = true;
            StartCoroutine(SpeedPowerDownRoutine());
        }

        else if (powerupType == PowerupType.SHIELD)
        {
            _currentActivatedShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity, transform);
            _shieldActivated = true;
            StartCoroutine(ShieldPowerDownRoutine());
        }
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _tripleShotActivated = false;
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(3);
        _speedPowerupActivated = false;
        
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(4);
        if (_currentActivatedShield != null)
        {
            _shieldActivated = false;
            Destroy(_currentActivatedShield);
        }

    }
}
