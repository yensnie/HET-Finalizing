using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Research;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EyeOnlyHardRunner : MonoBehaviour
{
    public static Global.GameObjectPattern selectedPatternSet;
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
    public float timeLeft = 30;
    
    // after this amount of time when eye gaze hit the objects, 
    // it will be counted as "lock" (eye only scenario)
    private float eyeLockTime = 2;

    // after this amount of seconds when selecting, 
    // active confirmation result (correct or incorrect) 
    private float confirmTime = 2;  

    // the saved sprite index of the main object
    private int currentRandomIndex = -1;

    public Sprite white;
    public Sprite blue;
    public Sprite yellow;
    public Sprite green;
    public Sprite red;

    void Start()
    {
        fillFromObjectListToPattern();
        fillObjectsWithSprites(8, 4);

        if (Global.currentState == TrialState.Eye) {
            GameObject.Find("headCursor").SetActive(false);
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

        switch (Global.currentState) {
            case TrialState.Eye:
                updateInEyeOnly();
                break;
            case TrialState.HeadEye:
                updateInHeadEye();
                break;
        }
    }

    // change to the main menu
    public void changeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    private void updateInEyeOnly() {
        if (selectedPatternSet != null && selectedPatternSet.objects.Length > 0) 
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
            eyeLockTime = 2;
            confirmTime = 2;
            return;
        }

        // eye lock time counting down, but will reset 
        // and stop the next seps if there is no selected object
        eyeLockTime -= Time.deltaTime;

        if (eyeLockTime <= 0) 
        {
            selectedPatternSet
                .objects[0]
                .transform
                .parent
                .gameObject
                .GetComponent<SpriteRenderer>()
                .sprite = yellow;

            confirmTime -= Time.deltaTime;
            if (confirmTime <= 0.0) 
            {
                selectedPatternSet
                    .objects[0]
                    .transform
                    .parent
                    .gameObject
                    .GetComponent<SpriteRenderer>()
                    .sprite = samePattern(selectedPatternSet, mainObjPattern) ? green : red;

                // Get the user attempts for eyes only hard
                if (samePattern(selectedPatternSet, mainObjPattern))
                {
                    // save data
                }
                else
                {
                    // save data
                }
            }
        }
    }

    private bool samePattern(Global.GameObjectPattern pattarnA, Global.GameObjectPattern patternB) {
        bool result = true;
        for (int index = 0; index < pattarnA.objects.Length; index++) {
            var spriteA = pattarnA.objects[index].GetComponent<SpriteRenderer>().sprite.name;
            var spriteB = patternB.objects[index].GetComponent<SpriteRenderer>().sprite.name;
            if (!(spriteA.Trim().Equals(spriteB))) {
                result = false;
                break;
            }
        }
        return result;
    }

    private void updateInHeadEye() {
        // trial time count down (in total)
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0) {
            //TODO: put the trail to end state
        } else {
            //Debug.Log(timeLeft);
        }

        if (selectedPatternSet != null && selectedPatternSet.objects.Length > 0) 
        {
            if (headSelectedPatternSet != null && headSelectedPatternSet == selectedPatternSet) 
            {
                selectedPatternSet
                    .objects[0]
                    .transform
                    .parent
                    .gameObject
                    .GetComponent<SpriteRenderer>()
                    .sprite = yellow;
                confirmTime -= Time.deltaTime;
                if (confirmTime <= 0.0)
                {
                    selectedPatternSet
                        .objects[0]
                        .transform
                        .parent
                        .gameObject
                        .GetComponent<SpriteRenderer>()
                        .sprite = samePattern(selectedPatternSet, mainObjPattern) ? green : red;

                    // Get the user attempts for head and eyes hard
                    if (samePattern(selectedPatternSet, mainObjPattern))
                    {
                        // save data
                    }
                    else
                    {
                        // save data
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
            confirmTime = 2;
            return;
        }
    }

    private void fillFromObjectListToPattern() {
        //------------------------- Main object set up
        mainObjPattern = new Global.GameObjectPattern();
        mainObjPattern.objects = mainObj;

        //------------------------- Sub objects group set up
        subObjsGroup = new Global.GameObjectPatternGroup();

        var numberOfObjects = 4;
        var currentIndex = 0;
        var tempArray = new GameObject[numberOfObjects];
        var tempArrayIndex = 0;

        for (int index = 0; index < subObjList.Length; index++) {
            tempArray[tempArrayIndex] = subObjList[index];
            tempArrayIndex++;

            if ((index + 1) % numberOfObjects == 0) {
                subObjsGroup.patterns[currentIndex].objects = tempArray;

                currentIndex++;
                tempArray = new GameObject[numberOfObjects];
                tempArrayIndex = 0;
            }
        }
    }

    private void fillObjectsWithSprites(int length, int components = 4) {
        // create an array of indexs in pattern spirtes array
        int[] indexs = new int[length];
        for (int index = 0; index < length; index++) {
            indexs[index] = index;
        }

        // the array to hole our suffeled sets of patterns
        int[][] finalOrderSets = new int[length][];

        // assign the array above with random values from the array of indexs
        for (int time = 0; time < indexs.Length; time++) {
            // first, make a randomly suffled version of indexs array
            int[] suffledIndexs = indexs;
            Utility.reshuffle(suffledIndexs);

            int[] array = new int[components];
            // then assign for first 4 items in to the current position of the final order array
            for (int index = 0; index < components; index++) {
                array[index] = suffledIndexs[index];
            }
            finalOrderSets[time] = array;
        }

        System.Random random = new System.Random();

        // get the random index
        int randomIndex = random.Next(0,finalOrderSets.Length - 1);
        for (int index = 0; index < components; index++) {
            // apply the random set of sprite pattern into main Object
            mainObjPattern
                .objects[index]
                .GetComponent<SpriteRenderer>()
                .sprite = spriteList[finalOrderSets[randomIndex][index]];
        }
        // save the order into main object
        mainObjPattern.order = finalOrderSets[randomIndex];
        
        for (int index = 0; index < length; index++) {
            for (int innerIndex = 0; innerIndex < components; innerIndex++) {
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
                    "Cannot apply for ColliderHandleHard of subframe object at index " + index
                );
            }
        }
    }
}