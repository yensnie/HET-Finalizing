using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Familiarization : MonoBehaviour
{
    enum Position
    {
        Up, Down, Left, Right
    }

    enum RecordState
    {
        Off, On
    }
    
    public GameObject background;

    public GameObject sampleObject;

    public Sprite backgroundNormal;
    public Sprite backgroundRecording;

    [HideInInspector]
    public bool didEyeSelect = false;

    [HideInInspector]
    public bool didHeadSelect = true;

    private RecordState currentRecordState = RecordState.Off;

    [HideInInspector]
    public float currentPitchValue = 0.0f;

    private List<float> tempPetchValues = new List<float>();

    void Start()
    {
        Helper.prepareCursors();
        randomizePosition();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Application.isEditor)
        {
            Application.Quit();
        }

        switch (Global.currentState)
        {
            case TrialState.Eye:
                break;
            case TrialState.Head:
                break;
            case TrialState.HeadEye:
                break;
            case TrialState.Order:
                break;
            case TrialState.Trial:
                nodRecognition();
                break;
        }
    }

    private void randomizePosition()
    {
        var positions = Enum.GetValues(typeof(Position));
        var random = new System.Random();
        var newPosition = (Position)positions
            .GetValue(random.Next(positions.Length));

        switch (newPosition)
        {
            case Position.Up:
                // do nothing, since it is already on top from initial
                sampleObject.transform.position = new Vector2(0, 3);
                break;
            case Position.Down:
                sampleObject.transform.position = new Vector2(0, -3);
                break;
            case Position.Left:
                sampleObject.transform.position = new Vector2(-4, 0);
                break;
            case Position.Right:
                sampleObject.transform.position = new Vector2(4, 0);
                break;
        }
    }

    // TODO: 4 conditions

    // TODO: record the head with a button tap
    private void nodRecognition()
    {
        if (Input.GetKey(KeyCode.C))
        {
            if (currentRecordState == RecordState.Off)
            {
                currentRecordState = RecordState.On;
                background.GetComponent<SpriteRenderer>().sprite = backgroundRecording;
            }

            if (currentRecordState == RecordState.On)
            {
                tempPetchValues.Add(currentPitchValue);
            }
        }

        if (Input.GetKeyUp(KeyCode.C)) 
        {
            // turn to Off
            if (currentRecordState == RecordState.On)
            {
                currentRecordState = RecordState.Off;
                background.GetComponent<SpriteRenderer>().sprite = backgroundNormal;
                // save data
                string textToSave = tempPetchValues.ToString();
                string moment = DateTime.Now.ToFileTime().ToString();
                string fileName = "data" + moment;
                string path = Application.dataPath + "/" + "Saved test data" + "/" + fileName;

                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    File.WriteAllText(path, textToSave);
                }

                // clear the temp array
                tempPetchValues = new List<float>();
            }
        }       
    }
}
