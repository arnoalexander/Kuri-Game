using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class ScreenController : Element {

		void Start() {
			// Mengubah orientasi layar menjadi landscape
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			Time.timeScale = 1;
			app.model.screenModel.gameOverPanel.SetActive (false);
			app.model.screenModel.gameOver = false;
			app.model.screenModel.pauseButton.SetActive (true);
			app.model.screenModel.pausePanel.SetActive (false);
			app.model.screenModel.paused = false;
		}

	}
}
