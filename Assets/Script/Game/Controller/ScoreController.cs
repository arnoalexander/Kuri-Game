using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Game {
	public class ScoreController : Element {

		private Text scoreText;
		private string text;
		public int score =0;
		void Start() {
			// mendapatkan komponen rigid body dari player
			scoreText = app.model.scoreModel.scoreText.GetComponent<Text>();
			scoreText.text= score.ToString();
		}

		void Update() {
			score++;
			scoreText.text = score.ToString();
		}

	}
}

