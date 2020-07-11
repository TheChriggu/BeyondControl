using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class WaitingRoomTest : MonoBehaviour
{
    public Text masterName;
    public Text clientName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        masterName.text = PhotonNetwork.MasterClient.NickName;

        var playerlist = PhotonNetwork.PlayerList;

        foreach (var player in playerlist)
        {
            if (player.NickName != masterName.text)
            {
                clientName.text = player.NickName;
            }
        }
    }
}
