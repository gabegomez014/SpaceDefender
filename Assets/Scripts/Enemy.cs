using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;

    private float _bottomBound = -5.5f;
    private float _topBound = 7;
    private float _leftBound = -9.3f;
    private float _rightBound = 9.3f;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogWarning("Could not find animator component");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovment();
    }

    void CalculateMovment()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y <= _bottomBound)
        {
            Vector3 respawnPos = new Vector3(Random.Range(_leftBound, _rightBound), _topBound);
            transform.position = respawnPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            ProjectileBehavior laser = other.GetComponent<ProjectileBehavior>();
            laser.EnemyHit();
            _animator.SetBool("isDestroyed", true);
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }

        else if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            player.HitByEnemy();
            _animator.SetBool("isDestroyed", true);
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }

    }
}
