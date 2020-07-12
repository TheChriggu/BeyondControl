using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewControlPanel : MonoBehaviour
{
    //public Robot robot;
    public NetworkComponent networkComponent;
    List<Order> MyOrders = new List<Order>();
    public UI_Button[] UIElements;

    private void Start()
    {
        UIElements = new UI_Button[transform.childCount];
        int n = 0;
        foreach (Transform child in transform)
        {
            UI_Button b = child.GetComponent<UI_Button>();
            UIElements[n] = b;
            n++;
        }
    }

    void clearList()
    {
        MyOrders = new List<Order>();
    }

    public void giveListToRobot()
    {
        collectOrders();

        networkComponent.HandleOrders(MyOrders);

        clearList();
    }

    void collectOrders()
    {
        foreach (var element in UIElements)
        {
            MyOrders.Add(element.returnOrder());
        }
    }
}
