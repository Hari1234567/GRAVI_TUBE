using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroybyDistance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (PlayerController.camRB.transform.position.magnitude > transform.position.magnitude + 500)
            {
                Destroy(gameObject);
            }
        }
    }
}
