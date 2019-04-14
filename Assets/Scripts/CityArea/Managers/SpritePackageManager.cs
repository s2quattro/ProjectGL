using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.U2D;
using GLCore;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class SpritePackageManager : MonoSingletonBase<SpritePackageManager>
	{
		// Refs
		private LocalStorage localStorage;
		private ItemMetadata itemListDic;
		public SpriteAtlas gearImgAtlas;

		// Fixed sprite data packs
		[SerializeField]
		private EquipmentSpriteData mainCharGearImgs;

		// Inner		
		private EquipmentItemListStruct equipmentRef;  // MainChar EquipmentList

		/*
		// Custom event
		[System.Serializable]
		public class TabReportEvent : UnityEvent { }
		[Header("Events")]*/
		[Space]
		// Event : The tab touched
		public UnityEvent onChangedGear;


		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		public void SetUp()
		{
			itemListDic = LinkContainer.Instance.itemMetadata;
			localStorage = LinkContainer.Instance.localStorage;

			exeRefreshMainCharGearInfo();
		}

		// Called by Equip Refresher
		public void exeRefreshMainCharGearInfo()
		{
			// Get MainChar's gear info
			equipmentRef = InventoryManager.Instance.GetEquipmentItems();

			// Extract and Create new data pack
			mainCharGearImgs = new EquipmentSpriteData(
				validSpriteChecker(equipmentRef.head),
				validSpriteChecker(equipmentRef.pickax),
				validSpriteChecker(equipmentRef.shoes)	);
		}
		private Sprite validSpriteChecker(ItemId anyGearId)
		{
			if (anyGearId == ItemId.NotExistId)
			{
				return null;
			}
			return itemListDic.itemDataDic[anyGearId].GetSprite;
		}

		// Called by Main Character Sprite Changer
		public EquipmentSpriteData exeGetGearImgsInfo()			
		{
			return mainCharGearImgs;
		}
	}

	//-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-

	/// <summary>
	///		Sprite set of Charactor gears
	/// </summary>
	public class EquipmentSpriteData
	{
		public Sprite hat_Sprite;
		public Sprite pickaxe_Sprite;
		public Sprite boots_Sprite;

		public EquipmentSpriteData(Sprite hat, Sprite pickaxe, Sprite boots)
		{
			hat_Sprite = hat;
			pickaxe_Sprite = pickaxe;
			boots_Sprite = boots;
		}
	}
}