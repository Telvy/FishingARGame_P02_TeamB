using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : FishGoState
{
    ARController arController = null;

    public override void Enter()
    {
        Debug.Log("entered Play State");
        arController = GameObject.FindObjectOfType<ARController>();
    }

    public override void Tick()
    {
        //arController.CreateBobber();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
