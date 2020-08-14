using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RotateWall : MonoBehaviour
{
    float speed;
    bool canSpin;
    // Start is called before the first frame update
    void Start()
    {
        canSpin = Random.Range(0, 10) > 8;
        speed = Random.Range(10, 30);
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpin && SceneManager.GetActiveScene().buildIndex==1)
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * speed));

        
    }
}
