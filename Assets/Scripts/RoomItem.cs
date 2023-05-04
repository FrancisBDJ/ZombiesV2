using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text roomName;
    public NetworkingManager NetworkParent;
    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }


    public void OnRoomPressed()
    {
        NetworkParent.JoinRoom(roomName.text);
    }



}
