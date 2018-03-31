using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class BackgroundController : Element {
		public float speed;
		public float changespeed;
		public Light light;
		private int change;
		// Use this for initialization
		void Start () {
			light.intensity = 1.5f;
			change = -1;
		}
		
		// Update is called once per frame
		void Update () {
			if ((!app.model.screenModel.paused) && (!app.model.screenModel.gameOver)) {
				Vector2 offset = new Vector2 (Time.time * speed, 0);
				GetComponent<Renderer> ().material.mainTextureOffset = offset;
				light.intensity = light.intensity + change * changespeed;
					if ((light.intensity >= 1.5) && (change == 1)) {
						change = -1;
					} else if ((light.intensity <= 0) && (change == -1)) {
						change = 1;
					}
			}
		}
		
	}
}
