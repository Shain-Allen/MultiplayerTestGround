using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Canvas GameBoard;
    [SerializeField] private GameObject networkManageUI;
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;

    [SerializeField] private GameObject player1Cam;
    [SerializeField] private GameObject player2Cam;

    [SerializeField] private GameAreaManager GMA;


    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            GMA.OnServerStart();
        });
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            GMA.OnServerStart();
            //player2Cam.SetActive(false);
            //player1Cam.SetActive(true);
            //GameBoard.worldCamera = player1Cam.GetComponent<Camera>();
            networkManageUI.SetActive(false);
        });
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            //player1Cam.SetActive(false);
            //player2Cam.SetActive(true);
            //GameBoard.worldCamera = player2Cam.GetComponent<Camera>();
            networkManageUI.SetActive(false);
        });
    }
}
