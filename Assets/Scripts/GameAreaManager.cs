using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameAreaManager : NetworkBehaviour
{
    [SerializeField] private Transform Player1Area;
    [SerializeField] private Transform Player2Area;

    [SerializeField] private ConnectionNotificationManager CNM;


    public void OnServerStart()
    {
        gameObject.GetComponent<NetworkObject>().RemoveOwnership();

        CNM.OnClientConnectionNotification += CNM_OnClientConnectionNotification;
    }


    private void CNM_OnClientConnectionNotification(ulong arg1, ConnectionNotificationManager.ConnectionStatus arg2)
    {
        if (arg2 == ConnectionNotificationManager.ConnectionStatus.Disconnected)
            return;

        if (arg1 == 0)
        {
            Player1Area.GetComponent<NetworkObject>().ChangeOwnership(arg1);
        }
        else
        {
            Player2Area.GetComponent<NetworkObject>().ChangeOwnership(arg1);
        }

        Debug.Log("Player 1 Area Owner ID" +Player1Area.GetComponent<NetworkObject>().OwnerClientId);
        Debug.Log("Player 2 Area Owner ID" + Player2Area.GetComponent<NetworkObject>().OwnerClientId);
    }
}
