﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    //public Robot robot;
    public NetworkComponent networkComponent;

    public GameObject ListOfOrdersVisuals;
    [SerializeField]
    List<Order> MyOrders = new List<Order>();

    public Button SpeedButton;
    public Button RotateButton;

    public Slider SpeedSlider;
    public Slider RotateSlider;

    float SpeedValue = 0;
    float RotateValue = 0;

    public Text SpeedNumber;
    public Text RotateNumber;

    #region sliders
    public void setSpeedNumber(float value)
    {
        SpeedNumber.text = value.ToString();
        SpeedValue = value;
    }
    public void setRotateNumber(float value)
    {
        RotateNumber.text = value.ToString();
        RotateValue = value;
    }
    #endregion

    #region Buttons
    public void GiveSpeed()
    {
        //robot.giveSpeedOrder(SpeedValue);
        Order speedOrder = new Order(Order.Type.Speed, SpeedValue);
        AddOrder(speedOrder);
    }
    public void GiveRotation()
    {
        //robot.giveRotationOrder(RotateValue);
        Order rotateOrder = new Order(Order.Type.Rotation, RotateValue);
        AddOrder(rotateOrder);
    }
    public void repeatLast()
    {
        Order repeatOrder = new Order(Order.Type.Repeat);
        AddOrder(repeatOrder);
    }
    public void passOrder()
    {
        Order passOrder = new Order(Order.Type.Pass);
        AddOrder(passOrder);
    }
    public void giveListToRobot()
    {
        //robot.addNewList(MyOrders);
        networkComponent.HandleOrders(MyOrders);
        clearList();
    }
    #endregion

    void clearList()
    {
        MyOrders = new List<Order>();
        foreach (Transform child in ListOfOrdersVisuals.transform)
        {
            Destroy(child.gameObject);
        }
    }
    void AddOrder(Order o)
    {
        MyOrders.Add(o);
        //Gameobject added To List Visuals
        GameObject TextObject = new GameObject();
        TextObject.transform.parent = ListOfOrdersVisuals.transform;
        RectTransform rectTransform = TextObject.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(160, 30);
        Text textOrder = TextObject.AddComponent<Text>();
        //Font of text
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        textOrder.font = ArialFont;
        textOrder.material = ArialFont.material;
        textOrder.color = Color.black;
        //Content of Text
        if (o.value != 0) textOrder.text = "Type: " + o.type.ToString() + " , " + o.value;
        else textOrder.text = "Type: " + o.type.ToString();
    }
}

public class Order
{
    public enum Type
    {
        Speed
       ,Rotation
       ,Repeat
       ,Pass
    }

    public Type type;
    public float value;

    public Order(Type _type, float _value = 0)
    {
        type = _type;
        value = _value;
    }
}