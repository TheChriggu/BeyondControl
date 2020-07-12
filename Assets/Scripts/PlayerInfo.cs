using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerInfo : MonoBehaviour
{
    public Text otherPlayerName;
    public Text goFirstMessage;
    // Start is called before the first frame update
    void Start()
    {
        var players = PhotonNetwork.PlayerList;

        if (PhotonNetwork.IsMasterClient)
        {
            otherPlayerName.text = players[1].NickName;
            goFirstMessage.text = "You go first.";
        }
        else
        {
            otherPlayerName.text = players[0].NickName;
            goFirstMessage.text = "Your partner goes first.";
        }        
    }
}
