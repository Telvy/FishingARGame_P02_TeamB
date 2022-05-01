using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    public event Action PondTouched = delegate { };

    [SerializeField] private ParticleSystem _waterSplash;
    [SerializeField] private LayerMask _hitLayers;
    [SerializeField] private float range = 10;

    private Pond pond;
    private ARController arController;
    private bool waterSplashing = false;

    private void Start()
    {
        //StartCoroutine(PondImpact());
        arController = GameObject.FindObjectOfType<ARController>();
    }

    private void OnEnable()
    {
        this.PondTouched += PondImpact;
    }

    private void Update()
    {
        DetectPond();
    }

    private void InvokePondTouched()
    {
        PondTouched?.Invoke();
    }

    private void DetectPond()
    {
        Vector3 direction = Vector3.down;
        Ray ray = new Ray(transform.position, transform.TransformDirection(direction * range));
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * range));
        if (Physics.Raycast(ray, out hit, _hitLayers))
        {
            pond = hit.transform.gameObject.GetComponent<Pond>();
            if (pond != null)
            {
                InvokePondTouched();
            }
        }
    }

    private void PondImpact()
    {
        arController.SearchForFish();
        ParticleSystem waterSplash = Instantiate(_waterSplash, transform.position, Quaternion.identity);
        Destroy(waterSplash, 1);
        this.PondTouched -= PondImpact;
    }

    public void DestroyBobber()
    {
        Destroy(this.gameObject);
    }

}
