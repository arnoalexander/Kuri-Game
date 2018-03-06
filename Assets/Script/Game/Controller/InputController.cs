using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

namespace Game {
	public class InputController : Element {

		private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown, prevSwipeLeft, prevSwipeRight, prevSwipeUp, prevSwipeDown, isDragging;
		private Vector2 startTouch, swipeDelta;

		void Start() {
			isDragging = swipeUp = swipeDown = swipeLeft = swipeRight = false;
		}

		void Update() {
			DetectKeyboardInput ();
			DetectSwipeInput ();
		}

		void DetectKeyboardInput() {
			if (Input.GetKeyDown (KeyCode.Space)) {
				app.controller.playerController.Jump ();
			}
			if (Input.GetKeyDown (KeyCode.D)) {
				app.controller.enemyController.Jump ();
			}
			if (Input.GetKeyDown (KeyCode.R)) {
				SceneManager.LoadScene("Game");
			}
		}

		void DetectSwipeInput() {
			// saving last state
			prevSwipeLeft = swipeLeft;
			prevSwipeRight = swipeRight;
			prevSwipeUp = swipeUp;
			prevSwipeDown = swipeDown;

			// detect
			tap = swipeUp = swipeDown = swipeLeft = swipeRight = false;
			if (Input.touches.Length > 0) {
				if (Input.touches [0].phase == TouchPhase.Began) {
					tap = true;
					isDragging = true;
					startTouch = Input.touches [0].position;
				} else if (Input.touches [0].phase == TouchPhase.Canceled || Input.touches [0].phase == TouchPhase.Ended) {
					isDragging = false;
					ResetSwipeInput ();
				}
			}

			// find length
			swipeDelta = Vector2.zero;
			if (isDragging) {
				if (Input.touches.Length > 0) {
					swipeDelta = Input.touches [0].position - startTouch;
				}
			}

			// check threshold & direction
			if (swipeDelta.magnitude > app.model.inputModel.swipeDeltaThreshold) {
				if (Mathf.Abs (swipeDelta.x) > Mathf.Abs (swipeDelta.y)) {
					if (swipeDelta.x < 0) {
						swipeLeft = true;
					} else {
						swipeRight = true;
					}
				} else {
					if (swipeDelta.y < 0) {
						swipeDown = true;
					} else {
						swipeUp = true;
					}
				}
			}

			// processing
			if (swipeUp) {
				if (!prevSwipeUp) {
					app.controller.playerController.Jump ();
				}
			}
		}

		void ResetSwipeInput() {
			startTouch = swipeDelta = Vector2.zero;
			isDragging = false;
		}

	}
}

