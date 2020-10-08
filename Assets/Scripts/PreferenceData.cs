using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreferenceData : MonoBehaviour
{   //{left, right, up
    private static string[,] keys = {{"", "", ""}, {"a", "d", "w"}, {"left", "right", "up"}};
    private static Vector3[] startlocs = {new Vector3(0f, 0f, 0f), new Vector3(-25f, 8f, 12f), new Vector3(25f, 8f, 12f)};
    private static int MAX_LIVES = 12;
    private static float MAX_HEALTH = 30000f;
    [SerializeField] private TextMeshProUGUI[] p1keyText = new TextMeshProUGUI[3];
    [SerializeField] private TextMeshProUGUI[] p2keyText = new TextMeshProUGUI[3];

    private void Update(){
        p1keyText[0].text = keys[1, 0];
        p1keyText[1].text = keys[1, 1];
        p1keyText[2].text = keys[1, 2];
        p2keyText[0].text = keys[2, 0];
        p2keyText[1].text = keys[2, 1];
        p2keyText[2].text = keys[2, 2];
    }

    public static void SetLeft(int id, string key){
        keys[id, 0] = key;
    }

    public static string GetLeft(int id){
        return keys[id, 0];
    }

    public static void SetRight(int id, string key){
        keys[id, 1] = key;
    }

    public static string GetRight(int id){
        return keys[id, 1];
    }

    public static void SetUp(int id, string key){
        keys[id, 2] = key;
    }

    public static string GetUp(int id){
        return keys[id, 2];
    }

    public static void SetLoc(int id, Vector3 loc){
        startlocs[id] = loc;
    }

    public static Vector3 GetLoc(int id){
        return startlocs[id];
    }

    public static void SetMaxLives(int amount){
        MAX_LIVES = amount;
    }

    public static int GetMaxLives(){
        return MAX_LIVES;
    }

    public static void SetMaxHealth(float amount){
        MAX_HEALTH = amount;
    }

    public static float GetMaxHealth(){
        return MAX_HEALTH;
    }

    public void ButtonSetKey(int id, int keyId){
        //TODO
    }

    public void FixPlayer2Controls(bool shouldFix){
        if(shouldFix){
            keys[2,0] = "j";
            keys[2,1] = "l";
            keys[2,2] = "i";
        }
        else{
            keys[2,0] = "left";
            keys[2,1] = "right";
            keys[2,2] = "up";
        }
    }
}
