using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : FishGoState
{
    public AudioClip _pondCreatedNotif;

    public override void Enter()
    {
        Debug.Log("entered Play State");
        OneShotSoundManager.Instance.PlaySound(_pondCreatedNotif, 1);

    }

    public override void Tick()
    {
        base.Tick();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
