using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcquireSurfaceState : FishGoState
{
    bool surfaceObjCreated = false;
    ARController arController = null;

    public override void Enter()
    {
        Debug.Log("entered Acquire Surface State");
        StartCoroutine(SetPondCreated());  
    }

    public override void Exit()
    {
        arController.PondCreated -= OnPondCreated;
    }

    public void OnPondCreated()
    {
        stateMachine.ChangeState<PlayState>();
    }

    IEnumerator SetPondCreated()
    {
        while (!arController)
            yield return null;
        arController.PondCreated += OnPondCreated;
    }


}
