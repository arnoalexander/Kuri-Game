using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game {
	public class ResumeController : Element {
		// Use this for initialization
		void Start () 
		{

		}

		// Update is called once per frame
		void Update () 
		{

		}
		void OnMouseDown(){
			// this object was clicked - do something
			app.model.screenModel.pauseButton.SetActive (true);
			app.model.screenModel.paused = false;
			app.model.screenModel.pausePanel.SetActive (false);
			Time.timeScale = 1;
		}


	}
}