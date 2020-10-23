using System.Collections;
using Tobii.Research;
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

    private bool isCalibrating
    {
        set 
        {
            calibrationCanvas.gameObject.SetActive(value);
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && (eyeTracker != null))
        {
            StartCoroutine(Calibrate(eyeTracker));
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
        // Create a calibration object.
        var calibration = new ScreenBasedCalibration(eyeTracker);
        // Enter calibration mode.
        calibration.EnterCalibrationMode();
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
            Debug.Log(string.Format("Show point on screen at ({0}, {1})", point.X, point.Y));
            calibrationPoint.rectTransform.anchoredPosition = new Vector2(Screen.width * point.X, Screen.height * (1 - point.Y));
            // Wait a little for user to focus.
            yield return new WaitForSeconds(.7f);
            // Collect data.
            CalibrationStatus status = calibration.CollectData(point);
            if (status != CalibrationStatus.Success)
            {
                // Try again if it didn't go well the first time.
                // Not all eye tracker models will fail at this point, but instead fail on ComputeAndApply.
                calibration.CollectData(point);
            }
        }

        // Compute and apply the calibration.
        CalibrationResult calibrationResult = calibration.ComputeAndApply();
        Debug.Log(string.Format("Compute and apply returned {0} and collected at {1} points.",
            calibrationResult.Status, calibrationResult.CalibrationPoints.Count));

        // Analyze the data and maybe remove points that weren't good.
        calibration.DiscardData(new NormalizedPoint2D(0.1f, 0.1f));

        // Redo collection at the discarded point.
        Debug.Log(string.Format("Show point on screen at ({0}, {1})", 0.1f, 0.1f));
        calibration.CollectData(new NormalizedPoint2D(0.1f, 0.1f));

        // Compute and apply again.
        calibrationResult = calibration.ComputeAndApply();
        Debug.Log(string.Format("Second compute and apply returned {0} and collected at {1} points.",
            calibrationResult.Status, calibrationResult.CalibrationPoints.Count));
        // See that you're happy with the result.
        // The calibration is done. Leave calibration mode.
        calibration.LeaveCalibrationMode();
        isCalibrating = false;
    }
}
