using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Object_Pool;
using UnityEngine;

namespace _Scripts.Scriptable_Objects
{
	[CreateAssetMenu(fileName = "Impact", menuName = "Impact System/Impact")]
	public class ImpactSO : ScriptableObject
	{
		[field: SerializeField]
		public ImpactType ImpactType;
		[field: SerializeField]
		public PoolAbleParticle EffectPrefab;
		[field: SerializeField]
		public List<AudioClip> SoundEffects;
	}
}
