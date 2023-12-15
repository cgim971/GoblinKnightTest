using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{
    public Button hostBtn;
    public Button clientBtn;


    private void Start()
    {
        hostBtn.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        clientBtn.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
    }
}
