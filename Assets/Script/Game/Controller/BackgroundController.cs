using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

	public float speed;
	public float changespeed;
	public Light light;
	private int change;
	// Use this for initialization
	void Start () {
		speed = 0.05f;
		light.intensity = 1.5f;
		changespeed = 0.003f;
		change = -1;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 offset = new Vector2 (Time.time * speed, 0);
		GetComponent<Renderer> ().material.mainTextureOffset = offset;

		light.intensity = light.intensity + change * changespeed;
		if ((light.intensity >= 1.5) && (change == 1))  {
			change = -1;
		} else if ((light.intensity <= 0) && (change == -1)) {
			change = 1;
		}
	}
		
}
