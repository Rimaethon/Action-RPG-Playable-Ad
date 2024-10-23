using _Scripts.Data;
using _Scripts.Event_System;
using _Scripts.Interfaces;
using _Scripts.Object_Pool;
using _Scripts.Player;
using Object_Pool;
using UnityEngine;

namespace _Scripts.AI
{
	public class AgentHealth : MonoBehaviour, IDamageAble
	{
		public bool isDead;
		[SerializeField]
		protected float characterHealth = 100f;
		public EnemyDetector EnemyDetector;
		private readonly EnemyDamagedEventArgs enemyDamagedEventArgs = new EnemyDamagedEventArgs();
		private BaseAIAgent agent;

		protected virtual void Awake()
		{
			agent = GetComponent<BaseAIAgent>();
		}

		private void OnEnable()
		{
			characterHealth = agent.configSO.Health;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("EnemyDetector"))
			{
				EnemyDetector.damageAbles.Add(this);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("EnemyDetector"))
			{
				EnemyDetector.damageAbles.Remove(this);
			}
		}

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
				return;

			characterHealth -= damage;
			if (characterHealth <= 0)
			{
				if (EnemyDetector.damageAbles.Contains(this))
				{
					EnemyDetector.damageAbles.Remove(this);
				}
				isDead = true;
				characterHealth = 0;
			}

			//Needs pooling since it generates a lot of garbage and it is called multiple times every frame
			enemyDamagedEventArgs.Damage = damage;
			enemyDamagedEventArgs.Position = transform.position;
			enemyDamagedEventArgs.isDead = isDead;
			EventManager.RaiseEvent(enemyDamagedEventArgs);

			if (isDead)
			{
				agent.SetDeadState();
			}
		}

		public void HandleImpact(ImpactData impactData)
		{
			ObjectPool particlePool = ObjectPoolManager.CreateInstance(agent.configSO.impactSOData.EffectPrefab, 10);
			PoolAbleObject instance = particlePool.GetObject(impactData.HitPoint + impactData.HitNormal * 0.001f, Quaternion.LookRotation(impactData.HitNormal));
			instance.transform.forward = impactData.HitNormal;
		}
	}
}
