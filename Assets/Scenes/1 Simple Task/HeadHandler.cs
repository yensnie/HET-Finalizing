using System;
using UnityEngine;
using System.Runtime.InteropServices;

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

    [DllImport("FreeTrackClient64")]
    public static extern bool FTGetData(ref FreeTrackData data);

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
        Debug.Log(info);

        transform.position = new Vector2(RawYaw * 5, RawPitch * 5);
        //TODO: find out a value to replace for 5 to work perfectly with all size of screen
    }
}
