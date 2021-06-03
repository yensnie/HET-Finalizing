using UnityEngine;
using UnityEngine.SceneManagement;

public class EyeOnlyHardRunner : MonoBehaviour
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

    void Start()
    {
        fillGameObjectsToPattern();
        fillObjectsWithSprites(8, 4);
        prepareComponents();
        prepareCursors();
    }

    private void OnGUI() {
        // Temporary: add FPS
        GUI.Label(
            new Rect(0, 0, 100, 100), 
            ((int)(1.0f / Time.smoothDeltaTime)).ToString()
        ); 
    }

    private void prepareComponents()
    {
        switch (Global.currentState)
        {
            case TrialState.Eye:
                _lockTime = 0.7;
                break;
            case TrialState.Head:
                _lockTime = 0.7;
                break;
            case TrialState.HeadEye:
                _lockTime = 0.7;
                break;
            case TrialState.Order:
                _confirmTime = 0.7;
                break;
        }
        resetLockTime();
    }

    private void resetLockTime()
    {
        lockTime = _lockTime;
        confirmTime = _confirmTime;
    }

    private void prepareCursors()
    {
        switch (Global.currentState)
        {
            case TrialState.Eye:
                GameObject.Find("headCursor").SetActive(false);
                break;
            case TrialState.Head:
                GameObject.Find("eyeCursor").SetActive(false);
                break;
            case TrialState.HeadEye:
                // hide the render of head cursor
                // but still need it
                GameObject
                    .Find("headCursor")
                    .GetComponent<Renderer>()
                    .enabled = false;
                break;
            case TrialState.Order:
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Application.isEditor)
            {
                Application.Quit();
            }
        }

        trialTimeHandle();

        switch (Global.currentState)
        {
            case TrialState.Eye:
                updateInEyeOnly();
                break;
            case TrialState.Head:
                updateInHeadOnly();
                break;
            case TrialState.HeadEye:
                updateHeadSupportEye();
                break;
            case TrialState.Order:
                updateEyeHeadOrder();
                break;
        }

        trialDoneTimeHandle();
    }

    private void trialTimeHandle()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0 && !trialDone && result == Result.Overtime)
        {
            trialDoneHandle(Result.Overtime, 25.0);
        }
    }

    private void trialDoneHandle(Result result, double time)
    {
        if (trialCount >= maxTrialsNumber)
        {
            saveTrialData(new TrialData(result, time));
            saveData();
        }
        else
        {
            saveTrialData(new TrialData(result, time));
            trialCount++;
        }
        trialDone = true;
    }

    private void trialDoneTimeHandle()
    {
        if (trialDone)
        {
            delayTime -= Time.deltaTime;
            if (delayTime > 0 && delayTime <= 0.5)
            {
                mainFrame.SetActive(false);
                foreach (GameObject frame in subFrame)
                {
                    frame.SetActive(false);
                }
            }
            if (delayTime <= 0)
            {
                mainFrame.SetActive(true);
                foreach (GameObject frame in subFrame)
                {
                    frame.SetActive(true);
                }
                trialFinish();
            }
        }
    }

    private void trialFinish()
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

    private void saveTrialData(TrialData data)
    {
        var index = trialCount - 1;
        tempTrialData[index] = new TrialData(data.getResult(), data.getTime());
    }

    private void saveData()
    {
        var methodName = "";
        switch (Global.currentState)
        {
            case TrialState.Eye:
                methodName = "Eyes-only";
                break;
            case TrialState.Head:
                methodName = "Head-only";
                break;
            case TrialState.HeadEye:
                methodName = " Multimodal 1";
                break;
            case TrialState.Order:
                methodName = " Multimodal 2";
                break;
        }

        for (int index = 0; index < tempTrialData.Length; index++)
        {
            var resultString = "N/A";
            switch (tempTrialData[index].getResult())
            {
                case Result.Correct:
                    resultString = "Correct";
                    break;
                case Result.Incorrect:
                    resultString = "Incorrect";
                    break;
                case Result.Overtime:
                    resultString = "Overtime";
                    break;
            }
            var time = tempTrialData[index].getTime();
            var data = new string[3]
            {
                (index + 1).ToString(),
                resultString,
                time.ToString()
            };

            string fileName = Global.participantName + "_" + methodName;
            CSVManager.appendtoFile(fileName, data);
        }
    }

    public void changeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    // Condition 1
    private void updateInEyeOnly()
    {

        if (trialDone)
        {
            return;
        }

        if (selectedPatternSet != null
            && selectedPatternSet.objects.Length > 0)
        {
            selectedPatternSet
                .objects[0]
                .transform
                .parent
                .gameObject
                .GetComponent<SpriteRenderer>()
                .sprite = blue;
        }
        else
        {
            // reset
            resetLockTime();
            return;
        }

        // eye lock time counting down, but will reset 
        // and stop the next seps if there is no selected object
        lockTime -= Time.deltaTime;

        if (lockTime <= 0)
        {

            if (samePattern(selectedPatternSet, mainObjPattern))
            {
                selectedPatternSet
                .objects[0]
                .transform
                .parent
                .gameObject
                .GetComponent<SpriteRenderer>()
                .sprite = green;
                result = Result.Correct;
                var takenTime = _timeLeft - timeLeft;
                trialDoneHandle(result, takenTime);
            }
            else
            {
                selectedPatternSet
                .objects[0]
                .transform
                .parent
                .gameObject
                .GetComponent<SpriteRenderer>()
                .sprite = red;
                result = Result.Incorrect;
                var takenTime = _timeLeft - timeLeft;
                trialDoneHandle(result, takenTime);
            }
        }
    }

    // Condition 2
    private void updateInHeadOnly()
    {
        if (headSelectedPatternSet == null)
        {
            return;
        }

        if (trialDone)
        {
            return;
        }

        if (headSelectedPatternSet != null
            && headSelectedPatternSet.objects.Length > 0)
        {
            headSelectedPatternSet
            .objects[0]
            .transform
            .parent
            .gameObject
            .GetComponent<SpriteRenderer>()
            .sprite = blue;
        }
        else
        {
            // reset
            resetLockTime();
            return;
        }

        // head lock time counting down, but will reset 
        // and stop the next seps if there is no selected object
        lockTime -= Time.deltaTime;

        if (lockTime <= 0)
        {
            if (samePattern(headSelectedPatternSet, mainObjPattern))
            {
                headSelectedPatternSet
                    .objects[0]
                    .transform
                    .parent
                    .gameObject
                    .GetComponent<SpriteRenderer>()
                    .sprite = green;
                result = Result.Correct;
                var takenTime = _timeLeft - timeLeft;
                trialDoneHandle(result, takenTime);
            }
            else
            {
                headSelectedPatternSet
                    .objects[0]
                    .transform
                    .parent
                    .gameObject
                    .GetComponent<SpriteRenderer>()
                    .sprite = red;
                result = Result.Incorrect;
                var takenTime = _timeLeft - timeLeft;
                trialDoneHandle(result, takenTime);
            }
        }
    }

    // Condition 3
    /*
    Hypothesis: 
    We could have some state like: Up - 1, Stable - 2, Down - 3 like Hidden Markov Models
    Use a stack to store the sequence, like {1, 1, 1, 1, 2, 2, 2, 3, 3, 3, 3, 3}
    with each element represent for a state in each frame.
    So if we could use some patterns to compare, if it matched, in the next frame we will
    count as a nod detected. The requirement for the observation in this scenario is only 
    when the eye cursor is aiming at an object, or is selecting a pattern.
    */
    // TODO: when start eye select, set current pitch as stable state
    private void updateHeadSupportEye()
    {
        HeadHandler trackerInstance = GameObject
        .Find("headCursor")
        .GetComponent<HeadHandler>();

        if (trialDone)
        {
            return;
        }

        if (selectedPatternSet != null
            && selectedPatternSet.objects.Length > 0)
        {
            lockTime -= Time.deltaTime;
            if (lockTime <= 0)
            {
                selectedPatternSet
                    .objects[0]
                    .transform
                    .parent
                    .gameObject
                    .GetComponent<SpriteRenderer>()
                    .sprite = purple;
                
                if (!trackerInstance.isObserving)
                {
                    // start observe the nod
                    trackerInstance.isObserving = true;
                }

                if (trackerInstance.didNod)
                {
                    if (samePattern(selectedPatternSet, mainObjPattern))
                    {
                        selectedPatternSet
                            .objects[0]
                            .transform
                            .parent
                            .gameObject
                            .GetComponent<SpriteRenderer>()
                            .sprite = green;
                        result = Result.Correct;
                        var takenTime = _timeLeft - timeLeft;
                        trialDoneHandle(result, takenTime);
                    }
                    else
                    {
                        selectedPatternSet
                            .objects[0]
                            .transform
                            .parent
                            .gameObject
                            .GetComponent<SpriteRenderer>()
                            .sprite = red;
                        result = Result.Incorrect;
                        var takenTime = _timeLeft - timeLeft;
                        trialDoneHandle(result, takenTime);
                    }
                }
            }
            else
            {
                selectedPatternSet
                    .objects[0]
                    .transform
                    .parent
                    .gameObject
                    .GetComponent<SpriteRenderer>()
                    .sprite = blue;
            }
        }
        else
        {
            resetLockTime();
        }
    }

    // Condition  4 - concept 2
    private void updateEyeHeadOrder()
    {
        if (trialDone)
        {
            return;
        }

        if (selectedPatternSet != null
            && selectedPatternSet.objects.Length > 0)
        {
            if (headSelectedPatternSet != null
                && headSelectedPatternSet == selectedPatternSet)
            {
                confirmTime -= Time.deltaTime;
                if (confirmTime <= 0)
                {
                    if (samePattern(selectedPatternSet, mainObjPattern))
                    {
                        selectedPatternSet
                            .objects[0]
                            .transform
                            .parent
                            .gameObject
                            .GetComponent<SpriteRenderer>()
                            .sprite = green;
                        result = Result.Correct;
                        var takenTime = _timeLeft - timeLeft;
                        trialDoneHandle(result, takenTime);
                    }
                    else
                    {
                        selectedPatternSet
                            .objects[0]
                            .transform
                            .parent
                            .gameObject
                            .GetComponent<SpriteRenderer>()
                            .sprite = red;
                        result = Result.Incorrect;
                        var takenTime = _timeLeft - timeLeft;
                        trialDoneHandle(result, takenTime);
                    }
                }
            }
            else
            {
                selectedPatternSet
                    .objects[0]
                    .transform
                    .parent
                    .gameObject
                    .GetComponent<SpriteRenderer>()
                    .sprite = blue;
                // reset
                resetLockTime();
            }
        }
        else
        {
            // reset
            resetLockTime();
        }
    }

    private bool samePattern(
        Global.GameObjectPattern patternA,
        Global.GameObjectPattern patternB
        )
    {
        bool result = true;
        for (int index = 0; index < patternA.objects.Length; index++)
        {
            var spriteA = patternA
                .objects[index]
                .GetComponent<SpriteRenderer>()
                .sprite
                .name;

            var spriteB = patternB
                .objects[index]
                .GetComponent<SpriteRenderer>()
                .sprite
                .name;

            if (!(spriteA.Trim().Equals(spriteB)))
            {
                result = false;
                break;
            }
        }
        return result;
    }

    private void fillGameObjectsToPattern()
    {
        //------------------------- Main object set up
        // this pattern store 4 game objects repesented 4 spirtes
        mainObjPattern = new Global.GameObjectPattern();
        mainObjPattern.objects = mainObj;

        //------------------------- Sub objects group set up
        // this pattern store 4 game objects repesented 4 spirtes
        // this group has 8 gameObjectPattern-s
        subObjsGroup = new Global.GameObjectPatternGroup();

        var components = 4;
        var groupIndex = 0;
        var tempArray = new GameObject[components];
        var tempArrayIndex = 0;

        // There are 8 groups, which mean 8*4 = 24 enitities in subObjList
        for (int index = 0; index < subObjList.Length; index++)
        {
            tempArray[tempArrayIndex] = subObjList[index];
            tempArrayIndex++;

            // after each group (4 components), reset tempArrayIndex, increase group index
            if ((index + 1) % components == 0)
            {
                subObjsGroup.patterns[groupIndex].objects = tempArray;

                groupIndex++;
                tempArray = new GameObject[components];
                tempArrayIndex = 0;
            }
        }
    }

    private void fillObjectsWithSprites(int length, int components = 4)
    {
        // create an array of indexs in pattern spirtes array
        int[] indexs = new int[length];
        for (int index = 0; index < length; index++)
        {
            indexs[index] = index;
        }

        // the array to hole our suffeled sets of patterns
        int[][] finalOrderSets = new int[length][];

        // assign the array above with random values from the array of indexs
        for (int index = 0; index < indexs.Length; index++)
        {
            // first, make a randomly suffled version of indexs array
            int[] suffledIndexs = indexs;
            Utility.reshuffle(suffledIndexs);

            int[] array = new int[components];
            // then assign for first 4 items in to the current position of the final order array
            for (int componentIndex = 0; componentIndex < components; componentIndex++)
            {
                array[componentIndex] = suffledIndexs[componentIndex];
            }
            finalOrderSets[index] = array;
        }

        var random = new System.Random();

        // get the random index, this will be the index of the pattern 
        // that will be used for main pattern
        int randomIndex = random.Next(0, finalOrderSets.Length - 1);

        for (int index = 0; index < components; index++)
        {
            // apply the random set of sprite pattern into main Object
            mainObjPattern
                .objects[index]
                .GetComponent<SpriteRenderer>()
                .sprite = spriteList[finalOrderSets[randomIndex][index]];
        }
        // save the order into main object
        mainObjPattern.order = finalOrderSets[randomIndex];

        for (int index = 0; index < length; index++)
        {
            for (int innerIndex = 0; innerIndex < components; innerIndex++)
            {
                // fill the sprites for objects in pattern object at specific position of sub object
                subObjsGroup
                    .patterns[index]
                    .objects[innerIndex]
                    .GetComponent<SpriteRenderer>()
                    .sprite = spriteList[finalOrderSets[index][innerIndex]];
            }

            // save the current order into sub object
            subObjsGroup.patterns[index].order = finalOrderSets[index];

            // Apply patterns into objects in subFrame array
            try
            {
                subFrame[index]
                    .GetComponent<ColliderHandleHard>()
                    .selectedPatternSet = subObjsGroup.patterns[index];
            }
            catch
            {
                Debug.Log(
                    "Cannot apply for `ColliderHandleHard` of subframe object at index " 
                        + index
                );
            }
        }
    }
}