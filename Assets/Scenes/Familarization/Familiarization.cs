using System;
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
        randomizePosition();
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

    private void randomizePosition()
    {
        var positions = Enum.GetValues(typeof(Position));
        var random = new Random();
        var newPosition = (Position)positions
            .GetValue(random.Next(positions.Length));

        switch (newPosition)
        {
            case Position.Up:
                // do nothing, since it is already on top from initial
                sampleObject.transform.position = new Vector2(0, 3);
                break;
            case Position.Down:
                sampleObject.transform.position = new Vector2(0, -3);
                break;
            case Position.Left:
                sampleObject.transform.position = new Vector2(-3, 0);
                break;
            case Position.Right:
                sampleObject.transform.position = new Vector2(3, 0);
                break;
        }
    }
}
