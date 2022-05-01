using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class TestBobber : MonoBehaviour
{
    [SerializeField] private ParticleSystem _waterSplash;
    [SerializeField] private LayerMask _hitLayers;
    [SerializeField] private float range = 10;

    private testPond pond;
    private bool waterSplashing = false;

    private void OnEnable()
    {
        //DetectPond();
    }

    private void Start()
    {
        //pond = GameObject.FindObjectOfType<testPond>();
    }
    //public void DetectPond()
    //{
    //    Vector3 direction = Vector3.down;
    //    Ray ray = new Ray(transform.position, transform.TransformDirection(direction * range));
    //    RaycastHit hit;
    //    Debug.DrawRay(transform.position, transform.TransformDirection(direction * range));
    //    if (Physics.Raycast(ray, out hit, _hitLayers))
    //    {
    //        pond = hit.transform.gameObject.GetComponent<testPond>();
    //        if (pond != null)
    //        {
    //            pond.SearchForFish();
    //            ParticleSystem waterSplash = Instantiate(_waterSplash, transform.position, Quaternion.identity);
    //            Destroy(waterSplash, 1);
    //        }
    //    }
    //}

    private void PondImpact()
    {
       // pond.SearchForFish();
        ParticleSystem waterSplash = Instantiate(_waterSplash, transform.position, Quaternion.identity);
        Destroy(waterSplash, 1);
    }

    public void DestroyBobber()
    {
        Destroy(this.gameObject);
    }

}
