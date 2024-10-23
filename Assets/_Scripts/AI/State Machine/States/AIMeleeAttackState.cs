using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.AI.State_Machine.States
{
	public class AIMeleeAttackState : IAIState
	{
		public void Enter(BaseAIAgent agent)
		{
		}

		public void Exit(BaseAIAgent agent)
		{
			agent.attacking = false;

			//       agent.Animator.CrossFade("Blend Tree", 0.1f);
		}

		public AIStates GetStateID()
		{
			return AIStates.MELEE_ATTACK;
		}

		public void Update(BaseAIAgent agent)
		{
			if (agent.attacking)
			{
				return;
			}

			if (!agent.hasTarget)
			{
				agent.StateMachine.ChangeState(AIStates.IDLE);
				return;
			}

			if (!agent.playerTransform.TryGetComponent(out IDamageAble status) || status.IsDead())
			{
				agent.playerTransform = null;
				agent.hasTarget = false;
				agent.StateMachine.ChangeState(AIStates.IDLE);
				return;
			}

			if (agent.hasTarget && Vector3.Distance(agent.transform.position, agent.playerTransform.transform.position) > agent.configSO.AttackDistance)
			{
				agent.StateMachine.ChangeState(AIStates.CHASE_PLAYER);
			}
			else
			{
				PerformAttack(agent);
			}
		}

		private void PerformAttack(BaseAIAgent agent)
		{
			if (agent.attacking) return;
			int a = Random.Range(0, 101);
			agent.transform.LookAt(agent.playerTransform.transform.position, Vector3.up);
			MeleeAIAgent meleeAgent = agent as MeleeAIAgent;

			if (a >= 50)
			{
				meleeAgent.AIAttack.PerformLightMeleeAttack(agent.configSO.LightDamage);

			}
			else
			{
				meleeAgent.AIAttack.PerformHeavyMeleeAttack(agent.configSO.HeavyDamage);
			}
		}
	}
}
