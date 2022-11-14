using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAreaManager : NetworkBehaviour
{
    [SerializeField] RectTransform Player1Area;
    [SerializeField] RectTransform Player2Area;

    [SerializeField] Transform cardPrefab;

    [SerializeField] Sprite[] cardImgs;

    private void Update()
    {
        if (!IsClient || GetComponent<NetworkObject>().OwnerClientId != NetworkManager.LocalClientId) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnCardServerRpc(1, new ServerRpcParams());
        }
    }

    [ServerRpc]
    void SpawnCardServerRpc(int cardimgIndex, ServerRpcParams serverRpcParams)
    {
        Transform tempcard = Instantiate(cardPrefab);
        
        tempcard.GetComponent<NetworkObject>().Spawn(true);

        if(serverRpcParams.Receive.SenderClientId == 0)
        {
            tempcard.SetParent(Player1Area, false);
        }
        else
        {
            tempcard.SetParent(Player2Area, false);
        }

        tempcard.GetComponent<NetworkObject>().ChangeOwnership(serverRpcParams.Receive.SenderClientId);

        setCardClientRPC(cardimgIndex);
    }

    [ClientRpc]
    void setCardClientRPC(int cardimgIndex)
    {
        //Debug.Log("I am " + OwnerClientId);

        //Debug.Log("Player Area 1 card count: " + Player1Area.childCount);
        //Debug.Log("Player Area 2 card count: " + Player2Area.childCount);
        if (Player1Area.childCount != 0)
        {
            for (int i = 0; i < Player1Area.childCount; i++)
            {
                //Debug.Log("Player 1 area card index: " + i);

                Transform tempcard = Player1Area.GetChild(i).gameObject.GetComponent<RectTransform>();
                Image tempcardImg = tempcard.GetChild(0).GetComponent<Image>();

                //Debug.Log("Object Belongs to " + tempcard.GetComponent<NetworkObject>().OwnerClientId);

                if (tempcard.gameObject.GetComponent<NetworkObject>().OwnerClientId == NetworkManager.Singleton.LocalClientId)
                {
                    tempcardImg.sprite = cardImgs[cardimgIndex];
                }
                else
                {
                    tempcardImg.sprite = cardImgs[0];
                }
            }
        }

        if (Player2Area.childCount != 0)
        {
            for (int i = 0; i < Player2Area.childCount; i++)
            {
                //Debug.Log("Player 2 area card index: " + i);

                Transform tempcard = Player2Area.GetChild(i).gameObject.GetComponent<RectTransform>();
                Image tempcardImg = tempcard.GetChild(0).GetComponent<Image>();

                //Debug.Log("Object Belongs to " + tempcard.GetComponent<NetworkObject>().OwnerClientId);

                if (tempcard.gameObject.GetComponent<NetworkObject>().OwnerClientId == NetworkManager.Singleton.LocalClientId)
                {
                    tempcardImg.sprite = cardImgs[cardimgIndex];
                }
                else
                {
                    tempcardImg.sprite = cardImgs[0];
                }
            }
        }
    }
}
