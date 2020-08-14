using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Manager/GameSetting")]
public class GameSetting : ScriptableObject
{
    [SerializeField]
    private string _gameVersion = "0.0.0";

    public string GameVersion
    {
        get
        {
            return _gameVersion;
        }
    }

    
    private string _nickName = "UNTITLED";

    public string NickName
    {
        get
        {
            return UIScript.playerName;
        }
    }
   
}
