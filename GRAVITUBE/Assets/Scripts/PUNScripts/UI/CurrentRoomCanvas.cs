using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{

    [SerializeField]
    private PlayerListingMenu _playerListingMenu;

    [SerializeField]
    private LeaveRoomMenu _leaveRoomMenu;

    private MultiplayerMasterCanvas _masterCanvas;
      public void FirstInitialize(MultiplayerMasterCanvas masterCanvas)
    {
        _masterCanvas = masterCanvas;
        _leaveRoomMenu.FirstInitialize(masterCanvas);
        _playerListingMenu.FirstInitialize(masterCanvas);
    }


}
