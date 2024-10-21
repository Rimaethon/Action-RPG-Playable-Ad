using _Scripts.Data;
using _Scripts.Event_System;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Managers
{
	public class LevelManager : MonoBehaviour
	{
		[SerializeField]
		private UIManager uiManager;
		private LevelProgressData levelProgressData;
		private float secondCounter = 1;

		private void Awake()
		{
			levelProgressData = new LevelProgressData
				(120,
				 0,
				 0,
				 0,
				 1, 500, 5);

			if (uiManager == null)
			{
				uiManager = FindObjectOfType<UIManager>();
			}

			uiManager.InitializeUI(levelProgressData);
		}

		private void OnEnable()
		{
			EventManager.Subscribe<EnemyDamagedEventArgs>(HandleEnemyKilled);
			EventManager.Subscribe<PlayerDamagedEventArgs>(HandlePlayerDamaged);
		}


		private void OnDisable()
		{
			EventManager.UnSubscribe<EnemyDamagedEventArgs>(HandleEnemyKilled);
			EventManager.UnSubscribe<PlayerDamagedEventArgs>(HandlePlayerDamaged);
		}

		private void Update()
		{
			secondCounter -= Time.deltaTime;
			if (!(secondCounter <= 0)) return;
			OnTimeUpdate();
			secondCounter = 1;
		}


		private void HandleEnemyKilled(EnemyDamagedEventArgs data)
		{
			if (!data.isDead)
				return;

			levelProgressData.killCount++;
			levelProgressData.coinAmount += 10;
			levelProgressData.experience += 10;

			if (levelProgressData.experience >= levelProgressData.experienceToNextLevel)
			{
				levelProgressData.currentLevel++;
				levelProgressData.experience -= levelProgressData.experienceToNextLevel;
				levelProgressData.experienceToNextLevel *= 2;
			}
			uiManager.UpdateUI(levelProgressData);
		}

		private void HandlePlayerDamaged(PlayerDamagedEventArgs damageData)
		{
			levelProgressData.playerHealth -= damageData.Damage;
			uiManager.UpdateUI(levelProgressData);
			if (levelProgressData.playerHealth > 0) return;

			LevelEndEventArgs levelEndEventArgs = new LevelEndEventArgs
			{
				isLevelCompleted = false,
				remainingTime=levelProgressData.remainingTime
			};

			EventManager.RaiseEvent(levelEndEventArgs);
		}

		private void OnTimeUpdate()
		{
			if (levelProgressData.remainingTime < 0)
				return;

			levelProgressData.remainingTime--;
			uiManager.UpdateUI(levelProgressData);
			if (levelProgressData.remainingTime != 0)
				return;

			LevelEndEventArgs levelEndEventArgs = new LevelEndEventArgs
			{
				isLevelCompleted = true,
				remainingTime=levelProgressData.remainingTime
			};
			EventManager.RaiseEvent(levelEndEventArgs);
		}
	}
}
