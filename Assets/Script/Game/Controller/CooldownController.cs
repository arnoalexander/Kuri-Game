using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
	public class CooldownController : Element {

		private float cooldown; 
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (app.controller.inputController.timeStamp <= Time.time) {
				cooldown = 0f;
			} else {
				cooldown = app.controller.inputController.timeStamp - Time.time;
			}
			app.model.cooldownModel.cooldownText.text = cooldown.ToString("F1");
		}
	}
}
