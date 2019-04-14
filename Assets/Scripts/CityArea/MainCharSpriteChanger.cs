using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class MainCharSpriteChanger : MonoBehaviour
	{
		// Renderer Refs
		public SpriteRenderer hatParts;
		public SpriteRenderer pickaxeParts;
		public SpriteRenderer bootsParts;

		// Inner

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		#region Changers

		public void exeHatChange()
		{
			// Import certain sprite data
			Sprite pickedImg = null;
			// apply
			hatParts.sprite = pickedImg;
		}

		public void exePickaxeChange()
		{
			// Import certain sprite data
			Sprite pickedImg = null;
			// apply
			pickaxeParts.sprite = pickedImg;
		}

		public void exeBootsChange()
		{
			// Import certain sprite data
			Sprite pickedImg = null;
			// apply
			bootsParts.sprite = pickedImg;
		}

		public void exeChangeViaGearInfo()
		{
			EquipmentSpriteData pickedGearInfo = SpritePackageManager.Instance.exeGetGearImgsInfo();

			hatParts.sprite = pickedGearInfo.hat_Sprite;
			pickaxeParts.sprite = pickedGearInfo.pickaxe_Sprite;
			bootsParts.sprite = pickedGearInfo.boots_Sprite;
		}

		#endregion
	}
}