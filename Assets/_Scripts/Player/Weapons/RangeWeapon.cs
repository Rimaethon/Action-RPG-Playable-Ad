using System.Collections;
using _Scripts.Data;
using _Scripts.Enums;
using _Scripts.Event_System;
using _Scripts.Interfaces;
using _Scripts.Object_Pool;
using _Scripts.Scriptable_Objects;
using Object_Pool;
using UnityEngine;

namespace _Scripts.Player.Weapons
{
	[RequireComponent(typeof(AudioSource))]
	public class RangeWeapon : MonoBehaviour, IWeapon
	{
		private const float reload_time = 3;
		public float Range = 20;
		[SerializeField]
		protected WeaponDataSO weaponDataSO;
		[SerializeField]
		protected Transform gunBarrel;
		[SerializeField]
		protected ParticleSystem muzzleFlash;
		[SerializeField]
		protected GameObject bulletTrailPrefab;
		private int bullets;
		private int damage = 16;
		private EnemyDetector enemyDetector;
		private int fireRate = 500;
		private float lastShotTime;
		private int magazineSize = 30;
		private PlayerAnimationManager playerAnimationManager;
		private ObjectPool trailRendererPool;

		public void InitializeWeapon(WeaponDataSO weaponData, EnemyDetector enemyDetector, PlayerAnimationManager playerAnimationManager)
		{
			this.playerAnimationManager = playerAnimationManager;
			trailRendererPool = ObjectPoolManager.CreateInstance(bulletTrailPrefab.GetComponent<PoolAbleObject>(), 50);

			damage = InitializeValues(ItemAttributeTypes.DAMAGE);
			magazineSize = InitializeValues(ItemAttributeTypes.MAGAZINE_SIZE);
			fireRate = InitializeValues(ItemAttributeTypes.FIRE_RATE);
			bullets = magazineSize;
			this.enemyDetector = enemyDetector;
		}

		public float GetRange()
		{
			return Range;
		}

		public void TryGiveDamage()
		{
			if (lastShotTime > 0)
			{
				lastShotTime -= Time.deltaTime;
				return;
			}

			if (bullets == 0)
			{
				PerformReload(playerAnimationManager);
				return;
			}

			lastShotTime = (float) 60 / fireRate;
			ShotLogic();
		}

		private int InitializeValues(ItemAttributeTypes attributeType)
		{
			int level = 1;
			ItemAttributeSO itemAttributeSOData = null;

			foreach (ItemAttributeSO variable in weaponDataSO._itemAttributes)
			{
				if (variable.attributeType != attributeType) continue;
				itemAttributeSOData = variable;
				break;
			}

			if (itemAttributeSOData == null)
			{
				return 0;
			}

			return (itemAttributeSOData.maxValue - itemAttributeSOData.baseValue) / itemAttributeSOData.maxLevel * level + itemAttributeSOData.baseValue;
		}

		private void ShotLogic()
		{
			if (enemyDetector.damageAbles.Count == 0)
			{
				return;
			}

			IDamageAble damageable = enemyDetector.damageAbles[0];
			float distance = Vector3.Distance(damageable.GetPosition(), gunBarrel.transform.position);

			if (damageable.IsDead() || distance > Range)
			{
				return;
			}

			Vector3 normalizedDirection = Vector3.Normalize(damageable.GetPosition() - gunBarrel.transform.position);
			Vector3 forward = gunBarrel.transform.forward;

			if (distance > 1.5f && Vector3.Dot(normalizedDirection, forward) < 0.8f)
			{
				return;
			}

			//AudioManager.Instance.PlaySFX(SFXClips.AK47ShotSound);
			StartCoroutine(PlayTrail(damageable));
			playerAnimationManager.PlayRifleMediumShot((float) 60 / fireRate);
			bullets--;

			BulletCountChangedEventArgs bulletCountChangedEventArgs = new BulletCountChangedEventArgs
			{
				bulletCount = bullets
			};

			EventManager.RaiseEvent(bulletCountChangedEventArgs);
			damageable.TakeDamage(damage);
		}

		private IEnumerator PlayTrail(IDamageAble damageAble)
		{
			muzzleFlash.Play();
			Vector3 startPoint = gunBarrel.transform.position;
			Vector3 endPoint = damageAble.GetPosition();
			TrailRenderer trailRenderer = trailRendererPool.GetObject(startPoint, Quaternion.identity).GetComponent<TrailRenderer>();
			trailRenderer.gameObject.SetActive(true);
			trailRenderer.emitting = true;
			float distance = Vector3.Distance(startPoint, endPoint);
			float remainingDistance = distance;

			while (remainingDistance > 0)
			{
				trailRenderer.transform.position = Vector3.Lerp(gunBarrel.transform.position, damageAble.GetPosition(),
																Mathf.Clamp01(1 - remainingDistance / distance));

				remainingDistance -= weaponDataSO.TrailRenderer.SimulationSpeed * Time.deltaTime;

				yield return null;
			}

			trailRenderer.transform.position = endPoint;

			damageAble.HandleImpact(new ImpactData
			{
				HitPoint = endPoint,
				HitNormal = endPoint - startPoint,
				ImpactType = ImpactType.KNOCK_BACK
			});

			yield return new WaitForSeconds(weaponDataSO.TrailRenderer.Duration);
			trailRenderer.emitting = false;
			trailRenderer.gameObject.SetActive(false);
			trailRenderer.gameObject.SetActive(false);
		}

		private void PerformReload(PlayerAnimationManager playerAnimation)
		{
			lastShotTime = reload_time;
			bullets = magazineSize;
			playerAnimation.PlayReloadAnimation("RifleReload");

			//			AudioManager.Instance.PlaySFX(SFXClips.AK47ReloadSound);
		}
	}
}
