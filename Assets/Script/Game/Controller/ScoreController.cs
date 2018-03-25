using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Game {
	public class ScoreController : Element {

		public Text scoreText;
		public int score;
		void Start() {
			// mendapatkan komponen rigid body dari player
			scoreText.text = score.ToString();
		}

		void Update() {
				score++;
				scoreText.text = score.ToString();
		}

	}
}

