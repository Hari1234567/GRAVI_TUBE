using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceFieldPull : MonoBehaviour
{
    Vector3 direction;
    float distance;
    bool alive = true;
    float rad;
    Rigidbody playerRB;
    int bonusMultiFactor;


    public float forceFactor = 20;
    // Start is called before the first frame update
    void Start()
    {
        rad = transform.localScale.x;
        transform.localScale = Vector3.zero;

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            playerRB = PlayerController.camRB;
        }
        else
        {
            playerRB = PhotoniPlayerController.camRB;
        }
        bonusMultiFactor = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            bonusMultiFactor = 1;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (PlayerListingMenu.hasUpgrades)
            {
                bonusMultiFactor = 1;
            }
            else
            {
                bonusMultiFactor = 0;
            }
        }
        
        if (alive)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(rad, rad, rad), 10 * Time.deltaTime);
        }
        direction = ((transform.position - playerRB.transform.position)).normalized;
        distance = ((transform.position - playerRB.transform.position)).magnitude;
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            alive = false;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 15 * Time.deltaTime);
            Destroy(gameObject, 0.3f);
        }
      
        if (distance < 300f)
        {
            if (playerRB.velocity.magnitude < PlayerController.maxSpeed+ 5 * PlayerController.speedBonus*bonusMultiFactor)
            {
                playerRB.AddForce(direction * (distance) * Time.deltaTime * 50 + PlayerController.forceBonus*5*direction* distance * Time.deltaTime * bonusMultiFactor);
               // Debug.Log("FORCE: "+(direction * (distance) * Time.deltaTime * 50).magnitude);
            }
        }
    }
}

