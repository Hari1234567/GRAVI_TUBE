using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerMasterCanvas : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private LobbyCanvas _lobbyCanvas;

    [SerializeField]
    private CurrentRoomCanvas _currentRoomCanvas;

    [SerializeField]
    private MessagePanel messagePanel;

    public LobbyCanvas LobbyCanvas
    {
        get
        {
            return _lobbyCanvas;
        }
    }

    public CurrentRoomCanvas CurrentRoomCanvas {
        get
        {
            return _currentRoomCanvas;
        }
     
    }

    public void Awake()
    {
        FirstInitialize();
    }

    public void FirstInitialize()
    {
        _lobbyCanvas.FirstInitialize(this);
        _currentRoomCanvas.FirstInitialize(this);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        messagePanel.printMessage("HOST LEFT THE GAME! " + newMasterClient.NickName + " is the new Host!");
    }

}
