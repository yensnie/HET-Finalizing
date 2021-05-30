
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;
using Tobii.Research.Unity;

public class Scene1 : MonoBehaviour
{
    private EyeTracker _eyeTracker;

    void Start()
    {
        _eyeTracker = EyeTracker.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // Quit if escape is pressed.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Application.isEditor)
            {
                Application.Quit();
            }
        }

        // We are expecting to have all objects.
        if (!_eyeTracker)
        {
            Debug.Log("no eye tracker instance");
            return;
        }

        // Thin out updates a bit.
        if (Time.frameCount % 6 != 0)
        {
            //return;
        }
        /*
        var info = string.Format("L:{0} R: {1}\n",
        _eyeTracker.LatestProcessedGazeData.Left.GazeOriginValid ? _eyeTracker.LatestProcessedGazeData.Left.GazeRayScreen.ToString() : "No gaze",
        _eyeTracker.LatestProcessedGazeData.Right.GazeOriginValid ? _eyeTracker.LatestProcessedGazeData.Right.GazeRayScreen.ToString() : "No gaze");
        Debug.Log(info);*/
        
        
        var leftX = _eyeTracker.LatestProcessedGazeData.Left.GazePointOnDisplayArea.x;
        var leftY = _eyeTracker.LatestProcessedGazeData.Left.GazePointOnDisplayArea.y;
        var rightX = _eyeTracker.LatestProcessedGazeData.Right.GazePointOnDisplayArea.x;
        var rightY = _eyeTracker.LatestProcessedGazeData.Right.GazePointOnDisplayArea.y;
        transform.position = new Vector3((leftX + rightX) / 2, (leftY + rightY) / 2, 0);
        Debug.Log(string.Format(
            "object corrdinate : x: {0}, y: {1}",
            (leftX + rightX) / 2,
            (leftY + rightY) / 2
            ));
    }
}