using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class WaitingRoom : MonoBehaviourPunCallbacks
{
    public Text operationMessage;
    public Text playerMessage;

    string otherPlayerName = "";

    float timeToStart = 5;

    // Update is called once per frame
    void Update()
    {
        var players = PhotonNetwork.PlayerList;



        /*if (otherPlayerName == "" && !PhotonNetwork.IsMasterClient)
        {
            otherPlayerName = PhotonNetwork.MasterClient.NickName;
        }*/

        if (PhotonNetwork.PlayerListOthers.Length > 0)
        {
            if (PhotonNetwork.IsMasterClient && otherPlayerName == "")
            {
                otherPlayerName = players[1].NickName;
            }
            else if(otherPlayerName == "")
            {
                otherPlayerName = players[0].NickName;
            }

            PhotonNetwork.CurrentRoom.IsOpen = false;

            playerMessage.text = "You are playing with " + otherPlayerName + ".";

            operationMessage.gameObject.SetActive(true);

            operationMessage.text = "Starting in " + Mathf.CeilToInt(timeToStart).ToString();

            if(timeToStart >= 0)
            {
                timeToStart -= Time.deltaTime;
            }

            if (PhotonNetwork.IsMasterClient && timeToStart <= 0)
            {
                SceneManager.LoadScene("Level1");
            }
        }
        else
        {
            playerMessage.text = "Waiting for other Player.";
            operationMessage.gameObject.SetActive(false);
            timeToStart = 5;
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }

    /*public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            otherPlayerName = PhotonNetwork.PlayerListOthers[0].NickName;
        }
    }*/
}
