using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLaunch : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;
    public Transform launchPosition;
    [SerializeField]
    private float launchSpeed;

    private void FixedUpdate() {
        //Invoke("Fire",1.9f);
    }

    public GameObject Fire()
    {   

        float xOffset = Random.Range(0f,-29f);
        float yoffSet = Random.Range(-30f,30f);

        float xAngle = Random.Range(xOffset, xOffset);
        float yAngle = Random.Range(yoffSet, yoffSet);

        launchPosition.Rotate(xAngle, yAngle, 0,Space.Self);



        GameObject ball = Instantiate(ballPrefab,launchPosition.position,launchPosition.rotation);


        Rigidbody rb = ball.GetComponent<Rigidbody>();


        StartCoroutine(Launch(rb,launchPosition));
       

        Destroy(ball,2.5f);
        return ball;
    }


    public IEnumerator Launch(Rigidbody rb,Transform launchPos){
        yield return new WaitForSeconds(0.2f);
        rb.AddForce(launchPos.forward * launchSpeed,ForceMode.Impulse);
        launchPosition.rotation = Quaternion.Euler(0, 0, 0);

    }
    
}
