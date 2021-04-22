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

    private float topBounds = 0;
    private float bottomBounds = -4;
    private float rightBounds = 9.3f;
    private float leftBounds = -9.3f;

    private Directions horizontalFlag;
    private Directions verticalFlag;

    // Update is called once per frame
    void Update()
    {
        CalculatePlayerMovement();
    }

    void CalculatePlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // Move the GameObject up or down based off User Input if we are not at the bounds already
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

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
}
