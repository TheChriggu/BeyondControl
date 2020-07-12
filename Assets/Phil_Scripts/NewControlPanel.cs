using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewControlPanel : MonoBehaviour
{
    public Robot robot;
    public NetworkComponent networkComponent;
    List<Order> MyOrders = new List<Order>();
    int numOfOrdersRequired = 5;

    void clearList()
    {

        //MyOrders = new List<Order>();
        //foreach (Transform child in ListOfOrdersVisuals.transform)
        //{
        //    Destroy(child.gameObject);
        //}
    }
}
