using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewYouBall : MonoBehaviour
{   
    Movement actions;
    private void OnTriggerEnter(Collider other) {


        if(other.gameObject.CompareTag("Post")){
            actions = FindObjectOfType<Movement>();
            actions.CollidedWithGoal = true;
            Destroy(gameObject,0.2f);

        }    


    }
}
