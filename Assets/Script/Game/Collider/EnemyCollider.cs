using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class EnemyCollider : Element
	{
		void OnCollisionEnter2D(Collision2D other) {
			if(other.gameObject.tag=="Player"){
				app.model.enemyModel.baseEnemySpeed = 0;
				Time.timeScale = 0;
				app.model.screenModel.gameOverPanel.SetActive (true);
				app.model.screenModel.gameOver = true;
				int score;
				score = app.controller.scoreController.score;
				if (score > PlayerPrefs.GetInt ("High Score")) {
					PlayerPrefs.SetInt ("High Score", score);
					app.model.screenModel.newHighScoreText.SetActive (true);
				} else {
					app.model.screenModel.newHighScoreText.SetActive (false);
				}
				Debug.Log ("Game Over");
			}

		}
			
		void OnTriggerEnter2D(Collider2D other) {
			if(other.gameObject.CompareTag(ObstacleModel.TAG_ACTIVE)) {
				app.controller.obstacleController.DeactivateObstacle (other.gameObject);
			}
		}
	}
}

