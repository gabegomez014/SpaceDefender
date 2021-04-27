using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float speed = 10;

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    public void EnemyHit()
    {
        Destroy(this.gameObject);
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        if (transform.position.y >= 7.2f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
