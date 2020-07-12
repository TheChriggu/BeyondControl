using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LevelGoal : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public string nextSceneName = "Level1";

    public GameObject robot;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Goal Trigger entered");
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("entered by player");
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Raising event");
                RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent((byte)EventCodes.LevelCompleted, null, options, SendOptions.SendReliable);
            }
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        Debug.Log("Photon Event received");
        EventCodes code = (EventCodes)photonEvent.Code;

        switch (code)
        {
            case EventCodes.LevelCompleted:
                Debug.Log("Level completed");
                StartCoroutine(OnLevelCompleted());
                break;
        }
    }

    IEnumerator OnLevelCompleted()
    {
        robot.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(7);

        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);

        /*if(PhotonNetwork.IsMasterClient)
        {
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
            PhotonNetwork.LoadLevel(nextSceneName);
        }*/


    }


}
