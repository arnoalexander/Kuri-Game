using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

	public float speed;
	// Use this for initialization
	void Start () {
		speed = 0.05f;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 offset = new Vector2 (Time.time * speed, 0);
		GetComponent<Renderer> ().material.mainTextureOffset = offset;
	}
}
