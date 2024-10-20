using System;

namespace _Scripts.Event_System
{
	public class PlayerDamagedEventArgs : EventArgs
	{
		public int Damage;
		public bool isDead;
	}
}
