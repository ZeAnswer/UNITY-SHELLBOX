using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    private int timescale = 1;
    private bool gameRunning = false;
    private PlayerManager playerManager;
    public Canvas pausemenu;
    private static GameManager instance;
    
    void Awake(){
        instance = this;
    }
    
    public static GameManager GetInstance(){
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timescale;
        if(Input.GetKeyDown(KeyCode.Escape)){
            EscPress();
        }
    }


    public void PauseGame(){
        timescale = 0;
    }

    private void ResumeGame(){
        timescale = 1;
    }

    public void StartGame(){
        gameRunning = true;
        playerManager.StartGame();
    }

    public void EscPress(){
        if (gameRunning){
            timescale = pausemenu.enabled ? 1 : 0;
            pausemenu.enabled = !pausemenu.enabled;
        }
    }

    public void ResetGame(){
        gameRunning = false;
        timescale = 1;
        pausemenu.enabled = false;
        playerManager.ResetGame();
    }

    public void QuitGame(){
        Application.Quit();
    }
}
