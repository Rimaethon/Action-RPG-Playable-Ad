using _Scripts.Data;
using _Scripts.Enums;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.AI
{
	public class MeleeAICombat : MonoBehaviour, IAIMeleeAttack
	{
		[SerializeField]
		private Collider damageCollider;
		private MeleeAIAgent agent;
		private Animator animator;
		private readonly float attackAnimationLength = 3f;
		private readonly float attackTimer = 1.1f;
		private int attackDamage;
		private float counter;

		private void Start()
		{
			animator = GetComponent<Animator>();
			damageCollider.enabled = false;
			damageCollider.isTrigger = true;
			agent = GetComponent<MeleeAIAgent>();
			agent.attacking = false;
		}

		private void Update()
		{
			//Luna doesn't allow caching
			if (!agent.attacking)
				return;

			if (counter < attackTimer)
			{
				counter += Time.deltaTime;
			}
			else
			{
				damageCollider.enabled = false;

				if (!(attackAnimationLength < counter))
					return;
				counter = 0;
				agent.attacking = false;
				animator.SetBool("IsLightAttacking", false);
				animator.SetBool("IsHeavyAttacking", false);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (damageCollider.enabled == false)
				return;

			if (!other.transform.root.gameObject.TryGetComponent(out IDamageAble damageAble))
				return;

			damageAble.TakeDamage(attackDamage);

			ImpactData impactData = new ImpactData
			{
				ImpactType = ImpactType.KNOCK_BACK,
				ImpactStrength = attackDamage,
				HitNormal = Vector3.forward,
				HitPoint = other.transform.root.gameObject.transform.position + Vector3.up
			};

			damageAble.HandleImpact(impactData);
		}

		public void PerformLightMeleeAttack(int damage)
		{
			agent.attacking = true;
			counter = 0;
			attackDamage = damage;
			agent.Animator.SetBool("IsLightAttacking", true);
			damageCollider.enabled = true;
		}

		public void PerformHeavyMeleeAttack(int damage)
		{
			counter = 0;
			agent.attacking = true;
			attackDamage = damage;
			damageCollider.enabled = true;
			agent.Animator.SetBool("IsHeavyAttacking", true);
		}
	}
}
