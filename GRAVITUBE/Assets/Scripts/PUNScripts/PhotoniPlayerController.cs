using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class PhotoniPlayerController : MonoBehaviourPunCallbacks { 


    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    public static Rigidbody camRB;
    public static float score = 0, scoreMultiplier = 1;
    static List<Player> leaderBoardList;
    bool raceOver = false;
    float ghostTime = 5;
    TextMeshProUGUI upsideDownText;




    public static bool ghostMode = false;
    public static bool magnetMode = false;
    bool positioned = false;

    public static int currencyMeter = 0;
    float nextGhostTime;
    float nextMagnetTime;
    Image crossHair;

    GameObject countDownText;


    public GameObject forceField;

    public Material halfDoorMat;
    float deltaTime = 0.0f;
    bool canMove = true;

    public static float maxSpeed = 70;


 

    TextMeshProUGUI currencyCounter;

    TextMeshProUGUI velocityText;

    TextMeshProUGUI progressText;

    TextMeshProUGUI posText;
    Slider boostTimer;
    bool leaderMenu = false;

    Vector3 oldPosition = Vector3.zero;

    Animator crossHairAnim;
    public static int highScore;


    float crossWidth, crossHeight;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis


    List<GameObject> players;
    public TextMeshProUGUI playerNameDisplay;
     Rigidbody myRB;


    public static int speedBonus, forceBonus, magnetBonus, boostBonus, fuelBonus;

    public static int playerID;

    float nextCountTime;
    float timer = 3;
    bool countDown = true;
    GameObject leaderBoardMenu;
    

    Vector3 currencyScale;

    public GameObject leaderBoardListing;
    Transform leaderBoardListingTransform;



    MaterialPropertyBlock colorBlock;

    int position=1;

    GameObject finishLine;
    MessagePanel messagePanel;
// Start is called before the first frame update

    void loadReferences()
    {
        Referencer referencer = GameObject.FindGameObjectWithTag("Referencer").GetComponent<Referencer>();
        currencyCounter = referencer.currencyCounter;
        velocityText = referencer.velocityText;
        posText = referencer.positionText;
        crossHair = referencer.CrossHair;
        progressText = referencer.progressText;
      
        countDownText = referencer.countDown;
        leaderBoardMenu = referencer.leaderBoardMenu;
        leaderBoardListingTransform = referencer.leaderBoardListingTransform;
        messagePanel = referencer.messagePanel;
        boostTimer = referencer.boostTimer;
        upsideDownText = referencer.upsideDownText;
    }
    void Start()
    {
        colorBlock = new MaterialPropertyBlock();
        players = new List<GameObject>();
        Random.InitState(photonView.Owner.ActorNumber);
        Color playerColor = new Color(Random.value, Random.value, Random.value);
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.GetPropertyBlock(colorBlock);
        colorBlock.SetColor("Color_61B2214E", playerColor);
        
        meshRenderer.SetPropertyBlock(colorBlock);
        playerNameDisplay.color = playerColor;
        loadReferences();
        leaderBoardList = new List<Player>();
       
        if (this.photonView.IsMine)
        {
            if(PlayerListingMenu.hasUpgrades)
            boostTimer.maxValue = ghostTime + PlayerController.boostBonus * 5;
            else
                boostTimer.maxValue = ghostTime ;
            GetComponentInChildren<Camera>().enabled=true;

            camRB = GetComponent<Rigidbody>();
            myRB = GetComponent<Rigidbody>();
        
            positioned = false;
            currencyScale = currencyCounter.transform.localScale;

            crossHairAnim = crossHair.GetComponent<Animator>();
            crossWidth = crossHair.rectTransform.rect.width;
            crossHeight = crossHair.rectTransform.rect.height;
           
           
            nextCountTime = Time.time + timer;
        }
        else
        {

            GetComponentInChildren<Camera>().enabled = false;
        }


    }

    
    void Update()
    {
        playerNameDisplay.text = photonView.Owner.NickName;

        if (photonView.IsMine)
        {

            velocityText.text = "SPEED: "+(int)myRB.velocity.magnitude;
            if (finishLine == null)
            {
                finishLine = GameObject.FindGameObjectWithTag("FinishLine");
            }
            else
            {
            
                progressText.text = "PROGRESS: "+(int)Mathf.Clamp(transform.position.magnitude * 100 / finishLine.transform.position.magnitude,0,100)+"%";
            }
            if (PhotonNetwork.PlayerList.Length != players.Count)
            {
                players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));



            }
            else
            {

                players = players.OrderByDescending(x => x.transform.position.magnitude).ToList();
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i] != null)
                    {
                        if (players[i].GetComponent<PhotonView>().IsMine)
                        {
                            position = i + 1;
                        }
                    }

                }
            }
            posText.text = position + " / " + players.Count;
            float localUp = Vector3.Dot(myRB.velocity, transform.up.normalized);
            float localRight = Vector3.Dot(myRB.velocity, transform.right.normalized);
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            float brake = Input.GetAxisRaw("Jump");
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * Time.deltaTime;
            rotY += UIScript.mouseSensitivity * mouseX * mouseSensitivity * Time.fixedDeltaTime;
            rotX += UIScript.mouseSensitivity * mouseY * mouseSensitivity * Time.fixedDeltaTime;
            myRB.AddForce(-myRB.velocity * 0.1f);
            myRB.AddForce(-brake * 10 * GetComponent<Rigidbody>().velocity.normalized * Time.deltaTime * 200f);
            myRB.AddForce(-brake * 10 * GetComponent<Rigidbody>().velocity.normalized * Time.deltaTime * 200f);
            if ((moveX > 0 && localRight < 13f) || (moveX < 0 && localRight > -13f) && !countDown)
                myRB.AddRelativeForce(new Vector3(moveX * 10, 0) * Time.deltaTime * 300f);
            if ((moveY > 0 && localUp < 13f) || (moveY < 0 && localUp > -13f)&&!countDown)
                myRB.AddRelativeForce(new Vector3(0, moveY * 10) * Time.deltaTime * 300f);
           
          

            if (canMove)
                transform.rotation = Quaternion.Euler(rotX, rotY, 0f);

           
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                canMove = !canMove;
                leaderMenu = !leaderMenu;
            }
            leaderBoardMenu.SetActive(leaderMenu || raceOver);

            countDown = Time.time < nextCountTime;
            countDownText.GetComponentInChildren<TextMeshProUGUI>().text = "" + Mathf.Round(nextCountTime - Time.time);
            countDownText.SetActive(countDown);
            crossHairAnim.SetBool("MouseDown",  Input.GetMouseButton(0) || Input.GetMouseButton(1));
            RaycastHit hit;
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !leaderMenu && !raceOver)
            {


                if (Physics.Raycast(new Ray(transform.position, Camera.main.ScreenToWorldPoint(crossHair.rectTransform.position + new Vector3(0, 0, 1f)) - Camera.main.transform.position), out hit))
                {
                    if (!countDown && hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Tube"))
                    {

                        Instantiate(forceField, hit.point + hit.normal.normalized * (forceField.transform.localScale.x / 2), Quaternion.identity);
                    }



                }
                else
                {

                }

            }

            if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && !raceOver && !leaderMenu)
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
            if (ghostMode)
            {

                halfDoorMat.SetFloat("Vector1_86F1458A", Mathf.Lerp(halfDoorMat.GetFloat("Vector1_86F1458A"), 0f, Time.deltaTime * 7f));
            }
            else
            {
                halfDoorMat.SetFloat("Vector1_86F1458A", Mathf.Lerp(halfDoorMat.GetFloat("Vector1_86F1458A"), 1f, Time.deltaTime * 7f));
            }
            if (ghostMode)
            {
                if (Vector3.Dot(myRB.velocity, transform.forward) < 70f)
                {
                    myRB.AddForce(transform.forward * Time.deltaTime * 2000f);
                }
            }
            if (Time.time > nextGhostTime)
            {
                ghostMode = false;
            }
            if (ghostMode)
            {
                boostTimer.value = nextGhostTime - Time.time;
                boostTimer.GetComponent<Animator>().SetBool("Low", boostTimer.value < 0.35f * ghostTime);
            }
            else
            {
                boostTimer.value = 0;
            }
            boostTimer.gameObject.SetActive(ghostMode);
            if (upsideDownText != null && !raceOver)
            {
                upsideDownText.gameObject.SetActive(Vector3.Dot(transform.up, Vector3.down) > 0);
            }

        }
        else
        {
            this.photonView.enabled = false;
        }
    }

    [PunRPC]

    void RPCUpdateLeaderboard(Player player)
    {
        LeaderboardListing newListing = Instantiate(leaderBoardListing, leaderBoardListingTransform).GetComponent<LeaderboardListing>();
        newListing.nameText.text = player.NickName;
        leaderBoardList.Add(player);
        
        newListing.positionText.text = leaderBoardList.Count  + "";
        Random.InitState(player.ActorNumber);

        newListing.setTextColor(new Color(Random.value, Random.value, Random.value));
        if(player!=PhotonNetwork.LocalPlayer)
        messagePanel.printMessage(player.NickName + " has reached finish with position " + leaderBoardList.Count);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FinishLine")
        {
            if (photonView.IsMine)
            {
                if (!raceOver)
                {
                    photonView.RPC("RPCUpdateLeaderboard", RpcTarget.All, photonView.Owner);
                    leaderMenu = true;
                    raceOver = true;
                }

            }
        }
            if (other.tag == "GhostLoop")
            {
                if (photonView.IsMine)
                {
                    
                    ghostMode = true;
                    if (PlayerListingMenu.hasUpgrades)
                        nextGhostTime = Time.time + ghostTime + PlayerController.boostBonus * 5;
                    else
                        nextGhostTime = Time.time + ghostTime;
                }
            
            
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        messagePanel.printMessage("You have disconnected from the server(" + cause.ToString() + ")");
    }

   


}
