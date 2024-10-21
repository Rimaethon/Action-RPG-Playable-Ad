using _Scripts.Event_System;
using _Scripts.Player;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Managers
{
	public class InputManager : MonoBehaviour
	{
		[SerializeField]
		private Joystick _joystick;
		[SerializeField]
		private PlayerController playerController;

		private void FixedUpdate()
		{
			if (playerController == null)
			{
				return;
			}

			playerController.moveDirection = new Vector3(_joystick.movementInput.x, 0, _joystick.movementInput.y);
		}

		private void OnEnable()
		{
			EventManager.Subscribe<LevelEndEventArgs>(HandlePlayerDeath);
		}

		private void OnDisable()
		{
			EventManager.UnSubscribe<LevelEndEventArgs>(HandlePlayerDeath);
		}

		private void HandlePlayerDeath(LevelEndEventArgs data)
		{
			_joystick.gameObject.SetActive(false);
		}
	}
}
