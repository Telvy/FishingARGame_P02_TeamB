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
       

    }

    public void ResetBobbers()
    {
        ActiveBobbers = 1;
    }

    


}
