using System;

namespace _Scripts.Event_System
{
	public class LevelEndEventArgs : EventArgs
	{
		public bool isLevelCompleted;
		public int remainingTime;
	}
}
