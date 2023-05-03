using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class NetworkingManager : MonoBehaviourPunCallbacks
{
    public Button playMpButton;
    public Button backButton;
    [SerializeField] private InputField _playerNameInput;
    public Button createPlayer;
    public TextMeshPro playerCurrentAlias;
    public GameObject playerPanel;
    [SerializeField] private InputField _roomInput;
    public GameObject roomPanel;
    public GameObject roomListPanel;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        if (PhotonNetwork.IsConnected)
        {
            StartCoroutine(DisconnectPlayer());
        }
        Debug.Log("Connecting to Server");
       
    }

    public void NewAlias()
    {
        if (_playerNameInput.text != null)
        {
            PhotonNetwork.NickName = "_playerNameInput.text";
            playerCurrentAlias.text = $"Player: {_playerNameInput.text}";
            Connect();
        }
    }

    private void Connect()
    {
        playerPanel.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    IEnumerator DisconnectPlayer()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Join a Lobby");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        roomListPanel.SetActive(true);
        //Debug.Log("Ready to Play Multiplayer");
        //playMpButton.interactable = true;
    }

    /*public void FindMatch()
    {
        Debug.Log("Searching Room...");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        MakeRoom();
    }

    private void MakeRoom()
    {
        int randomRoomName = Random.Range(0, 5000);

        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 6,
            PublishUserId = true
        };

        PhotonNetwork.CreateRoom($"RoomName_{randomRoomName}", roomOptions);
        Debug.Log($"Created Room {randomRoomName}");
    }*/

    public override void OnJoinedRoom()
    {
        Debug.Log($"Loading Multiplayer Scene");
        PhotonNetwork.LoadLevel("GameOnline");
        
    }
    
}
