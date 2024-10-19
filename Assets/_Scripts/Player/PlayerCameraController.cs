using System.Collections;
using _Scripts.Event_System;
using UnityEngine;

namespace _Scripts.Player
{
	public class PlayerCameraController : MonoBehaviour
	{
		private const float start_rotation = 40;
		[SerializeField]
		private Vector3 offset = new Vector3(0, 16, -16);
		[SerializeField]
		private float cameraShakeDuration = 0.5f;
		[SerializeField]
		private Transform player;

		private void Awake()
		{
			transform.rotation = Quaternion.Euler(start_rotation, 0, 0);
		}

		private void LateUpdate()
		{
			Vector3 desiredPosition = player.position + offset;
			transform.position = desiredPosition;
		}

		private void OnEnable()
		{
			EventManager.Subscribe<PlayerDamagedEventArgs>(ImpactShake);
		}

		private void OnDisable()
		{
			EventManager.UnSubscribe<PlayerDamagedEventArgs>(ImpactShake);
		}

		//This was normally DoTween but I didn't want to increase the build size just for this so I made a simple shake function
		private void ImpactShake(PlayerDamagedEventArgs args)
		{
			StopAllCoroutines();
			StartCoroutine(ShakePosition());
			StartCoroutine(ShakeRotation());
		}

		private IEnumerator ShakePosition()
		{
			Vector3 originalPosition = transform.position;
			float elapsed = 0.0f;

			while (elapsed < cameraShakeDuration)
			{
				float x = Random.Range(-1f, 1f) * 0.1f;
				float y = Random.Range(-1f, 1f) * 0.1f;

				transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

				elapsed += Time.deltaTime;
				yield return null;
			}

			transform.position = originalPosition;
		}

		private IEnumerator ShakeRotation()
		{
			Quaternion originalRotation = transform.rotation;
			float elapsed = 0.0f;

			while (elapsed < cameraShakeDuration)
			{
				float z = Random.Range(-1f, 1f) * 1f;

				transform.rotation = Quaternion.Euler(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y, originalRotation.eulerAngles.z + z);

				elapsed += Time.deltaTime;
				yield return null;
			}

			transform.rotation = originalRotation;
		}
	}
}
