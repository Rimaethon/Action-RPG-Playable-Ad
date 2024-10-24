using System.Collections;
using System.Collections.Generic;
using _Scripts.Event_System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Scripts.UI
{
	public class UIWorldSpaceCanvasManager : MonoBehaviour
	{
		[SerializeField]
		private RectTransform bulletCountView;
		[SerializeField]
		private RectTransform damageTextView;
		[SerializeField]
		private GameObject damageTextPrefab;
		[SerializeField]
		private int damageTextPoolSize = 10;
		[SerializeField]
		private Vector3 bulletCountViewOffset = new Vector3(0, 0, 0);
		[SerializeField]
		private Transform player;
		private readonly Queue<GameObject> damageTexts = new Queue<GameObject>();
		private Text bulletCountText;

		private void Awake()
		{
			bulletCountText = bulletCountView.GetComponentInChildren<Text>();

			for (int i = 0; i < damageTextPoolSize; i++)
			{
				damageTexts.Enqueue(Instantiate(damageTextPrefab, transform));
			}
		}

		private void LateUpdate()
		{
			bulletCountView.position = player.position + bulletCountViewOffset;
		}

		private void OnEnable()
		{
			EventManager.Subscribe<EnemyDamagedEventArgs>(ShowDamage);
			EventManager.Subscribe<BulletCountChangedEventArgs>(ChangeBulletCount);
		}

		private void OnDisable()
		{
			EventManager.UnSubscribe<EnemyDamagedEventArgs>(ShowDamage);
			EventManager.UnSubscribe<BulletCountChangedEventArgs>(ChangeBulletCount);
		}

		private void ShowDamage(EnemyDamagedEventArgs data)
		{
			StartCoroutine(DamageTextCoroutine(data.Position, data.Damage));
		}

		private IEnumerator DamageTextCoroutine(Vector3 position, float damage)
		{
			if (damageTexts.Count == 0)
			{
				damageTexts.Enqueue(Instantiate(damageTextPrefab, transform));
			}

			RectTransform textRectTransform = damageTexts.Dequeue().GetComponent<RectTransform>();
			textRectTransform.GetComponent<Text>().text = damage.ToString();
			textRectTransform.position = position;
			textRectTransform.gameObject.SetActive(true);

			float x = Random.Range(-2f, 2f);
			float y = Random.Range(2, 4.5f);
			Vector3 targetPosition = new Vector3(position.x + x, position.y + y, position.z);
			float duration = 1f;
			float elapsedTime = 0f;

			while (elapsedTime < duration)
			{
				textRectTransform.position = Vector3.Lerp(position, targetPosition, elapsedTime / duration);
				elapsedTime += Time.deltaTime;
				yield return null;
			}

			textRectTransform.position = targetPosition;
			textRectTransform.gameObject.SetActive(false);
			damageTexts.Enqueue(textRectTransform.gameObject);
		}

		private void ChangeBulletCount(BulletCountChangedEventArgs bulletCountChangedEventArgs)
		{
			bulletCountText.text = bulletCountChangedEventArgs.bulletCount.ToString();
		}
	}
}
