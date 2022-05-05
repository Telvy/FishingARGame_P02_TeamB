using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPond : MonoBehaviour
{
    //TEST DATA
    [Header("Data")]
    [SerializeField] private LayerMask _hitLayers;
    private float range = 10;
    [SerializeField] private Camera camera;
    private TestBobber newTestBobber;
    
    //TEST DATA

    [Header("Main Game Play Objects")]

    [Header("FX/Animiation")]
    [SerializeField] AudioClip _bobberCreatedSFX;
    [SerializeField] AudioClip CatchableSFX;
    [SerializeField] AudioClip CaughtSFX;
    [SerializeField] AudioClip MissedSFX;
    [SerializeField] ParticleSystem _caughtFish;

    [Header("Object Pooling")]
    [SerializeField] TestBobberPool BobberPool;

    //conditional data
    //private bool bobberCreated = false;
    private double timeTillCatchable;
    private float timeTillUncatchable = 1.5f;
    private bool catchable = false;
    private int ActiveBobbers = 1;

    public void Awake()
    {
        //BobberOffset = this.gameObject.transform;
    }

    public void Start()
    {

    }

    //Checks for user input to catch fish
    private void Update()
    {
        //User input
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Checks if the fish is in a catchable state
            if (catchable == true)
            {
                fishCaught();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            SpawnBobber();
            //TestRaycast();
        }
       
    }

    //private void SpawnBobber()
    //{
    //    if(0 < ActiveBobbers)
    //    {
    //        Vector3 direction = Vector3.down;
    //        Ray ray = new Ray(SpawnerPoint.position, transform.TransformDirection(direction * range));
    //        RaycastHit hit;
    //        Debug.DrawRay(SpawnerPoint.position, transform.TransformDirection(direction * range));
    //        if (Physics.Raycast(ray, out hit, _hitLayers))
    //        {
    //            testPond testpond = hit.transform.gameObject.GetComponent<testPond>();
    //            if (testpond != null)
    //            {
    //                Debug.Log("Bobber spawned");
    //                BobberInstance.transform.position = hit.point;
    //                BobberInstance.SetActive(true);
    //                OneShotSoundManager.Instance.PlaySound(_bobberCreatedSFX, 1);
    //                ActiveBobbers--;
    //                //bobberCreated = true;
    //            }
    //        }
    //    }
    //}

    private void TestRaycast()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        RaycastHit hit;
       
        if (Physics.Raycast(ray, out hit, _hitLayers))
        {
            Debug.DrawRay(ray.origin, forward, Color.green, 100f); // only draws once. Re-clicking does nothing
            Debug.Log(hit.transform.name);
        }
    }

    private void OnDrawGizmos()
    {
 
    }
    private void SpawnBobber()
    {
        if(0 < ActiveBobbers)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, _hitLayers))
            {
                Debug.DrawRay(ray.origin, hit.collider.transform.position, Color.green); // only draws once. Re-clicking does nothing
                testPond testpond = hit.transform.gameObject.GetComponent<testPond>();
                if (testpond != null)
                {
                    
                    Debug.Log("Bobber spawned");
                    Debug.Log(hit.point);
                    
                    newTestBobber.AssignPool(BobberPool);
                    newTestBobber = BobberPool.ActivateFromPool();
                    Debug.Log(newTestBobber);
                    newTestBobber.transform.position = hit.point;
                    newTestBobber.gameObject.SetActive(true);
                    OneShotSoundManager.Instance.PlaySound(_bobberCreatedSFX, 1);
                    ActiveBobbers--;
                    //bobberCreated = true;
                }
            }
        }
    }

    public void ResetBobbers()
    {
        ActiveBobbers = 1;
       // bobberCreated = false;
    }

    public void SearchForFish()
    {
        StartCoroutine(tillCatchable());
    }
    public void fishCaught()
    {
        Debug.Log("Fish Caught!");
        OneShotSoundManager.Instance.PlaySound(CaughtSFX, 1);
        Instantiate(_caughtFish, transform.position, Quaternion.identity);
        StopAllCoroutines();
        newTestBobber.testBobberAnimator.SetBool("fishHook", false);
        newTestBobber.gameObject.SetActive(false);
        ResetBobbers();
        catchable = false;
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
        newTestBobber.testBobberAnimator.SetBool("fishHook", true);
        yield return new WaitForSeconds(timeTillUncatchable);
        Debug.Log("Fish escaped!");
        StopAllCoroutines();
        OneShotSoundManager.Instance.PlaySound(MissedSFX, 1);
        newTestBobber.testBobberAnimator.SetBool("fishHook", false);
        newTestBobber.gameObject.SetActive(false);
        ResetBobbers();
        catchable = false;
    }
}
