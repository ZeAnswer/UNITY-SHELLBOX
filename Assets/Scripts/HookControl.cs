using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookControl : MonoBehaviour
{
    /*[SerializeField] private bool isConnected;
    [SerializeField] private bool isColliding;
    [SerializeField] private Joint joint;
    public float forceToBreak = 5000000;
    public float torqueToBreak = 5000000;
    private Collider collider;
    private Rigidbody rigidbody;
    [SerializeField] private string KEY_NAME;

    void Awake(){
        isConnected = false;
        isColliding = false;
        joint = null;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //collider.enabled = isColliding;
        if(Input.GetKeyDown(KEY_NAME)){
            StartHooking();
        }
        if(Input.GetKeyUp(KEY_NAME)){
            StopHooking();
        }
    }

    void OnCollisionEnter(Collision col)
     {
         // Set a tag to make the conection selective.
         if (isColliding && col.gameObject.GetComponent<Rigidbody>() != null && col.gameObject.tag != "ROPE" && col.gameObject.tag != "SWORD" && !isConnected)
         {
             Debug.Log("Entering CollisionEnter\n");
             isConnected = true;
             isColliding = false;
             // create a joint
             joint = gameObject.AddComponent<CharacterJoint>();
 
             //Set the anchor point where the wand and blade collide
             ContactPoint contact = col.contacts[0];
             joint.anchor = transform.InverseTransformPoint(contact.point);
             joint.connectedBody = col.contacts[0].otherCollider.transform.GetComponent<Rigidbody>();
 
             //Set the forces which will break the joint.
             joint.breakForce = forceToBreak;
             joint.breakTorque = torqueToBreak;
 
             // Stops objects from continuing to collide and creating more joints
             joint.enableCollision = false;
         }
     }

     void OnTriggerEnter(Collider other)
     {
         // Set a tag to make the conection selective.
         if (isColliding && other.gameObject.GetComponent<Rigidbody>() != null && !isConnected)
         {
             Debug.Log("Entering TriggerEnter\n");
             isConnected = true;
             isColliding = false;
             // create a joint
             joint = gameObject.AddComponent<CharacterJoint>();
 
             //Set the anchor point where the wand and blade collide
             joint.anchor = Vector3.zero;
             joint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
 
             //Set the forces which will break the joint.
             joint.breakForce = forceToBreak;
             joint.breakTorque = torqueToBreak;
 
             // Stops objects from continuing to collide and creating more joints
             joint.enableCollision = false;
         }
     }

    void OnJointBreak(float breakForce){
         Debug.Log("A joint has just been broken!, force: " + breakForce);
         StopHooking();
    }

     private void StartHooking(){
         isColliding = true;
     }

     private void StopHooking(){
         Debug.Log("Should have stopped Hooking now");
         isColliding = false;
         isConnected = false;
         if(joint != null){
             Destroy(joint);
             joint = null;
         }
     }*/
}
