using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    public static Rigidbody camRB;
    public static float score=0,scoreMultiplier=1;

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI pauseScore, pauseGems, pauseHighScore;
   
    public static bool ghostMode = false;
    public static bool magnetMode = false;
    bool positioned = false;

    public static int currencyMeter=0;
    float nextGhostTime;
    float nextMagnetTime;
    public Image crossHair;
    public GameObject forceField;
    public Material halfDoorMat;
    float deltaTime=0.0f;

    public TextMeshProUGUI upsideDownText;
    

    public static float maxSpeed = 70;
    

    public Slider boostTimer,magnetTimer,fuelTimer;

    public TextMeshProUGUI currencyCounter;
    
    public TextMeshProUGUI velocityText;

    public Slider multiplierSlider;

    public TextMeshProUGUI multiplierText;

    public TextMeshProUGUI highScoreText;
    
    Animator crossHairAnim;
    public static int highScore;


    float crossWidth, crossHeight;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    public float ghostTime = 10;
    public float magnetTime = 10;
    public float fuelTime = 50;
    float nextFuelTime;

    public static int speedBonus, forceBonus, magnetBonus, boostBonus, fuelBonus;


    Vector3 currencyScale;

    [RuntimeInitializeOnLoadMethod]
    public static void loadSavedData()
    {
        highScore = PlayerPrefs.GetInt("HIGHSCORE");
        currencyMeter = PlayerPrefs.GetInt("CURRENCY");
        speedBonus = PlayerPrefs.GetInt("SPEEDBONUS");
        forceBonus = PlayerPrefs.GetInt("FORCEBONUS");
        magnetBonus = PlayerPrefs.GetInt("MAGNETBONUS");
        boostBonus = PlayerPrefs.GetInt("BOOSTBONUS");
        fuelBonus = PlayerPrefs.GetInt("FUELBONUS");

       
        
        
        
    }
    void Start()
    {
        positioned = false;
        score = 0;
        loadSavedData();

        camRB = GetComponent<Rigidbody>();
        nextFuelTime = Time.time + fuelTime;
        positioned = false;
        currencyScale = currencyCounter.transform.localScale;
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            ManualFindGameObjects();
        }
        crossHairAnim = crossHair.GetComponent<Animator>();
        crossWidth = crossHair.rectTransform.rect.width;
        crossHeight = crossHair.rectTransform.rect.height;
        boostTimer.maxValue = ghostTime + boostBonus * 5 ;
        magnetTimer.maxValue = magnetTime + magnetBonus*5;
        fuelTimer.maxValue = fuelTime + fuelBonus*10;
        multiplierSlider.maxValue = 3;
        
        
    }

    public void ManualFindGameObjects()
    {
        crossHair = GameObject.Find("CrossHair").GetComponent<Image>();
        boostTimer = GameObject.Find("BoostTimer").GetComponent<Slider>();
        magnetTimer = GameObject.Find("MagnetTimer").GetComponent<Slider>();
        currencyCounter = GameObject.Find("CurrencyCounter").GetComponent<TextMeshProUGUI>();
        velocityText = GameObject.Find("VelocityText").GetComponent<TextMeshProUGUI>();

    }
    
    void Update()
    {
        
        if (!UIScript.pauseMenu && !UIScript.gameOver)
        {
            if (upsideDownText != null)
            {
                upsideDownText.gameObject.SetActive(Vector3.Dot(transform.up, Vector3.down) > 0);
            }
            if (!positioned)
            {

                transform.position = new Vector3(5f, Mathf.Cos(Random.Range(0, 2 * Mathf.PI)), Mathf.Sin(Random.Range(0, 2 * Mathf.PI)));
                    transform.rotation = Quaternion.LookRotation((TubeGenerator.centers[300] - TubeGenerator.centers[299]).normalized);
                    camRB.velocity = (TubeGenerator.centers[300] - TubeGenerator.centers[299]).normalized * 4f;
                    positioned = true;
               
            }
            pauseGems.text = currencyMeter + "";
            pauseScore.text = Mathf.Round(score) + "";
            
           pauseHighScore.text = highScore+ "";
            highScoreText.text = "HIGH: " + highScore;
            scoreText.text = "SCORE: " + Mathf.Round(score);

            scoreMultiplier =(int)(camRB.velocity.magnitude / 27f);
            multiplierSlider.value = Mathf.Lerp(multiplierSlider.value,scoreMultiplier,Time.deltaTime*10);
            multiplierText.SetText(Mathf.Round(multiplierSlider.value) + "x");
            score += scoreMultiplier*0.1f*(camRB.velocity.magnitude) * Time.deltaTime;
         
            fuelTimer.value = Mathf.Lerp(fuelTimer.value, nextFuelTime - Time.time, Time.deltaTime * 10);
            if (fuelTimer.value == 0)
            {
                if (score > highScore)
                {
                    highScore = (int)score;
                    PlayerPrefs.SetInt("HIGHSCORE", highScore);

                }
                PlayerPrefs.SetInt("CURRENCY", currencyMeter);
                UIScript.gameOver = true;
            }
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * Time.deltaTime;
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            float brake = Input.GetAxisRaw("Jump");
         

            currencyCounter.SetText(currencyMeter.ToString());
            velocityText.SetText("VELOCITY: " + ((int)camRB.velocity.magnitude).ToString());
            currencyCounter.transform.localScale = Vector3.Lerp(currencyCounter.transform.localScale, currencyScale, Time.deltaTime * 15);
            Rigidbody myRB = GetComponent<Rigidbody>();
            if (ghostMode)
            {

                halfDoorMat.SetFloat("Vector1_86F1458A", Mathf.Lerp(halfDoorMat.GetFloat("Vector1_86F1458A"), 0f, Time.deltaTime * 7f));
            }
            else
            {
                halfDoorMat.SetFloat("Vector1_86F1458A", Mathf.Lerp(halfDoorMat.GetFloat("Vector1_86F1458A"), 1f, Time.deltaTime * 7f));
            }
            float localUp = Vector3.Dot(myRB.velocity, transform.up.normalized);
            float localRight = Vector3.Dot(myRB.velocity, transform.right.normalized);

            myRB.AddForce(-brake * 10 * GetComponent<Rigidbody>().velocity.normalized * Time.deltaTime * 200f);
            
            if ((moveX > 0 && localRight < 13f) || (moveX < 0 && localRight > -13f))
                myRB.AddRelativeForce(new Vector3(moveX * 10, 0) * Time.deltaTime * 300f);
            if ((moveY > 0 && localUp < 13f) || (moveY < 0 && localUp > -13f))
                myRB.AddRelativeForce(new Vector3(0, moveY * 10) * Time.deltaTime * 300f);
            rotY +=UIScript.mouseSensitivity* mouseX * mouseSensitivity * Time.fixedDeltaTime;
            rotX +=UIScript.mouseSensitivity* mouseY * mouseSensitivity * Time.fixedDeltaTime;

            transform.rotation = Quaternion.Euler(rotX, rotY, 0f);
            if (ghostMode)
            {
                if (Vector3.Dot(myRB.velocity, transform.forward) < 70f)
                {
                    myRB.AddForce(transform.forward * Time.deltaTime * 2000f);
                }
            }
            crossHairAnim.SetBool("MouseDown", Input.GetMouseButton(0) || Input.GetMouseButton(1));
           
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {


                if (Physics.Raycast(new Ray(transform.position, Camera.main.ScreenToWorldPoint(crossHair.rectTransform.position + new Vector3(0, 0, 1f)) - Camera.main.transform.position), out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Tube"))
                    {

                        Instantiate(forceField, hit.point + hit.normal.normalized * (forceField.transform.localScale.x / 2), Quaternion.identity);
                    }



                }
                else
                {

                }

            }





            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                GameObject forceField = GameObject.FindGameObjectWithTag("ForceField");
                if (forceField != null)
                {
                    if (Vector3.Dot(transform.forward, forceField.transform.position - transform.position) > 0)
                        crossHair.rectTransform.position = Camera.main.WorldToScreenPoint(forceField.transform.position);
                }
            }
            else
            {

                crossHair.rectTransform.position = Vector3.Lerp(crossHair.rectTransform.position, new Vector3(Screen.width / 2, Screen.height / 2), Time.deltaTime * 10);
                if (Physics.Raycast(new Ray(transform.position, Camera.main.ScreenToWorldPoint(crossHair.rectTransform.position + new Vector3(0, 0, 1f)) - Camera.main.transform.position), out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Tube"))
                    {
                        crossHair.rectTransform.sizeDelta = new Vector3(crossWidth + 700 / hit.distance, crossHeight + 700 / hit.distance);
                    }
                    else
                    {
                        crossHair.rectTransform.sizeDelta = Vector3.Lerp(crossHair.rectTransform.sizeDelta, new Vector3(crossWidth, crossHeight), Time.deltaTime * 10);
                    }



                }
            }

            

            if (Time.time > nextGhostTime)
            {
                ghostMode = false;
            }
            if (ghostMode)
            {
                boostTimer.value = nextGhostTime - Time.time;
                boostTimer.GetComponent<Animator>().SetBool("Low", boostTimer.value < 0.35f * magnetTime);
            }
            else
            {
                boostTimer.value = 0;
            }
            if (magnetMode)
            {
                magnetTimer.value = nextMagnetTime - Time.time;
                magnetTimer.GetComponent<Animator>().SetBool("Low", magnetTimer.value < 0.35f * magnetTime);
            }
            else
            {
                magnetTimer.value = 0;
            }

            if (Time.time > nextMagnetTime)
            {
                magnetMode = false;
            }

            magnetTimer.gameObject.SetActive(magnetMode);
            boostTimer.gameObject.SetActive(ghostMode);

        }
      }

   


    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperRight;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            float collisionForce = collision.impulse.magnitude / Time.deltaTime;
            if (collisionForce > 1000)
            {
                nextFuelTime -= 5;
            } 
           
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            Destroy(other.gameObject);
            nextFuelTime = Time.time + fuelTime+ fuelBonus*10;
        }
        if(other.tag == "GhostLoop")
        {
            ghostMode = true;
            nextGhostTime = Time.time + ghostTime + boostBonus*5;
        }
        if(other.tag == "Currency")
        {
            currencyMeter++;
            currencyCounter.transform.localScale *= 1.3f;
            
            Destroy(other.gameObject);
        }
        if (other.tag == "MagnetLoop")
        {
        
            magnetMode = true;
            nextMagnetTime = Time.time + magnetTime+ magnetBonus*5;
        }
    }


}
