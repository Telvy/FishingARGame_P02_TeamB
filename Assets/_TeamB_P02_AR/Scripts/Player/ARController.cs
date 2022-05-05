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
    private Transform BobberOffset;

    [Header("UI Elements")]
    [SerializeField] private GameObject PlacePondUI;
    [SerializeField] private GameObject PlaceBobberUI;
    [SerializeField] private GameObject CatchFishUI;

    [Header("Audio Feedback")]
    [SerializeField] private AudioClip CatchableSFX;
    [SerializeField] private AudioClip CaughtSFX;
    [SerializeField] private AudioClip MissedSFX;

    [Header("VFX")]
    [SerializeField] private ParticleSystem CaughtFish;

    [Header("AR Detection")]
    public ARRaycastManager RaycastManager;
    public Camera arCamera;
    [SerializeField] private LayerMask _bobberLayer, _pondLayer;

    //conditional data
    private bool pondCreated = false;
    private bool bobberCreated = false;
    private double timeTillCatchable;
    private float timeTillUncatchable = 1.5f;
    private bool catchable = false;
    private int ActiveBobbers = 1;

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
                    Debug.Log("Pond created");
                    GameObject.Instantiate(PondObj, touches[0].pose.position, touches[0].pose.rotation);
                    pondCreated = true;

                    //Close Place Pond UI and open Place Bobber UI
                    PlacePondUI.SetActive(false);
                    PlaceBobberUI.SetActive(true);

                    InvokeCreatedPond();
                    _FishingStates = FishingStates.BOBBERCREATION;
                }
            }
        }
    }

    public void CreatingBobber()
    {
        if (0 < ActiveBobbers)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, _pondLayer))
                {
                    Pond pond = hit.transform.gameObject.GetComponent<Pond>();
                    Debug.Log(hit.point);
                    if (pond != null)
                    {
                        Debug.Log("Bobber spawned");
                        BobberInstance.transform.position = hit.point;
                        BobberInstance.SetActive(true);

                        //Close Place Bobber UI and open Catch Fish UI
                        PlaceBobberUI.SetActive(false);
                        CatchFishUI.SetActive(true);

                        ActiveBobbers--;
                        _FishingStates = FishingStates.CATCHINGFISH;
                    }
                }
            }
        }
    }

    public void CatchingFish()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
            Ray ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _bobberLayer))
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


    public void ResetBobbers()
    {
        ActiveBobbers = 1;
    }

    public void SearchForFish()
    {
        StartCoroutine(tillCatchable());
    }
 
    public void fishCaught()
    {
        Animator animator = BobberInstance.GetComponent<Animator>();
        Debug.Log("Fish Caught!");
        OneShotSoundManager.Instance.PlaySound(CaughtSFX, 1);
        Instantiate(CaughtFish, transform.position, Quaternion.identity);
        StopAllCoroutines();
        animator.SetBool("fishHook", false);
        BobberInstance.SetActive(false);
        ResetBobbers();
        catchable = false;



        //Close Catch Fish and open Place Bobber UI

        CatchFishUI.SetActive(false);
        PlaceBobberUI.SetActive(true);

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
        Animator animator = BobberInstance.GetComponent<Animator>();
        catchable = true;
        OneShotSoundManager.Instance.PlaySound(CatchableSFX, 1);
        animator.SetBool("fishHook", true);
        yield return new WaitForSeconds(timeTillUncatchable);
        Debug.Log("Fish escaped!");
        StopAllCoroutines();
        OneShotSoundManager.Instance.PlaySound(MissedSFX, 1);
        animator.SetBool("fishHook", false);
        BobberInstance.SetActive(false);
        ResetBobbers();
        catchable = false;

        //Close Catch Fish and open Place Bobber UI
        CatchFishUI.SetActive(false);
        PlaceBobberUI.SetActive(true);

        _FishingStates = FishingStates.BOBBERCREATION;
    }
}



