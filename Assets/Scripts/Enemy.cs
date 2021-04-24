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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.tag);

        if (other.tag == "Laser")
        {
            ProjectileBehavior laser = other.GetComponent<ProjectileBehavior>();
            laser.EnemyHit();
        }

        else if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            player.HitByEnemy();
        }

        Destroy(this.gameObject);
    }
}
