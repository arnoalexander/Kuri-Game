using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class ObstacleController : Element
	{

		void Start() {
			// inisialisasi pool halangan
			app.model.obstacleModel.poolObstacleSmall = new List<GameObject>();
			DeactivateObstacle (app.model.obstacleModel.prefabObstacleSmall);
			app.model.obstacleModel.indexPertamaObstacleSmall = ObstacleModel.INDEX_INACTIVE;
			app.model.obstacleModel.indexSelanjutnyaObstacleSmall = 0;
			for (int i = 0; i < app.model.obstacleModel.banyakPoolObstacleSmall; i++) {
				GameObject gameObject = (GameObject)Instantiate (app.model.obstacleModel.prefabObstacleSmall);
				app.model.obstacleModel.poolObstacleSmall.Add (gameObject);
			}

			// inisialisasi waktu antar obstacle
			app.model.obstacleModel.nextObstacleTime = 0.0f;
		}

		void Update() {
			// gerakkan obstacle
			int index_sekarang = app.model.obstacleModel.indexPertamaObstacleSmall;
			if (index_sekarang != ObstacleModel.INDEX_INACTIVE) {
				int index_batas = app.model.obstacleModel.indexSelanjutnyaObstacleSmall;
				float translation_x = -app.model.groundModel.baseGroundSpeed * Time.deltaTime;
				do {
					app.model.obstacleModel.poolObstacleSmall[index_sekarang].GetComponent<Rigidbody2D>().transform.Translate (
						translation_x, 0.0f, 0.0f
					);
					index_sekarang++;
					if (index_sekarang >= app.model.obstacleModel.poolObstacleSmall.Count) {
						index_sekarang = 0;
					}
				} while (index_sekarang != index_batas);
			}

			// menyembunyikan obstacle yang terlalu kiri
			if (app.model.obstacleModel.indexPertamaObstacleSmall != -1) {
				while (app.model.obstacleModel.poolObstacleSmall[app.model.obstacleModel.indexPertamaObstacleSmall].transform.position.x <= app.model.groundModel.xLeft) {
					DeactivateFromPool (ObstacleModel.ID_POOL_OBSTACLE_SMALL);
				}
			}


			// cek waktu kapan menggenerate obstacle baru
			if (Time.time >= app.model.obstacleModel.nextObstacleTime) {
				int tanahPalingKananPrefabIndex = app.controller.groundController.GetPrefabIndex (GroundModel.ID.GROUND_FLAT);
				int tanahPalingKananIndex = app.controller.groundController.GetIndexLastActive (GroundModel.ID.GROUND_FLAT);
				GameObject tanahPalingKanan = app.model.groundModel.pool [tanahPalingKananPrefabIndex] [tanahPalingKananIndex];
				Vector3 lokasiTanahPalingKanan = tanahPalingKanan.transform.position;

				app.model.obstacleModel.nextObstacleTime += 3.0f; //harusnya random waktunya

				int indexObstacleAktif = app.model.obstacleModel.indexSelanjutnyaObstacleSmall; //harusnya random obstacle yang mana
				ActivateFromPool(ObstacleModel.ID_POOL_OBSTACLE_SMALL);
				app.model.obstacleModel.poolObstacleSmall [indexObstacleAktif].transform.position = new Vector3 (
					lokasiTanahPalingKanan.x,
					lokasiTanahPalingKanan.y + app.model.groundModel.boundSize.y,
					lokasiTanahPalingKanan.z
				);
			}
		}

		public void ActivateObstacleSmall (GameObject obstacle) {
			obstacle.SetActive (true);
			obstacle.tag = ObstacleModel.TAG_SMALL;
		}

		public void DeactivateObstacle (GameObject obstacle) {
			obstacle.SetActive (false);
			obstacle.tag = ObstacleModel.TAG_INACTIVE;
		}

		// mengaktifkan obstacle paling kanan dari pool dengan ID id_pool
		public void ActivateFromPool(int id_pool) {
			int indexPertama, indexSelanjutnya;
			switch (id_pool) {
			case ObstacleModel.ID_POOL_OBSTACLE_SMALL:
				indexPertama = app.model.obstacleModel.indexPertamaObstacleSmall;
				indexSelanjutnya = app.model.obstacleModel.indexSelanjutnyaObstacleSmall;
				ActivateFromPool (
					app.model.obstacleModel.poolObstacleSmall,
					ref indexPertama,
					ref indexSelanjutnya
				);
				app.model.obstacleModel.indexPertamaObstacleSmall = indexPertama;
				app.model.obstacleModel.indexSelanjutnyaObstacleSmall = indexSelanjutnya;
				break;
			}
		}
		// fungsi antara ActivateFromPool
		private void ActivateFromPool(List<GameObject> pool, ref int indexPertama, ref int indexSelanjutnya) {
			if (indexPertama != indexSelanjutnya) {
				ActivateObstacleSmall (pool [indexSelanjutnya]);
				if (indexPertama == ObstacleModel.INDEX_INACTIVE) {
					indexPertama = indexSelanjutnya;
				}
				indexSelanjutnya++;
				if (indexSelanjutnya >= pool.Count) {
					indexSelanjutnya = 0;
				}
			}
		}

		// menonaktifkan obstacle paling kiri dari pool dengan ID id_pool
		public void DeactivateFromPool(int id_pool) {
			int indexPertama;
			switch (id_pool) {
			case ObstacleModel.ID_POOL_OBSTACLE_SMALL:
				indexPertama = app.model.obstacleModel.indexPertamaObstacleSmall;
				DeactivateFromPool (
					app.model.obstacleModel.poolObstacleSmall,
					ref indexPertama,
					app.model.obstacleModel.indexSelanjutnyaObstacleSmall
				);
				app.model.obstacleModel.indexPertamaObstacleSmall = indexPertama;
				break;
			}
		}
		// fungsi antara ActivateFromPool
		private void DeactivateFromPool(List<GameObject> pool, ref int indexPertama, int indexSelanjutnya) {
			if (indexPertama != ObstacleModel.INDEX_INACTIVE) {
				DeactivateObstacle (pool [indexPertama]);
				indexPertama++;
				if (indexPertama >= pool.Count) {
					indexPertama = 0;
				}
				if (indexPertama == indexSelanjutnya) {
					indexPertama = ObstacleModel.INDEX_INACTIVE;
				}
			}
		}

	}
}

