using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class PlayerController : Element {

		private Rigidbody2D playerRigidBody;

		void Start() {
			playerRigidBody = app.model.playerModel.playerGameObject.GetComponent<Rigidbody2D> ();
		}

		public void Jump() {
			playerRigidBody.AddForce(new Vector2(0.0f, app.model.playerModel.playerJumpSpeed));
		}

	}
}

