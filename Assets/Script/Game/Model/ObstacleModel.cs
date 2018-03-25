using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class ObstacleModel : Element
	{
		public enum ID {
			OBSTACLE_SMALL_SLIME
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
		public GameObject[] prefabs; // array dari prefab yang digunakan untuk rintangan
		public ID[] prefabIds; // berisi id dari prefab yang menjadi elemen array prefabs
		public int[] poolSizes; // berisi pool size dari prefab yang menjadi elemen array prefabs

		// pool halangan
		public List<GameObject>[] pool { get; set; }

		// tag untuk obstacle
		public const string TAG_INACTIVE = "Obstacle.Inactive"; // obstacle tidak aktif
		public const string TAG_ACTIVE = "Obstacle.Active"; // obstacle aktif

		// indeks dan konstanta pool objek tanah
		public const int INDEX_INACTIVE = -1; // indeks pertama jika tidak ada obstacle dari jenis tertentu yang aktif
		public int[] indexFirstActive { get; set; } // indeks untuk elemen yang sedang aktif dan diaktifkan paling dahulu
		public int[] indexNextActive { get; set; } // indeks untuk elemen yang tidak aktif dan siap diaktifkan selanjutnya
		public int[] prefabIdToPrefabIndex { get; set; } // memetakan id prefab ke indeks prefab

		// waktu random keluarnya obstacle
		public float nextObstacleTime { get; set;} // waktu antar obstacle
	}
}

