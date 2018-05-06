using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class GroundController : Element {
		public Sprite sprite1;
		public Sprite sprite2;

		void Start() {
			// inisialisasi pool tanah
			app.model.groundModel.pool = new List<GameObject>[app.model.groundModel.prefabs.Length];
			for (int prefabIndex = 0; prefabIndex < app.model.groundModel.prefabs.Length; prefabIndex++) {
				app.model.groundModel.pool [prefabIndex] = new List<GameObject> ();
				DeactivateGround (app.model.groundModel.prefabs [prefabIndex]);
				for (int iter_num = 0; iter_num < app.model.groundModel.poolSizes [prefabIndex]; iter_num++) {
					GameObject groundPrefab = (GameObject)Instantiate (app.model.groundModel.prefabs [prefabIndex]);
					app.model.groundModel.pool [prefabIndex].Add (groundPrefab);
				}
				InvokeRepeating ("IncreaseSpeed", 1f, 15f);
			}

			// inisialisasi index pool tanah
			app.model.groundModel.indexFirstActive = new int[app.model.groundModel.prefabs.Length];
			app.model.groundModel.indexNextActive = new int[app.model.groundModel.prefabs.Length];
			app.model.groundModel.prefabIdToPrefabIndex = new int[System.Enum.GetValues (typeof(GroundModel.ID)).Length];
			for (int prefabIndex = 0; prefabIndex < app.model.groundModel.prefabs.Length; prefabIndex++) {
				app.model.groundModel.indexFirstActive [prefabIndex] = GroundModel.INDEX_INACTIVE;
				app.model.groundModel.indexNextActive [prefabIndex] = 0;
				int prefabId = (int)app.model.groundModel.prefabIds [prefabIndex];
				app.model.groundModel.prefabIdToPrefabIndex [prefabId] = prefabIndex;
			}

			// inisialisasi variabel koordinat dan jarak
			app.model.groundModel.xRight = app.model.groundModel.poolDisplayWidthRatio * Camera.main.orthographicSize * ((float) Camera.main.pixelWidth / (float) Camera.main.pixelHeight);
			app.model.groundModel.xLeft = -app.model.groundModel.xRight;
			app.model.groundModel.yLast = 2.0f * (app.model.groundModel.groundDisplayHeightRatio - 0.5f) * Camera.main.orthographicSize;
			app.model.groundModel.boundSize = app.model.groundModel.prefabs[GetPrefabIndex(GroundModel.ID.GROUND_MIDDLE)].GetComponent<SpriteRenderer>().bounds.size;

			// mengaktifkan tanah di scene awal
			float xCurrent = app.model.groundModel.xLeft - app.model.groundModel.boundSize.x;
			while (xCurrent <= app.model.groundModel.xRight) {
				xCurrent += app.model.groundModel.boundSize.x;
				ActivateFromPool (GroundModel.ID.GROUND_MIDDLE);
				int prefabIndex = GetPrefabIndex (GroundModel.ID.GROUND_MIDDLE);
				int rightmostGroundIndex = GetIndexLastActive (GroundModel.ID.GROUND_MIDDLE);
				GameObject rightmostGround = app.model.groundModel.pool [prefabIndex] [rightmostGroundIndex];
				rightmostGround.transform.position = new Vector3 (
					xCurrent,
					app.model.groundModel.yLast,
					rightmostGround.transform.position.z
				);
			}
		}

		void Update() {
			// menggerakkan tanah
			for (int prefabIndex = 0; prefabIndex < app.model.groundModel.prefabs.Length; prefabIndex++) {
				int indexNow = app.model.groundModel.indexFirstActive [prefabIndex];
				if (indexNow != GroundModel.INDEX_INACTIVE) {
					int indexBorder = app.model.groundModel.indexNextActive [prefabIndex];
					float xTranslation = -app.model.groundModel.baseGroundSpeed * Time.deltaTime;
					do {
						app.model.groundModel.pool[prefabIndex][indexNow].GetComponent<Rigidbody2D>().transform.Translate(
							xTranslation, 0.0f, 0.0f
						);
						indexNow++;
						if (indexNow >= app.model.groundModel.pool[prefabIndex].Count) {
							indexNow = 0;
						}
					} while (indexNow != indexBorder);
				}
			}

			// menyembunyikan tanah yang melewati batas kiri
			for (int prefabIndex = 0; prefabIndex < app.model.groundModel.prefabs.Length; prefabIndex++) {
				GroundModel.ID prefabId = app.model.groundModel.prefabIds [prefabIndex];
				if (GetIndexFirstActive (prefabId) != GroundModel.INDEX_INACTIVE) {
					GameObject leftmostGround = app.model.groundModel.pool [prefabIndex] [GetIndexFirstActive (prefabId)];
					while (leftmostGround.transform.position.x <= app.model.groundModel.xLeft) {
						DeactivateFromPool (prefabId);
						leftmostGround = app.model.groundModel.pool [prefabIndex] [GetIndexFirstActive (prefabId)];
					}
				}
			}
			GameObject rightmostGround= app.model.groundModel.pool [GetPrefabIndex (GroundModel.ID.GROUND_MIDDLE)] [GetIndexLastActive (GroundModel.ID.GROUND_MIDDLE)];
			// menempatkan tanah baru jika tanah paling kanan melewati batas kanan
			/*
			if (app.controller.backgroundController.area == false) {
			 rightmostGround = app.model.groundModel.pool [GetPrefabIndex (GroundModel.ID.GROUND_MIDDLE)] [GetIndexLastActive (GroundModel.ID.GROUND_MIDDLE)];
			} else {
				rightmostGround = app.model.groundModel.pool [GetPrefabIndex (GroundModel.ID.GROUND_LAVA)] [GetIndexLastActive (GroundModel.ID.GROUND_LAVA)];
			}*/
			while ((rightmostGround.transform.position.x <= app.model.groundModel.xRight)) {
				//if (app.controller.backgroundController.area == false) {
					Debug.Log (app.controller.backgroundController.area);
					ActivateFromPool (GroundModel.ID.GROUND_MIDDLE);
					float xNext = rightmostGround.transform.position.x + app.model.groundModel.boundSize.x;
					rightmostGround = app.model.groundModel.pool [GetPrefabIndex (GroundModel.ID.GROUND_MIDDLE)] [GetIndexLastActive (GroundModel.ID.GROUND_MIDDLE)];
					rightmostGround.transform.position = new Vector3 (
						xNext,
						app.model.groundModel.yLast,
						rightmostGround.transform.position.z
					);
				if (app.controller.backgroundController.area) {
					rightmostGround.GetComponent<SpriteRenderer> ().sprite = sprite2;

				} else {
					rightmostGround.GetComponent<SpriteRenderer> ().sprite = sprite1;
				}
				//}

			}
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

		// mendapatkan index tempat prefab ber-id prefabId pada array of prefab
		public int GetPrefabIndex(GroundModel.ID prefabId) {
			return app.model.groundModel.prefabIdToPrefabIndex [(int) prefabId];
		}
			
		// mendapatkan index tempat ground yang sedang aktif, diaktifkan paling awal, dan memiliki prefab id prefabId, pada pool yang sesuai
		public int GetIndexFirstActive(GroundModel.ID prefabId) {
			int prefabIndex = GetPrefabIndex (prefabId);
			return app.model.groundModel.indexFirstActive [prefabIndex];
		}

		// mendapatkan index tempat ground yang sedang aktif, diaktifkan paling awal, dan memiliki prefab id prefabId, pada pool yang sesuai
		public int GetIndexLastActive(GroundModel.ID prefabId) {
			int prefabIndex = GetPrefabIndex (prefabId);
			if (app.model.groundModel.indexFirstActive [prefabIndex] == GroundModel.INDEX_INACTIVE)
				return GroundModel.INDEX_INACTIVE;
			int result = app.model.groundModel.indexNextActive [prefabIndex] - 1;
			if (result < 0)
				result = app.model.groundModel.pool [prefabIndex].Count - 1;
			return result;
		}

		// mengaktifkan ground selanjutnya dari pool untuk prefab dengan ID prefabId
		public void ActivateFromPool(GroundModel.ID prefabId) {
			int prefabIndex = GetPrefabIndex (prefabId);
			if (app.model.groundModel.indexFirstActive [prefabIndex] != app.model.groundModel.indexNextActive [prefabIndex]) {
				ActivateGround (app.model.groundModel.pool [prefabIndex][app.model.groundModel.indexNextActive [prefabIndex]]);
				if (app.model.groundModel.indexFirstActive [prefabIndex] == GroundModel.INDEX_INACTIVE) {
					app.model.groundModel.indexFirstActive [prefabIndex] = app.model.groundModel.indexNextActive [prefabIndex];
				}
				app.model.groundModel.indexNextActive [prefabIndex]++;
				if (app.model.groundModel.indexNextActive [prefabIndex] >= app.model.groundModel.pool [prefabIndex].Count) {
					app.model.groundModel.indexNextActive [prefabIndex] = 0;
				}
			}
		}

		// menonaktifkan ground pertama yang aktif dari pool untuk prefab dengan ID prefabId
		public void DeactivateFromPool(GroundModel.ID prefabId) {
			int prefabIndex = GetPrefabIndex (prefabId);
			if (app.model.groundModel.indexFirstActive [prefabIndex] != GroundModel.INDEX_INACTIVE) {
				DeactivateGround (app.model.groundModel.pool [prefabIndex][app.model.groundModel.indexFirstActive [prefabIndex]]);
				app.model.groundModel.indexFirstActive [prefabIndex]++;
				if (app.model.groundModel.indexFirstActive [prefabIndex] >= app.model.groundModel.pool [prefabIndex].Count) {
					app.model.groundModel.indexFirstActive [prefabIndex] = 0;
				}
				if (app.model.groundModel.indexFirstActive [prefabIndex] == app.model.groundModel.indexNextActive [prefabIndex]) {
					app.model.groundModel.indexFirstActive [prefabIndex] = GroundModel.INDEX_INACTIVE;
				}
			}
		}

		void IncreaseSpeed(){
			if (app.model.groundModel.baseGroundSpeed < app.model.groundModel.maxSpeed) {
				app.model.groundModel.baseGroundSpeed += app.model.groundModel.increaseSpeed;
			}
		}

	}
}

