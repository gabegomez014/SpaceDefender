using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : Enemy
{
    [SerializeField]
    private Transform _leftSmartShot;
    [SerializeField]
    private Transform _rightSmartShot;
    [SerializeField]
    private GameObject _smartShot;
    [SerializeField]
    private GameObject _laserCharge;
    [SerializeField]
    private GameObject _lasers;
    private GameObject _currentlyInstantiatedGameObject;

    private Image _healthBar;

    [SerializeField]
    private int _maxHealth = 10;
    private int _currentHealth;

    private float _originalSpeed;

    private bool _isIntroPlaying = true; // True until the boss is centered on the screen
    private bool _attackingPlayer = false;

    private Coroutine _laserCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        _shotCooldownTime = 1.5f;
        _bottomBound = 2;
        _topBound = 4;
        _leftBound = -6;
        _rightBound = 6;

        _currentHealth = _maxHealth;
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogWarning("Could not find the audio source for the boss");
        }

        GameObject healthBar = GameObject.Find("BossHealthBar");
        _healthBar = healthBar.GetComponent<Image>();

        if (_healthBar == null)
        {
            Debug.LogWarning("Could not find the Boss's Health Bar on the UI");
        }

        else
        {
            _healthBar.gameObject.SetActive(true);
        }

        StartCoroutine(BossIntro());
    }

    public override void CalculateMovment()
    {

        if (transform.position.x <= _leftBound && !_isIntroPlaying)
        {
            _moveCoolDown += 0.5f;
            _currentMoveDir.x = Random.Range(0.1f, 1);
        }

        else if (transform.position.x >= _rightBound && !_isIntroPlaying)
        {
            _moveCoolDown += 0.5f;
            _currentMoveDir.x = Random.Range(-1, -0.1f);
        }

        else if (transform.position.y <= _bottomBound && !_isIntroPlaying)
        {
            _moveCoolDown += 0.5f;
            _currentMoveDir.y = Random.Range(0.1f, 1);
        }

        else if (transform.position.y >= _topBound && !_isIntroPlaying)
        {
            _moveCoolDown += 0.5f;
            _currentMoveDir.y = Random.Range(-1, -0.1f);
        }

        if (_moveCoolDown <= 0 && !_isIntroPlaying)
        {
            _moveCoolDown += 1;
            _currentMoveDir = Random.insideUnitSphere;
            _currentMoveDir.z = 0;
        }

        else if (_moveCoolDown > 0)
        {
            _moveCoolDown -= Time.deltaTime;
        }

        Debug.Log("Current move direction is " + _currentMoveDir);

        transform.Translate(_currentMoveDir * Time.deltaTime * _speed);
    }

    public override void ScanEnvironment()
    {
        if (_isIntroPlaying)
        {
            return;
        }

        LayerMask layer = LayerMask.GetMask("Player");
        RaycastHit2D laserHit = Physics2D.CircleCast(transform.position, 1.5f, Vector2.down, 10, layer);
        RaycastHit2D leftProjectileHit = Physics2D.CircleCast(_leftSmartShot.position, 1.5f, Vector2.down, 10, layer);
        RaycastHit2D rightProjectileHit = Physics2D.CircleCast(_rightSmartShot.position, 1.5f, Vector2.down, 10, layer);

        if ( laserHit.collider != null && !_attackingPlayer)
        {
            _attackingPlayer = true;
            Shoot();
        }

        if (leftProjectileHit.collider != null)
        {
            if (_currentShotCoolDownTimer <= 0)
            {
                _currentShotCoolDownTimer += _shotCooldownTime;
                Instantiate(_smartShot, _leftSmartShot.position, Quaternion.identity);
            }
        }

        if (rightProjectileHit.collider != null)
        {
            if (_currentShotCoolDownTimer <= 0)
            {
                _currentShotCoolDownTimer += _shotCooldownTime;
                Instantiate(_smartShot, _rightSmartShot.position, Quaternion.identity);
            }
        }
    }

    public override void TakeDamage()
    {
        _currentHealth -= 1;

        if (_currentHealth <= 0)
        {
            _dead = true;
            _speed = 0;
            GameObject explosion = Instantiate(_explosionVFX, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_explosionSFX);
            _healthBar.gameObject.SetActive(false);
            Destroy(explosion, 2.5f);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.5f);
        }

        else
        {
            int vfxToActivate = (_maxHealth - _currentHealth) - 1;
            this.transform.GetChild(vfxToActivate).gameObject.SetActive(true);
            
            _healthBar.fillAmount = (float) _currentHealth / _maxHealth;
        }
    }

    public override void Shoot()
    {
        if (_attackingPlayer)
        {
            _laserCoroutine = StartCoroutine(LaserCoroutine());
        }
    }

    public void AttachHealthBarUI(Image healthBarImg)
    {
        _healthBar = healthBarImg;
    }

    IEnumerator BossIntro()
    {
        _currentMoveDir = new Vector3(0, -1);
        yield return new WaitForSeconds(1);
        _isIntroPlaying = false;
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
