using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ground_spawn : MonoBehaviour
{
    //
    // methods
    //

    // Start is called before the first frame update
    void Start()
    {
        obCreate("ground", 0, 0, 0); // start line
        for (int i = 0; i < 100; i++)
        {
            obCreate("ground", (i + 1)*-1, 0, 0); // ground cubes
        }
    }

    // object creation
    private void obCreate(string resName, float vecX, float vecY, float vecZ)
    {
        GameObject obName =
                            Instantiate(Resources.Load(resName),
                            new Vector3(vecX, vecY, vecZ),
                            Quaternion.identity) as GameObject;
    }
}
