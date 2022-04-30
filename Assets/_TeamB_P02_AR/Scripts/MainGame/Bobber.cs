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
            ParticleSystem waterSplash = Instantiate(_waterSplash, transform.position, Quaternion.identity);
            Destroy(waterSplash, 1);
        }
    }

    public void DestroyBobber()
    {
        Destroy(this.gameObject);
    }

}
