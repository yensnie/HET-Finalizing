using UnityEngine;

public class ColliderHandleHard : MonoBehaviour
{
    public Global.GameObjectPattern selectedPattern;
    private void registerSelectedObject() {
        if (!EyeOnlyHardRunner.trialDone && Global.currentState != TrialState.Head)
        {
            EyeOnlyHardRunner.selectedPatternSet = selectedPattern;
        }
    }

    private void deRegisterSelectedObject() {
        if (!EyeOnlyHardRunner.trialDone && Global.currentState != TrialState.Head)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite 
                =  GameObject.Find("GameRunner").GetComponent<EyeOnlyHardRunner>().white;
        }
        if (Global.currentState == TrialState.HeadEye) 
        { 
            GameObject
                .Find("headCursor")
                .GetComponent<HeadHandler>()
                .isObserving = false;
            GameObject
                .Find("headCursor")
                .GetComponent<HeadHandler>()
                .stateSequence
                .Clear();
        }       
        EyeOnlyHardRunner.selectedPatternSet = null;
    }

    private void registerHeadSelectedObject() {
        // with condition 3, the collider of Head system is not required 
        // but still need a Head tracker object
        switch (Global.currentState)
        {
            case TrialState.Eye:
                return;
            case TrialState.Head:
                break;
            case TrialState.HeadEye:
                return;
            case TrialState.Order:
                break;
        }
        if (EyeOnlyHardRunner.selectedPatternSet == selectedPattern) {
            EyeOnlyHardRunner.headSelectedPatternSet = selectedPattern;
        }
        
    }

    private void deRegisterHeadSelectedObject() {
        // with condition 3, the collider of Head system is not required 
        // but still need a Head tracker object
        switch (Global.currentState)
        {
            case TrialState.Eye:
                return;
            case TrialState.Head:
                break;
            case TrialState.HeadEye:
                return;
            case TrialState.Order:
                break;
        }
        if (EyeOnlyHardRunner.selectedPatternSet == selectedPattern) {
            if (Global.currentState == TrialState.Head && !EyeOnlyHardRunner.trialDone)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite 
                    = GameObject.Find("GameRunner").GetComponent<EyeOnlyHardRunner>().white;
            }
            EyeOnlyHardRunner.headSelectedPatternSet = null;
        }
        
    }

    /// <summary>
    /// Sent each frame where another object is within a trigger collider
    /// attached to this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("eyeCursor")) 
        {
            registerSelectedObject();
        }
        if (other.gameObject.name.Equals("headCursor")) 
        {
            registerHeadSelectedObject();
        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("eyeCursor")) 
        {
            deRegisterSelectedObject();
        }
        if (other.gameObject.name.Equals("headCursor")) 
        {
            deRegisterHeadSelectedObject();
        }
    }
}
