using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;

    [SerializeField]
    private RoomListing _roomListing;
    private List<RoomListing> listings = new List<RoomListing>();
    private MultiplayerMasterCanvas _masterCanvas;
    public void FirstInitialize(MultiplayerMasterCanvas masterCanvas)
    {
        _masterCanvas = masterCanvas;
      


    
        
    }
    public override void OnJoinedRoom()
    {
        
      

        _masterCanvas.LobbyCanvas.gameObject.SetActive(false);
        _masterCanvas.CurrentRoomCanvas.gameObject.SetActive(true);
        _content.DestroyChildren();
        listings.Clear();
    }

 
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        foreach (RoomInfo info in roomList)
        {
            int index = listings.FindIndex(x => x.RoomInfo.Name == info.Name);

            if (info.RemovedFromList)
            {

                if (index != -1)
                {
                    Destroy(listings[index].gameObject);
                    listings.RemoveAt(index);
                }
            }
            else
            {
               if (index == -1)
                {
                    RoomListing listing = Instantiate(_roomListing, _content);
                    if (listing != null)
                    {

                        listing.setRoomInfo(info);
                        listings.Add(listing);
                    }
                } 
            }
            
        }
    }
}
