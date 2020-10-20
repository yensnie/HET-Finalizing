using System;
using UnityEngine;

public enum TrialState {
    Eye, HeadEye
}

public static class Global
{
    public static TrialState currentState = TrialState.Eye;

    public class GameObjectPattern {
        public int[] order;
        public GameObject[] objects = new GameObject[4];


        public Sprite[] convertToSprites() {
            int length = this.objects.Length;
            if (length <= 0) { return null; }
            Sprite[] result = new Sprite[length];
            for (int index = 0; index < length; index++) {
                result[index] = this.objects[index].GetComponent<SpriteRenderer>().sprite;
            }
            return result;
        }
    }

    public class GameObjectPatternGroup {
        public GameObjectPattern[] patterns = 
        new GameObjectPattern[6] {
            new GameObjectPattern(),
            new GameObjectPattern(),
            new GameObjectPattern(),
            new GameObjectPattern(),
            new GameObjectPattern(),
            new GameObjectPattern(),
        };
    }
}
