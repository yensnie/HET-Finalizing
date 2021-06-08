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
    public float currentPitchValue = 0F;

    private List<HeadState> tempPetchValues = new List<HeadState>();

    private float currentStablePitch = 0F;

    private float estimatePitchDifference = 3;

    public void Awake() {
        QualitySettings.vSyncCount = 0;     // disable vSync
        Application.targetFrameRate = 30;
    }

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
                tryEyes();
                break;
            case TrialState.Head:
                tryHead();
                break;
            case TrialState.HeadEye:
                tryHeadSupportEye();
                break;
            case TrialState.Order:
                tryEyeHeadOrder();
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

    private void nodRecognition()
    {
        HeadHandler handler = GameObject
            .Find("headCursor")
            .GetComponent<HeadHandler>();
        
        if (Input.GetKey(KeyCode.C))
        {
            if (!handler.isObserving)
            {
                handler.isObserving = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.C)) 
        {
            if (handler.isObserving)
            {
                handler.isObserving = false;
            }
        }

        if (handler.didNod && !handler.isObserving)
        {
            background.GetComponent<SpriteRenderer>().sprite = backgroundRecording;
        }
    }

    // ---------------------- conditions

    private void tryEyes()
    {
        
    }

    private void tryHead()
    {

    }

    private void tryHeadSupportEye()
    {

    }

    private void tryEyeHeadOrder()
    {

    }
}
