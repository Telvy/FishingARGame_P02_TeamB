using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    public ParticleSystem _waterSplash;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.GetComponent<Pond>())
        {
            //Debug.Log("Pond has collided with bobber");
            Instantiate(_waterSplash, transform.position, Quaternion.identity);
        }
    }


}
