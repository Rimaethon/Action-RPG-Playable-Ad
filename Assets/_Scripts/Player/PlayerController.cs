using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Interfaces;
using _Scripts.Scriptable_Objects;
using UnityEngine;

namespace _Scripts.Player
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private float movementSpeed;
		[SerializeField]
		private float rotationSpeed;
		[SerializeField]
		private Transform weaponHolder;
		[SerializeField]
		private List<WeaponDataSO> weaponData = new List<WeaponDataSO>();
		public float TargetYRotation;
		public Vector3 moveDirection;
		public bool hasTarget;
		private readonly List<IWeapon> weapons = new List<IWeapon>();
		private float aimSmoothVelocity;
		private EnemyDetector enemyDetector;
		private float largestRange;
		private PlayerAnimationManager playerAnimationManager;
		private PlayerHealth playerHealth;

		private void Awake()
		{
			playerAnimationManager = GetComponent<PlayerAnimationManager>();
			playerHealth = GetComponent<PlayerHealth>();
			enemyDetector = GetComponentInChildren<EnemyDetector>();
			enemyDetector.playerTransform = transform;
			InitializeWeapons();
		}

		private void Update()
		{
			if (playerHealth.isDead)
			{
				return;
			}

			enemyDetector.GetDamageAblesInLargestRange();
			Aim();
			transform.position += moveDirection * (movementSpeed * Time.deltaTime);
			if (!hasTarget) return;
			HandleWeaponShooting();
		}

		private void InitializeWeapons()
		{
			foreach (WeaponDataSO weaponData in weaponData)
			{
				GameObject weaponObject = weaponData.weaponType == WeaponType.HANDGUN ?
											  Instantiate(weaponData.weaponPrefab, weaponHolder) : Instantiate(weaponData.weaponPrefab, transform);

				IWeapon weapon = weaponObject.GetComponent<IWeapon>();
				weapon.InitializeWeapon(weaponData, enemyDetector, playerAnimationManager);
				weapons.Add(weapon);
				largestRange = Mathf.Max(largestRange, weapon.GetRange());
			}
		}

		private void HandleWeaponShooting()
		{
			foreach (IWeapon weapon in weapons)
			{
				weapon.TryGiveDamage();
			}
		}

		private void Aim()
		{
			hasTarget = enemyDetector.damageAbles.Count > 0;
			Vector3 position = hasTarget ? enemyDetector.damageAbles[0].GetPosition() : transform.position;
			Vector3 lookDirection = moveDirection;

			if (hasTarget)
			{
				lookDirection = position - transform.position;
				lookDirection.y = 0;
			}

			if (lookDirection == Vector3.zero)
			{
				return;
			}

			Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
			TargetYRotation = rotation.eulerAngles.y;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
		}
	}
}
