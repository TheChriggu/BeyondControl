using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Robot : MonoBehaviourPunCallbacks
{
    public float timeBetweenOrders = 2;
    //public float moveSpeed = 5;
    public float acceleration = 10;
    public float rotationSpeed = 10;
    [SerializeField]
    float floorModifier = 1;
    [Tooltip("How much slower the Player moves on Grass: -0.3 = 30% slower")]
    public float grassmodifier = -0.3f;
    [Tooltip("How much faster the Player moves on Streets: 0.5 = 50% faster")]
    public float streetmodifier = 0.5f;
    bool onStreet = false;
    bool onGrass = false;

    float currentVelocity = 0;
    float targetVelocity;

    //float currentRotation = 0;
    Rigidbody2D body;
    Vector3 direction;
    //public GameObject debugging;
    float rotateAmount;
    bool countdownActive;
    float countdownEnd;
    bool lockRotation = false;

    List<Order> listOfOrders = new List<Order>();
    List<Order> listOfOrdersPast = new List<Order>();

    public NetworkComponent networkComponent;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
    #region movement
    void Update()
    {
        //Debugging Controlls
        #region Speed
        setSpeed();
        body.AddForce(transform.up * currentVelocity * floorModifier);
        #endregion

        #region rotation
        if (setRotation()) body.AddTorque(-rotateAmount * rotationSpeed);
        if (countdownActive) Debug.Log("Coundown: " + (countdownEnd - Time.time));
        if (countdownActive && countdownEnd < Time.time) OnCountDownEnd();
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
        else startCountdown(timeBetweenOrders);

        return false;
    }
    void startCountdown(float t)
    {
        if (countdownActive) return;
        countdownActive = true;
        countdownEnd = Time.time + t;
    }

    void OnCountDownEnd()
    {
        countdownActive = false;
        lockRotation = true;
    }


    void getBoosted(Vector3 dir, Vector3 from, float force, bool spinning)
    {
        if(spinning) body.AddForceAtPosition(dir * force, from, ForceMode2D.Impulse);
        else body.AddForce(dir * force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered Trigger");

        if (collision.gameObject.tag == "Booster")
        {
            Booster booster = collision.GetComponent<Booster>();
            getBoosted(collision.transform.up, collision.transform.position, booster.boostingPower, booster.playerStartsSpinning);
        }
        else if (collision.gameObject.tag == "Street")
        {
            enterStreet();
        }
        else if (collision.gameObject.tag == "Grass")
        {
            enterGrass();
        }
        else if (collision.gameObject.tag == "ExpensiveStuff")
        {
            ExpensiveStuff stuff = collision.gameObject.GetComponent<ExpensiveStuff>();
            DamageCounter.instance.AddPropertyDamage(stuff.monetaryValue, transform.position);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exited Trigger");
        if (collision.gameObject.tag == "Street")
        {
            exitStreet();
        }
        if (collision.gameObject.tag == "Grass")
        {
            exitGrass();
        }
    }


    #endregion

    #region floorModifiers
    void enterStreet()
    {
        //Debug.Log("Entered Street");
        onStreet = true;
        setModifier();
    }

    void enterGrass()
    {
        //Debug.Log("Entered Grass");
        onGrass = true;
        setModifier();
        DamageCounter.instance.AddPropertyDamage(50, transform.position);
    }  
    void exitStreet()
    {
        //Debug.Log("Exited Street");
        onStreet = false;
        setModifier();
    }

    void exitGrass()
    {
        //Debug.Log("Exited Grass");
        onGrass = false;
        setModifier();
    }

    void setModifier()
    {
        floorModifier = 1;
        if (onGrass) floorModifier += grassmodifier;
        if (onStreet) floorModifier += streetmodifier;
    }
    #endregion

    #region Ticks
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
        //Starts the regular Ticks
        InvokeRepeating("onTick", 0, timeBetweenOrders);

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
            targetVelocity = 0;
            return;
        }

        Order nextOrder = listOfOrders[0];
        //Removing Order from list
        listOfOrders.RemoveAt(0);

        if(PhotonNetwork.IsMasterClient || networkComponent.IsSingleplayer())
        {
            //Executing Order only on master client
            executeOrder(nextOrder);
        }

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

    public bool IsExecutingOrders()
    {
        return listOfOrders.Count > 0;
    }

    public List<Order> GetListOfOrders()
    {
        return listOfOrders;
    }
}
