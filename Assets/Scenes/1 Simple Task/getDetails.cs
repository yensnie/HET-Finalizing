using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getDetails : MonoBehaviour
{
    public string lastName;
    public string firstName;
    public string courseStudy;
    public string matriculationNo;

    public GameObject last_InputField;
    public GameObject firstName_InputField;
    public GameObject course_InputField;
    public GameObject matriculation_InputField;

    
    public void getUserDetails()
    {
        lastName = last_InputField.GetComponent<Text>().text;
        firstName = firstName_InputField.GetComponent<Text>().text;
        courseStudy = course_InputField.GetComponent<Text>().text;
        matriculationNo = matriculation_InputField.GetComponent<Text>().text;
        CSVManager.appendtoFile(new string[6] {
            lastName,
            firstName,
            courseStudy,
            matriculationNo,
            "2",
            "4"
        }); 
         Debug.Log("Details Updated");
    }
}
