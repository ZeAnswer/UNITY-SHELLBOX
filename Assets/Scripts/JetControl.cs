using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JetControl : MonoBehaviour
{
    private const float FORCE_AMOUNT = 500f;
    private string KEY_NAME;
    [SerializeField] public Rigidbody parentRigid;
    [SerializeField] public Transform parentTransform;
    [SerializeField] public PlayerControl parentControl;
    private ParticleSystem.EmissionModule em;
 
    void Start(){
        em = GetComponent<ParticleSystem>().emission;
        parentControl.OnPlayerDeath += EventHandler_OnPlayerDeath;
    }
    
    //particle effect on when key pressed, otherwise off
    void Update(){
        em.enabled = Input.GetKey(KEY_NAME);
    }

    //when the key is pressed, add force upwards (force amount decided by "FORCE_AMOUNT")
    void FixedUpdate(){
        if(Input.GetKey(KEY_NAME)){
            Vector3 forceDirection = transform.TransformDirection(Vector3.up);
            parentRigid.AddForceAtPosition(forceDirection * FORCE_AMOUNT, transform.position, ForceMode.Force);
            //Debug.DrawRay(transform.position, forceDirection * 10, Color.green);
        }
    }

    //upon creation, set keyname to react to
    public void SetKey(string keyname){
        KEY_NAME = keyname;
    }

    //upon death disable particle system
    private void EventHandler_OnPlayerDeath(object sender, PlayerDeathEventArgs e){
        em.enabled = false;
    }
}
