using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headcheck : MonoBehaviour
{
        human_controller human;
     public GameObject gordon;
    // Start is called before the first frame update
    void Start()
    {
        human = gordon.GetComponent<human_controller>();
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.CompareTag("Ground")){
            //print("Collied with limiter");
            human.CollidedWithGround = true;
        }
    }
}
