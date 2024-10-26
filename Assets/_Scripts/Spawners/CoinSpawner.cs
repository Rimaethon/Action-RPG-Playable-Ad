using System.Collections;
using _Scripts.Event_System;
using _Scripts.Object_Pool;
using _Scripts.Player;
using Object_Pool;
using UnityEngine;

namespace _Scripts.Spawners
{
	//Also it can give different things based on the enemy type in the future like health, ammo etc.That would make it more like a DropSpawner class
	public class CoinSpawner : MonoBehaviour
	{
		private const float max_speed = 10;
		private const float coin_start_y = 0.75f;
		[SerializeField]
		private GameObject coinPrefab;
		[SerializeField]
		private AnimationCurve coinSpeedCurve;
		private readonly WaitForSeconds animationWaitForSeconds = new WaitForSeconds(3.5f);
		private ObjectPool coinPool;
		private Transform playerTransform;

		private void Awake()
		{
			coinPool = ObjectPoolManager.CreateInstance(coinPrefab.GetComponent<PoolAbleObject>(), 50);
			playerTransform = FindObjectOfType<PlayerController>().transform;
		}

		private void OnEnable()
		{
			EventManager.Subscribe<EnemyDamagedEventArgs>(SpawnCoin);
		}

		private void OnDisable()
		{
			EventManager.UnSubscribe<EnemyDamagedEventArgs>(SpawnCoin);
		}

		private void SpawnCoin(EnemyDamagedEventArgs data)
		{
			if (!data.isDead)
			{
				return;
			}

			PoolAbleObject coin = coinPool.GetObject(new Vector3(data.Position.x, coin_start_y, data.Position.z), Quaternion.identity);
			coin.transform.rotation = Quaternion.Euler(90, 0, 0);
			StartCoroutine(CoinAnimation(playerTransform, coin));
		}

		private IEnumerator CoinAnimation(Transform playerTransform, PoolAbleObject coin)
		{
			Vector3 coinPosition = coin.transform.position;
			yield return animationWaitForSeconds;

			float time = Time.deltaTime;

			while (Vector3.Distance(coinPosition, playerTransform.position) > 0.2f)
			{
				float speed = coinSpeedCurve.Evaluate(time) * max_speed;
				time += Time.deltaTime;
				coinPosition = Vector3.MoveTowards(coin.transform.position, playerTransform.position, speed);
				coin.transform.position = coinPosition;
				yield return null;
			}

			coin.gameObject.SetActive(false);
		}
	}
}
