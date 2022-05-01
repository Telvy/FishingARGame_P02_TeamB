using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] private ParticleSystem _waterSplash;
    [SerializeField] private LayerMask _hitLayers;
    [SerializeField] private float range = 10;
    private bool waterSplashing = false;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.gameObject.GetComponent<Pond>())
    //    {
    //        //Debug.Log("Pond has collided with bobber");
    //        ParticleSystem waterSplash = Instantiate(_waterSplash, transform.position, Quaternion.identity);
    //        Destroy(waterSplash, 1);
    //    }
    //}

    private void Start()
    {
        StartCoroutine(WaterSplash());
    }

    private void Update()
    {
        DetectPond();
    }

    private void DetectPond()
    {
        Vector3 direction = Vector3.down;
        Ray ray = new Ray(transform.position, transform.TransformDirection(direction * range));
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * range));
        if (Physics.Raycast(ray, out hit, _hitLayers))
        {
            Pond pond = hit.transform.gameObject.GetComponent<Pond>();
            if (pond != null)
            {
                //StartCoroutine(WaterSplash());
                waterSplashing = true;
            }
        }
    }

    IEnumerator WaterSplash()
    {
        while (!waterSplashing)
            yield return null;
        ParticleSystem waterSplash = Instantiate(_waterSplash, transform.position, Quaternion.identity);
        Destroy(waterSplash, 1);
    }

    public void DestroyBobber()
    {
        Destroy(this.gameObject);
    }

}
