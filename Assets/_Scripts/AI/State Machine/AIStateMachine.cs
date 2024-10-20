using _Scripts.Interfaces;

namespace _Scripts.AI.State_Machine
{
	public class AIStateMachine
	{
		private readonly BaseAIAgent agent;
		private readonly IAIState[] states;
		private IAIState currentState;

		public AIStateMachine(BaseAIAgent agent)
		{
			this.agent = agent;
			states = new IAIState[4];
		}

		public void RegisterState(IAIState state)
		{
			int index = (int) state.GetStateID();
			states[index] = state;
		}

		public void Update()
		{
			currentState?.Update(agent);
		}

		public void ChangeState(AIState newStateType)
		{
			IAIState newState = states[(int) newStateType];

			if (currentState != null && newState == currentState)
			{
				return;
			}

			currentState?.Exit(agent);
			currentState = newState;
			currentState?.Enter(agent);
		}
	}
}
