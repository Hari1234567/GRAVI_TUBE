using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;


public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI _roomName;

    [SerializeField]
    private Slider lengthSlider;

    [SerializeField]
    private Toggle upgradesToggle;

    [SerializeField]
    private Toggle wallToggle;

    [SerializeField]
    private MessagePanel messagePanel;



    private MultiplayerMasterCanvas _masterCanvas;

    private ExitGames.Client.Photon.Hashtable customRoomProps = new ExitGames.Client.Photon.Hashtable();

   
    

    
    public void FirstInitialize(MultiplayerMasterCanvas masterCanvas)
    {
       
        _masterCanvas = masterCanvas;
    }
    public void createRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        string roomName = _roomName.text;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;
        List<int> wallIndices = new List<int>();
        List<int> wallRot = new List<int>();
        List<bool> wallorslit = new List<bool>(); 
        for(int i = 0; i < (int)lengthSlider.value*800; i++)
        {
            if(i%15 == 0 && Random.Range(0,100)>60f) {
                wallIndices.Add(i%799);
                wallRot.Add(Random.Range(0, 360));
                wallorslit.Add(Random.Range(0, 100) > 40);
            }
            else
            {
                wallIndices.Add(-1);
                wallRot.Add(-1);
                wallorslit.Add(false);
            }
        }
       
        if (customRoomProps["LENGTH"] == null)
        {
            customRoomProps.Add("LENGTH", (int)lengthSlider.value);
            customRoomProps.Add("WALL", wallToggle.isOn);
            customRoomProps.Add("UPGRADE", upgradesToggle.isOn);
            customRoomProps.Add("WALLINDICES", wallIndices.ToArray());
            customRoomProps.Add("WALLROT", wallRot.ToArray());
            customRoomProps.Add("WALLORSLIT", wallorslit.ToArray());
            
        }
        else
        {
            customRoomProps["LENGTH"] = (int)lengthSlider.value;
            customRoomProps["WALL"] = wallToggle.isOn;
            customRoomProps["UPGRADE"] = upgradesToggle.isOn;
            customRoomProps["WALLINDICES"] = wallIndices.ToArray();
            customRoomProps["WALLROT"] = wallRot.ToArray();
            customRoomProps["WALLORSLIT"] = wallorslit.ToArray();
        }
        
            int[] offsets = new int[3];
        offsets[0] = Random.Range(0, 100);
        offsets[1] = Random.Range(0, 100);
        offsets[2] = Random.Range(0, 100);
        if (customRoomProps["OFFSETS"] == null)
        {
            customRoomProps.Add("OFFSETS", offsets);
        }
        else
        {
            customRoomProps["OFFSETS"] = offsets;
        }
        roomOptions.CustomRoomProperties = customRoomProps;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);

    }
    public override void OnCreatedRoom()
    {
        
       _masterCanvas.CurrentRoomCanvas.gameObject.SetActive(true);
        _masterCanvas.LobbyCanvas.gameObject.SetActive(false);
        Debug.Log("Room Created Succesfully");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
  
        messagePanel.printMessage("Room Creation Failed, Please try again"); 
   
    }
}
