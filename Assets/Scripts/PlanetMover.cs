using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMover : MonoBehaviour
{

    [SerializeField]
    private float _minTravelSpeed = 1;

    private float _scale = 1;
    private float _travelSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _scale = Random.Range(0.15f, 0.85f);
        _travelSpeed = _minTravelSpeed / _scale;

        transform.localScale = new Vector3(_scale, _scale, _scale);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _travelSpeed);

        if (transform.position.y <= -10.5f)
        {
            Destroy(this.gameObject);
        }
    }
}
