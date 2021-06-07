using System;
using System.Linq;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public enum HeadState
{
    Up, Stable, Down
}

public class HeadHandler : MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FreeTrackData
    {
        public int dataid;
        public int camwidth, camheight;
        public Single Yaw, Pitch, Roll, X, Y, Z;
        public Single RawYaw, RawPitch, RawRoll;
        public Single RawX, RawY, RawZ;
        public Single x1, y1, x2, y2, x3, y3, x4, y4;
    }

    public Queue<HeadState> stateSequence = new Queue<HeadState>();
    private int stateSequenceLimit = 20;

    [HideInInspector]
    public bool didNod = false;

    [HideInInspector]
    public bool isObserving = false;

    // use `static extern` with DllImport to declare a method that is implemented externally.
    // (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/extern)
    // `ref` keyword is like inout in Swift 
    // (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/extern)
    // source code for FreeTrackClient: 
    // https://github.com/PeterN/freetrack/blob/master/FreetrackClient/FTClient.pas
    // source code for OpenTrackClient: 
    // https://github.com/opentrack/opentrack/blob/master/freetrackclient/freetrackclient.c
    [DllImport("FreeTrackClient64")]
    // there will be a function FTGetData in the source code
    public static extern bool FTGetData(ref FreeTrackData data);

    // FreeTrackData: https://github.com/opentrack/opentrack/blob/master/freetrackclient/fttypes.h
    [HideInInspector]
    public float Yaw = 0F;

    [HideInInspector]
    public float Pitch = 0F;

    [HideInInspector]
    public float Roll = 0F;

    [HideInInspector]
    public float X = 0F;

    [HideInInspector]
    public float Y = 0F;

    [HideInInspector]
    public float Z = 0F;

    [HideInInspector]
    public float RawYaw = 0F;

    [HideInInspector]
    public float RawPitch = 0F;

    [HideInInspector]
    public float RawRoll = 0F;

    [HideInInspector]
    public float RawX = 0F;

    [HideInInspector]
    public float RawY = 0F;

    [HideInInspector]
    public float RawZ = 0F;


    [HideInInspector]
    public float x1 = 0F;

    [HideInInspector]
    public float y1 = 0F;

    [HideInInspector]
    public float x2 = 0F;

    [HideInInspector]
    public float y2 = 0F;

    [HideInInspector]
    public float x3 = 0F;

    [HideInInspector]
    public float y3 = 0F;

    [HideInInspector]
    public float x4 = 0F;

    [HideInInspector]
    public float y4 = 0F;

    private HeadHandler.FreeTrackData trackData;

    // Start is called before the first frame update
    void Start()
    {
        trackData = new HeadHandler.FreeTrackData();
    }

    // Update is called once per frame
    void Update()
    {
        if (!HeadHandler.FTGetData(ref trackData))
        {
            Debug.Log("FTGetData returned false. FreeTrack likely not working.");
            return;
        }
        HeadHandler.FTGetData(ref trackData);

        // try to use pitch to detect nods (positive is up) - page 6
        // https://link.springer.com/content/pdf/10.1007%2F978-3-319-07491-7_16.pdf
        Yaw = trackData.Yaw;
        Pitch = trackData.Pitch;
        Roll = trackData.Roll;
        X = trackData.X;
        Y = trackData.Y;
        Z = trackData.Z;

        RawYaw = trackData.RawYaw;
        RawPitch = trackData.RawPitch;
        RawRoll = trackData.RawRoll;

        RawX = trackData.RawX;
        RawY = trackData.RawY;
        RawZ = trackData.RawZ;

        x1 = trackData.x1;
        y1 = trackData.y1;
        x2 = trackData.x2;
        y2 = trackData.y2;
        x3 = trackData.x3;
        y3 = trackData.y3;
        x4 = trackData.x4;
        y4 = trackData.y4;

        var info = string.Format("head X: {0}, head Y: {1}", X, Y);
        // Debug.Log(info);
        // Debug.Log(string.Format("Pitch: {0}", Pitch));

        EyeOnlyHardRunner runnerInstance = GameObject
            .Find("GameRunner")
            .GetComponent<EyeOnlyHardRunner>();

        EyeOnlyEasyRunner runnerEasyInstance = GameObject
            .Find("GameRunner").
            GetComponent<EyeOnlyEasyRunner>();

        Familiarization runnerTrialInstance = GameObject
            .Find("GameRunner").
            GetComponent<Familiarization>();

        if (runnerInstance != null)
        {
            // TODO: handle a variable for condition 3
        }

        if (runnerEasyInstance != null)
        {
            // TODO: handle a variable for condition 3
        }

        if (runnerTrialInstance != null)
        {
            runnerTrialInstance.currentPitchValue = this.Pitch * 100;
        }

        // Important: use opentrack with space shooter profile
        transform.position = new Vector2(-RawYaw * 15, RawPitch * 15);

        // Note: find out a value to replace for 15 to work perfectly 
        // with all size of screen

        if (Global.currentState == TrialState.HeadEye && !didNod && isObserving)
        {
            stateSequenceObseve();
        }
    }

    private void stateSequenceObseve()
    {
        // if (stateSequence.Count == stateSequenceLimit)
        // {
        //     stateSequence.Pop();
        // }
        // switch (Pitch)
        // {
        //     case float value when (value < -0.5):
        //         stateSequence.Push(HeadState.Up);
        //         break;
        //     case float value when (value > 0.5):
        //         stateSequence.Push(HeadState.Down);
        //         break;
        //     default:
        //         stateSequence.Push(HeadState.Stable);
        //         break;
        // }
        // if (stateSequence.Count == stateSequenceLimit)
        // {
        //     var sequence = Array.ConvertAll(stateSequence.ToArray(), item => (HeadState)item);

        //     Array.Reverse(sequence);

        //     foreach (HeadState[] templateSequence in templateSequences)
        //     {
        //         if (sequence.SequenceEqual(templateSequence))
        //         {
        //             this.didNod = true;
        //             break;
        //         }
        //     }
        // }
    }

    public static HeadState[][] templateSequences = new HeadState[][]
    {
        new HeadState[]
        {
            HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable,
            HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable,
            HeadState.Up, HeadState.Up, HeadState.Up, HeadState.Up, HeadState.Up, HeadState.Up, HeadState.Up, HeadState.Stable,
            HeadState.Up, HeadState.Up, HeadState.Up, HeadState.Up, HeadState.Up, HeadState.Up, HeadState.Up, HeadState.Up,
            HeadState.Up, HeadState.Up, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down,
            HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down
        },
        new HeadState[] {
            HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable,
            HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down,
            HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down,
            HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down,
            HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down
        },
        new HeadState[] {
            HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable,
            HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Down, HeadState.Down,
            HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down,
            HeadState.Down, HeadState.Down,  HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down,
            HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down,
            HeadState.Down, HeadState.Down
        },
        new HeadState[] {
            HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Down,
            HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down,
            HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down,
            HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down
        },
        new HeadState[] {
            HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable,HeadState.Down, HeadState.Down,
            HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down
        },
        new HeadState[] {
            HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Stable,
            HeadState.Stable, HeadState.Stable, HeadState.Stable, HeadState.Down, HeadState.Down, HeadState.Down,
            HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down,
            HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down,
            HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down, HeadState.Down
        }
    };
}
