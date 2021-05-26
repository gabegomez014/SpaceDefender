using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10;
    private Vector2 _shotDirection;

    private UIManager _uiManager;

    protected float _bottomBound = -7.5f;
    protected float _topBound = 8;
    protected float _leftBound = -11f;
    protected float _rightBound = 11f;

    private void Awake()
    {
        _shotDirection = Vector2.up;
    }

    private void Start()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogWarning("Could not find the UI Manager script");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    public void EnemyHit()
    {
        _uiManager.UpdateScore();
        Destroy(this.gameObject);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetShotDirection(Vector2 dir)
    {
        _shotDirection = dir;
    }

    void CalculateMovement()
    {
        transform.Translate(_shotDirection * Time.deltaTime * _speed);

        if (transform.position.y >= _topBound || transform.position.y <= _bottomBound || transform.position.x <= _leftBound || transform.position.x >= _rightBound)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player" && transform.tag == "Enemylaser")
        {
            other.GetComponent<Player>().HitByEnemy();
            Destroy(this.gameObject);
        }
    }
}
