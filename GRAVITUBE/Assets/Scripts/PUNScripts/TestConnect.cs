using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestConnect : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private MessagePanel messagePanel;
    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log("Connecting to Server");
        try
        {
            PhotonNetwork.GameVersion = MasterManager.GameSetting.GameVersion;
        PhotonNetwork.NickName = MasterManager.GameSetting.NickName;
        PhotonNetwork.AutomaticallySyncScene = true;
       
            PhotonNetwork.ConnectUsingSettings();
        }catch(System.Net.Sockets.SocketException s)
        {
            messagePanel.printMessage("Couldn't connect to server, please exit and try again!");
        }
        
        }

    
    public override void OnConnectedToMaster()
    {
        if(!PhotonNetwork.InLobby)
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected to Server with name "+ PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
         if(cause == DisconnectCause.MaxCcuReached)
        {
            messagePanel.printMessage("Maximum CCU reached, couldn't connect");

        }   
         if(cause == DisconnectCause.ClientTimeout)
        {
            messagePanel.printMessage("TIME OUT! Please check your internet connection!(Try exiting and re entering lobby)");
        }
    }

    
}
