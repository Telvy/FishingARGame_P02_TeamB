using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

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
}