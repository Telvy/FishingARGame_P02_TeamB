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
        CreateARRootController();
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

    private void CreateARRootController()
    {
        GameObject ARRootController = Instantiate(GameController.i.ARRootController, new Vector3(0, 0, 0), Quaternion.identity);
        ARRootController.transform.SetParent(GameController.i.transform);
        arController = ARRootController.GetComponent<ARController>();
    }

    IEnumerator SetPondCreated()
    {
        while (!arController)
            yield return null;
        arController.PondCreated += OnPondCreated;
    }


}
