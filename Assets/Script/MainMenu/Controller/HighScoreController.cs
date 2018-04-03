using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace MainMenu{
	public class HighScoreController : Element {
		// Use this for initialization
		private Text scoreText;
		void Start () 
		{
			scoreText = app.model.screenModel.highScoreValue.GetComponent<Text>();
			if (PlayerPrefs.HasKey ("High Score")) {
				Debug.Log ("High Score found");
				scoreText.text = PlayerPrefs.GetInt ("High Score").ToString();
			} else {
				Debug.Log ("High Score not found");
				PlayerPrefs.SetInt ("High Score", 0);
			}
			app.model.screenModel.showHighScore = false;
			app.model.screenModel.highScorePanel.SetActive (false);
		}

		// Update is called once per frame
		void Update () 
		{

		}
		public void ClickHighScore(){
			Debug.Log ("High Score");
			if (app.model.screenModel.showHighScore) {
				app.model.screenModel.highScorePanel.SetActive (false);
				app.model.screenModel.showHighScore = false;
			} else {
				app.model.screenModel.highScorePanel.SetActive (true);
				app.model.screenModel.showHighScore = true;
			}
		}
	}
}
