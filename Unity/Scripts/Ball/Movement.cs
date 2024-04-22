using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Movement : MonoBehaviour
{   
    public float Thrust = 15f;

    float ballX;
    float ballY;
    float ballZ;
    


    private Rigidbody rb;
    UdpSocket udpSocket;
    BallLaunch ballLaunch;

    Vector3 velocityBall;

    public bool CollidedWithAgent = false;
    public bool CollidedWithGoal = false;

    public bool CollidedWithLimiter= false;
    public bool CollidedWithBackground = false;
    

    private GameObject ball;
    private Transform ballPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ballLaunch = FindObjectOfType<BallLaunch>();
        udpSocket = FindObjectOfType<UdpSocket>();
    }

    public string STEP(float[] action)
    {   
        rb.AddForce(action[0] * Thrust , action[1] * Thrust , action[2]* Thrust , ForceMode.Impulse);

        BallIdentifier ball = (BallIdentifier)FindObjectOfType(typeof(BallIdentifier));

        if(ball == null){
           
            GameObject ball1 =  ballLaunch.Fire();
            
            Thread.Sleep(10);
            Rigidbody rbb = ball1.GetComponent<Rigidbody>();
            ballPos = ball1.GetComponent<Transform>(); 
            ballX = ball1.transform.position.x;
            ballY = ball1.transform.position.y;
            ballZ = ball1.transform.position.z;

            velocityBall = rbb.velocity;


        }else{
            Rigidbody rbb = ball.GetComponent<Rigidbody>();
            ballPos =ball.GetComponent<Transform>(); 
            ballX = ball.transform.position.x;
            ballY = ball.transform.position.y;
            ballZ = ball.transform.position.z;

             velocityBall = rbb.velocity;
        }

        
        float reward = 0f;
        float totalReward = 0f;
        bool done = false;
        int BlockReward = 1100;

        if(CollidedWithLimiter){
            done = true;
            totalReward = totalReward -3000f;
            CollidedWithLimiter = false;
            //Debug.Log("Collided with limiter");
        }
        else if(CollidedWithAgent)
        {
            //totalReward = totalReward + BlockReward;
            done = true;
            //Debug.Log(totalReward);
            CollidedWithAgent = false;
            //Debug.Log("Collided with Agent");
        }

        else if(CollidedWithGoal)
        {
            done = true;
            reward =  -1000f;
            //Debug.Log("Collided with the goal post");
            totalReward = totalReward + reward;
            CollidedWithGoal = false;
        }
        else if(CollidedWithBackground)
        {
            done = true;
            //Debug.Log("Collided with Bg");
            CollidedWithBackground = false;
        }else{
            done = false;
        }
        
        int doneToSend = 0;
        
        if(done){
            doneToSend = 1;
        }


        float CUBE_X = transform.position.x;
        float CUBE_Y = transform.position.y;
        float CUBE_Z = transform.position.z;



        float DELTA_BALL_X = ballPos.position.x - CUBE_X;
        float DELTA_BALL_Y = ballPos.position.y - CUBE_Y;
        float DELTA_BALL_Z = ballPos.position.z - CUBE_Z;


       

        
        Vector3 delta = ballPos.position - transform.position;
        float DISTANCE_FROM_BALL = delta.magnitude;
        

        float[] observations = {CUBE_X,CUBE_Y,CUBE_Z,DELTA_BALL_X,DELTA_BALL_Y,DELTA_BALL_Z,ballX,ballY,ballZ};
        
        //velocityBall.x,velocityBall.y,velocityBall.z,DISTANCE_FROM_BALL,rb.velocity.x,rb.velocity.y,rb.velocity.z};

        if(DISTANCE_FROM_BALL < 7.5f){
            float divideFactor =(float) Math.Pow(5,DISTANCE_FROM_BALL);
            //Debug.Log(divideFactor);
            totalReward = (float)((totalReward + (float)(50 - DISTANCE_FROM_BALL)))/divideFactor;  
            
            if(CollidedWithAgent){
                totalReward += 2000f;
            }
        }

        
        totalReward = totalReward / 10;

        //Debug.Log(DISTANCE_FROM_BALL);


        //Debug.Log(totalReward);

        string obsString = string.Join(" ",observations);

        //Debug.Log(doneToSend);
        string outputString = obsString + " " + totalReward.ToString() + " " + doneToSend.ToString();
        return outputString;

    }
    


    public string RESET(){
        
        
        
        transform.rotation = Quaternion.Euler(0,0,0);
        transform.position = new Vector3(7.0f,0.5f,14.3f);
        rb.velocity = Vector3.zero;
       

        float CUBE_X = transform.position.x;
        float CUBE_Y = transform.position.y;
        float CUBE_Z = transform.position.z;

        Vector3 delta = ballLaunch.launchPosition.position - transform.position;
        float DISTANCE_FROM_BALL = delta.magnitude;

        float DELTA_BALL_X = ballLaunch.launchPosition.position.x - CUBE_X;
        float DELTA_BALL_Y = ballLaunch.launchPosition.position.y - CUBE_Y;
        float DELTA_BALL_Z = ballLaunch.launchPosition.position.z - CUBE_Z;

        float[] observations = {CUBE_X,CUBE_Y,CUBE_Z,DELTA_BALL_X,DELTA_BALL_Y,DELTA_BALL_Z,ballX,ballY,ballZ};
        //float[] observations = {CUBE_X,CUBE_Y,CUBE_Z,DELTA_BALL_X,DELTA_BALL_Y,DELTA_BALL_Z,ballLaunch.launchPosition.position.x,
       // ballLaunch.launchPosition.position.y,ballLaunch.launchPosition.position.z,0,0,0,DISTANCE_FROM_BALL,0,0,0};

        string obsString = string.Join(" ",observations);
        int totalReward = 0;
        int doneToSend = 0;
        string outputString = obsString + " " + totalReward.ToString() + " " + doneToSend.ToString();
        return outputString;

    }

    private void Fire(){
        ballLaunch.Fire();
    }

    
 
}
