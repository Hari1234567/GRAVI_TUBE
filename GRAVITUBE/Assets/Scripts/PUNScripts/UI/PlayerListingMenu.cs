using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;

    [SerializeField]
    private TextMeshProUGUI lengthText;

    [SerializeField]
    private TextMeshProUGUI wallText;

    [SerializeField]
    private TextMeshProUGUI upgradesText;

    [SerializeField]
    private PlayerListing _playerListing;
    private List<PlayerListing> listings = new List<PlayerListing>();

    public static int[] offsets;
        public static int length;
    public static bool hasWall;
    public static bool hasUpgrades;

    public static int[] wallIndices, wallRot;
    public static bool[] wallorslit;

  



    [SerializeField]
    private Button playBut;

    private MultiplayerMasterCanvas _masterCanvas;

    public void FirstInitialize(MultiplayerMasterCanvas masterCanvas)
    {
        _masterCanvas = masterCanvas;

    }

    public override void OnEnable()
    {
        base.OnEnable();
        GetCurrentRoomPlayers();
        offsets = (int[])PhotonNetwork.CurrentRoom.CustomProperties["OFFSETS"];
        length = (int)PhotonNetwork.CurrentRoom.CustomProperties["LENGTH"];
        hasWall = (bool)PhotonNetwork.CurrentRoom.CustomProperties["WALL"];
        hasUpgrades = (bool)PhotonNetwork.CurrentRoom.CustomProperties["UPGRADE"];
        wallIndices = (int[])PhotonNetwork.CurrentRoom.CustomProperties["WALLINDICES"];
        wallRot = (int[])PhotonNetwork.CurrentRoom.CustomProperties["WALLROT"];
        wallorslit = (bool[])PhotonNetwork.CurrentRoom.CustomProperties["WALLORSLIT"];

        lengthText.SetText("TRACK LENGTH : " + length);
        string ans = hasWall ? "YES" : "NO";
        wallText.text = "WALLS :" + ans;
        ans = hasUpgrades ? "YES" : "NO";
        upgradesText.text = "UPGRADES :" + ans;
        if (!PhotonNetwork.IsMasterClient)
        {
            playBut.gameObject.SetActive(false);
        }
        else
        {
            playBut.gameObject.SetActive(true);
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < listings.Count; i++)
        {
            Destroy(listings[i].gameObject);
        }
        listings.Clear();
    }
    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;
        foreach(KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
        
    }




    private void AddPlayerListing(Player player)
    {
        int index = listings.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            listings[index].setPlayerInfo(player);
        }
        else
        {
            PlayerListing listing = Instantiate(_playerListing, _content);

            if (listing != null)
            {

                listing.setPlayerInfo(player);
                listings.Add(listing);
            }
        }
        
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = listings.FindIndex(x => x.Player.NickName == otherPlayer.NickName);

            if (index != -1)
            {
                Destroy(listings[index].gameObject);
                listings.RemoveAt(index);
            }
        
    }
    
    public void OnClickStartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        TubeGenerator.Reset();
        PhotonNetwork.LoadLevel(3);
       
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(PhotonNetwork.LocalPlayer == newMasterClient)
        {
            playBut.gameObject.SetActive(true);
        }
    }



}
