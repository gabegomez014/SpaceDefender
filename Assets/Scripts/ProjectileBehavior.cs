using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float speed = 10;

    private UIManager _uiManager;

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

    void CalculateMovement()
    {
        transform.Translate(transform.up * Time.deltaTime * speed);

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
