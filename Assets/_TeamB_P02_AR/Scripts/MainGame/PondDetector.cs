using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondDetector : MonoBehaviour
{
    [SerializeField] private ParticleSystem _waterSplash;
    private ParticleSystem _waterSplashInstance;
    private ARController arController;

    private void Awake()
    {
        _waterSplashInstance = Instantiate(_waterSplash, transform.position, Quaternion.identity);
        _waterSplashInstance.transform.position = this.transform.position;
    }

    private void Start()
    {
        arController = GameObject.FindObjectOfType<ARController>();
    }

    private void OnEnable()
    {
        _waterSplash.transform.position = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Pond>())
        {
            arController.SearchForFish();
            PondImpact();
        }
    }

    private void PondImpact()
    {
        _waterSplashInstance.gameObject.SetActive(true);
        _waterSplashInstance.Play();
    }

}
