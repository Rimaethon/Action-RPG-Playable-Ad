using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Interfaces;
using _Scripts.Scriptable_Objects;
using UnityEngine;

namespace _Scripts.Player.Weapons
{
	public class RadiantField : MonoBehaviour, IWeapon
	{
		public float Range;
		private float cooldown;
		private int damage;
		private EnemyDetector enemyDetector;
		private float timer;
		private WeaponDataSO weaponDataSO;

		public void InitializeWeapon(WeaponDataSO weaponData, EnemyDetector enemyDetector, PlayerAnimationManager playerAnimationManager)
		{
			weaponDataSO = weaponData;
			damage = InitializeValues(ItemAttributeTypes.DAMAGE);
			Range = InitializeValues(ItemAttributeTypes.RANGE);
			cooldown = InitializeValues(ItemAttributeTypes.COOLDOWN);
			transform.localScale = new Vector3(Range * 2, 0.01f, Range * 2);
			this.enemyDetector = enemyDetector;
		}

		public float GetRange()
		{
			return Range;
		}

		public void TryGiveDamage()
		{
			timer -= Time.fixedDeltaTime;

			if (timer > 0)
			{
				return;
			}

			List<IDamageAble> damageAbles = new List<IDamageAble>(enemyDetector.damageAbles);

			foreach (IDamageAble t in damageAbles)
			{
				if (Vector3.Distance(t.GetPosition(), transform.position) > Range)
				{
					break;
				}

				t?.TakeDamage(damage);
			}

			timer = cooldown;
		}

		private int InitializeValues(ItemAttributeTypes attributeType)
		{
			int level = 1;
			ItemAttributeSO itemAttributeSO = null;

			foreach (ItemAttributeSO variable in weaponDataSO._itemAttributes)
			{
				if (variable.attributeType != attributeType) continue;
				itemAttributeSO = variable;
				break;
			}

			if (itemAttributeSO == null)
			{
				return 0;
			}

			return (itemAttributeSO.maxValue - itemAttributeSO.baseValue) / itemAttributeSO.maxLevel * level + itemAttributeSO.baseValue;
		}
	}
}
