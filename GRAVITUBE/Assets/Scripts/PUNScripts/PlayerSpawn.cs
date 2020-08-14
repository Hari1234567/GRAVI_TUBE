using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerSpawn : MonoBehaviourPun
{

    [SerializeField]
    private GameObject playerGameObject;

  
        // Start is called before the first frame update
    void Start()
    {
        
      
      MasterManager.NetworkInstantiate(playerGameObject, 6 * new Vector3(2f, Mathf.Cos(Random.Range(0, 2 * Mathf.PI)), Mathf.Sin(Random.Range(0, 2 * Mathf.PI))), Quaternion.identity);
      
       
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
