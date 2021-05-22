using UnityEngine;
using Tobii.Research;

public enum TrialState
{
    Eye, Head, Order, HeadEye
}
public enum AttemptState
{
    Correct, Incorrect, Unknown, Timeout
}
public static class Global
{
    public static TrialState currentState = TrialState.Eye;

    public class GameObjectPattern
    {
        public int[] order;
        public GameObject[] objects = new GameObject[4];

        public Sprite[] convertToSprites()
        {
            int length = this.objects.Length;
            if (length <= 0) { return null; }
            Sprite[] result = new Sprite[length];
            for (int index = 0; index < length; index++)
            {
                result[index] = 
                this
                    .objects[index]
                    .GetComponent<SpriteRenderer>()
                    .sprite;
            }
            return result;
        }
    }

    public class GameObjectPatternGroup
    {
        public GameObjectPattern[] patterns =
        new GameObjectPattern[8] {
            new GameObjectPattern(),
            new GameObjectPattern(),
            new GameObjectPattern(),
            new GameObjectPattern(),
            new GameObjectPattern(),
            new GameObjectPattern(),
            new GameObjectPattern(),
            new GameObjectPattern(),
        };
    }
}

class Utility
{
    /// <summary>
    /// shuffle the elements in an array
    /// </summary>
    /// <param name = "array">The array that need 
    /// to reorder the elements inside</param>
    public static void reshuffle<T>(T[] array)
    {
        for (int index = 0; index < array.Length; index++)
        {
            T temp = array[index];
            int random = Random.Range(index, array.Length);
            array[index] = array[random];
            array[random] = temp;
        }
    }

    /// <summary>
    /// turn a NormalizedPoint2D to a 2D vector
    /// </summary>
    public static Vector2 ToVector2(NormalizedPoint2D value)
    {
        return new Vector2(value.X, value.Y);
    }

    /// <summary>
    /// turn a 3D point to a 3D vector
    /// </summary>
    public static Vector3 ToVector3(Point3D point)
    {
        return new Vector3(point.X, point.Y, point.Z);
    }

    /// <summary>
    /// get the first found eye tracker
    /// </summary>
    public static void getFirstEyeTracker(System.Action<IEyeTracker> handle)
    {
        EyeTrackerCollection trackers = EyeTrackingOperations.FindAllEyeTrackers();
        foreach (IEyeTracker eyeTracker in trackers)
        {
            Debug.Log(
                string.Format
                (
                    "Adress: {0}, Name: {1}, Mode: {2}, Serial number: {3}, Firmware version: {4}",
                    eyeTracker.Address,
                    eyeTracker.DeviceName,
                    eyeTracker.Model,
                    eyeTracker.SerialNumber,
                    eyeTracker.FirmwareVersion
                )
            );
        }
        if (trackers.Count > 0)
        {
            // --- connect 1st eye tracker
            var eyeTracker = trackers[0];
            Debug.Log("did get the eye tracker");
            handle(eyeTracker);
        }
    }
}