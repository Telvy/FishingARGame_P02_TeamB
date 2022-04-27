using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcquireSurfaceState : FishGoState
{
    public override void Enter()
    {
        Debug.Log("entered Acquire Surface State");
        GameController.i.ARRootController.ARRoot.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Tick()
    {
        base.Tick();
    }
}
