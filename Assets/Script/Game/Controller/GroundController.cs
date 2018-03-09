using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class GroundController : Element {

		void Start() {
			// inisialisasi pool komponen prefab
			app.model.groundModel.prefabsRigidbody = new List<Rigidbody2D>();
			app.model.groundModel.prefabsCollider = new List<Collider2D>();
			app.model.groundModel.prefabsSprite = new List<Sprite>();
			for (int i = 0; i < app.model.groundModel.prefabs.Length; i++) {
				app.model.groundModel.prefabsRigidbody.Add (app.model.groundModel.prefabs [i].GetComponent<Rigidbody2D> ());
				app.model.groundModel.prefabsCollider.Add (app.model.groundModel.prefabs [i].GetComponent<Collider2D> ());
				app.model.groundModel.prefabsSprite.Add (app.model.groundModel.prefabs [i].GetComponent<Sprite> ());

			}

			// inisialisasi pool tanah
			app.model.groundModel.prefabs [0].SetActive (false);
			app.model.groundModel.prefabs [0].tag = GroundModel.TAG_INACTIVE;
			app.model.groundModel.pool = new List<GameObject>();
			app.model.groundModel.poolRigidbody = new List<Rigidbody2D>();
			app.model.groundModel.poolCollider = new List<Collider2D>();
			app.model.groundModel.poolSprite = new List<Sprite>();
			app.model.groundModel.indexFirstActive = GroundModel.INDEX_INACTIVE;
			app.model.groundModel.indexNextActive = 0;
			for (int i = 0; i < app.model.groundModel.poolSize; i++) {
				GameObject groundObject = (GameObject)Instantiate (app.model.groundModel.prefabs[0]);
				app.model.groundModel.pool.Add (groundObject);
				app.model.groundModel.poolRigidbody.Add (groundObject.GetComponent<Rigidbody2D> ());
				app.model.groundModel.poolCollider.Add (groundObject.GetComponent<Collider2D> ());
				app.model.groundModel.poolSprite.Add (groundObject.GetComponent<Sprite> ());
			}

			// inisialisasi variabel koordinat dan jarak
			app.model.groundModel.xRight = app.model.groundModel.poolDisplayWidthRatio * Camera.main.orthographicSize * ((float) Camera.main.pixelWidth / (float) Camera.main.pixelHeight);
			app.model.groundModel.xLeft = -app.model.groundModel.xRight;
			app.model.groundModel.yLast = 2.0f * (app.model.groundModel.groundDisplayHeightRatio - 0.5f) * Camera.main.orthographicSize;
			app.model.groundModel.boundSize = app.model.groundModel.prefabs[0].GetComponent<SpriteRenderer>().bounds.size;

			// mengaktifkan tanah di scene awal
			float xCurrent = app.model.groundModel.xLeft - app.model.groundModel.boundSize.x;
			while (xCurrent <= app.model.groundModel.xRight) {
				xCurrent += app.model.groundModel.boundSize.x;
				ActivateFromPool (GroundModel.ID_GROUND_FLAT);
				GameObject gameObject = GetLastActive();
				gameObject.transform.position = new Vector3 (
					xCurrent,
					app.model.groundModel.yLast,
					gameObject.transform.position.z
				);
			}
		}

		void Update() {
			// menggerakkan tanah
			int index_now = app.model.groundModel.indexFirstActive;
			if (index_now != GroundModel.INDEX_INACTIVE) {
				int index_border = app.model.groundModel.indexNextActive;
				float translation_x = -app.model.groundModel.baseGroundSpeed * Time.deltaTime;
				do {
					app.model.groundModel.pool[index_now].GetComponent<Rigidbody2D>().transform.Translate (
						translation_x, 0.0f, 0.0f
					);
					index_now++;
					if (index_now >= app.model.groundModel.pool.Count) {
						index_now = 0;
					}
				} while (index_now != index_border);
			}

			// menyembunyikan tanah yang melewati batas kiri
			while (GetFirstActive().transform.position.x <= app.model.groundModel.xLeft) {
				DeactivateFromPool ();
			}

			// menempatkan tanah baru jika tanah paling kanan melewati batas kanan
			GameObject rightmost_ground = GetLastActive();
			while (rightmost_ground.transform.position.x <= app.model.groundModel.xRight) {
				ActivateFromPool (GroundModel.ID_GROUND_FLAT);
				float xNext = rightmost_ground.transform.position.x + app.model.groundModel.boundSize.x;
				rightmost_ground = GetLastActive();
				rightmost_ground.transform.position = new Vector3 (
					xNext,
					app.model.groundModel.yLast,
					rightmost_ground.transform.position.z
				);
			}
		}

		// mengaktifkan ground
		public void ActivateGround(int poolIndex, int groundId) {
			app.model.groundModel.pool [poolIndex].SetActive (true);
			app.model.groundModel.pool [poolIndex].tag = GroundModel.TAG_ACTIVE;
			app.model.groundModel.poolRigidbody [poolIndex] = app.model.groundModel.prefabsRigidbody [groundId];
			app.model.groundModel.poolCollider [poolIndex] = app.model.groundModel.prefabsCollider [groundId];
			app.model.groundModel.poolSprite [poolIndex] = app.model.groundModel.prefabsSprite [groundId];

		}

		// menonaktifkan ground
		public void DeactivateGround(int poolIndex) {
			app.model.groundModel.pool [poolIndex].SetActive (false);
			app.model.groundModel.pool [poolIndex].tag = GroundModel.TAG_INACTIVE;
		}

		// mendapatkan indeks dari ground aktif pertama (paling kiri)
		public int GetIndexFirstActive() {
			return app.model.groundModel.indexFirstActive;
		}

		// mengembalikan ground aktif pertama (paling kiri)
		public GameObject GetFirstActive() {
			return app.model.groundModel.pool [GetIndexFirstActive ()];
		}

		// mendapatkan indeks dari ground aktif terakhir (paling kanan)
		public int GetIndexLastActive() {
			if (app.model.groundModel.indexFirstActive == -1) {
				return -1;
			}
			int result = app.model.groundModel.indexNextActive - 1;
			if (result < 0) {
				result = app.model.groundModel.pool.Count - 1;
			}
			return result;
		}

		// mengembalikan ground aktif terakhir (paling kanan)
		public GameObject GetLastActive() {
			return app.model.groundModel.pool [GetIndexLastActive ()];
		}

		// mengaktifkan ground selanjutnya sebagai ground bertipe groundId
		public void ActivateFromPool(int groundId) {
			ActivateGround (app.model.groundModel.indexNextActive, groundId);
			if (app.model.groundModel.indexFirstActive == GroundModel.INDEX_INACTIVE) {
				app.model.groundModel.indexFirstActive = app.model.groundModel.indexNextActive;
			}
			app.model.groundModel.indexNextActive++;
			if (app.model.groundModel.indexNextActive >= app.model.groundModel.pool.Count) {
				app.model.groundModel.indexNextActive = 0;
			}
		}

		// menonaktifkan ground paling kiri
		public void DeactivateFromPool() {
			if (app.model.groundModel.indexFirstActive != GroundModel.INDEX_INACTIVE) {
				DeactivateGround (app.model.groundModel.indexFirstActive);
				app.model.groundModel.indexFirstActive++;
				if (app.model.groundModel.indexFirstActive >= app.model.groundModel.pool.Count) {
					app.model.groundModel.indexFirstActive = 0;
				}
				if (app.model.groundModel.indexFirstActive == app.model.groundModel.indexNextActive) {
					app.model.groundModel.indexFirstActive = GroundModel.INDEX_INACTIVE;
				}
			}
		}

	}
}

