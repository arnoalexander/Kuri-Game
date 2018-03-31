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
			app.model.enemyModel.baseEnemySpeed = app.model.enemyModel.baseEnemySpeed-app.model.enemyModel.enemyAccelSpeed;
		}

		void OnTriggerEnter2D(Collider2D other) {
			if(other.gameObject.CompareTag(ObstacleModel.TAG_ACTIVE)) {
				app.controller.obstacleController.DeactivateObstacle (other.gameObject);
				StartCoroutine (AccelerateEnemy ());
			}
		}


		void OnCollisionEnter2D(Collision2D other) {
			if (other.gameObject.tag == GroundModel.TAG_ACTIVE) {
				app.model.playerModel.isJump = false;
				Debug.Log ("[PLAYER] On Ground");
			}

		}

		void onTriggerExit2D(Collider2D other){
			if (other.gameObject.tag == GroundModel.TAG_ACTIVE) {
				app.model.playerModel.isJump = true;
				Debug.Log ("[PLAYER] Jump");
			}
		}
	}
}