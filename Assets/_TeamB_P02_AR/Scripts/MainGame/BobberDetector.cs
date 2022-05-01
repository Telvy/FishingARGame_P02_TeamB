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
        //REMINDER: replace type TestSpawnBobber with type Bobber once we are done
        //testing the general fishing mechanics
        if (other.gameObject.GetComponent<Bobber>())
        {
            //parent.BobberObj.GetComponent<Bobber) = other.GetComponent<Bobber>();
            parent.SearchForFish();
            Debug.Log("pond collided with bobber");
        }
    }

}
