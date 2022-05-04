using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pond : MonoBehaviour
{
    [SerializeField] private AudioClip _pondCreatedNotif;

    private void OnEnable()
    {
        OneShotSoundManager.Instance.PlaySound(_pondCreatedNotif, 1);
    }

}
