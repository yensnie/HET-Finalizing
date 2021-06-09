using UnityEngine.SceneManagement;
using UnityEngine;

public class PrepareRunner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !Application.isEditor)
        {
            switch (Global.currentLevel)
        {
            case TrialLevel.Easy:
                SceneManager.LoadScene("EyeOnluSceneEasy");
                break;
            case TrialLevel.Hard:
                SceneManager.LoadScene("EyeOnlySceneHard");
                break;
            default:
                return;
        }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Application.isEditor)
            {
                SceneManager.LoadScene("Menu & Calibration");
            }
        }
    }
}
