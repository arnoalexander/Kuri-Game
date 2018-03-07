using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class PlayerCollider : Element
	{
		void OnTriggerEnter2D(Collider2D other) {
			if(other.gameObject.CompareTag(ObstacleModel.TAG_SMALL)) {
				app.controller.obstacleController.DeactivateObstacle (other.gameObject);
				app.model.enemyModel.enemyGameObject.GetComponent<Rigidbody2D>().transform.Translate (
					app.model.enemyModel.obstacleTranslation, 0.0f, 0.0f
				);
			}
		}
	}
}