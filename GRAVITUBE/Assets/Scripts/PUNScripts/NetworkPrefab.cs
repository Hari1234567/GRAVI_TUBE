using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NetworkPrefab
{
    public GameObject Prefab;
    public string Path;

    public NetworkPrefab(GameObject obj, string path)
    {
        Prefab = obj;
        Path = prefabPathModify(path);
    }

    private string prefabPathModify(string oldPath)
    {
        int extensionLength = System.IO.Path.GetExtension(oldPath).Length;
        int startIndex = oldPath.ToLower().IndexOf("resources");

        if (startIndex == -1)
        {
            return string.Empty ;
        }
        return oldPath.Substring(startIndex+10, oldPath.Length-(10+startIndex+extensionLength));
    }



}
