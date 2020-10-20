using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;
using Tobii.Research.Unity;

public class HeadBehaviour : MonoBehaviour
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

    [DllImport("FreeTrackClient64")]
    public static extern bool FTGetData(ref FreeTrackData data);

    [DllImport("FreeTrackClient64")]
    public static extern string FTGetDllVersion();

    [DllImport("FreeTrackClient64")]
    public static extern void FTReportID(Int32 name);

    [DllImport("FreeTrackClient64")]
    public static extern string FTProvider();

    public float Yaw = 0F;
    public float Pitch = 0F;
    public float Roll = 0F;
    public float X = 0F;
    public float Y = 0F;
    public float Z = 0F;

    public float RawYaw = 0F;
    public float RawPitch = 0F;
    public float RawRoll = 0F;
    public float RawX = 0F;
    public float RawY = 0F;
    public float RawZ = 0F;

    public float x1 = 0F;
    public float y1 = 0F;
    public float x2 = 0F;
    public float y2 = 0F;
    public float x3 = 0F;
    public float y3 = 0F;
    public float x4 = 0F;
    public float y4 = 0F;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HeadBehaviour.FreeTrackData FreeTrackData;
        FreeTrackData = new HeadBehaviour.FreeTrackData();
        if (!HeadBehaviour.FTGetData(ref FreeTrackData))
        {
            Debug.Log("FTGetData returned false. FreeTrack likely not working.");
            return;
        }
        HeadBehaviour.FTGetData(ref FreeTrackData);

        Yaw = FreeTrackData.Yaw;
        Pitch = FreeTrackData.Pitch;
        Roll = FreeTrackData.Roll;
        X = FreeTrackData.X;
        Y = FreeTrackData.Y;
        Z = FreeTrackData.Z;

        RawYaw = FreeTrackData.RawYaw;
        RawPitch = FreeTrackData.RawPitch;
        RawRoll = FreeTrackData.RawRoll;

        RawX = FreeTrackData.RawX;
        RawY = FreeTrackData.RawY;
        RawZ = FreeTrackData.RawZ;

        x1 = FreeTrackData.x1;
        y1 = FreeTrackData.y1;
        x2 = FreeTrackData.x2;
        y2 = FreeTrackData.y2;
        x3 = FreeTrackData.x3;
        y3 = FreeTrackData.y3;
        x4 = FreeTrackData.x4;
        y4 = FreeTrackData.y4;

        var info = string.Format("head X: {0}, head Y: {1}", X, Y);
        Debug.Log(info);

        transform.position = new Vector2(RawYaw * 5,RawPitch * 5);  
        //TODO: find out a value to replace for 5 to work perfectly with all size of screen
    }
}
