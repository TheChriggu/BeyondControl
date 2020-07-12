using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public enum EventCodes
{
    None = 0,
    Order,
    RequestReset,
    ConfirmReset,
    DenyReset
}

public class NetworkComponent : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private List<Order> ownOrders;
    private Dictionary<int, Order> otherOrdersUnsorted = new Dictionary<int, Order>();

    public Robot robot;
    public GameObject playerDisconnectedPanel;

    public bool singlePlayer = true;

    public ListOfOrdersVisuals visuals;

    // Start is called before the first frame update
    void Start()
    {
        ownOrders = new List<Order>();
        otherOrdersUnsorted = new Dictionary<int, Order>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ownOrders.Count > 0 && ownOrders.Count == otherOrdersUnsorted.Count)
        {
            CombineOrderListsAndSendToRobot();
        }
    }

    public void HandleOrders(List<Order> orders)
    {
        if(singlePlayer)
        {
            robot.addNewList(orders);
        }
        /*else if (PhotonNetwork.IsMasterClient)
        {
            ownOrders = orders;
        }*/
        else
        {
            ownOrders = orders;

            for (int i = 0; i < orders.Count; i++)
            {
                Vector3 contentData = new Vector3(i, (int)orders[i].type, orders[i].value);
                object content = (object)contentData;
                RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others};
                PhotonNetwork.RaiseEvent((byte)EventCodes.Order, content, options, SendOptions.SendReliable);
            }
        }
    }

    private void CombineOrderListsAndSendToRobot()
    {
        var clientOrders = new List<Order>();
        for (int i = 0; i < otherOrdersUnsorted.Count; i++)
        {
            clientOrders.Add(otherOrdersUnsorted[i]);
        }

        var allOrders = new List<Order>();

        for(int i = 0; i < clientOrders.Count; i++)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                allOrders.Add(ownOrders[i]);
                allOrders.Add(clientOrders[i]);
            }
            else
            {
                allOrders.Add(clientOrders[i]);
                allOrders.Add(ownOrders[i]);
            }

        }

        robot.addNewList(allOrders);

        //Debugging
        visuals.AddOrders(allOrders);

        ownOrders = new List<Order>();
        otherOrdersUnsorted = new Dictionary<int, Order>();
    }

    public void OnEvent(EventData photonEvent)
    {
        EventCodes code = (EventCodes)photonEvent.Code;

        switch (code)
        {
            case EventCodes.Order:
                Vector3 contentData = (Vector3)photonEvent.CustomData;
                var order = new Order((Order.Type)contentData.y, contentData.z);

                if(!otherOrdersUnsorted.ContainsKey((int)contentData.x))
                {
                    otherOrdersUnsorted.Add((int)contentData.x, order);
                }
                break;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerDisconnectedPanel.SetActive(true);
    }

    public bool IsSingleplayer()
    {
        return singlePlayer;
    }
}
