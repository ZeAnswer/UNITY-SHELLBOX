using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{   
    //general
    private Rigidbody rigidBody;
    private Vector3 axisdrag = new Vector3(-1f, -1f, 1f);
    private const float fallMultiplier = 2f;
	[SerializeField] public ParticleSystem ps;
	
    //The inner parts of the prefab
    [SerializeField] public GameObject jetLeftobj;
    [SerializeField] public GameObject jetRightobj;
    [SerializeField] public GameObject swordobj;
    [SerializeField] public GameObject eggShellobj;
    [SerializeField] public Transform massCenterObject;

    //specifics for this player
    [SerializeField] private int playerID = 0;
    [SerializeField] private string keyLeft;
    [SerializeField] private string keyRight;
    [SerializeField] private string keyUp;
    private Vector3 START_LOCATION;
    
    //health info
    private float MAX_HEALTH;
    private bool ALIVE;
    private float health;
    public Text text_health;
    private int healthperc;

    //Player sends this event when it dies
    private PlayerDeathEventArgs playerDeathEventArgs;
    public event EventHandler<PlayerDeathEventArgs> OnPlayerDeath;
    
    

    void Awake(){
        //setting basic things, "Hide" disables the object (game not started yet)
        MAX_HEALTH = PreferenceData.GetMaxHealth();
        rigidBody = GetComponent<Rigidbody>();
        playerDeathEventArgs = new PlayerDeathEventArgs();
        ALIVE = false;
        Hide();
    }
    
    void Start(){
        //register for the PlayerHit event
        GetComponent<HitControl>().OnPlayerHit += EventHandler_OnPlayerHit;
    }

    void Update(){
        //use GravitySphere to decide on new center of mass
        GetComponent<Rigidbody>().centerOfMass = Vector3.up * massCenterObject.localPosition.y;
        
        //Directions of Velocity and Angular Velocity, mainly for debugging/raycasting
        Vector3 velDirection = GetComponent<Rigidbody>().velocity;
        Vector3 angvelDirection = GetComponent<Rigidbody>().angularVelocity * 5;
        //Debug.DrawRay(transform.position, velDirection, Color.red);
        //Debug.DrawRay(transform.position, angvelDirection, Color.magenta);

        //Increase gravity slightly (by "fallMultiplier") when object is falliing
        if(GetComponent<Rigidbody>().velocity.y < 0){
            GetComponent<Rigidbody>().velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        //specific drag for each axis, decided by "axisdrag"
        GetComponent<Rigidbody>().velocity += (Vector3.Scale(GetComponent<Rigidbody>().velocity, axisdrag) - GetComponent<Rigidbody>().velocity) * Time.deltaTime;
        
    }

    //Used when creating a new player- set the id number of the player for future use
    public void SetPlayerId(int id){
        playerID = id;
        playerDeathEventArgs.playerID = id;
    }

    //Used when creating a new player- set the starting location of the player. Player is moved there upon respawn
    public void SetStartingLocation(Vector3 location){
        START_LOCATION = location;
    }

    //Using the ID, reads appropriate key names from "PreferenceData" and sets own parts (jets/sword) to respond to those keys.
    public void  UpdateKeys(){
        keyLeft = PreferenceData.GetLeft(playerID);
        keyRight = PreferenceData.GetRight(playerID);
        keyUp = PreferenceData.GetUp(playerID);
        jetLeftobj.GetComponent<JetControl>().SetKey(keyRight);
        jetRightobj.GetComponent<JetControl>().SetKey(keyLeft);
        swordobj.GetComponent<SwordControl>().SetKey(keyUp);
        
        //sets the color (in the future- skin/color/etc.) of this player by ID
        eggShellobj.GetComponent<EggControl>().setMat(playerID);
    }

    //send PlayerDeath event upon death
    private void SendEventPlayerDeath(){
        if (OnPlayerDeath != null) OnPlayerDeath(this, playerDeathEventArgs);
    }

    //handling the event of being hit. If health>0 (i.e. alive) lowers health. If it caused death, calls "Death()"
    private void EventHandler_OnPlayerHit(object sender, PlayerHitEventArgs e){
        if (health > 0){
            health -= e.damage_amount;
            UpdateTextHealth();
            if (health <=0 && ALIVE){
                ALIVE = false;
                Death();
            }
        }
        //Do more Stuff
    }

    //deactivates object
    private void Hide(){
        gameObject.SetActive(false);
    }

    //activates object
    private void Show(){
        gameObject.SetActive(true);
    }

    //waits for (delay) seconds and re-enables collisions.
    IEnumerator UnGhostDelay(){
        yield return new WaitForSeconds(0.5f);
        Collider[] Cols = gameObject.GetComponents<Collider>();
            foreach (var collider  in Cols) {
                if(collider.gameObject!=gameObject) 
                    collider.enabled = true;
            }
    }

    //disables collisions for this player. Was planned to be used upon respawn so you couldn't insta-die, currently unused cause fixes must be made
    private void Ghost(){
        Collider[] Cols = gameObject.GetComponents<Collider>();
        foreach (var collider  in Cols) {
            if(collider.gameObject!=gameObject) 
                collider.enabled = false;
        }
    }

    //re-enables collisions after a certain delay
    private void UnGhost(){
        StartCoroutine(UnGhostDelay());
    }

    //sends PlayerDeath event
    private void PlayerDeath(){
        SendEventPlayerDeath();
        //Do More Stuff
    }

    //resets velocity and angular velocity, used upon respawn so player will stay in place
    private void StopMovement(){
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    //called upon death. stops movement, disables object, generates a death ParticleSystem and sends PlayerDeath event.
    public void Death(){
        StopMovement();
        Hide();
        Instantiate(ps, transform.position, Quaternion.identity);
        SendEventPlayerDeath();
        //death effect
    }

    // called to respawn. takes player to starting location, sets back health to max, enables object
    public void Birth(){
        transform.position = START_LOCATION;
        transform.rotation = Quaternion.identity;
        health = MAX_HEALTH;
        UpdateKeys();
        UpdateTextHealth();
        ALIVE = true;
        //Ghost();
        Show();
        //UnGhost();
    }

    //used upon player creation, links the specific text field to this player
    public void SetTextHealth(Text my_text){
        text_health = my_text;
    }

    //updates text field to show current health percentage
    private void UpdateTextHealth(){
        healthperc = (int)(Mathf.Max(((health * 100)/MAX_HEALTH), 0));
        text_health.text = "Health: " + healthperc.ToString() + "%";
    }

    public void ResetPlayer(){
        StopMovement();
        transform.position = START_LOCATION;
        transform.rotation = Quaternion.identity;
        health = MAX_HEALTH;
        UpdateTextHealth();
        ALIVE = false;
        Hide();
    }
}