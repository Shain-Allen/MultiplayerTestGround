using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayAreaManager : NetworkBehaviour
{
    [SerializeField] RectTransform myArea;
    [SerializeField] RectTransform opponentArea;
    [SerializeField] RectTransform NewCardHolder;

    [SerializeField] Transform cardPrefab;

    [SerializeField] Sprite[] cardImgs;

    private void Update()
    {
        if (!IsClient) return;

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

        tempcard.SetParent(NewCardHolder, false);

        tempcard.GetComponent<NetworkObject>().ChangeOwnership(serverRpcParams.Receive.SenderClientId);

        setCardClientRPC(cardimgIndex);
    }

    [ClientRpc]
    void setCardClientRPC(int cardimgIndex)
    {
        Debug.Log("I am " + OwnerClientId);

        for (int i = 0; i < NewCardHolder.childCount; i++)
        {
            Transform tempcard = NewCardHolder.GetChild(i).gameObject.GetComponent<RectTransform>();
            Image tempcardImg = tempcard.GetChild(0).GetComponent<Image>();

            Debug.Log("Object Belongs to " + tempcard.GetComponent<NetworkObject>().OwnerClientId);

            if (tempcard.gameObject.GetComponent<NetworkObject>().OwnerClientId == OwnerClientId)
            {
                tempcardImg.sprite = cardImgs[cardimgIndex];
                tempcard.GetComponent<NetworkObject>().TrySetParent(myArea, false);
            }
            else
            {
                tempcardImg.sprite = cardImgs[0];
                tempcard.GetComponent<NetworkObject>().TrySetParent(opponentArea, false);
            }
        }
    }
}
