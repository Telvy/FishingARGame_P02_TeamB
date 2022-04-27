using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartState : FishGoState
{
    bool loadFinished = false;

    public override void Enter()
    {
        Debug.Log("entered Start State");
        StartCoroutine(EnterGameScene());
    }

    public override void Tick()
    {
        if (loadFinished)
        {
            stateMachine.ChangeState<SetupGameState>();
        }
    }

    public override void Exit()
    {
        loadFinished = false;
    }

    IEnumerator EnterGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");

        //do not change states until GameScene is finished loading!
        while (!asyncLoad.isDone)
            yield return null;

        loadFinished = true;
    }
}
