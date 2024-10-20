using _Scripts.AI.State_Machine;
using _Scripts.AI.State_Machine.States;
using _Scripts.Object_Pool;
using _Scripts.Scriptable_Objects;
using UnityEngine;

namespace _Scripts.AI
{
	public class BaseAIAgent : PoolAbleObject
	{
		public AIAgentConfigSO configSO;
		public Transform playerTransform;
		public bool hasTarget;
		public Animator Animator;
		public bool attacking;
		private CapsuleCollider capsuleCollider;
		public AIStateMachine StateMachine;

		protected override void Awake()
		{
			base.Awake();
			StateMachine = new AIStateMachine(this);
			playerTransform = GameObject.FindWithTag("Player").transform;
			Animator = GetComponent<Animator>();
			capsuleCollider = GetComponent<CapsuleCollider>();
		}

		protected virtual void Start()
		{
			StateMachine.RegisterState(new AIDeadState());
		}

		protected virtual void Update()
		{
			StateMachine.Update();
		}

		private void OnEnable()
		{
			hasTarget = true;
			attacking = false;
			capsuleCollider.enabled = true;
			StateMachine.ChangeState(AIState.CHASE_PLAYER);
			Animator.SetBool("IsDead", false);
		}

		public void SetDeadState()
		{
			StateMachine.ChangeState(AIState.DEAD);
			capsuleCollider.enabled = false;
			StartCoroutine(DisableOnEndCoroutine(3));
		}
	}
}
