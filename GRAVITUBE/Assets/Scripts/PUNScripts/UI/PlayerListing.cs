using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI playerNameText;



    public Player Player { get; private set; }

    public void setPlayerInfo(Player player)
    {
        Player = player;
        playerNameText.text = player.NickName;
        
        float r, g, b;
        Random.InitState(player.ActorNumber);

     
        playerNameText.color = new Color(Random.value,Random.value,Random.value);
        
    }

  

}
