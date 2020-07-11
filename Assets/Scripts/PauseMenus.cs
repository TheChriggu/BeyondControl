using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PauseMenus : MonoBehaviour
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
        SceneManager.LoadScene("StartScreen");
    }

    public void OnResumeGame()
    {
        pauseMenu.SetActive(false);
    }
}
