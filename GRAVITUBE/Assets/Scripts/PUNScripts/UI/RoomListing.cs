using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;



    public RoomInfo RoomInfo { get; private set; }

    public void setRoomInfo(RoomInfo info)
    {
        RoomInfo = info;
        _text.text = info.Name;
        
        
        
    }
 

    public void onClickJoinRoom()
    {


        TubeGenerator.Reset();
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }
}
