using UnityEngine;
using Tobii.Research;

public class EyeHandler : MonoBehaviour
{
    private IEyeTracker eyeTracker;

    // private ScreenBasedCalibration screenBasedCalibration;

    private Vector3 currentPosition;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        EyeTrackerCollection trackers = EyeTrackingOperations.FindAllEyeTrackers();
        foreach (IEyeTracker eyeTracker in trackers)
        {
            Debug.Log(string.Format(
                "Adress: {0}, Name: {1}, Mode: {2}, Serial number: {3}, Firmware version: {4}", 
                eyeTracker.Address, 
                eyeTracker.DeviceName, 
                eyeTracker.Model, 
                eyeTracker.SerialNumber, 
                eyeTracker.FirmwareVersion
                ));
        }
        if (trackers.Count > 0)
        {
            // --- connect 1st eye tracker
            eyeTracker = trackers[0];
            // --- assign the tracker to calibration
            // screenBasedCalibration = new ScreenBasedCalibration(eyeTracker);
        }
    }
    void Start()
    {
        try {
            eyeTracker.GazeDataReceived += GazeDataReceivedFromTracker;
        } catch {}
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPosition != null) {
            transform.position = new Vector2(currentPosition.x, currentPosition.y); 
        }
    }

    void OnDestroy()
    {
        try {
            eyeTracker.GazeDataReceived -= GazeDataReceivedFromTracker;
        } catch {}
    }

    private void GazeDataReceivedFromTracker(object sender, GazeDataEventArgs e)
    {
        if (e.LeftEye.GazePoint.Validity == Validity.Invalid || e.RightEye.GazePoint.Validity == Validity.Invalid)
        {
            return;
        }

        var combinedEyeGazePoint = (
            Utility.ToVector2(e.LeftEye.GazePoint.PositionOnDisplayArea) + 
            Utility.ToVector2(e.RightEye.GazePoint.PositionOnDisplayArea)
            ) / 2f;
        var position = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width * combinedEyeGazePoint.x, Screen.height * (1 - combinedEyeGazePoint.y), 10)
            );    // the z should be 10 cuz the camera currently has z value -10
        //TODO: use something similar to LatestProcessedGazeData in the ScreenBasedPrefabDemo

        currentPosition = position;
    }

    private static void CalibrationData(IEyeTracker tracker)
    {
        var dataContractSerializer = new System.Runtime.Serialization.DataContractSerializer(typeof(CalibrationData));

        // retrieve the calibraiton data from the eye tracker
        CalibrationData data = tracker.RetrieveCalibrationData();

        //print out
        Debug.Log(data.ToString());
    }
}
