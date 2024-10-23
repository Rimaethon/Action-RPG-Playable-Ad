using _Scripts.Data;
using UnityEngine;

namespace _Scripts.Interfaces
{
	public interface IDamageAble
	{
		bool IsDead();
		Vector3 GetPosition();
		void TakeDamage(int damage);
		void HandleImpact(ImpactData impactData);
	}
}
