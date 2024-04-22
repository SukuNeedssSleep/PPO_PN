using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    
    UdpSocket udpSocket;


    public GameObject Agent;
    human_controller actions;
    bool newInp = false;

    int id;
    string tempStr = "4";


    private void Awake()
    {
        udpSocket = FindObjectOfType<UdpSocket>();
        actions = Agent.GetComponent<human_controller>();
    }

    public void UpdatePythonRcvdText(string str)
    {
        tempStr = str;
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
                    string StepReturn = actions.STEP(tempStr);
                    udpSocket.SendData(StepReturn);
                    newInp = false;
                    break;
                    
                    
        }
        }


	
       
    }

}
