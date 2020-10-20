using UnityEngine.UI;
using UnityEngine;
using Tobii.Research.Unity;

public class EyeBehaviourDemo : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Attach text object here.")]
    private Text _text;

    private EyeTracker _eyeTracker;
    private Calibration _calibration;
    private TrackBoxGuide _trackBoxGuide;
    void Start()
    {
        _eyeTracker = EyeTracker.Instance;
        _calibration = Calibration.Instance;
        _trackBoxGuide = TrackBoxGuide.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Screen.fullScreen)
        {
            _text.text = "<color=red>Please run in full screen!</color>";
            return;
        }

        // Quit if escape is pressed.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Application.isEditor)
            {
                Application.Quit();
            }
        }

        // We are expecting to have all objects.
        if (!_eyeTracker || !_calibration || !_trackBoxGuide)
        {
            return;
        }

        // Thin out updates a bit.
        if (Time.frameCount % 6 != 0)
        {
            return;
        }

        if (!_eyeTracker.LatestProcessedGazeData.Left.GazePointValid) { return; }
        var left = _eyeTracker.LatestProcessedGazeData.Left.GazePointOnDisplayArea;
        var right = _eyeTracker.LatestProcessedGazeData.Right.GazePointOnDisplayArea;
        var combine = (left + right) / 2f;
        transform.position = new Vector2(combine.x, combine.y);
    }


}
