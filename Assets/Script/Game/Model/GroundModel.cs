using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class GroundModel : Element {

		// variabel bebas
		public float baseGroundSpeed; // kecepatan awal gerakan tanah, default = 0.06
		public float poolDisplayWidthRatio; // rasio antara jarak x_right-x_left dengan lebar layar
		public float groundDisplayHeightRatio; // rasio antara jarak pijakan ke bawah layar dengan tinggi layar
		public GameObject prefabDatar; // prefab untuk tanah pijakan yang datar
		public GameObject prefabBawah; // prefab untuk bawah tanah
		public int banyakPoolPrefabDatar; // kapasitas pool prefab datar
		public int banyakPoolPrefabBawah; // kapasitas pool prefab bawah

		// tag untuk ground
		public const string TAG_INACTIVE = "Ground.Inactive"; // ground tidak aktif (tidak ditampilkan)
		public const string TAG_ACTIVE = "Ground"; // ground aktif (ditampilkan)

		// pool objek tanah
		public List<GameObject> poolPrefabDatar { get; set;}
		public List<GameObject> poolPrefabBawah { get; set;}

		// indeks dan konstanta pool objek tanah
		public const int INDEX_INACTIVE = -1; // indeks pertama jika tidak ada ground dari jenis tertentu yang aktif
		public const int ID_POOL_PREFAB_DATAR = 1; // id pool prefab datar
		public const int ID_POOL_PREFAB_BAWAH = 2; // id pool prefab bawah
		public int indexPertamaPrefabDatar { get; set;} // indeks paling awal dari elemen pool prefab datar yang aktif. -1 jika tidak ada yang aktif
		public int indexSelanjutnyaPrefabDatar { get; set;} // indeks elemen pool prefab datar yang siap untuk diaktifkan selanjutnya
		public int indexPertamaPrefabBawah { get; set;} // indeks paling awal dari elemen pool prefab bawah yang aktif. -1 jika tidak ada yang aktif
		public int indexSelanjutnyaPrefabBawah { get; set;} // indeks elemen pool prefab bawah yang siap untuk diaktifkan selanjutnya

		// koordinat dan jarak
		public float xLeft { get; set;} // jika absis sebuah ground lebih kiri dari ini, maka ground tersebut dinonaktifkan
		public float xRight { get; set;} // jika absis dari ground paling kanan yang aktif lebih kiri dari ini, maka ground dari pool akan diaktifkan dan diletakkan di paling kanan
		public float yLast { get; set;} // ketinggian dari ground paling kanan
		public Vector3 jarakTetangga { get; set;} // jarak antara sebuah kotak ground dengan kotak ground yang lain.

	}
}
