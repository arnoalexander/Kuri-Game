using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Game {
	public class ScreenModel : Element {
		public GameObject gameOverPanel;
		public GameObject pauseButton;
		public GameObject pausePanel;
		public Text gameOverText;
		public GameObject newHighScoreText;
		public bool gameOver;
		public bool paused;
	}
}
