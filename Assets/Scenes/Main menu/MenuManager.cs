using UnityEngine.UI;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public InputField nameField;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void saveCurrentName()
    {
        Global.participantName = nameField.text;
    }
}
