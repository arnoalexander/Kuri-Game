using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class GroundController : Element {
		
		private GameObject[] activeGround;
		private Rigidbody2D groundRigidbody;
		private List<Rigidbody2D> groundRigidbodyList;

		void Start() {
			groundRigidbodyList = new List<Rigidbody2D> ();
			activeGround = GameObject.FindGameObjectsWithTag ("Ground");
			foreach (GameObject ground in activeGround) {
				groundRigidbody = ground.GetComponent<Rigidbody2D> ();
				groundRigidbodyList.Add (groundRigidbody);
			}
		}

		void Update() {
			foreach (Rigidbody2D rigidbody in groundRigidbodyList) {
				rigidbody.transform.Translate (-app.model.groundModel.groundSpeed, 0.0f, 0.0f);
			}
		}

	}
}

