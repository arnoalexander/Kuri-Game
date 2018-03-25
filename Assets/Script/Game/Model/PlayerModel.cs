using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class PlayerModel : Element {

		public GameObject playerGameObject; // player yang ingin dikontrol
		public float playerJumpSpeed; // kekuatan lompatan pemain
		public bool isJump;
		public float playerWidthPosition; // posisi pemain terhadap lebar layar. 0 untuk sisi kiri layar, 1 untuk sisi kanan layar


	}
}
