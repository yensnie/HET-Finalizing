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

    private Result result = Result.Overtime;

    private TrialData[] tempTrialData = new TrialData[10];

    // current selected pattern by eye cursor
    public Global.GameObjectPattern selectedPatternSet;

    // current selected pattern by head cursor
    public Global.GameObjectPattern headSelectedPatternSet;

    // The main pattern object
    public GameObject[] mainObj;

    private Global.GameObjectPattern mainObjPattern;
    public GameObject[] subObjList;

    // pattern objects list
    private Global.GameObjectPatternGroup subObjsGroup;

    public GameObject mainFrame;

    // the frame object list (which co-responding 
    // with pattern objects as container)
    public GameObject[] subFrame;

    // list of sprites for patterns
    public Sprite[] spriteList;

    // the trial time left, will counted down right from start
    private const double _timeLeft = 25;
    private double timeLeft = 25;

    // after this amount of time when eye gaze hit the objects, 
    // it will be counted as "lock" (eye only scenario)
    private double _lockTime = 0;
    private double lockTime = 0;

    // after this amount of seconds when selecting, 
    // active confirmation result (correct or incorrect) 
    private double _confirmTime = 0;
    private double confirmTime = 0;

    public Sprite white;
    public Sprite blue;
    public Sprite yellow;
    public Sprite green;
    public Sprite red;

    public Sprite purple;

    private int trialCount = 1;
    private const int maxTrialsNumber = 10;

    public bool trialDone = false;

    // 2s for state delay, 0.5 for baseline screen
    private double delayTime = 2.5;

}
