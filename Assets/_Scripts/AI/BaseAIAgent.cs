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
		public AIStateMachine StateMachine;
		public Animator Animator;
		private CapsuleCollider capsuleCollider;
		public bool hasTarget;
		public bool attacking;

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
			StateMachine.ChangeState(AIStates.CHASE_PLAYER);
			Animator.SetBool("IsDead", false);
		}

		public void SetDeadState()
		{
			StateMachine.ChangeState(AIStates.DEAD);
			capsuleCollider.enabled = false;
			StartCoroutine(DisableOnEndCoroutine(3));
		}
	}
}
