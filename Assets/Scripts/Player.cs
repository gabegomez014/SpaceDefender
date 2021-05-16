using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject[] shieldPrefabs;

    public AudioClip laserShotSFX;
    public AudioClip outOfAmmoSFX;

    [SerializeField]
    private AudioClip _powerupSFX;

    [SerializeField]
    private float speedPowerupMultiplier = 5;

    [SerializeField]
    private float _cooldownTime = 0.1f;
    private float _currentCoolDownTimer = 0;

    [SerializeField]
    private Image _boostChargeMeter;

    [SerializeField]
    private CamManager _camManager;

    private int _lives = 3;
    private int _shieldsAmount = 0;
    [SerializeField]
    private int _maxBoostCharge = 5;

    [SerializeField]
    private float _boostRechargeRate;
    private float _topBounds = 0;
    private float _bottomBounds = -4;
    private float _rightBounds = 9.3f;
    private float _leftBounds = -9.3f;
    private float _currentBoostCharge;

    private Directions _horizontalFlag;
    private Directions _verticalFlag;

    private bool _boostActivated = false;
    private bool _tripleShotActivated = false;
    private bool _speedPowerupActivated = false;
    private bool _shieldActivated = false;

    private SpawnManager _spawnManager;

    private UIManager _uiManager;

    private AudioSource _audioSource;

    private int _ammoCount = 15;

    private GameObject _currentActivatedShield;
    [SerializeField]
    private GameObject _rightEngineFire;
    [SerializeField]
    private GameObject _leftEngineFire;
    [SerializeField]
    private GameObject _normalThrusters;
    [SerializeField]
    private GameObject _boostThrusters;

    void Start()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_boostChargeMeter == null)
        {
            _boostChargeMeter = _uiManager.transform.Find("BoostUI").GetComponent<Image>();
        }

        if (_camManager == null)
        {
            _camManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamManager>();
        }

        _currentBoostCharge = _maxBoostCharge;
    }

    // Update is called once per frame
    void Update()
    {
        // Code to switch boostActivated boolean
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ActivateBoost();
        }

        else { ActivateNormalThrusters(); }

        if (_boostActivated)
        {
            if (_currentBoostCharge > 0)
            {
                _currentBoostCharge -= Time.deltaTime;
            }

            else
            {
                ActivateNormalThrusters();
            }

            UpdateBoostMeter();
        }

        else
        {
            if (_currentBoostCharge < _maxBoostCharge)
            {
                _currentBoostCharge += Time.deltaTime * _boostRechargeRate;
            }

            else
            {
                _currentBoostCharge = _maxBoostCharge;
            }

            UpdateBoostMeter();
        }

        // Checking all cooldown related aspects for shooting projectiles
        if (_currentCoolDownTimer > 0)
        {
            _currentCoolDownTimer -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space)) { Shoot(); }

        // Calculate Player movements given User Input
        CalculatePlayerMovement();
    }

    void UpdateBoostMeter()
    {
        _boostChargeMeter.fillAmount = _currentBoostCharge / _maxBoostCharge;
    }

    void ActivateBoost()
    {
        _boostActivated = true;
        _normalThrusters.SetActive(false);
        _boostThrusters.SetActive(true);
    }

    void ActivateNormalThrusters()
    {
        _boostActivated = false;
        _normalThrusters.SetActive(true);
        _boostThrusters.SetActive(false);
    }

    void Shoot()
    {
        if (_currentCoolDownTimer <= 0)
        {

            Vector3 spawnLocation = transform.position;
            spawnLocation.y += 1.85f;

            if (_tripleShotActivated)
            {
                // Instantiate triple shot
                spawnLocation.x = spawnLocation.x - 0.5f;
                Instantiate(tripleShotPrefab, spawnLocation, Quaternion.identity);
                _audioSource.PlayOneShot(laserShotSFX);
            }

            else if (_ammoCount > 0)
            {
                _ammoCount -= 1;
                _uiManager.DecreaseAmmo(_ammoCount);
                Instantiate(projectile, spawnLocation, Quaternion.identity);
                _audioSource.PlayOneShot(laserShotSFX);
            }

            else
            {
                _audioSource.PlayOneShot(outOfAmmoSFX, 1);
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

        if (transform.position.x >= _rightBounds) { _horizontalFlag = Directions.RIGHT; }

        else if (transform.position.x <= _leftBounds) { _horizontalFlag = Directions.LEFT; }

        else { _horizontalFlag = Directions.CLEAR; }
    }

    public void HitByEnemy()
    {
        if (_shieldActivated)
        {
            _shieldsAmount -= 1;

            if (_shieldsAmount <= 0)
            {
                _shieldsAmount = 0;
                _shieldActivated = false;
            }

            Destroy(_currentActivatedShield);

            if (_shieldsAmount > 0)
            {
                _currentActivatedShield = Instantiate(shieldPrefabs[_shieldsAmount - 1], transform.position, Quaternion.identity, transform);
            }

            return;
        }

        _lives -= 1;
        _uiManager.DecreaseLife(_lives);
        _camManager.PlayerHit();

        if (_lives == 2)
        {
            _leftEngineFire.SetActive(true);
        }

        if (_lives == 1)
        {
            _rightEngineFire.SetActive(true);
        }

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
            if (_shieldActivated)
            {
                return;
            }
            _currentActivatedShield = Instantiate(shieldPrefabs[2], transform.position, Quaternion.identity, transform);
            _shieldActivated = true;
            _shieldsAmount += 3;
        }

        else if (powerupType == PowerupType.HEALTH)
        {
            if (_lives < 3)
            {
                _lives += 1;
                _uiManager.IncreaseLife(_lives);
            }

            if (_lives == 3)
            {
                _leftEngineFire.SetActive(false);
            }

            if (_lives == 2)
            {
                _rightEngineFire.SetActive(false);
            }
        }

        else if (powerupType == PowerupType.AMMO)
        {
            _ammoCount = 15;
            _uiManager.AmmoRefilled();
        }

        _audioSource.PlayOneShot(_powerupSFX);
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
}
