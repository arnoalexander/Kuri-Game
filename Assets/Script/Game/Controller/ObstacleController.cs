using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class ObstacleController : Element
	{

		void Start() {
			// inisialisasi pool halangan
			app.model.obstacleModel.pool = new List<GameObject>[app.model.obstacleModel.prefabs.Length];
			for (int prefabIndex = 0; prefabIndex < app.model.obstacleModel.prefabs.Length; prefabIndex++) {
				app.model.obstacleModel.pool [prefabIndex] = new List<GameObject> ();
				DeactivateObstacle (app.model.obstacleModel.prefabs [prefabIndex]);
				for (int iter_num = 0; iter_num < app.model.obstacleModel.poolSizes [prefabIndex]; iter_num++) {
					GameObject obstaclePrefab = (GameObject)Instantiate (app.model.obstacleModel.prefabs [prefabIndex]);
					app.model.obstacleModel.pool [prefabIndex].Add (obstaclePrefab);
				}
			}

			// inisialisasi index pool halangan
			app.model.obstacleModel.indexFirstActive = new int[app.model.obstacleModel.prefabs.Length];
			app.model.obstacleModel.indexNextActive = new int[app.model.obstacleModel.prefabs.Length];
			app.model.obstacleModel.boundSizes = new Vector3[app.model.obstacleModel.prefabs.Length];
			app.model.obstacleModel.prefabIdToPrefabIndex = new int[System.Enum.GetValues (typeof(ObstacleModel.ID)).Length];
			for (int prefabIndex = 0; prefabIndex < app.model.obstacleModel.prefabs.Length; prefabIndex++) {
				app.model.obstacleModel.indexFirstActive [prefabIndex] = ObstacleModel.INDEX_INACTIVE;
				app.model.obstacleModel.indexNextActive [prefabIndex] = 0;
				app.model.obstacleModel.boundSizes [prefabIndex] = app.model.obstacleModel.prefabs [prefabIndex].GetComponent<SpriteRenderer> ().bounds.size;
				int prefabId = (int)app.model.obstacleModel.prefabIds [prefabIndex];
				app.model.obstacleModel.prefabIdToPrefabIndex [prefabId] = prefabIndex;
			}

			// inisialisasi waktu antar obstacle
			app.model.obstacleModel.nextObstacleTime = 0.0f;
		}

		void Update() {
			// gerakkan obstacle
			for (int prefabIndex = 0; prefabIndex < app.model.obstacleModel.prefabs.Length; prefabIndex++) {
				int indexNow = app.model.obstacleModel.indexFirstActive [prefabIndex];
				if (indexNow != ObstacleModel.INDEX_INACTIVE) {
					int indexBorder = app.model.obstacleModel.indexNextActive [prefabIndex];
					float xTranslation = -app.model.groundModel.baseGroundSpeed * Time.deltaTime;
					do {
						app.model.obstacleModel.pool[prefabIndex][indexNow].GetComponent<Rigidbody2D>().transform.Translate(
							xTranslation, 0.0f, 0.0f
						);
						indexNow++;
						if (indexNow >= app.model.obstacleModel.pool[prefabIndex].Count) {
							indexNow = 0;
						}
					} while (indexNow != indexBorder);
				}
			}

			// menyembunyikan obstacle yang terlalu kiri
			for (int prefabIndex = 0; prefabIndex < app.model.obstacleModel.prefabs.Length; prefabIndex++) {
				ObstacleModel.ID prefabId = app.model.obstacleModel.prefabIds [prefabIndex];
				if (GetIndexFirstActive (prefabId) != ObstacleModel.INDEX_INACTIVE) {
					GameObject leftmostObstacle = app.model.obstacleModel.pool [prefabIndex] [GetIndexFirstActive (prefabId)];
					while (leftmostObstacle.transform.position.x <= app.model.groundModel.xLeft) {
						DeactivateFromPool (prefabId);
						leftmostObstacle = app.model.obstacleModel.pool [prefabIndex] [GetIndexFirstActive (prefabId)];
					}
				}
			}
				
			// cek waktu kapan menggenerate obstacle baru
			if (Time.time >= app.model.obstacleModel.nextObstacleTime) {
				int rightmostGroundPrefabIndex = app.controller.groundController.GetPrefabIndex (GroundModel.ID.GROUND_MIDDLE);
				int rightmostGroundPoolIndex = app.controller.groundController.GetIndexLastActive (GroundModel.ID.GROUND_MIDDLE);
				GameObject rightmostGround = app.model.groundModel.pool [rightmostGroundPrefabIndex] [rightmostGroundPoolIndex];
				Vector3 rightmostGroundPosition = rightmostGround.transform.position;

				app.model.obstacleModel.nextObstacleTime += 3.0f; // TODO: harusnya random

				ObstacleModel.ID nextObstacleID = ObstacleModel.ID.OBSTACLE_GROUND_SPIKE; // TODO: seharusnya random
				ActivateFromPool(nextObstacleID);

				// pemosisian
				GameObject rightmostObstacle = app.model.obstacleModel.pool[GetPrefabIndex(nextObstacleID)][GetIndexLastActive(nextObstacleID)];
				if (nextObstacleID == ObstacleModel.ID.OBSTACLE_GROUND_SPIKE) {
					rightmostObstacle.transform.position = new Vector3 (
						app.model.groundModel.xRight,
						rightmostGroundPosition.y + (-app.model.groundModel.boundSize.y + app.model.obstacleModel.boundSizes[GetPrefabIndex(nextObstacleID)].y) * 0.5f,
						rightmostObstacle.transform.position.z
					);
				} else {
					rightmostObstacle.transform.position = new Vector3 (
						app.model.groundModel.xRight,
						rightmostGroundPosition.y + (app.model.groundModel.boundSize.y + app.model.obstacleModel.boundSizes[GetPrefabIndex(nextObstacleID)].y) * 0.5f,
						rightmostObstacle.transform.position.z
					);
				}

			}
		}

		public void ActivateObstacle (GameObject obstacle) {
			obstacle.SetActive (true);
			obstacle.tag = ObstacleModel.TAG_ACTIVE;
		}

		public void DeactivateObstacle (GameObject obstacle) {
			obstacle.SetActive (false);
			obstacle.tag = ObstacleModel.TAG_INACTIVE;
		}

		// mendapatkan index tempat prefab ber-id prefabId pada array of prefab
		public int GetPrefabIndex(ObstacleModel.ID prefabId) {
			return app.model.obstacleModel.prefabIdToPrefabIndex [(int) prefabId];
		}

		// mendapatkan index tempat obstacle yang sedang aktif, diaktifkan paling awal, dan memiliki prefab id prefabId, pada pool yang sesuai
		public int GetIndexFirstActive(ObstacleModel.ID prefabId) {
			int prefabIndex = GetPrefabIndex (prefabId);
			return app.model.obstacleModel.indexFirstActive [prefabIndex];
		}

		// mendapatkan index tempat obstacle yang sedang aktif, diaktifkan paling awal, dan memiliki prefab id prefabId, pada pool yang sesuai
		public int GetIndexLastActive(ObstacleModel.ID prefabId) {
			int prefabIndex = GetPrefabIndex (prefabId);
			if (app.model.obstacleModel.indexFirstActive [prefabIndex] == ObstacleModel.INDEX_INACTIVE)
				return ObstacleModel.INDEX_INACTIVE;
			int result = app.model.obstacleModel.indexNextActive [prefabIndex] - 1;
			if (result < 0)
				result = app.model.obstacleModel.pool [prefabIndex].Count - 1;
			return result;
		}

		// mengaktifkan obstacle selanjutnya dari pool untuk prefab dengan ID prefabId
		public void ActivateFromPool(ObstacleModel.ID prefabId) {
			int prefabIndex = GetPrefabIndex (prefabId);
			if (app.model.obstacleModel.indexFirstActive [prefabIndex] != app.model.obstacleModel.indexNextActive [prefabIndex]) {
				ActivateObstacle (app.model.obstacleModel.pool [prefabIndex][app.model.obstacleModel.indexNextActive [prefabIndex]]);
				if (app.model.obstacleModel.indexFirstActive [prefabIndex] == ObstacleModel.INDEX_INACTIVE) {
					app.model.obstacleModel.indexFirstActive [prefabIndex] = app.model.obstacleModel.indexNextActive [prefabIndex];
				}
				app.model.obstacleModel.indexNextActive [prefabIndex]++;
				if (app.model.obstacleModel.indexNextActive [prefabIndex] >= app.model.obstacleModel.pool [prefabIndex].Count) {
					app.model.obstacleModel.indexNextActive [prefabIndex] = 0;
				}
			}
		}

		// menonaktifkan obstacle pertama yang aktif dari pool untuk prefab dengan ID prefabId
		public void DeactivateFromPool(ObstacleModel.ID prefabId) {
			int prefabIndex = GetPrefabIndex (prefabId);
			if (app.model.obstacleModel.indexFirstActive [prefabIndex] != ObstacleModel.INDEX_INACTIVE) {
				DeactivateObstacle (app.model.obstacleModel.pool [prefabIndex][app.model.obstacleModel.indexFirstActive [prefabIndex]]);
				app.model.obstacleModel.indexFirstActive [prefabIndex]++;
				if (app.model.obstacleModel.indexFirstActive [prefabIndex] >= app.model.obstacleModel.pool [prefabIndex].Count) {
					app.model.obstacleModel.indexFirstActive [prefabIndex] = 0;
				}
				if (app.model.obstacleModel.indexFirstActive [prefabIndex] == app.model.obstacleModel.indexNextActive [prefabIndex]) {
					app.model.obstacleModel.indexFirstActive [prefabIndex] = ObstacleModel.INDEX_INACTIVE;
				}
			}
		}

	}
}

