using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawnBobber : MonoBehaviour
{

    [Header("Data")]
    [SerializeField] private GameObject Bobber;
    [SerializeField] private Camera camera;
    private float range = 10;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBobber1();
        }

    }
    private void SpawnBobber1()
    {
        Vector3 direction = Vector3.down;
        Ray ray = new Ray(transform.position, transform.TransformDirection(direction * range));
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * range));

        if (Physics.Raycast(ray, out hit))
        {
            Pond pond = hit.transform.gameObject.GetComponent<Pond>();
            if(pond != null)
            {
                Debug.Log("Bobber spawned");
                Instantiate(Bobber, hit.point, Quaternion.identity);
            }
        }
    }

    private void SpawnBobber2()
    {
        Vector3 touchPos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
    }

}
