using _Scripts.AI.State_Machine.States;
using _Scripts.Interfaces;

namespace _Scripts.AI
{
	public class MeleeAIAgent : BaseAIAgent
	{
		public IAIMeleeAttack AIAttack;

		protected override void Awake()
		{
			base.Awake();
			AIAttack = GetComponent<IAIMeleeAttack>();
		}

		protected override void Start()
		{
			base.Start();
			StateMachine.RegisterState(new AIMeleeChaseState());
			StateMachine.RegisterState(new AIIdleState());
			StateMachine.RegisterState(new AIMeleeAttackState());
			StateMachine.ChangeState(AIStates.IDLE);
		}
	}
}
