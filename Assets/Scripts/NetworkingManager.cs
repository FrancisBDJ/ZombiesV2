using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
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
    [SerializeField] private TMP_InputField _playerNameInput;
    public Button createPlayerButton;
    public TMP_Text playerCurrentAlias;
    public GameObject playerInputPanel;
    [SerializeField] private TMP_InputField _roomInput;
    public GameObject roomPanel;
    public TMP_Text roomPanelRoomName;
    public GameObject roomListPanel;
    private List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;
    public RoomItem RoomItemPrefab;
    private List<RoomItem> playerItemsList = new List<RoomItem>();
    public Transform PlayerContentObject;
    public RoomItem PlayerItemPrefab;
    public Button leaveRoomButton;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        if (PhotonNetwork.IsConnected)
        {
            StartCoroutine(DisconnectPlayer());
        }
        
       
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
        Debug.Log("Connecting to Server");
        playerInputPanel.SetActive(false);
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
        Debug.Log("Connected to  Master Server");
        Debug.Log("Joinning a Lobby");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
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
    }*/

    public void MakeRoom()
    {
        string roomName = _roomInput.text;
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 5,
            PublishUserId = true
        };

        PhotonNetwork.CreateRoom($"{roomName}", roomOptions);
        Debug.Log($"Created Room {roomName}");
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
        
    }

    private void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(RoomItemPrefab, contentObject);
            newRoom.NetworkParent = this;
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }

    private void UpdatePlayerList()
    {
        foreach (RoomItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            RoomItem newPlayerItem = Instantiate(PlayerItemPrefab, PlayerContentObject);
            
            playerItemsList.Add((newPlayerItem));
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
        roomListPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomPanelRoomName.text = $"Room: {PhotonNetwork.CurrentRoom.Name}";
        UpdatePlayerList();
        //Debug.Log($"Loading Multiplayer Scene");
        //PhotonNetwork.LoadLevel("GameOnline");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
        roomPanel.SetActive(false);
        roomListPanel.SetActive(true);
        UpdatePlayerList();
    }

    

    public void LeaveRoomButton()
    {
        Debug.Log($"Leaving Room: {PhotonNetwork.CurrentRoom.Name}");
        
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        
    }
    
}
