using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    public InputField inputField;
    public Dropdown dropdown;
    List<String> availableOrders = new List<string>();
    float currentValue = 0;
    float maxSpeed = 20;
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
            inputField.gameObject.SetActive(true);
        }
        else
        {
            //hiding the input field
            dropdown.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 30);
            inputField.gameObject.SetActive(false);
        }

        checkValues();
    }

    public void onInputValueChanged(string value)
    {
        currentValue = float.Parse(value);

        //restricts possible values
        checkValues();

        inputField.SetTextWithoutNotify(currentValue.ToString());
    }

    void checkValues()
    {
        if (type == Order.Type.Speed)
        {
            if (currentValue < -maxSpeed) currentValue = -maxSpeed;
            if (currentValue > maxSpeed) currentValue = maxSpeed;
        }
        if (type == Order.Type.Rotation)
        {
            if (currentValue < -179) currentValue = -179;
            if (currentValue > 179) currentValue = 179;
        }
    }

    public Order returnOrder()
    {
        Order order = new Order(type, currentValue);
        return order;
    }
}
