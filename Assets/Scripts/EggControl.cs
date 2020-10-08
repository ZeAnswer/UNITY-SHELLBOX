using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggControl : MonoBehaviour
{
    private Vector3 centreOfMass = new Vector3(0f, 0f, -0.75f);
    public Material matblue;
    public Material matred;
    private MeshRenderer mr;
    
    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    public void setMat(int id){
        if (id == 1){
            mr.material=matblue;
        }
        else{
            mr.material=matred;
        }
    }

}
