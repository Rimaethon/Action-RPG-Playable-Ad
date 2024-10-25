using _Scripts.Event_System;
using UnityEngine;

public class PlayableController : MonoBehaviour
{
	private void OnEnable()
	{
		EventManager.Subscribe<LevelEndEventArgs>(HandleLevelEnd);
		Luna.Unity.LifeCycle.OnPause += PauseGameplay;
		Luna.Unity.LifeCycle.OnResume += ResumeGameplay;
	}

	private void OnDisable()
	{
		EventManager.UnSubscribe<LevelEndEventArgs>(HandleLevelEnd);
		Luna.Unity.LifeCycle.OnPause -= PauseGameplay;
		Luna.Unity.LifeCycle.OnResume -= ResumeGameplay;
	}


	private void ResumeGameplay()
	{
		Time.timeScale = 1;
	}

	private void PauseGameplay()
	{
		Time.timeScale = 0;
	}
	private void HandleLevelEnd(LevelEndEventArgs obj)
	{
		if(!obj.isLevelCompleted)
			Luna.Unity.Analytics.LogEvent("PlayerLost", obj.remainingTime);
		else
			Luna.Unity.Analytics.LogEvent("PlayerWon",0);

		Luna.Unity.LifeCycle.GameEnded();
		Luna.Unity.Playable.InstallFullGame();
	}
}
