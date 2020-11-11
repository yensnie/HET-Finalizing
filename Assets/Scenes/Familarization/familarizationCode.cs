using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Research;
using UnityEngine.SceneManagement;

public class familarizationCode : MonoBehaviour
{
    
    [SerializeField] private GameObject countDownPanel;

    private void Start()
    {
        StartCoroutine(SessionOver());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Application.isEditor)
            {
                Application.Quit();
            }
        }
    }


    IEnumerator SessionOver()
    {
        yield return new WaitForSeconds(10);
        countDownPanel.SetActive(true);
    }

    // change to the main menu
    public void changeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
