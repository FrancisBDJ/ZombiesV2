using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public TMP_Text roomName;
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
