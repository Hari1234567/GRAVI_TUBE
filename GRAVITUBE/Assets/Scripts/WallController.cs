using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WallController : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool ghostMode;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            ghostMode = PlayerController.ghostMode;
        }
        else
        {
            ghostMode = PhotoniPlayerController.ghostMode;
        }
        if (ghostMode)
        {
            BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();
            foreach(BoxCollider collider in colliders)
            {
                collider.isTrigger = true;
            }
        }
        else
        {
            BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();
            foreach (BoxCollider collider in colliders)
            {
                collider.isTrigger = false;
            }
        }
    }
}
