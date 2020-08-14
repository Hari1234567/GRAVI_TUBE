using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    bool init = false;
    Rigidbody camRb;
    int cur = 10;

    // Start is called before the first frame update
    void Start()
    {
        camRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((!init&&TubeGenerator.centers.Count != 0)||cur>760)
        {
            transform.position = TubeGenerator.centers[10];
            cur = 10;
            init = true;
            transform.rotation = Quaternion.LookRotation(TubeGenerator.centers[11] - TubeGenerator.centers[10]);
        }

        try
        {
            if (transform.position.magnitude > TubeGenerator.centers[cur + 1].magnitude)
            {
                cur = cur + 1;
            }


            Vector3 dir = (TubeGenerator.centers[cur + 1] - TubeGenerator.centers[cur]).normalized;

            camRb.velocity = dir * 6;
        }
        catch (System.NullReferenceException n) { }


    }
}
