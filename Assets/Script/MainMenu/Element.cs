using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMenu {
	// Kelas dasar untuk semua elemen
	public class Element : MonoBehaviour {
		private Application application;

		public Application app {
			get {
				if (!application) {
					application = GameObject.FindObjectOfType<Application>();
				}
				return application;
			}
		}
	}
}