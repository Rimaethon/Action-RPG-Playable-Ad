using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.AI.State_Machine.States
{
	public class AIMeleeChaseState : IAIState
	{
		public void Enter(BaseAIAgent agent)
		{
			agent.Animator.SetBool("IsWalking", true);
		}

		public void Exit(BaseAIAgent agent)
		{
			agent.Animator.SetBool("IsWalking", false);

		}

		public AIStates GetStateID()
		{
			return AIStates.CHASE_PLAYER;
		}

		public void Update(BaseAIAgent agent)
		{
			if (Vector3.Distance(agent.transform.position, agent.playerTransform.transform.position) <= agent.configSO.AttackDistance)
			{
				agent.StateMachine.ChangeState(AIStates.MELEE_ATTACK);
				return;
			}

			if (!agent.hasTarget)
			{
				agent.StateMachine.ChangeState(AIStates.IDLE);
				return;
			}

			agent.transform.position = Vector3.MoveTowards(agent.transform.position, agent.playerTransform.transform.position,
														   agent.configSO.Speed * Time.deltaTime);

			agent.transform.LookAt(agent.playerTransform);
		}
	}
}
