using _Scripts.Enums;
using UnityEngine;

namespace _Scripts.Scriptable_Objects
{
	[CreateAssetMenu(fileName = "Weapon Data", menuName = "Data/Weapon Data")]
	public class WeaponDataSO : UpgradeAbleItemSO
	{
		public BulletTrailDataSO TrailRenderer;
		public GameObject weaponPrefab;
		public WeaponType weaponType;
	}
}
