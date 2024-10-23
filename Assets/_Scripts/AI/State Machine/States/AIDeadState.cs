using _Scripts.Interfaces;

namespace _Scripts.AI.State_Machine.States
{
	public class AIDeadState : IAIState
	{
		public void Enter(BaseAIAgent agent)
		{
			agent.Animator.SetBool("IsDead", true);
			agent.Animator.SetBool("IsWalking", false);
			agent.Animator.SetBool("IsHeavyAttacking", false);
			agent.Animator.SetBool("IsLightAttacking", false);
		}

		public void Exit(BaseAIAgent agent)
		{
		}

		public AIStates GetStateID()
		{
			return AIStates.DEAD;
		}

		public void Update(BaseAIAgent agent)
		{
		}
	}
}
