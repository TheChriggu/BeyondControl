using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    public GameObject inputField;
    public Dropdown dropdown;
    List<String> availableOrders = new List<string>();
    float currentValue = 0;
    Order.Type type = Order.Type.Pass;

    private void Start()
    {
        foreach (Order.Type pieceType in Enum.GetValues(typeof(Order.Type)))
        {
            //Debug.Log(pieceType);
            availableOrders.Add(pieceType.ToString());
        }
        dropdown.AddOptions(availableOrders);
        dropdown.SetValueWithoutNotify(3);
    }

    public void DropdownChanged(int value)
    {
        type = (Order.Type)value;

        //Updating visuals
        float width = transform.parent.GetComponent<RectTransform>().sizeDelta.x;
        if (value < 2) //the first two orders need a value Assigned
        {
            //showing the input field
            dropdown.GetComponent<RectTransform>().sizeDelta = new Vector2(width/2, 30);
            inputField.SetActive(true);
        }
        else
        {
            //hiding the input field
            dropdown.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 30);
            inputField.SetActive(false);
        }
    }

    public void onInputValueChanged(string value)
    {
        currentValue = float.Parse(value);
    }

    public Order returnOrder()
    {
        Order order = new Order(type, currentValue);
        return order;
    }
}
