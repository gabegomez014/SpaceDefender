using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType
{
    SPEED,
    TRIPLESHOT,
    SHIELD,
    HEALTH,
    AMMO,
    HEATEDSHOT,
    SYSTEMOVERRIDE,
    HOMING
}

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;
    [SerializeField]
    private PowerupType _powerupType;
    private bool _isMagnetized = false;
    private Player _player;


    private void Awake()
    {
        _player = GameObject.Find("Player").GetComponent<Player>(); 

        if (_player == null)
        {
            Debug.LogWarning("Could not find the player script");
        }
    }

    private void OnEnable()
    {
        Player.magnetizing = GettingMagnetized;
        Player.notMagnetizing = NotMagnetized;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMagnetized)
        {
            Vector3 directionToPlayer = _player.transform.position - transform.position;
            transform.Translate(directionToPlayer.normalized * Time.deltaTime * _speed * 2); // Making the magnetizination attract the power-ups a bit faster than what they fall
        }

        else
        {
            transform.Translate(Vector3.down * Time.deltaTime * _speed);
        }

        if (transform.position.y <= -6.3f)
        {
            Destroy(this.gameObject);
        }
    }

    public void GettingMagnetized()
    {
        _isMagnetized = true;
    }

    public void NotMagnetized()
    {
        _isMagnetized = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();
            player.PowerupCollected(_powerupType);
            Destroy(this.gameObject);
        }

        else if (collision.tag == "Enemylaser")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
