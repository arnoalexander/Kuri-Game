using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class GroundController : Element {

		void Start() {
			// inisialisasi pool tanah
			app.model.groundModel.poolPrefabBawah = new List<GameObject>();
			app.model.groundModel.poolPrefabDatar = new List<GameObject>();
			DeactivateGround (app.model.groundModel.prefabBawah);
			DeactivateGround (app.model.groundModel.prefabDatar);
			app.model.groundModel.indexPertamaPrefabBawah = GroundModel.INDEX_INACTIVE;
			app.model.groundModel.indexPertamaPrefabDatar = GroundModel.INDEX_INACTIVE;
			app.model.groundModel.indexSelanjutnyaPrefabBawah = 0;
			app.model.groundModel.indexSelanjutnyaPrefabDatar = 0;
			for (int i = 0; i < app.model.groundModel.banyakPoolPrefabBawah; i++) {
				GameObject gameObject = (GameObject)Instantiate (app.model.groundModel.prefabBawah);
				app.model.groundModel.poolPrefabBawah.Add (gameObject);
			}
			for (int i = 0; i < app.model.groundModel.banyakPoolPrefabDatar; i++) {
				GameObject gameObject = (GameObject)Instantiate (app.model.groundModel.prefabDatar);
				app.model.groundModel.poolPrefabDatar.Add (gameObject);
			}

			// inisialisasi variabel koordinat dan jarak
			app.model.groundModel.xRight = app.model.groundModel.poolDisplayWidthRatio * Camera.main.orthographicSize * ((float) Camera.main.pixelWidth / (float) Camera.main.pixelHeight);
			app.model.groundModel.xLeft = -app.model.groundModel.xRight;
			app.model.groundModel.yLast = 2.0f * (app.model.groundModel.groundDisplayHeightRatio - 0.5f) * Camera.main.orthographicSize;
			app.model.groundModel.jarakTetangga = app.model.groundModel.prefabBawah.GetComponent<SpriteRenderer>().bounds.size;

			// mengaktifkan tanah di scene awal
			float xCurrent = app.model.groundModel.xLeft - app.model.groundModel.jarakTetangga.x;
			while (xCurrent <= app.model.groundModel.xRight) {
				xCurrent += app.model.groundModel.jarakTetangga.x;
				ActivateFromPool (GroundModel.ID_POOL_PREFAB_DATAR);
				GameObject gameObject = GetLastActiveFromPool (GroundModel.ID_POOL_PREFAB_DATAR);
				gameObject.transform.position = new Vector3 (
					xCurrent,
					app.model.groundModel.yLast,
					gameObject.transform.position.z
				);
			}
		}

		void Update() {
			// menggerakkan tanah
			int index_sekarang = app.model.groundModel.indexPertamaPrefabDatar;
			if (index_sekarang != GroundModel.INDEX_INACTIVE) {
				int index_batas = app.model.groundModel.indexSelanjutnyaPrefabDatar;
				float translation_x = -app.model.groundModel.baseGroundSpeed * Time.deltaTime;
				do {
					app.model.groundModel.poolPrefabDatar[index_sekarang].GetComponent<Rigidbody2D>().transform.Translate (
						translation_x, 0.0f, 0.0f
					);
					index_sekarang++;
					if (index_sekarang >= app.model.groundModel.poolPrefabDatar.Count) {
						index_sekarang = 0;
					}
				} while (index_sekarang != index_batas);
			}

			// menyembunyikan tanah yang melewati batas kiri
			while (GetFirstActiveFromPool (GroundModel.ID_POOL_PREFAB_DATAR).transform.position.x <= app.model.groundModel.xLeft) {
				DeactivateFromPool (GroundModel.ID_POOL_PREFAB_DATAR);
			}

			// menempatkan tanah baru jika tanah paling kanan melewati batas kanan
			GameObject tanah_paling_kanan = GetLastActiveFromPool (GroundModel.ID_POOL_PREFAB_DATAR);
			while (tanah_paling_kanan.transform.position.x <= app.model.groundModel.xRight) {
				ActivateFromPool (GroundModel.ID_POOL_PREFAB_DATAR);
				float xCurrent = tanah_paling_kanan.transform.position.x + app.model.groundModel.jarakTetangga.x;
				tanah_paling_kanan = GetLastActiveFromPool (GroundModel.ID_POOL_PREFAB_DATAR);
				tanah_paling_kanan.transform.position = new Vector3 (
					xCurrent,
					app.model.groundModel.yLast,
					tanah_paling_kanan.transform.position.z
				);
			}

			/*
			foreach (Rigidbody2D rigidbody in groundRigidbodyList) {
				rigidbody.transform.Translate (-app.model.groundModel.groundSpeed, 0.0f, 0.0f);
			}
			*/
		}

		// mengaktifkan ground
		public void ActivateGround(GameObject ground) {
			ground.SetActive (true);
			ground.tag = GroundModel.TAG_ACTIVE;
		}

		// menonaktifkan ground
		public void DeactivateGround(GameObject ground) {
			ground.SetActive (false);
			ground.tag = GroundModel.TAG_INACTIVE;
		}

		// mendapatkan ground aktif pertama (paling kiri) dari pool dengan ID id_pool
		public GameObject GetFirstActiveFromPool(int id_pool) {
			switch (id_pool) {
			case GroundModel.ID_POOL_PREFAB_BAWAH:
				return GetFirstActiveFromPool (
					app.model.groundModel.poolPrefabBawah,
					app.model.groundModel.indexPertamaPrefabBawah
				);
				break;
			case GroundModel.ID_POOL_PREFAB_DATAR:
				return GetFirstActiveFromPool (
					app.model.groundModel.poolPrefabDatar,
					app.model.groundModel.indexPertamaPrefabDatar
				);
				break;
			}
			return null;
		}
		// fungsi antara GetFirstActiveFromPool
		private GameObject GetFirstActiveFromPool(List<GameObject> pool, int indexPertama) {
			if (indexPertama == GroundModel.INDEX_INACTIVE)
				return null;
			return pool [indexPertama];
		}

		// mendapatkan ground aktif terakhir (paling kanan) dari pool dengan ID id_pool
		public GameObject GetLastActiveFromPool(int id_pool) {
			switch (id_pool) {
			case GroundModel.ID_POOL_PREFAB_BAWAH:
				return GetLastActiveFromPool (
					app.model.groundModel.poolPrefabBawah,
					app.model.groundModel.indexPertamaPrefabBawah,
					app.model.groundModel.indexSelanjutnyaPrefabBawah
				);
				break;
			case GroundModel.ID_POOL_PREFAB_DATAR:
				return GetLastActiveFromPool (
					app.model.groundModel.poolPrefabDatar,
					app.model.groundModel.indexPertamaPrefabDatar,
					app.model.groundModel.indexSelanjutnyaPrefabDatar
				);
				break;
			}
			return null;
		}
		// fungsi antara GetLastActiveFromPool
		private GameObject GetLastActiveFromPool(List<GameObject> pool, int indexPertama, int indexSelanjutnya) {
			if (indexPertama == GroundModel.INDEX_INACTIVE)
				return null;
			int last_index = indexSelanjutnya - 1;
			if (last_index < 0)
				last_index += pool.Count;
			return pool [last_index];
		}

		// mengaktifkan ground paling kanan dari pool dengan ID id_pool
		public void ActivateFromPool(int id_pool) {
			int indexPertama, indexSelanjutnya;
			switch (id_pool) {
			case GroundModel.ID_POOL_PREFAB_BAWAH:
				indexPertama = app.model.groundModel.indexPertamaPrefabBawah;
				indexSelanjutnya = app.model.groundModel.indexSelanjutnyaPrefabBawah;
				ActivateFromPool (
					app.model.groundModel.poolPrefabBawah,
					ref indexPertama,
					ref indexSelanjutnya
				);
				app.model.groundModel.indexPertamaPrefabBawah = indexPertama;
				app.model.groundModel.indexSelanjutnyaPrefabBawah = indexSelanjutnya;
				break;
			case GroundModel.ID_POOL_PREFAB_DATAR:
				indexPertama = app.model.groundModel.indexPertamaPrefabDatar;
				indexSelanjutnya = app.model.groundModel.indexSelanjutnyaPrefabDatar;
				ActivateFromPool (
					app.model.groundModel.poolPrefabDatar,
					ref indexPertama,
					ref indexSelanjutnya
				);
				app.model.groundModel.indexPertamaPrefabDatar = indexPertama;
				app.model.groundModel.indexSelanjutnyaPrefabDatar = indexSelanjutnya;
				break;
			}
		}
		// fungsi antara ActivateFromPool
		private void ActivateFromPool(List<GameObject> pool, ref int indexPertama, ref int indexSelanjutnya) {
			if (indexPertama != indexSelanjutnya) {
				ActivateGround (pool [indexSelanjutnya]);
				Debug.Log ("[GROUND] Activated, Index: " + indexSelanjutnya.ToString ());
				if (indexPertama == GroundModel.INDEX_INACTIVE) {
					indexPertama = indexSelanjutnya;
				}
				indexSelanjutnya++;
				if (indexSelanjutnya >= pool.Count) {
					indexSelanjutnya = 0;
				}
			}
		}

		// menonaktifkan ground paling kiri dari pool dengan ID id_pool
		public void DeactivateFromPool(int id_pool) {
			int indexPertama;
			switch (id_pool) {
			case GroundModel.ID_POOL_PREFAB_BAWAH:
				indexPertama = app.model.groundModel.indexPertamaPrefabBawah;
				DeactivateFromPool (
					app.model.groundModel.poolPrefabBawah,
					ref indexPertama,
					app.model.groundModel.indexSelanjutnyaPrefabBawah
				);
				app.model.groundModel.indexPertamaPrefabBawah = indexPertama;
				break;
			case GroundModel.ID_POOL_PREFAB_DATAR:
				indexPertama = app.model.groundModel.indexPertamaPrefabDatar;
				DeactivateFromPool (
					app.model.groundModel.poolPrefabDatar,
					ref indexPertama,
					app.model.groundModel.indexSelanjutnyaPrefabDatar
				);
				app.model.groundModel.indexPertamaPrefabDatar = indexPertama;
				break;
			}
		}
		// fungsi antara ActivateFromPool
		private void DeactivateFromPool(List<GameObject> pool, ref int indexPertama, int indexSelanjutnya) {
			if (indexPertama != GroundModel.INDEX_INACTIVE) {
				DeactivateGround (pool [indexPertama]);
				indexPertama++;
				if (indexPertama >= pool.Count) {
					indexPertama = 0;
				}
				if (indexPertama == indexSelanjutnya) {
					indexPertama = GroundModel.INDEX_INACTIVE;
				}
			}
		}

	}
}

