using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberDetector : MonoBehaviour
{
    private Pond parent;

    private void Start()
    {
        parent = transform.parent.GetComponent<Pond>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bobber>())
        {
            parent.BobberObj = other.gameObject;
            Debug.Log("pond collided with bobber");
        }
    }

}
