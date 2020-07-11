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
    Order
}

public class NetworkComponent : MonoBehaviourPunCallbacks//, IOnEventCallback
{
    private List<Order> masterOrders;
    private Dictionary<int, Order> clientOrdersUnsorted = new Dictionary<int, Order>();

    public Robot robot;

    private void OnLevelWasLoaded()
    {
        //Instantiate the robot from prefab in resources folder over the network to ensure proper positioning on both screens
        //PhotonNetwork.Instantiate(Prefabname, position, rotation);
    }

    // Start is called before the first frame update
    void Start()
    {
        masterOrders = new List<Order>();
        clientOrdersUnsorted = new Dictionary<int, Order>();
    }

    // Update is called once per frame
    void Update()
    {
        if(masterOrders.Count > 0 && masterOrders.Count == clientOrdersUnsorted.Count)
        {
            CombineOrderListsAndSendToRobot();
        }
    }

    public void HandleOrders(List<Order> orders)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            masterOrders = orders;
        }

        else
        {
            for (int i = 0; i < orders.Count; i++)
            {
                Vector3 contentData = new Vector3(i, (int)orders[i].type, orders[i].value);
                object content = (object)contentData;
                RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
                PhotonNetwork.RaiseEvent((byte)EventCodes.Order, content, options, SendOptions.SendReliable);
            }
        }
    }

    private void CombineOrderListsAndSendToRobot()
    {
        var clientOrders = new List<Order>();
        for (int i = 0; i < clientOrdersUnsorted.Count; i++)
        {
            clientOrders.Add(clientOrdersUnsorted[i]);
        }

        var allOrders = new List<Order>();

        for(int i = 0; i < clientOrders.Count; i++)
        {
            allOrders.Add(masterOrders[i]);
            allOrders.Add(clientOrders[i]);
        }

        robot.addNewList(allOrders);

        masterOrders = new List<Order>();
        clientOrdersUnsorted = new Dictionary<int, Order>();
    }

    public void OnEvent(EventData photonEvent)
    {
        EventCodes code = (EventCodes)photonEvent.Code;

        switch (code)
        {
            case EventCodes.Order:
                Vector3 contentData = (Vector3)photonEvent.CustomData;
                var order = new Order((Order.Type)contentData.y, contentData.z);
                clientOrdersUnsorted.Add((int)contentData.x, order);
                break;
        }
    }
}
