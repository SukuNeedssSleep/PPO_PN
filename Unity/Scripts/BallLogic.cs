using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLogic : MonoBehaviour
{   
    public bool isInPos;
    public string state;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Post"){
            Debug.Log("Goal");
            state = "Goal";
          
            
        }else if(other.tag == "Bg"){
            Debug.Log("Miss");
            state = "Miss";
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.tag == "PosCheck"){
            isInPos = true;

        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "PosCheck"){
            isInPos = false;
        }
    }
}
