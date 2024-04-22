using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimiterCheck : MonoBehaviour
{
   
    Movement human;
    public GameObject Agent;
    // Start is called before the first frame update
    void Start()
    {
        human = Agent.GetComponent<Movement>();
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.CompareTag("Limiter")){
            //print("Collied with limiter");
            human.CollidedWithLimiter = true;
        }

           
        if(other.gameObject.CompareTag("Ball")){
            //print("Collied with limiter");
            human.CollidedWithLimiter = true;
            Destroy(other.gameObject,0.2f);
        }
    }  
    
}
