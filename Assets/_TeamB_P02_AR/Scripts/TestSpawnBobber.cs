using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestSpawnBobber : MonoBehaviour
{

    [Header("Data")]
    [SerializeField] private GameObject Bobber;
    [SerializeField] private AudioClip _bobberCreatedSFX;
    [SerializeField] private LayerMask _hitLayers;
    private float range = 10;
    private int ActiveBobbers = 1;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (0 < ActiveBobbers)
            {
                SpawnBobber();
                ActiveBobbers--;
                //Pond.instance.SearchForFish();
            }
        }

    }

    public void ResetBobbers()
    {
        ActiveBobbers = 1;
    }

    private void SpawnBobber()
    {
        Vector3 direction = Vector3.down;
        Ray ray = new Ray(transform.position, transform.TransformDirection(direction * range));
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * range));

        if (Physics.Raycast(ray, out hit, _hitLayers))
        {
            Pond pond = hit.transform.gameObject.GetComponent<Pond>();
            if(pond != null)
            {
                Debug.Log("Bobber spawned");
                OneShotSoundManager.Instance.PlaySound(_bobberCreatedSFX, 1);
                Instantiate(Bobber, hit.point, Quaternion.identity);
            }
        }
    }


}
