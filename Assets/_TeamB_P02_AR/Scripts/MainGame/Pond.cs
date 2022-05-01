using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pond : MonoBehaviour
{
    //TEST DATA
    [Header("Data")]
    [SerializeField] private LayerMask _hitLayers;
    [SerializeField] private Transform SpawnerPoint;
    private float range = 10;
    private int ActiveBobbers = 1;
    //TEST DATA




    [Header("Main Game Play Objects")]
    [SerializeField] private GameObject PondObj;
    [SerializeField] public GameObject BobberObj;
    private GameObject BobberInstance = null;
    [SerializeField] private Transform BobberOffset;

    [Header("FX/Animiation")]
    [SerializeField] AudioClip _bobberCreatedSFX;
    [SerializeField] AudioClip CatchableSFX;
    [SerializeField] AudioClip CaughtSFX;
    [SerializeField] AudioClip MissedSFX;

    //conditional data
    private bool pondCreated = false;
    private bool bobberCreated = false;
    //private int ActiveBobbers = 5;
    private double timeTillCatchable;
    private float timeTillUncatchable = 1.5f;
    private bool catchable = false;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!bobberCreated)
            {
                SpawnBobber();
                //ActiveBobbers--;
            }
        }
    }

    private void SpawnBobber()
    {
        Vector3 direction = Vector3.down;
        Ray ray = new Ray(SpawnerPoint.position, transform.TransformDirection(direction * range));
        RaycastHit hit;
        Debug.DrawRay(SpawnerPoint.position, transform.TransformDirection(direction * range));

        if (Physics.Raycast(ray, out hit, _hitLayers))
        {
            Pond pond = hit.transform.gameObject.GetComponent<Pond>();
            if (pond != null)
            {
                Debug.Log("Bobber spawned");
                BobberInstance.transform.position = hit.point;
                BobberInstance.SetActive(true);
                OneShotSoundManager.Instance.PlaySound(_bobberCreatedSFX, 1);
                bobberCreated = true;
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
       // _FishingStates = FishingStates.BOBBERCREATION;
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
        OneShotSoundManager.Instance.PlaySound(MissedSFX, 1);
        BobberInstance.SetActive(false);
        ResetBobbers();
        catchable = false;
        //_FishingStates = FishingStates.BOBBERCREATION;
    }
}
