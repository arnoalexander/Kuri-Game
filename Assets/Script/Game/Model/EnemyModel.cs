using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class EnemyModel : Element
	{
		public GameObject enemyGameObject; // player yang ingin dikontrol
		public float enemyJumpSpeed; // kekuatan lompatan pemain
		public float enemyWidthPosition; // posisi pemain terhadap lebar layar. 0 untuk sisi kiri layar, 1 untuk sisi kanan layar

	}
}

