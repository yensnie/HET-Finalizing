using System.Collections;
using UnityEngine;
using Tobii.Research;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EyeOnlyRunner : MonoBehaviour
{
    public static GameObject selectedObj;
    public static GameObject headSelectedObj;

    public GameObject objMain;      // The main pattern object
    public GameObject[] subObj;     // pattern objects list
    public GameObject[] subFrame;   // the frame object list (which co-responding with pattern objects as container)
    public Sprite[] spriteList;     // list of sprites for patterns

    public float timeLeft = 30;     // the trial time left, will counted down right from start

    // after this amount of time when eye gaze hit the objects, it will be counted as "lock" (eye only scenario)
    private float eyeLockTime = 2;

    // after this amount of seconds when selecting, active confirmation result (correct or incorrect) 
    private float confirmTime = 2;

    private int currentRandomIndex = -1;    // the saved sprite index of the main object

    public Sprite white;
    public Sprite blue;
    public Sprite yellow;
    public Sprite green;
    public Sprite red;

    [SerializeField] private GameObject countDownPanel;

    public int correctAttempts;
    public int incorrectAttempts;
    public bool correctAttempt;
    public bool incorrectAttempt;

    public string lastName;
    public string firstName;
    public string courseStudy;
    public string matriculationNo;

    public GameObject lastName_InputField;
    public GameObject firstName_InputField;
    public GameObject course_InputField;
    public GameObject matriculation_InputField;

    void Start()
    {
        StartCoroutine(SessionOver());

        // fill random pattern sprites
        this.fillSubObjectWithRandomSprite();

        // then chose one of them randomly as the main object to compare
        this.getRandomMainObjSprite();

        if (Global.currentState == TrialState.Eye)
        {
            GameObject.Find("headCursor").SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Application.isEditor)
            {
                Application.Quit();
            }
        }

        switch (Global.currentState)
        {
            case TrialState.Eye:
                updateInEyeOnly();
                break;
            case TrialState.HeadEye:
                updateInHeadEye();
                break;
        }
    }

    // Countdown timer
    IEnumerator SessionOver()
    {
        yield return new WaitForSeconds(25);
        GameObject.Find("eyeCursor").SetActive(false);
        if (Global.currentState == TrialState.HeadEye)
        {
            GameObject.Find("headCursor").SetActive(false);
        }
        countDownPanel.SetActive(true);
    }

    // change to the main menu
    public void changeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    private void updateInEyeOnly()
    {
        if (selectedObj != null)
        {
            selectedObj.GetComponent<SpriteRenderer>().sprite = blue;
        }
        else
        {
            eyeLockTime = 2;
            confirmTime = 2;
            return;
        }
        // the selected pattern must be one of the pattern in the provided patterns list, so compare to find out its index
        int selectedIndex = System.Array.IndexOf(subFrame, selectedObj);

        if (selectedIndex == -1)
        {
            try
            {
                selectedObj.GetComponent<SpriteRenderer>().sprite = white;
            }
            catch { }
            selectedObj = null;
            eyeLockTime = 2;
            confirmTime = 2;
            return;
        }

        //eye lock time counting down, but will reset and stop the next seps if there is no selected object
        eyeLockTime -= Time.deltaTime;
        //  when the lock time is over, start the confirm time
        if (eyeLockTime <= 0)
        {
            selectedObj.GetComponent<SpriteRenderer>().sprite = yellow;

            confirmTime -= Time.deltaTime;
            if (confirmTime <= 0.0)
            {
                selectedObj.GetComponent<SpriteRenderer>().sprite = 
                    (selectedIndex == currentRandomIndex) ? green : red;

                //Get the user attempts for eyes only easy
                if (selectedIndex == currentRandomIndex)
                {
                    // do something
                }
                else
                {
                    // do something
                }
            }
        }
    }

    private void updateInHeadEye()
    {
        if (selectedObj)
        {
            if (headSelectedObj && headSelectedObj == selectedObj)
            {
                selectedObj.GetComponent<SpriteRenderer>().sprite = yellow;
            }
            else
            {
                selectedObj.GetComponent<SpriteRenderer>().sprite = blue;
            }
        }

        // the selected pattern must be one of the pattern in the provided patterns list, so compare to find out its index
        int selectedIndex = System.Array.IndexOf(subFrame, selectedObj);

        int headSelectedIndex = System.Array.IndexOf(subFrame, headSelectedObj);

        if (selectedIndex == -1 || headSelectedIndex == -1)
        {
            confirmTime = 2;
            return;
        }

        if (selectedIndex == headSelectedIndex)
        {
            confirmTime -= Time.deltaTime;
            if (confirmTime <= 0.0)
            {
                selectedObj.GetComponent<SpriteRenderer>().sprite =
                    (selectedIndex == currentRandomIndex) ? green : red;

                //Get the user attempts for head and eye easy
                if (selectedIndex == currentRandomIndex)
                {
                    // do something
                }
                else
                {
                    // do something
                }
            }
        }
    }

    private void getRandomMainObjSprite()
    {

        System.Random random = new System.Random();
        int randomIndex = random.Next(0, spriteList.Length - 1);

        // when the random main object has been chosen, save its index from the pattern's sprites array
        currentRandomIndex = randomIndex;

        // apply the pattern sprite from the sprites array by the random index
        objMain.GetComponent<SpriteRenderer>().sprite = spriteList[randomIndex];
    }

    private void fillSubObjectWithRandomSprite()
    {
        // first, shuffle the order of the sprites array
        Utility.reshuffle(spriteList);

        // finally, fill them into the pattern objects
        for (int index = 0; index < subObj.Length; index++)
        {
            subObj[index].GetComponent<SpriteRenderer>().sprite = spriteList[index];
        }
    }
}