using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Scriptable_Objects
{
	[CreateAssetMenu(fileName = "UpgradeAbleItem", menuName = "Data/UpgradeAbleItem")]
	public class UpgradeAbleItemSO : ScriptableObject
	{
		public List<ItemAttributeSO> _itemAttributes = new List<ItemAttributeSO>();
		public bool isLocked;
		public int unlockPrice;
		public int unlockLevel;
		public Sprite backgroundIcon;
		public int itemID;
	}
}
