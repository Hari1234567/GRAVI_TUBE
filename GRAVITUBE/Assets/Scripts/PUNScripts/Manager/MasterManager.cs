using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(menuName = "Singletons/MasterManager")]
public class MasterManager : ScriptableSingletonObject<MasterManager>
{
    [SerializeField]
    private GameSetting _gameSetting;
    public static GameSetting GameSetting
    {
        get
        {
            return Instance._gameSetting;
        }
    }
    [SerializeField]
    private List<NetworkPrefab> _networkPrefabs = new List<NetworkPrefab>();
    public static GameObject NetworkInstantiate(GameObject obj, Vector3 position, Quaternion rotation)
    {
        
        foreach (NetworkPrefab networkPrefab in Instance._networkPrefabs)
        {
            if (networkPrefab.Prefab == obj)
            {
                if (networkPrefab.Path != string.Empty)
                {
                    GameObject result = PhotonNetwork.Instantiate(networkPrefab.Path, position, rotation);
                    return result;
                }
                else
                {
                    Debug.Log("Empty Path");
                    return null;
                }
            }
        }
        return null;
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void populateNetworkPrefab()
    {

#if UNITY_EDITOR
        if(Instance!=null)
        Instance._networkPrefabs.Clear();
        GameObject[] results = Resources.LoadAll<GameObject>("");
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i].GetComponent<PhotonView>() != null)
            {
                string path = AssetDatabase.GetAssetPath(results[i]);
                Instance._networkPrefabs.Add(new NetworkPrefab(results[i], path));
                
            }

        }
       
        for(int i = 0; i < Instance._networkPrefabs.Count; i++)
        {
            Debug.Log(Instance._networkPrefabs[i].Prefab.name);
        }
#endif
    }

}
