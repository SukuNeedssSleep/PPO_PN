using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class human_controller : MonoBehaviour
{   

     UdpSocket udpSocket;
     BallLaunch ballLaunch;
    human_calcuations meth;
    ArrayConversion np;

    public bool CollidedWithAgent = false;
    public bool CollidedWithGoal = false;
    public bool CollidedWithBackground = false;
    public bool CollidedWithLimiter = false;
       public bool CollidedWithGround = false;

    public float SCALAR;

    public GameObject[] joints;

     public GameObject toroso;
        public GameObject handRight;
           public GameObject handLeft;

     private Transform ballPos;
   


    void Awake(){

        udpSocket = FindObjectOfType<UdpSocket>();
        ballLaunch = FindObjectOfType<BallLaunch>();
        np = FindObjectOfType<ArrayConversion>();
        meth = FindObjectOfType<human_calcuations>();

        if (joints == null || joints.Length == 0)
        {
            Debug.LogError("No joints assigned. Please assign Rigidbody components to the 'joints' array.");
            return;
        }
    }
    void FixedUpdate(){

    }

    public string STEP(string input){

        
        float CUBE_X = joints[0].transform.position.x;
        float CUBE_Y = joints[0].transform.position.y;
        float CUBE_Z = joints[0].transform.position.z;


        float[,] resultArray = np.ConvertStringToArray(input);

        if (resultArray != null)
        {
            // Print the result array
            for (int i = 0; i < resultArray.GetLength(0); i++)
            {
                for (int j = 0; j < resultArray.GetLength(1); j++)
                {
                    //Debug.Log(resultArray[i, j] + " ");
                }
                //Debug.Log("");
            }
        }
        else
        {
            Debug.LogError("Error converting string to array.");
        }


        for(int i=0;i<12;i++){
            float[] tempFloat = GetNthArray(resultArray,i);
            print(tempFloat.ToString());
            //print(tempFloat[0].ToString() + " " + tempFloat[1].ToString()+ " " + tempFloat[2].ToString());
            Move(tempFloat,joints[i]);
        }


        BallIdentifier ball = (BallIdentifier)FindObjectOfType(typeof(BallIdentifier));

        if(ball == null){
           
            GameObject ball1 =  ballLaunch.Fire();

            ballPos = ball1.GetComponent<Transform>(); 

        }else{

            ballPos =ball.GetComponent<Transform>(); 
        }

        float DELTA_BALL_X = ballPos.position.x - transform.position.x;
        float DELTA_BALL_Y = ballPos.position.y - transform.position.y;
        float DELTA_BALL_Z = ballPos.position.z - transform.position.z;


        float reward = 0f;
        float totalReward = 0f;
        bool done = false;
        float BlockReward = 150f;

        /*===========================Rewards========================================*/

        if(CollidedWithGoal ==false && CollidedWithLimiter == false && CollidedWithBackground ==false && CollidedWithGround == false){
            totalReward += 10f;
        }

        if(CollidedWithGround){
            done = true;
            totalReward = totalReward - 150f;
            CollidedWithGround = false;
        }
        if(CollidedWithLimiter){
            done = true;
            totalReward = totalReward - 1000f;
            CollidedWithLimiter = false;
        }
        //totalReward += 1f;
        else if(CollidedWithAgent){
            totalReward = totalReward + BlockReward;
            joints[0].transform.position = new Vector3(9.5f,0.0f,18.4f);
        

            joints[0].transform.localPosition = new Vector3(3.57f,5.25f, 6.6f);
            joints[1].transform.localPosition = new Vector3(4.07f,4.19f, 6.47f);
            joints[2].transform.localPosition = new Vector3(4.228f,2.34f, 6.23f);
            joints[3].transform.localPosition = new Vector3(4.454f,0.49f, 6.58f);
            joints[4].transform.localPosition = new Vector3(2.99f,4.19f, 6.47f);
            joints[5].transform.localPosition = new Vector3(2.808f,2.24f, 6.23f);
            joints[6].transform.localPosition = new Vector3(2.563f,0.49f, 6.58f);
            joints[7].transform.localPosition = new Vector3(3.375f,9.23f, 6.61f);
            joints[8].transform.localPosition = new Vector3(5.052f,7.69f, 6.43f);  
            joints[9].transform.localPosition = new Vector3(6.175f,7.31f, 6.42f); 
            joints[10].transform.localPosition = new Vector3(2.121f,7.69f, 6.43f); 
            joints[11].transform.localPosition = new Vector3(1.01f,7.31f, 6.42f);
            toroso.transform.localPosition = new Vector3(3.59f,7.41f,6.55f);

            foreach (GameObject joint in joints){
                Rigidbody rb = joint.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                joint.transform.localRotation = Quaternion.Euler(0,0,0);
            }



            CollidedWithAgent = false;
           // Debug.Log("HIt agent");
        }
        else if(CollidedWithGoal)
        {
            done = true;
            reward =  -150f;
            //Debug.Log("Collided with the goal post");
            totalReward = totalReward + reward;
            //Debug.Log(totalReward);
           // Debug.Log("DOne has been set to true");
            CollidedWithGoal = false;
        }
        else if(CollidedWithBackground)
        {
            done = true;
           // Debug.Log("Collided with Bg");
            CollidedWithBackground = false;
        }
        else{
            done = false;
        }
       // print(DISTANCE_FROM_FLOOR);
       

        float head_y = joints[7].transform.position.y;
        float DISTANCE_FROM_FLOOR = head_y;
        if(DISTANCE_FROM_FLOOR < 1.5){
            totalReward = totalReward - 100f;

        }else if(DISTANCE_FROM_FLOOR > 4.55){
            totalReward = totalReward + 0.5f;

        }
        
        int doneToSend = 0;
        
        if(done){
            doneToSend = 1;
        }

     

        Vector3 delta = ballPos.position - toroso.transform.position;
        float DISTANCE_FROM_BALL_TORSO = delta.magnitude;

        Vector3 hand_right_delta = ballPos.position - handRight.transform.position;
        float DISTANCE_FROM_HAND_RIGHT = hand_right_delta.magnitude;

        Vector3 hand_left_delta = ballPos.position - handRight.transform.position;
        float DISTANCE_FROM_HAND_LEFT = hand_left_delta.magnitude;

        Vector3 leg_right_delta = ballPos.position - joints[3].transform.position;
        float DISTANCE_FROM_LEG_RIGHT = leg_right_delta.magnitude;

        
        Vector3 leg_left_delta = ballPos.position - joints[6].transform.position;
        float DISTANCE_FROM_LEG_LEFT = leg_left_delta.magnitude;

        
        totalReward = totalReward + ((float)(50-DISTANCE_FROM_HAND_LEFT))/10;
        totalReward = totalReward + ((float)(50-DISTANCE_FROM_HAND_RIGHT))/10;
        
        totalReward = totalReward + ((float)(50-DISTANCE_FROM_LEG_LEFT))/10;
        totalReward = totalReward + ((float)(50-DISTANCE_FROM_LEG_RIGHT))/10;
        if(DISTANCE_FROM_HAND_LEFT < 0.25 && DISTANCE_FROM_HAND_RIGHT < 0.25){
            totalReward = totalReward + 10f;
            if(CollidedWithAgent == true){
                totalReward = totalReward + 100f;
            }
        }else if(DISTANCE_FROM_LEG_LEFT < 0.25 || DISTANCE_FROM_LEG_RIGHT < 0.25){
            totalReward = totalReward + 10f;
            if(CollidedWithAgent == true){
                totalReward = totalReward + 100f;
            }
        }
        

        
        if(DISTANCE_FROM_BALL_TORSO < 0.25 && CollidedWithAgent == true){
            
            totalReward = totalReward + BlockReward;

            //Debug.Log(totalReward);

        }
        
        Vector3 delta2 = ballPos.position - transform.position;
        float DISTANCE_FROM_BALL = delta.magnitude;
        //Debug.Log(DISTANCE_FROM_BALL);


        /* =================================================================================================*/
        //Rewaed Calc for no being on floor



        List<float> observations = meth.CalculateAndDisplayJointInfo();
        observations.Add(DELTA_BALL_X);
        observations.Add(DELTA_BALL_Y);
        observations.Add(DELTA_BALL_Z);
        observations.Add(CUBE_X);
        observations.Add(CUBE_Y);
        observations.Add(CUBE_Z);

        float[] observation = observations.ToArray();
        
        string obsString = string.Join(" ",observation);

        string outputString = obsString + " " + totalReward.ToString() + " " + doneToSend.ToString();
        return outputString;

    }

    public string RESET(){

        joints[0].transform.position = new Vector3(9.5f,0.0f,18.4f);
       

        joints[0].transform.localPosition = new Vector3(3.57f,5.25f, 6.6f);
        joints[1].transform.localPosition = new Vector3(4.07f,4.19f, 6.47f);
        joints[2].transform.localPosition = new Vector3(4.228f,2.34f, 6.23f);
        joints[3].transform.localPosition = new Vector3(4.454f,0.49f, 6.58f);
        joints[4].transform.localPosition = new Vector3(2.99f,4.19f, 6.47f);
        joints[5].transform.localPosition = new Vector3(2.808f,2.24f, 6.23f);
        joints[6].transform.localPosition = new Vector3(2.563f,0.49f, 6.58f);
        joints[7].transform.localPosition = new Vector3(3.375f,9.23f, 6.61f);
        joints[8].transform.localPosition = new Vector3(5.052f,7.69f, 6.43f);  
        joints[9].transform.localPosition = new Vector3(6.175f,7.31f, 6.42f); 
        joints[10].transform.localPosition = new Vector3(2.121f,7.69f, 6.43f); 
        joints[11].transform.localPosition = new Vector3(1.01f,7.31f, 6.42f);
        toroso.transform.localPosition = new Vector3(3.59f,7.41f,6.55f);

        foreach (GameObject joint in joints){
            Rigidbody rb = joint.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            joint.transform.localRotation = Quaternion.Euler(0,0,0);
        }

        Rigidbody rbTorso = toroso.GetComponent<Rigidbody>();
        rbTorso.velocity = Vector3.zero;
        rbTorso.angularVelocity = Vector3.zero;
        toroso.transform.localRotation = Quaternion.Euler(0,0,0);



        List<float> observations = meth.CalculateAndDisplayJointInfo();
        observations.Add(100);
        observations.Add(100);
        observations.Add(120);
        observations.Add(9.5f);
        observations.Add(0f);
        observations.Add(18.4f);

        float[] observation = observations.ToArray();


        string obsString = string.Join(" ",observations);
        int totalReward = 0;
        int doneToSend = 0;

        string outputString = obsString + " " + totalReward.ToString() + " " + doneToSend.ToString();
        return outputString;
    }

    public void Move(float[] angles,GameObject joint){
        
        

        Rigidbody rb = joint.GetComponent<Rigidbody>();

        rb.AddTorque(angles[0] * SCALAR,angles[1] * SCALAR,angles[2] * SCALAR,ForceMode.Impulse);

        //print("Added Toruq");
    }

    static float[] GetNthArray(float[,] array, int n)
    {
        // Get the length of the second dimension (number of columns)
        int columns = array.GetLength(1);

        // Check if n is a valid index
        if (n < 0 || n >= array.GetLength(0))
        {
            print("Invalid index.");
            return null;
        }

        // Initialize a new array to store the nth row
        float[] nthArray = new float[columns];

        // Copy the elements of the nth row to the new array
        for (int j = 0; j < columns; j++)
        {
            nthArray[j] = array[n, j];
        }

        return nthArray;
    }



    
}


