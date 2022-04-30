using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pond : MonoBehaviour
{
    [SerializeField] private GameObject[] FishObj;
    public Bobber Bobber;
    public TestSpawnBobber TestSpawnBobber;

    double timeTillCatchable;
    [SerializeField] float timeTillUncatchable = 1.5f;
    public bool catchable = false;

    [Header("FX/Animiation")]
    [SerializeField] AudioClip CatchableSFX;
    [SerializeField] AudioClip CaughtSFX;
    [SerializeField] AudioClip MissedSFX;

    //public static Pond instance;

    //Checks for user input to catch fish
    private void Update()
    {
        //User input
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Checks if the fish is in a catchable state
            if (catchable==true)
            {
                fishCaught();
            }
        }
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
        Bobber.DestroyBobber();
        TestSpawnBobber.ResetBobbers();
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
    IEnumerator tillCatchable() {
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
        OneShotSoundManager.Instance.PlaySound(MissedSFX, 1);
        Bobber.DestroyBobber();
        TestSpawnBobber.ResetBobbers();
        Debug.Log("Fish escaped!");
        catchable = false;
    }
}
