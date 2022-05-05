using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] private AudioClip _bobberCreatedNotif;
    [SerializeField] 

    private void OnEnable()
    {
        OneShotSoundManager.Instance.PlaySound(_bobberCreatedNotif, 1);
    }

}
