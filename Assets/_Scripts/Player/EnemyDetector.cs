using System.Collections.Generic;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Player
{
	public class EnemyDetector : MonoBehaviour
	{
		public Transform playerTransform;
		public List<IDamageAble> damageAbles = new List<IDamageAble>();

		public void GetDamageAblesInLargestRange()
		{
			damageAbles.Sort((a, b) => Vector3.Distance(a.GetPosition(), playerTransform.position).CompareTo(Vector3.Distance(b.GetPosition(), playerTransform.position)));
		}
	}
}
