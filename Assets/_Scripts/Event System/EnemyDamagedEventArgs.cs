using System;
using UnityEngine;

namespace _Scripts.Event_System
{
	public class EnemyDamagedEventArgs : EventArgs
	{
		public int Damage;
		public bool isDead;
		public Vector3 Position;
	}
}
