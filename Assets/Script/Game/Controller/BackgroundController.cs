using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class BackgroundController : Element {
		public float speed;
		public float changeOffset1;
		public float changeOffset2;
		public bool area;
		private int change;
		private float offsetx;
		// Use this for initialization
		void Start () {
			change = -1;
			area = false;
			offsetx = 0;
			Vector2 offset = new Vector2 (offsetx, 0);
			GetComponent<Renderer> ().material.mainTextureOffset = offset/150;
		}
		
		// Update is called once per frame
		void Update () { 
			if ((!app.model.screenModel.paused) && (!app.model.screenModel.gameOver)) {
				offsetx = offsetx + app.model.groundModel.baseGroundSpeed / 8250;
				Vector2 offset = new Vector2 (offsetx, 0);
				GetComponent<Renderer> ().material.mainTextureOffset = offset;
				Debug.Log (GetComponent<Renderer> ().material.mainTextureOffset.x);
			}
			float temp = GetComponent<Renderer> ().material.mainTextureOffset.x - Mathf.Floor (GetComponent<Renderer> ().material.mainTextureOffset.x);
			if ((temp > changeOffset1) && (!area)) {
				area = !area; 	
			}
			if ((temp > changeOffset2) && (area)) {
				area = !area;
			}
		}

	}
}
