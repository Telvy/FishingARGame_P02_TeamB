using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARController : MonoBehaviour
{
    public event Action PondCreated = delegate { };

    public GameObject Pond;
    public ARRaycastManager RaycastManager;
    private bool pondCreated = false;

    private void Update()
    {
        CreatePond();
    }

    private void InvokeCreatedPond()
    {
        PondCreated?.Invoke();
    }

    private void CreatePond()
    {
        if (!pondCreated)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                List<ARRaycastHit> touches = new List<ARRaycastHit>();

                RaycastManager.Raycast(Input.GetTouch(0).position, touches, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

                if (touches.Count > 0)
                {

                    GameObject.Instantiate(Pond, touches[0].pose.position, touches[0].pose.rotation);
                    pondCreated = true;
                    InvokeCreatedPond();
                }
            }
        }
    }

}
