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

        if (otherPlayerName == "" && !PhotonNetwork.IsMasterClient)
        {
            otherPlayerName = PhotonNetwork.MasterClient.NickName;
        }

        if (PhotonNetwork.PlayerListOthers.Length > 0)
        {
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
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            otherPlayerName = PhotonNetwork.PlayerListOthers[0].NickName;
        }
    }
}
