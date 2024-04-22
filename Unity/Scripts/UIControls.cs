using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIControls : MonoBehaviour
{   
    public void Reset(){
	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	print("Scene has been reset");
	}

	
}
