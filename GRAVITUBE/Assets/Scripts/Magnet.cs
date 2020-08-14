using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Magnet : MonoBehaviour
{
 

    void Update()
    {
        
        if (PlayerController.magnetMode && SceneManager.GetActiveScene().buildIndex==1)
        {
            if (Vector3.Magnitude(transform.position - PlayerController.camRB.transform.position) < 30f)
            {
                transform.position = Vector3.Lerp(transform.position, PlayerController.camRB.transform.position, Time.deltaTime * 15f);
            }
        }

        transform.Rotate(new Vector3(0, 0, Time.deltaTime * 40f));

    }
}
