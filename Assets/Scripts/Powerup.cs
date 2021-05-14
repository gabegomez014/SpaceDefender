using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType
{
    SPEED,
    TRIPLESHOT,
    SHIELD,
    HEALTH
}

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;
    [SerializeField]
    private PowerupType _powerupType;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y <= -6.3f)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();
            player.PowerupCollected(_powerupType);
            Destroy(this.gameObject);
        }
    }
}
