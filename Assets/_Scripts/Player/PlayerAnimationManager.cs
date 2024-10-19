using System.Collections;
using _Scripts.Event_System;
using UnityEngine;

namespace _Scripts.Player
{
	//Most of the code is written in a specific way because of Luna Plugin constraints since there is no blend tree and parameter caching
	public class PlayerAnimationManager : MonoBehaviour
	{
		private readonly Vector2 moveDown = new Vector2(0, -1);
		private readonly Vector2 moveUp = new Vector2(0, 1);
		private Animator animator;
		private string currentTrigger = "Idle";
		private PlayerController playerController;

		private void Awake()
		{
			animator = GetComponent<Animator>();
			playerController = GetComponent<PlayerController>();
		}

		private void Update()
		{
			UpdateAnimation();
		}

		private void OnEnable()
		{
			EventManager.Subscribe<LevelEndEventArgs>(HandleDeathAnimation);
		}

		private void OnDisable()
		{
			EventManager.UnSubscribe<LevelEndEventArgs>(HandleDeathAnimation);
		}

		private void HandleDeathAnimation(LevelEndEventArgs obj)
		{
			if (obj.isLevelCompleted)
			{
				return;
			}

			animator.Play("Death");
		}

		public void PlayRifleMediumShot(float fireRate)
		{
			StartCoroutine(PlayMediumShot(fireRate));
		}

		public void PlayReloadAnimation(string animationName)
		{
			StartCoroutine(PlayRifleReloadAnimationCoroutine(animationName));
		}

		private void UpdateAnimation()
		{
			Vector3 moveDirection = transform.InverseTransformDirection(playerController.moveDirection);

			if (moveDirection.magnitude == 0)
			{
				SetAnimationTrigger("Idle");
				return;
			}

			if (!playerController.hasTarget)
			{
				SetAnimationTrigger(moveDirection.magnitude != 0 ? "MoveUp" : "Idle");
				return;
			}

			if (Mathf.Abs(playerController.TargetYRotation - transform.eulerAngles.y) > 5f)
			{
				return;
			}

			//I don't want it to change the animation so often
			if (moveDirection.magnitude == 0 && currentTrigger != "Idle")
			{
				if (moveDirection.magnitude == 0)
				{
					SetAnimationTrigger("Idle");
				}

				return;
			}

			Vector2 moveDirection2D = new Vector2(moveDirection.x, moveDirection.z);

			float distanceMoveUp = Vector2.Distance(moveDirection2D, moveUp);
			float distanceMoveDown = Vector2.Distance(moveDirection2D, moveDown);

			if (distanceMoveDown < distanceMoveUp)
			{
				SetAnimationTrigger("MoveDown");
			}
			else
			{
				SetAnimationTrigger("MoveUp");
			}
		}

		private void SetAnimationTrigger(string newTrigger)
		{
			if (newTrigger == currentTrigger) return;
			animator.SetBool(currentTrigger, false);
			animator.SetBool(newTrigger, true);
			currentTrigger = newTrigger;
		}

		private IEnumerator PlayRifleReloadAnimationCoroutine(string animationName)
		{
			animator.SetBool("RifleMediumShot", false);
			animator.SetBool(animationName, true);
			yield return new WaitForSeconds(3);
			animator.SetBool("RifleMediumShot", true);
			animator.SetBool(animationName, false);
		}

		private IEnumerator PlayMediumShot(float fireRate)
		{
			animator.SetBool("RifleMediumShot", true);
			animator.SetBool("RifleReload", false);
			yield return new WaitForSeconds(fireRate);
			animator.SetBool("RifleMediumShot", false);
		}
	}
}
