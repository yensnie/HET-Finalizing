using System.Collections;
using Tobii.Research;
using Tobii.Research.Unity;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationRunner : MonoBehaviour
{

    private IEyeTracker eyeTracker;

    // private ScreenBasedCalibration screenBasedCalibration;

    [SerializeField]
    private Image calibrationPoint;

    [SerializeField]
    private Canvas calibrationCanvas;

    [SerializeField]
    private Image panel;

    private CalibrationThread calibrationThread;
    
    private Tobii.Research.Unity.CalibrationPoint pointScript;

    private bool isCalibrating
    {
        set
        {
            calibrationCanvas.gameObject.SetActive(value);
            pointScript.gameObject.SetActive(value);
            panel.color = value ? Color.black : new Color(0, 0, 0, 0);
            Debug.Log(string.Format("is being calibrate: {0}", value.ToString()));
        }
    }

    void Awake()
    {
        EyeTrackerCollection trackers = EyeTrackingOperations.FindAllEyeTrackers();
        foreach (IEyeTracker eyeTracker in trackers)
        {
            Debug.Log(
                string.Format
                (
                    "Adress: {0}, Name: {1}, Mode: {2}, Serial number: {3}, Firmware version: {4}",
                    eyeTracker.Address,
                    eyeTracker.DeviceName,
                    eyeTracker.Model,
                    eyeTracker.SerialNumber,
                    eyeTracker.FirmwareVersion
                )
            );
        }
        if (trackers.Count > 0)
        {
            // --- connect 1st eye tracker
            eyeTracker = trackers[0];
            Debug.Log("did get the eye tracker");
        }

        isCalibrating = false;
    }

    void Start()
    {
        pointScript = calibrationPoint.GetComponent<CalibrationPoint>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && (eyeTracker != null))
        {
            StartCoroutine(Execute(eyeTracker));
        }
    }

    // ------------------------------- calibration
    IEnumerator Execute(IEyeTracker eyeTracker)
    {
        if (eyeTracker != null)
        {
            yield return Calibrate(eyeTracker);
        }
        yield break;
    }
    //TODO: if call the IEnumerator function directly, will not happen, need to read about this
    private IEnumerator Calibrate(IEyeTracker eyeTracker)
    {
        isCalibrating = true;

        // // Create a calibration object.
        // var calibration = new ScreenBasedCalibration(eyeTracker);

        // // Enter calibration mode.
        // calibration.EnterCalibrationMode();

        if (calibrationThread != null)
        {
            calibrationThread.StopThread();
            calibrationThread = null;
        }

        calibrationThread = new CalibrationThread(eyeTracker, true);

        // Only continue if the calibration thread is running.
        for (int i = 0; i < 10; i++)
        {
            if (calibrationThread.Running)
            {
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }

        if (!calibrationThread.Running) {
            Debug.LogError("Failed to start calibration thread");
            calibrationThread.StopThread();
            calibrationThread = null;
            isCalibrating = false;
            yield break;
        }

        var result = calibrationThread.EnterCalibrationMode();

        yield return StartCoroutine(WaitForResult(result));

        // Define the points on screen we should calibrate at.
        // The coordinates are normalized, i.e. (0.0f, 0.0f) is the upper left corner and (1.0f, 1.0f) is the lower right corner.
        var pointsToCalibrate = new NormalizedPoint2D[] {
                new NormalizedPoint2D(0.5f, 0.5f),
                new NormalizedPoint2D(0.1f, 0.1f),
                new NormalizedPoint2D(0.1f, 0.9f),
                new NormalizedPoint2D(0.9f, 0.1f),
                new NormalizedPoint2D(0.9f, 0.9f),
            };
        // Collect data.
        foreach (var point in pointsToCalibrate)
        {
            // Show an image on screen where you want to calibrate.
            // Debug.Log(string.Format("Show point on screen at ({0}, {1})", point.X, point.Y));
            var vector = Utility.ToVector2(point);
            calibrationPoint.rectTransform.anchoredPosition = new Vector2(Screen.width * vector.x, Screen.height * (1 - vector.y));
            pointScript.StartAnim();
            Debug.Log(string.Format("Show point on screen at ({0}, {1})", Screen.width * vector.x, Screen.height * (1 - vector.y)));

            // Wait a little for user to focus.
            yield return new WaitForSeconds(.7f);

            var collectionResult = calibrationThread.CollectData(new CalibrationThread.Point(vector));
            
            yield return StartCoroutine(WaitForResult(collectionResult));

            if ( collectionResult.Status == CalibrationStatus.Failure)
            {
                Debug.Log("There was an error gathering data for this calibration point: " + vector);
            }
        }

        // Compute and apply the calibration.
        var computeResult = calibrationThread.ComputeAndApply();

        yield return StartCoroutine(WaitForResult(computeResult));

        var leaveResult = calibrationThread.LeaveCalibrationMode();

        yield return StartCoroutine(WaitForResult(leaveResult));

        calibrationThread.StopThread();
        calibrationThread = null;
        isCalibrating = false;
    }

    private IEnumerator WaitForResult(CalibrationThread.MethodResult result)
        {
            // Wait for the thread to finish the blocking call.
            while (!result.Ready)
            {
                yield return new WaitForSeconds(0.02f);
            }

            Debug.Log(result);
        }
}
