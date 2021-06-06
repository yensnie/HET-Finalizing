using System;
using System.Linq;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;

public class HeadHandler : MonoBehaviour
{
    enum HeadState
    {
        Up, Stable, Down
    }
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

    public Stack stateSequence = new Stack();
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
            runnerTrialInstance.currentPitchValue = this.Pitch;
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
/*
sample from stable to up

  0.09173943  0.09173254  0.09172636  0.09172062  0.0917168  0.09171275  0.09171075  0.09170461  0.09169844  0.09169473  0.09169363  0.09168824  0.09167854  0.09166034  0.09164134  0.09159151  0.09150608  0.09090519  0.08956576  0.07838697  0.06243464  0.01870899  -0.02766373  -0.06186529  -0.08768398  -0.1238028  -0.1588343  -0.1967616  -0.2206765  -0.2401465  -0.2596805  -0.2412996  -0.2095762  -0.2125795  -0.2424917

*/

/*
sample from stable to down

  0.04353351  0.04357822  0.04362859  0.04367425  0.04371917  0.04377039  0.04381727  0.04386645  0.04392805  0.04397717  0.04402101  0.0440627  0.04409101  0.04411681  0.04412851  0.04413433  0.04413572  0.04413572  0.04417688  0.04423834  0.04575139  0.04805502  0.06695019  0.08760682  0.1411864  0.1823661  0.2547947  0.310247  0.3464949  0.3764213  0.3915134  0.4011171  0.411667  0.4183913  0.4244556

*/
    private void stateSequenceObseve()
    {
        if (stateSequence.Count == stateSequenceLimit)
        {
            stateSequence.Pop();
        }
        switch (Pitch)
        {
            case float value when (value < -0.5):
            stateSequence.Push(HeadState.Up);
            break;
            case float value when (value > 0.5):
            stateSequence.Push(HeadState.Down);
            break;
            default:
            stateSequence.Push(HeadState.Stable);
            break;
        }
        if (stateSequence.Count == stateSequenceLimit)
        {
            var sequence = Array.ConvertAll(stateSequence.ToArray(), item => (HeadState)item); 

            Array.Reverse(sequence); 
            // TODO: reverse the templateSequences's array element instead (may be)

            foreach (HeadState[] templateSequence in templateSequences)
            {
                if (sequence.SequenceEqual(templateSequence))
                {
                    this.didNod = true;
                    break;
                }
            }
        }
    }

// TODO: will need more template sequences, may be we even need more frames
    private HeadState[][] templateSequences = new HeadState[][]
    {
        new HeadState[] 
        { 
            HeadState.Up, HeadState.Stable, HeadState.Stable, HeadState.Down, HeadState.Down, 
            HeadState.Stable, HeadState.Stable, HeadState.Up, HeadState.Up 
        },
        new HeadState[] { 
            HeadState.Up, HeadState.Stable, HeadState.Down, HeadState.Stable, HeadState.Up, 
            HeadState.Stable, HeadState.Down, HeadState.Stable, HeadState.Up 
        },
    };
}
