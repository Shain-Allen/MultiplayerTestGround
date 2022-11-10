using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerNetwork : NetworkBehaviour
{

    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnedObjectTrans;

    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Update()
    {
        //Debug.Log(OwnerClientId + " Random Number: " + randomNumber.Value);
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            spawnedObjectTrans = Instantiate(spawnedObjectPrefab);

            spawnedObjectTrans.GetComponent<NetworkObject>().Spawn(true);

            //TestServerRPC();
            //randomNumber.Value = Random.Range(0, 100);
        }
        
        Vector3 moveDir = new Vector3(0, 0, 0);
        
        if (Input.GetKey(KeyCode.W)) moveDir.z += 1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z -= 1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x -= 1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x += 1f;
        
        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Destroy(spawnedObjectTrans.gameObject);
        }
    }

    [ServerRpc]
    private void TestServerRPC()
    {
        Debug.Log("Test ServerRPC" + OwnerClientId);
    }
}
