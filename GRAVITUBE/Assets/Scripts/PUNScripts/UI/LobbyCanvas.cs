using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour
{
    [SerializeField]
    private CreateRoom _createRoom;

    [SerializeField]
    private RoomListingMenu _roomListingMenu;

    private MultiplayerMasterCanvas _masterCanvas;

    public void FirstInitialize(MultiplayerMasterCanvas masterCanvas)
    {
        _masterCanvas = masterCanvas;
        _createRoom.FirstInitialize(masterCanvas);
        _roomListingMenu.FirstInitialize(masterCanvas);
    }

}
