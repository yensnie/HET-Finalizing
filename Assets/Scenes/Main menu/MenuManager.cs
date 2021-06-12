using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public InputField nameField;
    
    public void saveCurrentName()
    {
        Global.participantName = nameField.text;
    }

    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Space))
        {
            Global.currentState = TrialState.Trial;
            SceneManager.LoadScene("Familiarization");
        }
    }
}
