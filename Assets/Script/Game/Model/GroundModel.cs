using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class GroundModel : Element {

		public enum ID {
			GROUND_BOTTOM,
			GROUND_FLAT
		}

		void OnValidate() {
			if (prefabs.Length != poolSizes.Length) {
				int[] newPoolSize = new int[prefabs.Length];
				for (int i = 0; i < prefabs.Length; i++) {
					if (i >= poolSizes.Length)
						break;
					newPoolSize [i] = poolSizes [i];
				}
				poolSizes = newPoolSize;
			}
			if (prefabs.Length != prefabIds.Length) {
				ID[] newPrefabIds = new ID[prefabs.Length];
				for (int i = 0; i < prefabs.Length; i++) {
					if (i >= prefabIds.Length)
						break;
					newPrefabIds [i] = prefabIds [i];
				}
				prefabIds = newPrefabIds;
			}
		}

		// VARIABEL BEBAS
		public float baseGroundSpeed; // kecepatan awal gerakan tanah, default = 2.7
		public float poolDisplayWidthRatio; // rasio antara jarak x_right-x_left dengan lebar layar
		public float groundDisplayHeightRatio; // rasio antara jarak pijakan ke bawah layar dengan tinggi layar
		public GameObject[] prefabs; // array dari prefab yang digunakan untuk ground
		public ID[] prefabIds; // berisi id dari prefab yang menjadi elemen array prefabs
		public int[] poolSizes; // berisi pool size dari prefab yang menjadi elemen array prefabs

		// tag untuk ground
		public const string TAG_INACTIVE = "Ground.Inactive"; // ground tidak aktif (tidak ditampilkan)
		public const string TAG_ACTIVE = "Ground"; // ground aktif (ditampilkan)

		// pool objek tanah
		public List<GameObject>[] pool { get; set; }

		// indeks dan konstanta pool objek tanah
		public const int INDEX_INACTIVE = -1; // indeks jika tidak ada ground dari jenis tertentu yang aktif
		public int[] indexFirstActive { get; set; } // indeks untuk elemen yang sedang aktif dan diaktifkan paling dahulu
		public int[] indexNextActive { get; set; } // indeks untuk elemen yang tidak aktif dan siap diaktifkan selanjutnya
		public int[] prefabIdToPrefabIndex { get; set; } // memetakan id prefab ke indeks prefab

		// koordinat dan jarak
		public float xLeft { get; set;} // jika absis sebuah ground lebih kiri dari ini, maka ground tersebut dinonaktifkan
		public float xRight { get; set;} // jika absis dari ground paling kanan yang aktif lebih kiri dari ini, maka ground dari pool akan diaktifkan dan diletakkan di paling kanan
		public float yLast { get; set;} // ketinggian dari ground paling kanan
		public Vector3 boundSize { get; set;} // jarak antara sebuah kotak ground dengan kotak ground yang lain.

	}
}
