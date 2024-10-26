using _Scripts.AI;
using _Scripts.Object_Pool;
using _Scripts.Player;
using Object_Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Spawners
{
	public class EnemySpawner : MonoBehaviour
	{
		[SerializeField]
		private GameObject enemyPrefab;
		[SerializeField]
		private float spawnRadius = 20f;
		[SerializeField]
		private float spawnCooldown = 1f;
		[SerializeField]
		private Transform playerTransform;
		private float counter;
		private EnemyDetector enemyDetector;
		private Camera mainCamera;
		private ObjectPool objectPool;

		private void Start()
		{
			mainCamera = Camera.main;
			counter = spawnCooldown;
			enemyDetector = playerTransform.GetComponentInChildren<EnemyDetector>();
			objectPool = ObjectPoolManager.CreateInstance(enemyPrefab.GetComponent<PoolAbleObject>(), 50);
			SpawnEnemy();
		}

		private void FixedUpdate()
		{
			counter -= Time.fixedDeltaTime;
			if (!(counter <= 0)) return;
			counter = spawnCooldown;
			SpawnEnemy();
		}

		private void SpawnEnemy()
		{
			if (!TryGetSpawnPoint(out Vector3 spawnPoint)) return;
			PoolAbleObject poolAbleObject = objectPool.GetObject(spawnPoint, Quaternion.identity);
			poolAbleObject.GetComponent<BaseAIAgent>().playerTransform = playerTransform;
			poolAbleObject.GetComponent<AgentHealth>().EnemyDetector = enemyDetector;
		}

		private bool TryGetSpawnPoint(out Vector3 spawnPoint)
		{
			Vector3 playerPosition = playerTransform.position;

			for (int i = 0; i < 30; i++)
			{
				Vector3 randomPoint = playerPosition + Random.insideUnitSphere * spawnRadius;
				randomPoint.y = playerPosition.y;
				if (!IsValidSpawnPoint(randomPoint)) continue;
				spawnPoint = randomPoint;
				return true;
			}

			spawnPoint = Vector3.zero;
			return false;
		}

		private bool IsValidSpawnPoint(Vector3 point)
		{
			Vector3 viewportPoint = mainCamera.WorldToViewportPoint(point);
			return viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1 || viewportPoint.z <= 0;
		}
	}
}
