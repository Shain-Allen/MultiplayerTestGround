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
        if (Player1Area.childCount != 0)
        {
            for (int i = 0; i < Player1Area.childCount; i++)
            {
                Transform tempcard = Player1Area.GetChild(i).gameObject.GetComponent<RectTransform>();
                Image tempcardImg = tempcard.GetChild(0).GetComponent<Image>();

                if (tempcard.gameObject.GetComponent<NetworkObject>().OwnerClientId == NetworkManager.Singleton.LocalClientId)
                {
                    tempcardImg.sprite = cardImgs[cardimgIndex];
                }
                else
                {
                    tempcardImg.sprite = cardImgs[0];
                    tempcardImg.transform.localScale = new Vector3(-1, -1, 1);
                }
            }
        }

        if (Player2Area.childCount != 0)
        {
            for (int i = 0; i < Player2Area.childCount; i++)
            {
                Transform tempcard = Player2Area.GetChild(i).gameObject.GetComponent<RectTransform>();
                Image tempcardImg = tempcard.GetChild(0).GetComponent<Image>();

                if (tempcard.gameObject.GetComponent<NetworkObject>().OwnerClientId == NetworkManager.Singleton.LocalClientId)
                {
                    tempcardImg.sprite = cardImgs[cardimgIndex];
                    tempcardImg.transform.localScale = new Vector3(-1, -1, 1);
                }
                else
                {
                    tempcardImg.sprite = cardImgs[0];
                }
            }
        }
    }
}
