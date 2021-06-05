using UnityEngine;

public class EyeOnlyEasyRunner : EyeOnlyBaseRunner
{
    // Start is called before the first frame update
    void Start()
    {
        fillGameObjectsToPattern();
        fillObjectsWithSprites(2, 2);
        prepareComponents();
        prepareCursors();
    }


}
