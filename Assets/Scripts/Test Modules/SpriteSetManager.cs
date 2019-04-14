using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;



namespace TestZone
{
	[DisallowMultipleComponent]
	public class SpriteSetManager : MonoSingletonBase<SpriteSetManager>
	{
		/*
		// Custom event
		[System.Serializable]
		public class TabReportEvent : UnityEvent<int> { }
		[Header("Events")]
		[Space]
		// Event : The tab touched
		public TabReportEvent onTouchTab;
		*/

		public List<Sprite> bodyList;
		//EquipmentSpriteData mainCharEquipment;

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		private void Start()
		{
			// Load sprite set info
			//mainCharEquipment = new EquipmentSpriteData();
		}

		public Sprite getBodySprite(int index)
		{
			return bodyList[index];
		}
	}



	public class EquipmentSpriteData
	{
		public Sprite bodySprite;
	}
}
