using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARController : MonoBehaviour
{
    public event Action PondCreated = delegate { };
    //public event Action BobberCreated = delegate { };

    [Header("Main Game Play Objects")]
    [SerializeField] private GameObject PondObj;
    [SerializeField] public GameObject BobberObj;
    private GameObject BobberInstance = null;
    [SerializeField] private Transform BobberOffset;

    [Header("Audio Feedback")]
    [SerializeField] private AudioClip _pondCreatedNotif;
    [SerializeField] private AudioClip _bobberCreatedNotif;
    [SerializeField] private AudioClip CatchableSFX;
    [SerializeField] private AudioClip CaughtSFX;
    [SerializeField] private AudioClip MissedSFX;

    [Header("AR Detection")]
    public ARRaycastManager RaycastManager;
    public Camera arCamera;
    [SerializeField] private LayerMask _hitLayers;

    //conditional data
    private bool pondCreated = false;
    private bool bobberCreated = false;
    //private int ActiveBobbers = 5;
    private double timeTillCatchable;
    private float timeTillUncatchable = 1.5f;
    private bool catchable = false;

    enum FishingStates
    {
        PONDCREATION,
        BOBBERCREATION,
        CATCHINGFISH
    }

    private FishingStates _FishingStates;

    private void InvokeCreatedPond()
    {
        PondCreated?.Invoke();
    }

    //private void InvokeCreatedBobber()
    //{
    //    BobberCreated?.Invoke();
    //}

    public void Awake()
    {
        BobberOffset = this.gameObject.transform;
    }

    public void Start()
    {
        BobberInstance = Instantiate(BobberObj, BobberOffset.position, Quaternion.identity);
        BobberInstance.transform.position = BobberOffset.position;
        BobberInstance.SetActive(false);
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
            case FishingStates.CATCHINGFISH:
                CatchingFish();
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
                    GameObject.Instantiate(PondObj, touches[0].pose.position, touches[0].pose.rotation);
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
                if (Physics.Raycast(ray, out hit, _hitLayers))
                {
                    Pond pond = hit.transform.gameObject.GetComponent<Pond>();
                    if (pond != null)
                    {
                        BobberInstance.transform.position = hit.point;
                        BobberInstance.SetActive(true);
                        OneShotSoundManager.Instance.PlaySound(_bobberCreatedNotif, 1);
                        bobberCreated = true;
                        //ActiveBobbers--;
                        _FishingStates = FishingStates.CATCHINGFISH;
                        //InvokeCreatedBobber();
                    }
                }
            }
        }
    }

    public void CatchingFish()
    {
        if (bobberCreated)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, _hitLayers))
                {
                    GameObject lastHitObj = hit.transform.gameObject;
                    Bobber bobber = lastHitObj.GetComponent<Bobber>();
                    if (bobber != null)
                    {
                        if (catchable == true)
                        {
                            fishCaught();
                        }
                    }
                }
            }
        }  
    }


    public void ResetBobbers()
    {
        //ActiveBobbers = 1;
        bobberCreated = false;
    }

    public void SearchForFish()
    {
        StartCoroutine(tillCatchable());
    }
 
    public void fishCaught()
    {
        Debug.Log("Fish Caught!");
        OneShotSoundManager.Instance.PlaySound(CaughtSFX, 1);
        StopAllCoroutines();
        BobberInstance.SetActive(false);
        ResetBobbers();
        catchable = false;
        _FishingStates = FishingStates.BOBBERCREATION;
    }
    //Returns a pseudorandom double between the two values passed in
    public double RandomDoubleWithinRange(double lowerLimit, double upperLimit)
    {
        //defines a random variable
        var random = new Random();
        //uses random variable to instaniate a pseudorandom double between 0 & 1
        var rDouble = random.NextDouble();
        //Converts the random double to a number between lowerLimit & upperLimit
        rDouble = rDouble * (upperLimit - lowerLimit) + lowerLimit;
        return rDouble;
    }
    //A coroutine to wait a random amount of time till the fish is catchable
    IEnumerator tillCatchable()
    {
        //gets a random double between 1-10
        timeTillCatchable = RandomDoubleWithinRange(1, 10);
        //Waits timeTillCatchable (in seconds)
        yield return new WaitForSeconds((float)timeTillCatchable);
        StartCoroutine(tillUncatchable());
    }
    IEnumerator tillUncatchable()
    {
        catchable = true;
        OneShotSoundManager.Instance.PlaySound(CatchableSFX, 1);
        yield return new WaitForSeconds(timeTillUncatchable);
        Debug.Log("Fish escaped!");
        StopAllCoroutines();
        OneShotSoundManager.Instance.PlaySound(MissedSFX, 1);
        BobberInstance.SetActive(false);
        ResetBobbers();
        catchable = false;
        _FishingStates = FishingStates.BOBBERCREATION;
    }
}



