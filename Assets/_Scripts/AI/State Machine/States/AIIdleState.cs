using _Scripts.Interfaces;

namespace _Scripts.AI.State_Machine.States
{
	public class AIIdleState : IAIState
	{
		public void Enter(BaseAIAgent agent)
		{
		}

		public void Exit(BaseAIAgent agent)
		{
		}

		public AIStates GetStateID()
		{
			return AIStates.IDLE;
		}

		public void Update(BaseAIAgent agent)
		{
			if (!agent.hasTarget)
			{
				return;
			}

			agent.StateMachine.ChangeState(AIStates.CHASE_PLAYER);
		}
	}
}
