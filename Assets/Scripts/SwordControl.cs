using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordControl : MonoBehaviour
{
    private ConfigurableJoint swordSpring;
    private Vector3 swordOutPosition = Vector3.down * 0.8f;
    private Vector3 swordInPosition = Vector3.up * 1.05f;
    [SerializeField] private string KEY_NAME;
    [SerializeField] public PlayerControl parentControl;
    

    void Awake(){
        swordSpring = GetComponent<ConfigurableJoint>();
    }

    void Start(){
        parentControl.OnPlayerDeath += EventHandler_OnPlayerDeath;
    }

    //take sword out upon key-press, bring it back in upon key-release
    void Update(){
        if(Input.GetKeyDown(KEY_NAME)){
            swordOut();
        }
        if(Input.GetKeyUp(KEY_NAME)){
            swordIn();
        }
    }

    //brings sword to "out" position by setting the joint's target position
    private void swordOut(){
        swordSpring.targetPosition = swordOutPosition;
    }

    //brings sword to "in" position by setting the joint's target position
    private void swordIn(){
        swordSpring.targetPosition = swordInPosition;
    }

    //upon creation, sets the keyname for the sword to react to
    public void SetKey(string keyname){
        KEY_NAME = keyname;
    }

    private void EventHandler_OnPlayerDeath(object sender, PlayerDeathEventArgs e){
        swordIn();
    }
}
