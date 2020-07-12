using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdControlPanel : MonoBehaviour
{
    public GameObject[] Orders;
    public GameObject[] Visuals;
    int currentOrder = 0;

    private void Start()
    {
        showOrder(currentOrder);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) nextOrder();
        if (Input.GetKeyDown(KeyCode.A)) prevOrder();
        //quickly increases value
        if (Orders[currentOrder].GetComponent<UI_Button>().returnOrder().type == Order.Type.Rotation)
        {
            if (Input.GetKey(KeyCode.W)) increaseValue(1);
            if (Input.GetKey(KeyCode.S)) increaseValue(-1);
        }
        //slowly increases value
        else
        {
            if (Input.GetKeyDown(KeyCode.W)) increaseValue(1);
            if (Input.GetKeyDown(KeyCode.S)) increaseValue(-1);
        }
    }

    public void showOrder(int i)
    {
        currentOrder = i;
        foreach (var order in Orders)
        {
            order.SetActive(false);
        }
        Orders[i].SetActive(true);
    }

    void nextOrder()
    {
        currentOrder = (currentOrder + 1) % Orders.Length;
        showOrder(currentOrder);
    }
    void prevOrder()
    {
        currentOrder -= 1;
        if (currentOrder < 0) currentOrder = Orders.Length;
        showOrder(currentOrder);
    }

    void increaseValue(int i = 1)
    {
        Orders[currentOrder].GetComponent<UI_Button>().increaseValue(i);
    }
}
