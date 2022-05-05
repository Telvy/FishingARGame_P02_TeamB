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
    
    //TEST DATA
    [Header("Main Game Play Objects")]
    private TestBobber newTestBobber;
    [SerializeField] private BaseCatchable[] CatchableObj;

    [Header("FX/Animiation")]
    [SerializeField] AudioClip _bobberCreatedSFX;
    [SerializeField] AudioClip CatchableSFX;
    [SerializeField] AudioClip CaughtSFX;
    [SerializeField] AudioClip MissedSFX;
    [SerializeField] ParticleSystem _caughtFish;
    [SerializeField] Spin SpinningCatchable;

    [Header("Object Pooling")]
    [SerializeField] TestBobberPool BobberPool;

    //conditional data
    private double timeTillCatchable;
    private float timeTillUncatchable = 1.5f;
    private bool catchable = false;
    private int ActiveBobbers = 1;

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
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(RandomDoubleWithinRange(0, 1.0));
        }
       
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
                    newTestBobber = BobberPool.ActivateFromPool();
                    newTestBobber.AssignPool(BobberPool);
                    Debug.Log(newTestBobber);
                    newTestBobber.transform.position = hit.point;
                    newTestBobber.gameObject.SetActive(true);
                    OneShotSoundManager.Instance.PlaySound(_bobberCreatedSFX, 1);
                    ActiveBobbers--;
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
        Debug.Log("Fish Caught!");
        OneShotSoundManager.Instance.PlaySound(CaughtSFX, 1);
        ParticleSystem caughtFishParticles = Instantiate(_caughtFish, transform.position, Quaternion.identity);
        Destroy(caughtFishParticles, 1);
        StopAllCoroutines();
        newTestBobber.testBobberAnimator.SetBool("fishHook", false);
        newTestBobber.gameObject.SetActive(false);
        StartCoroutine(DisplayCatch());
    }

    IEnumerator DisplayCatch()
    {
        float time = 5;
        GameObject caughtFish = Instantiate(DetermineCatch(), SpinningCatchable.transform.position, Quaternion.identity);
        caughtFish.transform.parent = SpinningCatchable.transform;
        Destroy(caughtFish, time);
        yield return new WaitForSeconds(time);
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

    private GameObject DetermineCatch()
    {
        GameObject caughtItem = null;

        double chance = RandomDoubleWithinRange(0, 1.0);
        for (int i = 0; i < CatchableObj.Length; i++)
        {
            if (chance <= CatchableObj[i].catchChance)
            {
                caughtItem = CatchableObj[i].CatchableObj;
                break;
            }
            else
            {
                chance -= CatchableObj[i].catchChance;
            }
        }

        return caughtItem;
    }
}
