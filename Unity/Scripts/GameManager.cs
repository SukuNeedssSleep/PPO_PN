using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public int rounds;
    public int goal;
    int count;
    public int balls;
    public float Timer;
    public bool isInPos = false;
    public bool ran = false;


    public GameObject ball;
    public GameObject ballTracking;
    public GameObject player;
    BallLogic ballData;

    public GameObject[] score;

    public GameObject OverLay;
    public GameObject Transition;
    public GameObject black;

    public GameObject GameOver;
    public GameObject Goal;
    private Animation anim;

    bool hasnotspawned = true;

    void Start(){
        ballData = ball.GetComponent<BallLogic>();
        anim = ball.GetComponent<Animation>();
        OverLay.SetActive(false);
        Transition.SetActive(false);
    }

    void FixedUpdate(){

        
        isInPos = ballData.isInPos;


        if(isInPos && hasnotspawned == true){
            StartCoroutine(Load());            
        }


        if(ballData.state == "Goal" && count  == 0){
                SoundManager.PlaySound2D(SoundManager.Sound.Goal);
                rounds --;
                score[rounds].GetComponent<Renderer>().material.color = Color.green;
                goal++;
                count++;
                Destroy(ball);
                StartCoroutine(LoadTransition());

        }else if(ballData.state == "Miss"  && count  == 0){
                SoundManager.PlaySound2D(SoundManager.Sound.Miss);
                //anim.Play("Miss");
                rounds --;
                score[rounds].GetComponent<Renderer>().material.color = Color.red;
                count++;
                Destroy(ball);
                StartCoroutine(LoadTransition());
        }

        
        


        
       
        
        


        
    }


    IEnumerator Load(){
        Transition.SetActive(true);
        hasnotspawned = false;
        ballTracking.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        Transition.SetActive(false);
        OverLay.SetActive(true);
        ballTracking.SetActive(true);
    }

    IEnumerator LoadTransition(){
        black.SetActive(true);
        OverLay.SetActive(false);
        yield return new WaitForSeconds(0.9f);
        black.SetActive(false);
        OverLay.SetActive(true);
        ballTracking.SetActive(true);


    }
    IEnumerator LoadGoal(float animTime){
        OverLay.SetActive(false);
        Goal.SetActive(true);
        ballTracking.SetActive(false);
        yield return new WaitForSeconds(animTime);
        Goal.SetActive(false);
        OverLay.SetActive(true);
        ballTracking.SetActive(true);
    }
    
    void Reset(){
        ball.transform.localPosition = new Vector3(0,0,0f);
        player.transform.position = new Vector3(9.5f,0.0f,18.4f);
    }

}
