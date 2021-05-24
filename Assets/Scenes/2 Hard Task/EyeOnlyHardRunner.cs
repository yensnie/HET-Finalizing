using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Research;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EyeOnlyHardRunner : MonoBehaviour
{
    // current selected pattern by eye cursor
    public static Global.GameObjectPattern selectedPatternSet;

    // current selected pattern by head cursor
    public static Global.GameObjectPattern headSelectedPatternSet;

    // The main pattern object
    public GameObject[] mainObj;

    private Global.GameObjectPattern mainObjPattern;
    public GameObject[] subObjList;

    // pattern objects list
    private Global.GameObjectPatternGroup subObjsGroup;

    // the frame object list (which co-responding 
    // with pattern objects as container)
    public GameObject[] subFrame;

    // list of sprites for patterns
    public Sprite[] spriteList;

    // the trial time left, will counted down right from start
    public float timeLeft = 25;

    // after this amount of time when eye gaze hit the objects, 
    // it will be counted as "lock" (eye only scenario)
    private float _lockTime = 0;
    private float lockTime = 0;

    // after this amount of seconds when selecting, 
    // active confirmation result (correct or incorrect) 
    private float _confirmTime = 0;
    private float confirmTime = 0;

    public Sprite white;
    public Sprite blue;
    public Sprite yellow;
    public Sprite green;
    public Sprite red;

    private int trialCount = 0;
    private const int trialTimes = 10;

    public static bool trialDone = false;

    void Start()
    {
        fillGameObjectsToPattern();
        fillObjectsWithSprites(8, 4);

        prepareComponents();

        prepareCursors();
    }

    private void prepareComponents()
    {
        switch (Global.currentState)
        {
            case TrialState.Eye:
                _lockTime = 2;
                _confirmTime = 2;
                break;
            case TrialState.HeadEye:
                _lockTime = 2;
                _confirmTime = 2;
                break;
            case TrialState.Order:
                _lockTime = 2;
                _confirmTime = 2;
                break;
            case TrialState.Head:
                _lockTime = 2;
                _confirmTime = 2;
                break;
        }
        resetTime();
    }

    private void resetTime() 
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

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            if (trialCount >= trialTimes) 
            {
                // TODO: save the temporally data
            } else {
                // reshuffle, increase trial count
                fillObjectsWithSprites(8, 4);
                trialCount++;

                // TODO: save data temporally
            }
            
        }

        switch (Global.currentState)
        {
            case TrialState.Eye:
                updateInEyeOnly();
                break;
            case TrialState.HeadEye:
                updateHeadEyeParallel();
                break;
            case TrialState.Order:
                updateEyeHeadOrder();
                break;
            case TrialState.Head:
                updateInHeadOnly();
                break;
        }
    }

    public void changeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    // Condition 1
    private void updateInEyeOnly()
    {
        var patternBackground = selectedPatternSet
            .objects[0]
            .transform
            .parent
            .gameObject
            .GetComponent<SpriteRenderer>()
            .sprite;
        if (!trialDone)
        {
            if (selectedPatternSet != null && selectedPatternSet.objects.Length > 0)
            {
                patternBackground = blue;
            }
            else
            {
                // reset
                resetTime();
                return;
            }
        }

        // eye lock time counting down, but will reset 
        // and stop the next seps if there is no selected object
        lockTime -= Time.deltaTime;

        if (lockTime <= 0)
        {
            patternBackground = yellow;

            confirmTime -= Time.deltaTime;
            if (confirmTime <= 0.0)
            {
                if (!trialDone)
                {
                    trialDone = true;
                }
                if (samePattern(selectedPatternSet, mainObjPattern))
                {
                    patternBackground  = green;

                    // TODO: save data

                }
                else
                {
                    patternBackground  = red;
                    // TODO: save data
                }
            }
        }
    }

    // Condition 2
    private void updateInHeadOnly()
    {
        var patternBackground = selectedPatternSet
            .objects[0]
            .transform
            .parent
            .gameObject
            .GetComponent<SpriteRenderer>()
            .sprite;

        if (!trialDone)
        {
            if (headSelectedPatternSet != null && headSelectedPatternSet.objects.Length >0)
            {
                patternBackground = blue;
            }
            else
            {
                // reset
                resetTime();
                return;
            }
        }

        // head lock time counting down, but will reset 
        // and stop the next seps if there is no selected object
        lockTime -= Time.deltaTime;

        if (lockTime <= 0)
        {
            patternBackground = yellow;

            confirmTime -= Time.deltaTime;
            if (confirmTime <= 0)
            {
                if (!trialDone)
                {
                    trialDone = true;
                }
                if (samePattern(headSelectedPatternSet, mainObjPattern))
                {
                    patternBackground = green;

                    // TODO: save data

                }
                else
                {
                    patternBackground = red;

                    // TODO: save data

                }
            }
        }
    }

    // Condition 3
    private void updateHeadEyeParallel()
    {
        var patternBackground = selectedPatternSet
            .objects[0]
            .transform
            .parent
            .gameObject
            .GetComponent<SpriteRenderer>()
            .sprite;

        if (selectedPatternSet != null && selectedPatternSet.objects.Length > 0)
        {
            if (lockTime <= 0)
            {
                patternBackground = yellow;
                if (headSelectedPatternSet != null && headSelectedPatternSet == selectedPatternSet)
                {
                    if (!trialDone)
                    {
                        trialDone = true;
                    }
                    if (samePattern(selectedPatternSet, mainObjPattern))
                    {
                        patternBackground = green;
                        // TODO: save data as correct
                    }
                    else
                    {
                        patternBackground = red;
                        // TODO: save data as incorrect
                    }
                }
            }
            else
            {
                if (!trialDone)
                {
                    patternBackground = blue;
                }
            }

            lockTime -= timeLeft.deltaTime;
        }
        else
        {
            if (!trialDone)
            {
                resetTime();
            }
        }
    }

    // Condition  4 - concept 2
    private void updateEyeHeadOrder()
    {
        var patternBackground = selectedPatternSet
            .objects[0]
            .transform
            .parent
            .gameObject
            .GetComponent<SpriteRenderer>()
            .sprite;
        
        if (selectedPatternSet != null && selectedPatternSet.objects.Length > 0)
        {
            if (headSelectedPatternSet != null && headSelectedPatternSet == selectedPatternSet)
            {
                confirmTime -= Time.deltaTime;
                if (confirmTime <= 0)
                {
                    if (!trialDone)
                    {
                        trialDone = true;
                    }
                    if (samePattern(selectedPatternSet, mainObjPattern))
                    {
                        patternBackground = green;

                        // TODO: save data as correct

                    }
                    else
                    {
                        patternBackground = red;

                        // TODO: save data

                    }
                }
            } 
            else 
            {
                if (!trialDone)
                {
                    patternBackground = blue;
                    // reset
                    resetTime();
                }
            }
        }
        else
        {
            if (!trialDone)
            {
                // reset
            resetTime();
            }
        }
    }


    private bool samePattern(Global.GameObjectPattern pattarnA, Global.GameObjectPattern patternB)
    {
        bool result = true;
        for (int index = 0; index < pattarnA.objects.Length; index++)
        {
            var spriteA = pattarnA
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
                    .selectedPattern = subObjsGroup.patterns[index];
            }
            catch
            {
                Debug.Log(
                    "Cannot apply for `ColliderHandleHard` of subframe object at index " + index
                );
            }
        }
    }
}