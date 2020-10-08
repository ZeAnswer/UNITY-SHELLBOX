using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitControl : MonoBehaviour
{
    private Rigidbody rb;
    private string tg;
    private float PAIN_MODIFIER;
    private float DAMAGE_MODIFIER;
    private float EXPLOSION_FORCE_MAX;
    private float MAGNITUDE_MAX;
    private float EXPLOSION_RADIUS;
    private float SWORD_MODIFIER;
    private ExplosionEffect ee;
    //private PlayerHitEventArgs playerHitEventArgs;
    public event EventHandler<PlayerHitEventArgs> OnPlayerHit;
    
    void Awake(){
        EXPLOSION_RADIUS = 0f;
        EXPLOSION_FORCE_MAX = 5000f;
        MAGNITUDE_MAX = 50f;
        rb = GetComponent<Rigidbody>();
        tg = gameObject.tag;
        //playerHitEventArgs = new PlayerHitEventArgs();
    }

    void Start(){
        if (tg == "SWORD"){
            PAIN_MODIFIER = 0.75f;
            DAMAGE_MODIFIER = 2f;
            SWORD_MODIFIER = 1f;
        }
        if (tg == "SHELL"){
            PAIN_MODIFIER = 2f;
            DAMAGE_MODIFIER = 0.75f;
            SWORD_MODIFIER = 0f;
        }
        ee=ExplosionEffect.GetInstance();
    }
    //will be a smarter way later
    int GetContactType(GameObject other){
        int contactType = 0;
        if (tg == "SHELL"){
            if(other.tag == "SHELL")
                contactType = 1;
            else if(other.tag == "WORLD")
                contactType = 2;
            else
                contactType = 3;
        }
        else if(tg == "SWORD"){
            if(other.tag == "SHELL")
                contactType = 3;
            else
                contactType = 4;
        }
        return contactType;
    }

    //Contact types (will be enums later) 1:shellXshell, 2:shellXsurface, 3:shellXsword, 4:swordXsurface/sword
    void OnCollisionEnter(Collision collision){
        float other_modifier = 0.2f;
        float modifier;
        Vector3 contact_point = collision.contacts[0].point;
        float magnitude = collision.relativeVelocity.magnitude;
        GameObject other = collision.gameObject;
        if(other.tag != "ROPE" && other.tag != "HOOK"){
            //Debug.Log("Collision- hit " + other.tag + "\n");
            if (other.tag != "WORLD"){
                //Debug.Log(name + " Collision- hit " + other.name + " magnitude of " + magnitude.ToString() + "\n");
                other_modifier = other.GetComponent<HitControl>().GetModifier();
            }
            modifier = (other_modifier * PAIN_MODIFIER) + SWORD_MODIFIER;
            DoExplosion(modifier, contact_point, magnitude, GetContactType(other));
        }
    }

    private float GetExplosionForce(float modifier, float magnitude){
        float exp_force = (Mathf.Min(magnitude, MAGNITUDE_MAX) / MAGNITUDE_MAX) * EXPLOSION_FORCE_MAX * modifier;
        return exp_force;
    }

    private void DoExplosion(float modifier, Vector3 contact_point, float magnitude, int contactType){
        float exp_force = GetExplosionForce(modifier, magnitude);
        if(exp_force > 200){
            SoundEffectManager.GetInstance().PlaySoundEffect(contact_point, exp_force, contactType);
            rb.AddExplosionForce(exp_force, contact_point, EXPLOSION_RADIUS);
            ExplosionEffect.GetInstance().MakeExplosionEffect(contact_point, exp_force);
            //Debug.Log("Did EXPLOSION with force " + exp_force.ToString() + "\n");
            SendEventPlayerHit(exp_force);}
        //add particle effect
    }

    public float GetModifier(){
        return DAMAGE_MODIFIER;
    }


    private void SendEventPlayerHit(float amount){
        PlayerHitEventArgs playerHitEventArgs = new PlayerHitEventArgs();
        playerHitEventArgs.damage_amount = amount;
        if (OnPlayerHit != null) OnPlayerHit(this, playerHitEventArgs);
    }

}
