using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTracking : MonoBehaviour
{
    // Start is called before the first frame update\\

    public udpBall udpReceive;
    public GameObject ball;

    public float x_max;
    public float y_max;

    public string data;

    void Start()
    {
        
    }

    public void UpdatePythonRcvdText(string str)
    {
        data = str;
        print(data);
    }
    // Update is called once per frame
    void FixedUpdate()
    {


        data = data.Remove(0, 1);
        data = data.Remove(data.Length-1, 1);
        string[] points = data.Split(',');


        float x = float.Parse((points[0]));
        float y = float.Parse((points[1]));
        float z = float.Parse((points[2]))/9f;

        float X_multipilier  = (float)(x_max /float.Parse((points[3])));
        float y_multipilier  = (float)(y_max /float.Parse((points[4])));

        

        ball.transform.localPosition = new Vector3(x*X_multipilier ,y*y_multipilier,z);
        
        

    } 



}

/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTracking : MonoBehaviour
{
    // Start is called before the first frame update\\

    public UDPReceive udpReceive;
    public GameObject ball;
    public float speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string data = udpReceive.data;

        data = data.Remove(0, 1);
        data = data.Remove(data.Length-1, 1);
        print(data);
        string[] points = data.Split(',');
        print(points[0]);

        float x1 = float.Parse((points[0]))/100;
        float y1 = float.Parse((points[1]))/100;
        float z1 = float.Parse((points[2]))/100;

       var  c_pos = new Vector3(x1,y1,z1);

        float x2 = float.Parse((points[0]))/100;
        float y2 = float.Parse((points[1]))/100;
        float z2 = float.Parse((points[2]))/100;

       var  target_pos = new Vector3(x2,y2,z2);
        
        var step = speed * Time.deltaTime;
        ball.transform.localPosition = Vector3.MoveTowards(c_pos,target_pos,step);

    } 
}



*/
