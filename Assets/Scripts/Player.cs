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

    [SerializeField]
    private float cooldownTime = 0.1f;
    private float currentCoolDownTimer = 0;

    private int lives = 3;

    private float topBounds = 0;
    private float bottomBounds = -4;
    private float rightBounds = 9.3f;
    private float leftBounds = -9.3f;

    private Directions horizontalFlag;
    private Directions verticalFlag;

    private bool boostActivated = false;

    // Update is called once per frame
    void Update()
    {
        // Code to switch boostActivated boolean
        if ( Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) )
        {
            boostActivated = true;
        }

        else { boostActivated = false; }

        // Checking all cooldown related aspects for shooting projectiles
        if (currentCoolDownTimer > 0)
        {
            currentCoolDownTimer -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space)) { Shoot(); }

        // Calculate Player movements given User Input
        CalculatePlayerMovement();
    }

    void Shoot()
    {
        if (currentCoolDownTimer <= 0)
        {
            Vector3 spawnLocation = transform.position;
            spawnLocation.y += 0.9f;
            Instantiate(projectile, spawnLocation, Quaternion.identity);
            currentCoolDownTimer += cooldownTime;
        }
    }

    void CalculatePlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // Move the GameObject up or down based off User Input if we are not at the bounds already
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (boostActivated)
        {
            direction = direction * boostMultiplier;
        } 

        if (horizontalFlag == Directions.RIGHT && horizontalInput > 0) { direction.x = 0; }
        else if (horizontalFlag == Directions.LEFT && horizontalInput < 0) { direction.x = 0; }
        if (verticalFlag == Directions.UP && verticalInput > 0) { direction.y = 0; }
        else if (verticalFlag == Directions.DOWN && verticalInput < 0) { direction.y = 0; }

        transform.Translate(direction * Time.deltaTime * speed);

        if (transform.position.y >= topBounds) { verticalFlag = Directions.UP; }

        else if (transform.position.y <= bottomBounds) { verticalFlag = Directions.DOWN; }

        else { verticalFlag = Directions.CLEAR; }

        if (transform.position.x >= rightBounds) {   horizontalFlag = Directions.RIGHT; }

        else if (transform.position.x <= leftBounds) { horizontalFlag = Directions.LEFT; }

        else { horizontalFlag = Directions.CLEAR; }
    }

    public void HitByEnemy()
    {
        lives -= 1;

        if (lives <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
