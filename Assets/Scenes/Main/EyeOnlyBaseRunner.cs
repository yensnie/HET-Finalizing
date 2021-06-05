using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeOnlyBaseRunner : MonoBehaviour
{
    public struct TrialData
    {
        private double time;
        private Result result;

        public TrialData(Result result, double time)
        {
            this.time = time;
            this.result = result;
        }

        public double getTime()
        {
            return this.time;
        }

        public Result getResult()
        {
            return this.result;
        }
    }
    public enum Result
    {
        Correct, Incorrect, Overtime
    }

    [HideInInspector]
    public Result result = Result.Overtime;

    [HideInInspector]
    public TrialData[] tempTrialData = new TrialData[10];

    // current selected pattern by eye cursor
    [HideInInspector]
    public Global.GameObjectPattern selectedPatternSet;

    // current selected pattern by head cursor
    [HideInInspector]
    public Global.GameObjectPattern headSelectedPatternSet;

    // The main pattern object
    public GameObject[] mainObj;

    [HideInInspector]
    public Global.GameObjectPattern mainObjPattern;
    public GameObject[] subObjList;

    // pattern objects list
    [HideInInspector]
    public Global.GameObjectPatternGroup subObjsGroup;

    public GameObject mainFrame;

    // the frame object list (which co-responding 
    // with pattern objects as container)
    public GameObject[] subFrame;

    // list of sprites for patterns
    public Sprite[] spriteList;

    // the trial time left, will counted down right from start
    [HideInInspector]
    public const double _timeLeft = 25;

    [HideInInspector]
    public double timeLeft = 25;

    // after this amount of time when eye gaze hit the objects, 
    // it will be counted as "lock" (eye only scenario)
    [HideInInspector]
    public double _lockTime = 0;

    [HideInInspector]
    public double lockTime = 0;

    // after this amount of seconds when selecting, 
    // active confirmation result (correct or incorrect)
    [HideInInspector]
    public double _confirmTime = 0;

    [HideInInspector]
    public double confirmTime = 0;

    public Sprite white;
    public Sprite blue;
    public Sprite yellow;
    public Sprite green;
    public Sprite red;
    public Sprite purple;

    [HideInInspector]
    public int trialCount = 1;

    [HideInInspector]
    public const int maxTrialsNumber = 10;

    [HideInInspector]
    public bool trialDone = false;

    // 2s for state delay, 0.5 for baseline screen
    [HideInInspector]
    public double delayTime = 2.5;

    public void Awake() {
        QualitySettings.vSyncCount = 0;     // disable vSync
        Application.targetFrameRate = 60;
    }
}
