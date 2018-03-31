using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ExitController : MonoBehaviour {
		// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	void Update () 
	{

	}
	public void ClickExit(){
		#if UNITY_STANDALONE
		//Quit the application
		Application.Quit();
		#endif

		//If we are running in the editor
		#if UNITY_EDITOR
		//Stop playing the scene
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}
