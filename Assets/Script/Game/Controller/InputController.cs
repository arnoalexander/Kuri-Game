using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

namespace Game {
	public class InputController : Element {
		public GameObject smoke;
		public bool issmoke;
		public float coolDown = 5f;
		public float timeStamp;
		Animator anim;

		void Start() {
			app.model.inputModel.isDragging = false;
			app.model.inputModel.tap = false;
			app.model.inputModel.swipeUp = false;
			app.model.inputModel.swipeDown = false;
			app.model.inputModel.swipeLeft = false;
			app.model.inputModel.swipeRight = false;
			timeStamp = Time.time;
			anim = app.model.playerModel.playerGameObject.GetComponent<Animator> ();

		}

		void Update() {
			DetectKeyboardInput (); // for debug purpose
			DetectMouseInput (); // for debug purpose
			DetectSwipeInput ();
		}

		void DetectMouseInput(){
			/*
			if (Input.GetMouseButtonDown (0)) {
				app.controller.weaponController.CreateBall ();
			}
			*/
		}

		IEnumerator SlowDownEnemy()
		{
			print(Time.time);
			issmoke = true;
			float dif = app.model.playerModel.playerGameObject.transform.position.x - app.model.enemyModel.enemyGameObject.transform.position.x;
			if (dif <= 5) {
				float tempspeed = app.model.enemyModel.baseEnemySpeed;
				app.model.enemyModel.baseEnemySpeed = app.model.enemyModel.baseEnemySpeed - app.model.enemyModel.enemySlowSpeed;
				yield return new WaitForSeconds (1);
				print (Time.time);
				app.model.enemyModel.baseEnemySpeed = app.model.enemyModel.baseEnemySpeed + app.model.enemyModel.enemySlowSpeed;
			} else {
				yield return new WaitForSeconds (1);
			}
				
			issmoke = false;
			smoke.SetActive (false);
		}

		void DetectKeyboardInput() {
			if ((Input.GetKeyDown (KeyCode.Space))&&(app.model.playerModel.isJump==false)) {
				app.controller.playerController.Jump ();
			}
			if (Input.GetKeyDown (KeyCode.D)) {
				app.controller.enemyController.Jump ();
			}
			if (Input.GetKeyDown (KeyCode.R)) {
				SceneManager.LoadScene("Game");
			}
			if ((Input.GetKeyDown (KeyCode.M))&&(issmoke==false)&&(app.model.playerModel.isJump==false)) {
				if (timeStamp <= Time.time) {
					smoke.SetActive (true);
					timeStamp = Time.time + coolDown;
					StartCoroutine (SlowDownEnemy ());
				}
			}
			if ((Input.GetKeyDown (KeyCode.S))&&(app.model.playerModel.isJump==false)&&(app.model.playerModel.isSlide==false)) {
				StartCoroutine (Slide());
			}
		}

		IEnumerator Slide()
		{
			print(Time.time);
			app.model.playerModel.isSlide = true;
			anim.SetTrigger ("Slide");
			yield return new WaitForSeconds (0.6f);
			anim.SetTrigger ("SlideFinish");
			app.model.playerModel.isSlide = false;
			smoke.SetActive (false);
		}


		void DetectSwipeInput() {
			// saving last state
			app.model.inputModel.tapPrev = app.model.inputModel.tap;
			app.model.inputModel.swipeUpPrev = app.model.inputModel.swipeUp;
			app.model.inputModel.swipeDownPrev = app.model.inputModel.swipeDown;
			app.model.inputModel.swipeLeftPrev = app.model.inputModel.swipeLeft;
			app.model.inputModel.swipeRightPrev = app.model.inputModel.swipeRight;

			// detect
			app.model.inputModel.tap = false;
			app.model.inputModel.swipeUp = false;
			app.model.inputModel.swipeDown = false;
			app.model.inputModel.swipeLeft = false;
			app.model.inputModel.swipeRight = false;
			if (Input.touches.Length > 0) {
				if (Input.touches [0].phase == TouchPhase.Began) {
					app.model.inputModel.tap = true;
					app.model.inputModel.isDragging = true;
					app.model.inputModel.startTouch = Input.touches [0].position;
				} else if (Input.touches [0].phase == TouchPhase.Canceled || Input.touches [0].phase == TouchPhase.Ended) {
					app.model.inputModel.isDragging = false;
					ResetSwipeInput ();
				}
			}

			// find length
			app.model.inputModel.swipeDelta = Vector2.zero;
			if (app.model.inputModel.isDragging) {
				if (Input.touches.Length > 0) {
					app.model.inputModel.swipeDelta = Input.touches [0].position - app.model.inputModel.startTouch;
				}
			}

			// check threshold & direction
			if (app.model.inputModel.swipeDelta.magnitude > app.model.inputModel.swipeDeltaThreshold) {
				if (Mathf.Abs (app.model.inputModel.swipeDelta.x) > Mathf.Abs (app.model.inputModel.swipeDelta.y)) {
					if (app.model.inputModel.swipeDelta.x < 0) {
						app.model.inputModel.swipeLeft = true;
					} else {
						app.model.inputModel.swipeRight = true;
					}
				} else {
					if (app.model.inputModel.swipeDelta.y < 0) {
						app.model.inputModel.swipeDown = true;
					} else {
						app.model.inputModel.swipeUp = true;
					}
				}
			}

			// processing
			if ((app.model.inputModel.swipeUp)&&(app.model.playerModel.isJump==false)) { // swipe up
				if (!app.model.inputModel.swipeUpPrev) {
					app.controller.playerController.Jump ();
				}
			} 
			if ((app.model.inputModel.swipeLeft)&&(app.model.playerModel.isJump==false)&&(issmoke==false)) { // swipe left
				if (!app.model.inputModel.swipeLeftPrev) {
					if (timeStamp <= Time.time) {
						smoke.SetActive (true);
						timeStamp = Time.time + coolDown;
						StartCoroutine (SlowDownEnemy ());
					}
				}
			} 
			if ((app.model.inputModel.swipeDown)&&(app.model.playerModel.isJump==false)&&(app.model.playerModel.isSlide==false)) {
				if (!app.model.inputModel.swipeDownPrev) {
					StartCoroutine (Slide ());
				}
			}
		}

		void ResetSwipeInput() {
			app.model.inputModel.startTouch = app.model.inputModel.swipeDelta = Vector2.zero;
			app.model.inputModel.isDragging = false;
		}

	}
}

