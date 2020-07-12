using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class ResetMenu : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameObject resetMenu;
    public GameObject WaitingForAnswerMenu;
    public GameObject resetMenuDeniedMessage;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent((byte)EventCodes.RequestReset, null, options, SendOptions.SendReliable);
            WaitingForAnswerMenu.SetActive(true);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        EventCodes code = (EventCodes)photonEvent.Code;

        switch (code)
        {
            case EventCodes.RequestReset:
                resetMenu.SetActive(true);
                break;
            case EventCodes.DenyReset:
                OnResetDenied();
                break;
            case EventCodes.ConfirmReset:
                if (PhotonNetwork.IsMasterClient)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
                }
                break;
        }
    }

    public void OnDenyResetButton()
    {
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)EventCodes.DenyReset, null, options, SendOptions.SendReliable);
    }

    public void OnConfirmResetButton()
    {
        resetMenu.SetActive(false);
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent((byte)EventCodes.ConfirmReset, null, options, SendOptions.SendReliable);
    }

    private void OnResetDenied()
    {
        WaitingForAnswerMenu.SetActive(false);
        resetMenu.SetActive(false);
        StartCoroutine("ResetDeniedCountdown");
    }


    private IEnumerator ResetDeniedCountdown()
    {
        resetMenuDeniedMessage.SetActive(true);
        yield return new WaitForSeconds(5);
        resetMenuDeniedMessage.SetActive(false);
    }
}
