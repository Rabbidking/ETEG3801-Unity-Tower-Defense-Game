using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public ManagerGame gameManager;
    void Update()
    {
        ScanForKeyStroke();
        //gameManager.ToggleGameOver();
    }
    void ScanForKeyStroke()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            gameManager.TogglePauseMenu();
        }
    }
    public void startGame()
    {
        SceneManager.LoadScene(1);
    }
    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
