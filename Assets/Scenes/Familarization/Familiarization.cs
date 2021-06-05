using UnityEngine;

public class Familiarization : MonoBehaviour
{
    enum Position
    {
        Up, Down, Left, Right
    }
    
    public GameObject sampleObject;
    void Start()
    {
        Helper.prepareCursors();
    }

    void Update()
    {
        switch (Global.currentState)
        {
            case TrialState.Eye:
                break;
            case TrialState.Head:
                break;
            case TrialState.HeadEye:
                break;
            case TrialState.Order:
                break;
            case TrialState.Trial:
                break;
        }
    }
}
