using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game {
	public class RestartController : Element {
		// Use this for initialization
		void Start () 
		{

		}

		// Update is called once per frame
		void Update () 
		{

		}
		void OnMouseDown(){
		// this object was clicked - do something
		SceneManager.LoadScene("Game");
		}


	}
}