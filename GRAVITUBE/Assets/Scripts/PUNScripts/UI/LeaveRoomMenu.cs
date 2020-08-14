using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LeaveRoomMenu : MonoBehaviour
{

    private MultiplayerMasterCanvas _masterCanvas;
     public void OnClickLeaveRoom() { 
    
        PhotonNetwork.LeaveRoom(true);
        _masterCanvas.CurrentRoomCanvas.gameObject.SetActive(false);
        _masterCanvas.LobbyCanvas.gameObject.SetActive(true);
    }

    public void FirstInitialize(MultiplayerMasterCanvas masterCanvas)
    {
        _masterCanvas = masterCanvas;
    }
    

}
