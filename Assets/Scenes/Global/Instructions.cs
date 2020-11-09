using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    [SerializeField] private GameObject Instructions_Eyes_only ;
    [SerializeField] private GameObject Instructions_Head_Eyes;
    void Start()
    {
        if (Global.currentState == TrialState.Eye)
        {
            Instructions_Eyes_only.SetActive(true);         
        }
        else if (Global.currentState == TrialState.HeadEye)
        {
            Instructions_Head_Eyes.SetActive(true);        
        }
    }

}
