using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Game{
	public class MenuController : MonoBehaviour {
		// Use this for initialization
		void Start () 
		{

		}

		// Update is called once per frame
		void Update () 
		{

		}
		public void ClickMenu(){
			Debug.Log ("Start bos");
			//SceneManager.LoadScene(1);
			StartCoroutine(LoadYourAsyncScene());
		}
		IEnumerator LoadYourAsyncScene()
		{
			// The Application loads the Scene in the background at the same time as the current Scene.
			//This is particularly good for creating loading screens. You could also load the Scene by build //number.
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);

			//Wait until the last operation fully loads to return anything
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
		}


	}
}
