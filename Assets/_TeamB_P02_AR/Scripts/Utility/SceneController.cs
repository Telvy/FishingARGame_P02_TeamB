using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController i;

    Scene currentScene;

    private void Awake()
    {
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ExitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }

    public void ChangeState()
    {
        GameController.i.FishGoSM.ChangeState<AcquireSurfaceState>();
    }

    private void Update()
    {
        //currentScene = SceneManager.GetActiveScene();
        //string sceneName = currentScene.name;

        //if(sceneName == "GameScene")
        //{
        //    ChangeState();
        //}
        //else if(sceneName == "testAR")
        //{
        //    ChangeState();
        //}
        //else
        //{
        //    return;
        //}
    }
}