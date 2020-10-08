using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] public GameObject eggPlayerPrefab;
    private int[] playing = {1, 2};
    private GameObject[] players = new GameObject[3];
    private int[] lives = {0, 0, 0};
    [SerializeField] private Text[] texts_lives = new Text[3];
    [SerializeField] private Text[] texts_health = new Text[3];
    public Canvas winScreen;
    [SerializeField] public TextMeshProUGUI wintext;
    private static PlayerManager instance;
    
    public static PlayerManager GetInstance(){
        return instance;
    }
    
    void Awake(){
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(int i in playing){
            CreatePlayer(i, PreferenceData.GetLoc(i));
            lives[i] = PreferenceData.GetMaxLives();
        }
    }

    //creates player with id "id" and starting location "location". Player sets the keys corresponding to the ID.
    private void CreatePlayer(int id, Vector3 location){
        GameObject clone = Instantiate(eggPlayerPrefab, location, Quaternion.identity);
        PlayerControl pc = clone.GetComponent<PlayerControl>();
        pc.SetPlayerId(id);
        pc.SetStartingLocation(location);
        pc.UpdateKeys();
        pc.OnPlayerDeath += EventHandler_OnPlayerDeath;
        clone.name = "Player"+id.ToString();
        players[id] = clone;
        pc.SetTextHealth(texts_health[id]);
    }

    public void StartGame(){
        foreach(int i in playing){
            PlayerBirth(i);
        }
        Update_text_lives();
    }

    private void PlayerGameOver(int id){
        int winId = 3-id;
        wintext.text = "Player " + winId.ToString() + " Wins!";
        GameManager.GetInstance().PauseGame();
        winScreen.enabled = true;
    }

    private void PlayerDeath(int id){
        players[id].GetComponent<PlayerControl>().Death();
    }

    IEnumerator PlayerBirthDelay(int id){
        yield return new WaitForSeconds(0.5f);
        if (--lives[id] >= 0){
            players[id].GetComponent<PlayerControl>().Birth();
        }
        else{
            PlayerGameOver(id);
        }
        Update_text_lives();
    }

    private void PlayerBirth(int id){
        StartCoroutine(PlayerBirthDelay(id));
    }

    private void EventHandler_OnPlayerDeath(object sender, PlayerDeathEventArgs e){
        PlayerBirth(e.playerID);
        //Do more Stuff
    }

    private void Update_text_lives(){
        foreach(int i in playing){
            texts_lives[i].text = "Lives: " + (lives[i] + 1).ToString();
        }
    }

    public void ResetGame(){
        winScreen.enabled = false;
        foreach(int i in playing){
            lives[i] = PreferenceData.GetMaxLives();
            players[i].GetComponent<PlayerControl>().ResetPlayer();
            Update_text_lives();
        }
    }
}
