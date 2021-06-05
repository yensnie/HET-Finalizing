using UnityEngine;

public class EyeOnlyHardRunner : EyeOnlyBaseRunner
{
    void Start()
    {
        fillGameObjectsToPattern(8, 4);
        fillObjectsWithSprites(8, 4);
        prepareComponents();
        prepareCursors();
    }

    public override void trialFinish()
    {
        if (trialCount == maxTrialsNumber)
        {
            // TODO: finish the scene
        }
        else
        {
            trialDone = false;

            // reshuffle for new sprites
            fillObjectsWithSprites(8, 4);

            // reset
            timeLeft = _timeLeft;
            delayTime = 2.5;
            resetLockTime();

            // reset HeadHandler state in HeadEye mode
            GameObject
                .Find("headCursor")
                .GetComponent<HeadHandler>()
                .didNod = false;
        }
    }
}