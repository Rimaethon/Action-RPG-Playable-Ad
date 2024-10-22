using System;
using System.Collections.Generic;
using _Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField]
		private Image levelSlider;
		[SerializeField]
		private Text levelSliderText;
		[SerializeField]
		private Text levelText;
		[SerializeField]
		private Text killCountText;
		[SerializeField]
		private Text remainingTimeText;
		[SerializeField]
		private Text coinAmountText;
		[SerializeField]
		private RectTransform healthBar;
		[SerializeField]
		private GameObject heartPrefab;
		private readonly List<Image> playerHealthViews = new List<Image>();
		private int disabledHealthViewCount;

		public void InitializeUI(LevelProgressData data)
		{
			InitializeHealthBar(data.playerHealth);
			UpdateUI(data);
		}

		public void UpdateUI(LevelProgressData data)
		{
			killCountText.text = data.killCount.ToString();
			coinAmountText.text = data.coinAmount.ToString();
			UpdateSlider(data.experienceToNextLevel, data.experience, data.currentLevel);
			TimeSpan timeSpan = TimeSpan.FromSeconds(data.remainingTime);
			remainingTimeText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
			UpdateHealthBar(data.playerHealth);
		}

		private void UpdateHealthBar(int playerHealth)
		{
			if (playerHealthViews.Count - disabledHealthViewCount == playerHealth)
			{
				return;
			}

			for (int i = 0; i < playerHealth; i++)
			{
				if (disabledHealthViewCount >= playerHealthViews.Count)
				{
					return;
				}

				playerHealthViews[disabledHealthViewCount].enabled = false;
				disabledHealthViewCount++;
			}
		}

		private void InitializeHealthBar(int playerHealthAmount)
		{
			foreach (Transform child in healthBar)
			{
				Destroy(child.gameObject);
			}

			for (int i = 0; i < playerHealthAmount; i++)
			{
				playerHealthViews.Add(Instantiate(heartPrefab, healthBar).GetComponent<Image>());
			}
		}

		private void UpdateSlider(int experienceToNextLevel, int experience, int playerLevel)
		{
			levelSlider.fillAmount = (float) experience / experienceToNextLevel;
			levelSliderText.text = $"{experience}/{experienceToNextLevel}";
			levelText.text = playerLevel.ToString();
		}
	}
}
