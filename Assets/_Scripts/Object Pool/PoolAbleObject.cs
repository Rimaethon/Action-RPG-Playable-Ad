using System.Collections;
using Object_Pool;
using UnityEngine;

namespace _Scripts.Object_Pool
{
	public class PoolAbleObject : MonoBehaviour
	{
		[HideInInspector]
		public ObjectPool Parent;

		protected virtual void Awake()
		{
		}

		protected virtual void OnDisable()
		{
			if (Parent != null)
			{
				Parent.ReturnObjectToPool(this);
			}
		}

		protected IEnumerator DisableOnEndCoroutine(float time)
		{
			yield return new WaitForSeconds(time);
			gameObject.SetActive(false);
		}
	}
}
