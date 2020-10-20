using UnityEngine;
using Tobii.Research;
using UnityEngine.UI;

public class EyeBehaviour : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Attach text object here.")]
    private Text _text;
    private IEyeTracker eyeTracker;

    private ScreenBasedCalibration screenBasedCalibration;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // --- get the eye trackers and assign the first one
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
            screenBasedCalibration = new ScreenBasedCalibration(eyeTracker);
        }
    }

    void Start()
    {
        eyeTracker.GazeDataReceived += GazeDataReceivedFromTracker;
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
        if (eyeTracker == null)
        {
            return;
        }
        //CalibrationData(this.eyeTracker);
        if (eyeTracker != null)
        {
            print(eyeTracker);
        }
    }

    void OnDestroy()
    {
        eyeTracker.GazeDataReceived -= GazeDataReceivedFromTracker;
    }

    private void GazeDataReceivedFromTracker(object sender, GazeDataEventArgs e)
    {
        if (e.LeftEye.GazePoint.Validity == Validity.Invalid || e.RightEye.GazePoint.Validity == Validity.Invalid)
        {
            return;
        }

        var combinedEyeGazePoint = (
            ToVector2(e.LeftEye.GazePoint.PositionOnDisplayArea) + 
            ToVector2(e.RightEye.GazePoint.PositionOnDisplayArea)
            ) / 2f;
        var position = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width * combinedEyeGazePoint.x, Screen.height * (1 - combinedEyeGazePoint.y), 10)
            );    // the z should be 10 cuz the camera currently has z value -10
        //TODO: use something similar to LatestProcessedGazeData in the ScreenBasedPrefabDemo

        _text.text = string.Format("{0}\n{1}", position,Input.mousePosition);
        transform.position = position;
    }

    private Vector2 ToVector2(NormalizedPoint2D value)
    {
        return new Vector2(value.X, value.Y);
    }

    private Vector3 ToVector3(Point3D point)
    {
        return new Vector3(point.X, point.Y, point.Z);
    }

    private static void CalibrationData(IEyeTracker tracker)
    {
        var dataContractSerializer = new System.Runtime.Serialization.DataContractSerializer(typeof(CalibrationData));

        // retrieve the calibraiton data from the eye tracker
        CalibrationData data = tracker.RetrieveCalibrationData();

        //print out
        Debug.Log(data.ToString());
    }

    private void printDisplayArea(IEyeTracker tracker)
    {
        // Get the display area.
        DisplayArea displayArea = eyeTracker.GetDisplayArea();
        Debug.Log(string.Format(
            "Got display area from tracker with serial number {0} with height {1}, width {2} and coordinates:",
            eyeTracker.SerialNumber,
            displayArea.Height,
            displayArea.Width));
        Debug.Log(string.Format("Bottom Left: ({0}, {1}, {2})",
            displayArea.BottomLeft.X, displayArea.BottomLeft.Y, displayArea.BottomLeft.Z));
        Debug.Log(string.Format("Bottom Right: ({0}, {1}, {2})",
            displayArea.BottomRight.X, displayArea.BottomRight.Y, displayArea.BottomRight.Z));
        Debug.Log(string.Format("Top Left: ({0}, {1}, {2})",
            displayArea.TopLeft.X, displayArea.TopLeft.Y, displayArea.TopLeft.Z));
        Debug.Log(string.Format("Top Right: ({0}, {1}, {2})",
            displayArea.TopRight.X, displayArea.TopRight.Y, displayArea.TopRight.Z));

        // Set the display area. A new object is used to show usage.
        DisplayArea displayAreaToSet = new DisplayArea(displayArea.TopLeft, displayArea.BottomLeft, displayArea.TopRight);
        eyeTracker.SetDisplayArea(displayAreaToSet);
    }
}
