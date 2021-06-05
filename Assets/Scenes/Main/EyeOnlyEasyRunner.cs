using UnityEngine;

public class EyeOnlyEasyRunner : EyeOnlyBaseRunner
{
    // Start is called before the first frame update
    void Start()
    {
        fillGameObjectsToPattern(4, 2);
        fillObjectsWithSprites(4, 2);
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
            fillObjectsWithSprites(4, 2);

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
