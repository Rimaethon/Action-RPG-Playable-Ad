using _Scripts.AI;

namespace _Scripts.Interfaces
{
	public interface IAIState
	{
		AIStates GetStateID();
		void Enter(BaseAIAgent agent);
		void Update(BaseAIAgent agent);
		void Exit(BaseAIAgent agent);
	}
}
