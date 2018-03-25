using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class PlayerCollider : Element
	{
		IEnumerator AccelerateEnemy()
		{
			print(Time.time);
			float tempspeed = app.model.enemyModel.baseEnemySpeed;
			app.model.enemyModel.baseEnemySpeed = app.model.enemyModel.baseEnemySpeed+app.model.enemyModel.enemyAccelSpeed;
			yield return new WaitForSeconds(1);
			print(Time.time);
			app.model.enemyModel.baseEnemySpeed = app.model.enemyModel.baseEnemySpeed-app.model.enemyModel.enemyAccelSpeed;
		}
		void OnTriggerEnter2D(Collider2D other) {
			if(other.gameObject.CompareTag(ObstacleModel.TAG_ACTIVE)) {
				app.controller.obstacleController.DeactivateObstacle (other.gameObject);
				StartCoroutine (AccelerateEnemy ());
			}
		}


		void OnCollisionEnter2D(Collision2D other) {
			if (other.gameObject.tag == "Ground") {
				app.model.playerModel.isJump = false;
				Debug.Log ("hit");
			}

		}

		void onTriggerExit2D(Collider2D other){
			if (other.gameObject.tag == "Ground") {
				app.model.playerModel.isJump = true;
				Debug.Log ("jump");
			}
		}
	}
}