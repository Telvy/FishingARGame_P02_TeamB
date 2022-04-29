using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFoundObj : MonoBehaviour
{
    ARController arController = null;
    void Start()
    {
        FoundObj();
    }

    public void FoundObj()
    {
        arController = GameObject.FindObjectOfType<ARController>();
        if(arController != null)
        {
            Debug.Log("Found Obj!");
        }
        else
        {
            Debug.Log("no Obj found...");
        }
    }
}
