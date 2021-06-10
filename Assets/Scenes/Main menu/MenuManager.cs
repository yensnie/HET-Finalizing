using UnityEngine.UI;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public InputField nameField;
    
    public void saveCurrentName()
    {
        Global.participantName = nameField.text;
    }
}
