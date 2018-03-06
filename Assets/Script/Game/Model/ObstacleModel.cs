using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class ObstacleModel : Element
	{

		// variabel bebas
		public int banyakPoolObstacleSmall;
		public GameObject prefabObstacleSmall;

		// pool halangan
		public List<GameObject> poolObstacleSmall { get; set;}

		// tag untuk obstacle
		public const string TAG_INACTIVE = "Obstacle.Inactive"; // obstacle tidak aktif
		public const string TAG_SMALL = "Obstacle.Small"; // obstacle kecil aktif

		// indeks dan konstanta pool objek tanah
		public const int INDEX_INACTIVE = -1; // indeks pertama jika tidak ada obstacle dari jenis tertentu yang aktif
		public const int ID_POOL_OBSTACLE_SMALL = 1; // id pool prefab obstacle kecil
		public int indexPertamaObstacleSmall { get; set;} // indeks paling awal dari elemen pool obstacle kecil yang aktif. -1 jika tidak ada yang aktif
		public int indexSelanjutnyaObstacleSmall { get; set;} // indeks elemen pool obstacle kecil yang siap untuk diaktifkan selanjutnya

		// waktu random keluarnya obstacle
		public float nextObstacleTime { get; set;} // waktu antar obstacle
	}
}

