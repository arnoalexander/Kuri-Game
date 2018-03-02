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
				2.5f * (app.model.enemyModel.enemyWidthPosition - 0.5f) * ((float)Camera.main.pixelWidth / (float)Camera.main.pixelHeight) * Camera.main.orthographicSize,
				app.model.enemyModel.enemyGameObject.transform.position.y,
				app.model.enemyModel.enemyGameObject.transform.position.z
			);
		}
		public void Jump() {
			enemyRigidBody.AddForce(new Vector2(0.0f, app.model.enemyModel.enemyJumpSpeed));
		}
	}

}

