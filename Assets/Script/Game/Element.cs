using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	// Kelas dasar untuk semua elemen
	public class Element : MonoBehaviour {
		public Application app {get {return GameObject.FindObjectOfType<Application>(); }}
	}
}