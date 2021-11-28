using UnityEngine;
using TMPro;
using System.Text;
using System;
using Unity.Netcode;

public class PasswordNetworkManager : MonoBehaviour
{
    [SerializeField] TMP_InputField passwordInputField;
    [SerializeField] GameObject passwordNetworkManager;
    [SerializeField] GameObject passwordCanvas;
    [SerializeField] GameObject leaveButton;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) { return; }

        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
    }

    public void Host()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }

    public void Client()
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(passwordInputField.text);
        NetworkManager.Singleton.StartClient();
    }

    public void Leave()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
        }

        passwordCanvas.SetActive(true);
        leaveButton.SetActive(false);
    }

    private void HandleServerStarted()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            HandleClientConnected(NetworkManager.Singleton.LocalClientId);
        }
    }

    private void HandleClientConnected(ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            passwordCanvas.SetActive(false);
            leaveButton.SetActive(true);
        }
    }

    private void HandleClientDisconnect(ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            passwordCanvas.SetActive(true);
            leaveButton.SetActive(false);
        }
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback)
    {
        string password = Encoding.ASCII.GetString(connectionData);
        bool approveConnection = password == passwordInputField.text;

        print(NetworkManager.Singleton.ServerClientId);
        print(NetworkManager.Singleton.LocalClientId);

        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;

        switch (NetworkManager.Singleton.ConnectedClients.Count)
        {

            case 1:
                spawnPos = new Vector3(2f, 0f, 0f);
                spawnRot = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 2:
                spawnPos = new Vector3(4f, 0f, 0f);
                spawnRot = Quaternion.Euler(0f, 225f, 0f);
                break;
        }

        callback(true, null, approveConnection, spawnPos, spawnRot);
    }
}

