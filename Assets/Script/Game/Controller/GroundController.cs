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
				Debug.Log (app.model.groundModel.indexSelanjutnyaPrefabDatar.ToString() + " " + gameObject.transform.position.ToString());
			}
		}

		void Update() {
			// menggerakkan tanah
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

		// mengaktifkan ground dari pool dengan ID id_pool
		// prekondisi: ada ground di pool yang tidak aktif
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
			ActivateGround (pool [indexSelanjutnya]);
			if (indexPertama == GroundModel.INDEX_INACTIVE) {
				indexPertama = indexSelanjutnya;
			}
			indexSelanjutnya++;
			if (indexSelanjutnya >= pool.Count) {
				indexSelanjutnya = indexSelanjutnya % pool.Count;
			}
		}

	}
}

