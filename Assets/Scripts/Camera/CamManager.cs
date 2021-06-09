using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;

public class CamManager : MonoBehaviour
{   
    //Inspector field for a Shake Preset to use as the shake parameters.
    public ShakePreset ShakePreset;

    private Shaker MyShaker;

    private void Start()
    {
        MyShaker = GetComponent<Shaker>();

        if (MyShaker == null)
        {
            Debug.LogWarning("Could not find the Shaker component");
        }

        transform.parent.position = new Vector3(0, 1, -10);
    }

    public void PlayerHit()
    {
        MyShaker.Shake(ShakePreset);
    }
}
