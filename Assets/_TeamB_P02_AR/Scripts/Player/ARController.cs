using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARController : MonoBehaviour
{
    public event Action PondCreated = delegate { };
    public event Action BobberCreated = delegate { };

    [Header("Main Game Play Objects")]
    [SerializeField] private GameObject Pond;
    [SerializeField] private GameObject Bobber;

    [Header("Audio Feedback")]
    [SerializeField] private AudioClip _pondCreatedNotif;
    [SerializeField] private AudioClip _bobberCreatedNotif;

    [Header("AR Detection")]
    private GameObject lastHitObj;
    public ARRaycastManager RaycastManager;
    public Camera arCamera;

    private bool pondCreated = false;
    private bool bobberCreated = false;

    enum FishingStates
    {
        PONDCREATION,
        BOBBERCREATION,
    }

    private FishingStates _FishingStates;
    private void Start()
    {
        
    }
    private void InvokeCreatedPond()
    {
        PondCreated?.Invoke();
    }

    private void InvokeCreatedBobber()
    {
        BobberCreated?.Invoke();
    }

    public void Update()
    {
        GameFishingStates();
    }

    private void GameFishingStates()
    {
        switch (_FishingStates)
        {
            default: //Create Pond
                CreatingPond();
                break;
            case FishingStates.BOBBERCREATION:
                CreatingBobber();
                break;
        }
    }


    public void CreatingPond()
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
                    OneShotSoundManager.Instance.PlaySound(_pondCreatedNotif, 1);
                    InvokeCreatedPond();
                    _FishingStates = FishingStates.BOBBERCREATION;
                }
            }
        }
    }


    public void CreatingBobber()
    {
        if (!bobberCreated)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {

                    lastHitObj = hit.transform.gameObject;
                    Pond pond = hit.transform.gameObject.GetComponent<Pond>();
                    if (pond != null)
                    {
                        Instantiate(Bobber, hit.point, Quaternion.identity);
                        OneShotSoundManager.Instance.PlaySound(_bobberCreatedNotif, 1);
                        bobberCreated = true;
                        InvokeCreatedBobber();
                    }
                }
            }
        }
    }

}



