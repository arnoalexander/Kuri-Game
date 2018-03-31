using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game {
	public class PauseController : Element {

		// Use this for initialization
		void Start () 
		{
		}

		// Update is called once per frame
		void Update () 
		{

		}
		void OnMouseDown(){
			app.model.screenModel.pauseButton.SetActive (false);
			app.model.screenModel.paused = true;
			app.model.screenModel.pausePanel.SetActive (true);
			Time.timeScale = 0;
		}


	}
}