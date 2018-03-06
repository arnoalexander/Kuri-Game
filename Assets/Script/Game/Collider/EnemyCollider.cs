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
				Debug.Log ("Game Over");
			}

		}
	}
}

