using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class human_calcuations : MonoBehaviour
{
   public Rigidbody[] joints;
        public GameObject toroso;
        
     BallLaunch ballLaunch;
        


    void Start()
    {
        if (joints == null || joints.Length == 0)
        {
            Debug.LogError("No joints assigned. Please assign Rigidbody components to the 'joints' array.");
            return;
        }

         ballLaunch = FindObjectOfType<BallLaunch>();
    }

    void Update()
    {
        CalculateAndDisplayJointInfo();
    }

    public List<float> CalculateAndDisplayJointInfo()
    {
        List<float> data = new List<float>{};
       
        foreach (Rigidbody joint in joints)
        {
            // Position
            Vector3 position = joint.position;

            // Velocity
            Vector3 velocity = joint.velocity;

            // Angular Velocity
            Vector3 angularVelocity = joint.angularVelocity;

            // Angle
            float angle = Quaternion.Angle(joint.rotation, Quaternion.identity);




            // Display information for each joint
            //Debug.Log($"Joint {joint.name} - Position: {position}, Velocity: {velocity}, Angular Velocity: {angularVelocity}, Angle: {angle}");
            data.Add(position.x);
            data.Add(position.y);
            data.Add(position.z);
            data.Add(velocity.x);
            data.Add(velocity.y);
            data.Add(velocity.z);
            data.Add(angularVelocity.x);
            data.Add(angularVelocity.y);
            data.Add(angularVelocity.z);
            data.Add(angle);
        }

    return data;
    }   
}
