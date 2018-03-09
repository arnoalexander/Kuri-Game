using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class GroundModel : Element {

		// variabel bebas
		public float baseGroundSpeed; // kecepatan awal gerakan tanah, default = 2.7
		public float poolDisplayWidthRatio; // rasio antara jarak x_right-x_left dengan lebar layar
		public float groundDisplayHeightRatio; // rasio antara jarak pijakan ke bawah layar dengan tinggi layar
		public GameObject[] prefabs;
		public int poolSize;

		// tag untuk ground
		public const string TAG_INACTIVE = "Ground.Inactive"; // ground tidak aktif (tidak ditampilkan)
		public const string TAG_ACTIVE = "Ground"; // ground aktif (ditampilkan)

		// list dari komponen prefabs
		public List<Rigidbody2D> prefabsRigidbody { get; set; }
		public List<Collider2D> prefabsCollider { get; set; }
		public List<Sprite> prefabsSprite { get; set; }

		// pool objek tanah
		public List<GameObject> pool { get; set;}
		public List<Rigidbody2D> poolRigidbody { get; set; }
		public List<Collider2D> poolCollider { get; set; }
		public List<Sprite> poolSprite { get; set; }

		// indeks dan konstanta pool objek tanah
		public const int INDEX_INACTIVE = -1; // indeks pertama jika tidak ada ground dari jenis tertentu yang aktif
		public const int ID_GROUND_FLAT = 0; // id prefab tanah datar, sesuaikan dengan index prefab datar pada array prefabs
		public const int ID_GROUND_UNDER = 1; // id prefab tanah bawah, sesuaikan dengan index prefab bawah pada array prefabs
		public int indexFirstActive { get; set; }
		public int indexNextActive { get; set; }

		// koordinat dan jarak
		public float xLeft { get; set;} // jika absis sebuah ground lebih kiri dari ini, maka ground tersebut dinonaktifkan
		public float xRight { get; set;} // jika absis dari ground paling kanan yang aktif lebih kiri dari ini, maka ground dari pool akan diaktifkan dan diletakkan di paling kanan
		public float yLast { get; set;} // ketinggian dari ground paling terakhir diaktifkan
		public Vector3 boundSize { get; set;} // jarak antara sebuah kotak ground dengan kotak ground yang lain.

	}
}
