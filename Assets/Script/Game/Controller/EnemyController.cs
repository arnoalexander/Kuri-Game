using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class EnemyController : Element
	{
		private Rigidbody2D enemyRigidBody;

		void Start() {
			// mendapatkan komponen rigid body dari player
			enemyRigidBody = app.model.enemyModel.enemyGameObject.GetComponent<Rigidbody2D> ();

			// menempatkan pemain sesuai resolusi layar
			app.model.enemyModel.enemyGameObject.transform.position = new Vector3(
				4f * (app.model.enemyModel.enemyWidthPosition - 0.5f) * ((float)Camera.main.pixelWidth / (float)Camera.main.pixelHeight) * Camera.main.orthographicSize,
				app.model.enemyModel.enemyGameObject.transform.position.y,
				app.model.enemyModel.enemyGameObject.transform.position.z
			);

			InvokeRepeating ("IncreaseSpeed", 1f, 15f);
		}

		public void Jump() {
			enemyRigidBody.AddForce(new Vector2(0.0f, app.model.enemyModel.enemyJumpSpeed));
		}

		void Update(){
			float translation_x = app.model.enemyModel.baseEnemySpeed * Time.deltaTime;
			app.model.enemyModel.enemyGameObject.GetComponent<Rigidbody2D>().transform.Translate (
				translation_x, 0.0f, 0.0f
			);
		}

		void IncreaseSpeed(){
			if (app.model.enemyModel.baseEnemySpeed < app.model.enemyModel.maxSpeed) {
				app.model.enemyModel.baseEnemySpeed += app.model.enemyModel.increaseSpeed;
			}
		}

	}

}

