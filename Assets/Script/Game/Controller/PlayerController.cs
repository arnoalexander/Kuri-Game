using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class PlayerController : Element {

		private Rigidbody2D playerRigidBody;

		void Start() {
			// mendapatkan komponen rigid body dari player
			playerRigidBody = app.model.playerModel.playerGameObject.GetComponent<Rigidbody2D> ();

			// menempatkan pemain sesuai resolusi layar
			app.model.playerModel.playerGameObject.transform.position = new Vector3(
				2.0f * (app.model.playerModel.playerWidthPosition - 0.5f) * ((float)Camera.main.pixelWidth / (float)Camera.main.pixelHeight) * Camera.main.orthographicSize,
				app.model.playerModel.playerGameObject.transform.position.y,
				app.model.playerModel.playerGameObject.transform.position.z
			);
		}

		public void Jump() {
			playerRigidBody.AddForce(new Vector2(0.0f, app.model.playerModel.playerJumpSpeed));
		}

	}
}

