using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PauseMenus : MonoBehaviourPunCallbacks
{
    public GameObject playerDisconnectedMenu;
    public GameObject pauseMenu;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
        }
    }

    public void OnBackToMainMenu()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void OnResumeGame()
    {
        pauseMenu.SetActive(false);
    }
}
