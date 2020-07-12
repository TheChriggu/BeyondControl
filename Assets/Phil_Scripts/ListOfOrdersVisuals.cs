using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListOfOrdersVisuals : MonoBehaviour
{
    bool firstOrder = true; //first order doesn't get deleted instantly

    public Robot robot;
    //public GameObject ListOfOrders; simply using the object holding this script now

    private void Update()
    {
        if(robot.IsExecutingOrders())
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void AddOrders(List<Order> orderList)
    {
        //Debug.Log("Showing List of size: " + orderList.Count);
        clearList();

        foreach (Order o in orderList)
        {
            //Gameobject added To List Visuals
            GameObject TextObject = new GameObject();
            TextObject.transform.parent = transform;
            RectTransform rectTransform = TextObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(160, 30);
            Text textOrder = TextObject.AddComponent<Text>();
            //Font of text
            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            textOrder.font = ArialFont;
            textOrder.material = ArialFont.material;
            textOrder.color = Color.black;
            //textOrder.fontSize = 20;
            //Content of Text
            if (o.value != 0) textOrder.text = "Type: " + o.type.ToString() + " , " + o.value;
            else textOrder.text = "Type: " + o.type.ToString();
        }
    }

    void clearList()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void removeLatestOrder()
    {
        if (firstOrder)
        {
            firstOrder = false;
            return;
        }
        Destroy(transform.GetChild(0).gameObject);
    }
}
