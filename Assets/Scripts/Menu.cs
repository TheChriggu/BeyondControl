using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    
    public void StartLoginScreen(string sceneName)
    {
        SceneManager.LoadScene("LoginScreen");
    }


    
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
