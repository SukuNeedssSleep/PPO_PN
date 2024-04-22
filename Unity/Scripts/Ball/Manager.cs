using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Manager : MonoBehaviour
{
    
    UdpSocket udpSocket;


    public GameObject Agent;
    Movement actions;
    bool newInp = false;

    int id;
    string tempStr = "4";


    private void Awake()
    {
        udpSocket = FindObjectOfType<UdpSocket>();
        actions = Agent.GetComponent<Movement>();
    }

	public void BallInPosition(bool tog){
		print(tog);
	}

    public void UpdatePythonRcvdText(string str)
    {
        tempStr = str;
        //print(tempStr);
        newInp = true;
    }


    private void FixedUpdate() {

        
        if(newInp == true){
            switch (tempStr){
                case "Reset":
                    string observations = actions.RESET();
                    udpSocket.SendData(observations);
                    newInp = false;
                    break;

                default:
                    
                    float[] actionID = {0f,0f,0f};
                    string[] stringRN = tempStr.Split(",");

                    for( int i = 0; i < 3; i++){
                        actionID[i] = float.Parse(stringRN[i]);
                    }


                    string StepReturn = actions.STEP(actionID);
                    udpSocket.SendData(StepReturn);
                    newInp = false;
                    break;
                    
                    
        }
        }


	
       
    }


    



}
