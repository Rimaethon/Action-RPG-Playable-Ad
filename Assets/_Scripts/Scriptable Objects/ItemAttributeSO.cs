using _Scripts.Enums;
using UnityEngine;

namespace _Scripts.Scriptable_Objects
{
	[CreateAssetMenu(fileName = "Item Attribute", menuName = "Data/Item Attribute")]
	public class ItemAttributeSO : ScriptableObject
	{
		public Sprite icon;
		public string title;
		public int maxLevel;
		public int baseValue;
		public int maxValue;
		public ItemAttributeTypes attributeType;
	}
}
