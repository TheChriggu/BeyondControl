using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;

public class Login : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";
    public Text userName;
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnLogin()
    {
        string name = userName.text;

        if (name != "")
        {
            PhotonNetwork.NickName = name;
            Connect();
        }
    }

    void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        TryCreateNewRoom();
    }

    void TryCreateNewRoom()
    {
        RoomOptions options = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(null, options, null);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        TryCreateNewRoom();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SceneManager.LoadScene("WaitingRoom");
        }
    }
}
