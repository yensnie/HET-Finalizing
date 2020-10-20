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
        SceneManager.LoadScene(scene);
    }
}
