using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Runner : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Application.isEditor)
            {
                Application.Quit();
            }
        }
    }
    public void quit()
    {
        if (!Application.isEditor)
        {
            Application.Quit();
        }
    }

    public void changeScene(string scene)
    {
        var simpleScene = "EyeOnlySceneSimple";
        var hardScene = "EyeOnlySceneHard";
        var sceneName = "";
        switch (scene)
        {
            case "EyeEasy":
                sceneName = simpleScene;
                Global.currentState = TrialState.Eye;
                break;
            case "EyeHard":
                sceneName = hardScene;
                Global.currentState = TrialState.Eye;
                break;
            case "HeadEyeEasy":
                sceneName = simpleScene;
                Global.currentState = TrialState.HeadEye;
                break;
            case "HeadEyeHard":
                sceneName = hardScene;
                Global.currentState = TrialState.HeadEye;
                break;
            default:
                return;
        }
        SceneManager.LoadScene(sceneName);
    }
}
