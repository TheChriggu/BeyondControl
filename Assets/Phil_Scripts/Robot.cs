﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public float timeBetweenOrders = 2;
    //public float moveSpeed = 5;
    public float acceleration = 10;
    public float rotationSpeed = 10;

    float currentVelocity = 0;
    float targetVelocity;

    //float currentRotation = 0;
    Rigidbody2D rigidbody;
    Vector3 direction;
    //public GameObject debugging;
    float rotateAmount;
    bool countdownActive;
    float countdownEnd;
    bool lockRotation = false;

    List<Order> listOfOrders = new List<Order>();
    List<Order> listOfOrdersPast = new List<Order>();

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        InvokeRepeating("onTick", 0, timeBetweenOrders);
    }
    #region movement
    void Update()
    {
        //Debugging Controlls
        #region Speed
        setSpeed();
        rigidbody.AddForce(transform.up * currentVelocity);
        #endregion

        #region rotation
        if (setRotation()) rigidbody.AddTorque(-rotateAmount * rotationSpeed);
        #endregion
    }

    // slowly a-/deccelerating to the targeted Speed
    void setSpeed()
    {
        //Debug.Log("Speed difference: " + (targetVelocity - currentVelocity));

        //accelerating
        if (currentVelocity < targetVelocity) currentVelocity += Time.deltaTime * acceleration; //= Mathf.Clamp(currentVelocity + Time.deltaTime * acceleration, oldVelocity, targetVelocity);
        //deccelerating
        if (currentVelocity > targetVelocity) currentVelocity -= Time.deltaTime * acceleration; //= Mathf.Clamp(currentVelocity - Time.deltaTime * acceleration, oldVelocity, targetVelocity);
    }

    bool setRotation()
    {
        if (lockRotation) return false;

        rotateAmount = Vector3.Cross(direction, transform.up).z;

        if (Mathf.Abs(rotateAmount) > 0.01) return true;
        return false;
    }
    #endregion

    #region Ticks
    //void startCountdown(float t)
    //{
    //    //if (countdownEnd > Time.time) return;
    //    countdownActive = true;
    //    countdownEnd = Time.time + t;
    //}

    void onTick()
    {
        Debug.Log("TICK!");
        executeNextOrder();
    }
    #endregion
    //void DebugControlls()
    //{
    //    //float Velocity = 0;

    //    if (Input.GetKey(KeyCode.W)) currentVelocity += 1;
    //    if (Input.GetKey(KeyCode.S)) currentVelocity -= 1;
    //}

    //float DebugRotation()
    //{
    //    float Rotation = 0;

    //    if (Input.GetKey(KeyCode.A)) Rotation -= 1 * Time.deltaTime;
    //    if (Input.GetKey(KeyCode.D)) Rotation += 1 * Time.deltaTime;

    //    Rotation *= rotationSpeed;

    //    return Rotation;
    //}

    #region execute Orders
    public void addNewList(List<Order> newOrders)
    {
        foreach (var order in newOrders)
        {
            listOfOrders.Add(order);
        }
    }
    void executeNextOrder()
    {
        if (listOfOrders.Count < 1)
        {
            Debug.LogWarning("No more Orders");
            return;
        }

        Order nextOrder = listOfOrders[0];
        //Removing Order from list
        listOfOrders.RemoveAt(0);

        //Executing Order
        executeOrder(nextOrder);

        //adding past List it to Memory
        listOfOrdersPast.Add(nextOrder);
    }

    void executeOrder(Order order)
    {
        switch (order.type)
        {
            case Order.Type.Speed:
                giveSpeedOrder(order.value);
                break;
            case Order.Type.Rotation:
                giveRotationOrder(order.value);
                break;
            case Order.Type.Repeat:
                for (int i = 0; i < listOfOrdersPast.Count; i++)
                {
                    int n = listOfOrdersPast.Count - 1 - i;
                    if (listOfOrdersPast[n].type != Order.Type.Repeat)
                    {
                        executeOrder(listOfOrdersPast[n]);
                        return;
                    }
                }
                break;
            case Order.Type.Pass:
                break;
            default:
                break;
        }
    }

    void giveSpeedOrder(float speed)
    {
        targetVelocity = speed;
    }

    void giveRotationOrder(float rotation)
    {
        lockRotation = false;
        direction = Quaternion.AngleAxis(-rotation, Vector3.forward) * transform.up;
        //debugging.transform.position = transform.position + direction;
    }
    #endregion
}