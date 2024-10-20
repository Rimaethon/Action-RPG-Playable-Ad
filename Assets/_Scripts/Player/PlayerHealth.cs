using _Scripts.Data;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Player
{
	public class PlayerHealth : MonoBehaviour, IDamageAble
	{
		public bool isDead;
		private int playerHealth = 5;

		public bool IsDead()
		{
			return isDead;
		}

		public Vector3 GetPosition()
		{
			return transform.position;
		}

		public void TakeDamage(int damage)
		{
			if (isDead)
			{
				return;
			}

			playerHealth -= damage;
			if (playerHealth > 0) return;
			isDead = true;

		}

		public void HandleImpact(ImpactData impactData)
		{
		}
	}
}
