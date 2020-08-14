using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class LeaveLobby : MonoBehaviourPun
{
    public void OnClickLeaveLobby()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}
