using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using GLCore;
using TMPro;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class InventorySlotBehavior : MonoBehaviour
	{
		// Refs
		public Image imgCtrl;
		public Image frameImgCtrl;
		public TextMeshProUGUI textCtrl;
		public ItemMetadata itemMeta;		

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		public void exeSwapImg(Sprite itemImg, uint itemCount)
		{
			imgCtrl.enabled = true;
			imgCtrl.sprite = itemImg;

			if (itemCount > 1)
				textCtrl.text = itemCount.ToString();
			else
				textCtrl.text = "";

			frameImgCtrl.raycastTarget = true;
		}
		public void exeSwapImg(Sprite itemImg, uint itemCount, ItemRarity rarity)
		{
			imgCtrl.enabled = true;
			imgCtrl.sprite = itemImg;

			if (itemCount > 1)
				textCtrl.text = itemCount.ToString();
			else
				textCtrl.text = "";


			setFrameColorRarity(rarity);
			frameImgCtrl.raycastTarget = true;
		}


		public void exeSetEmpty()
		{
			imgCtrl.enabled = false;
			imgCtrl.sprite = null;
			textCtrl.text = "";
			frameImgCtrl.color = Color.white;
			frameImgCtrl.raycastTarget = false;
		}

		void setFrameColorRarity(ItemRarity rarity)
		{
			switch (rarity)
			{
				case ItemRarity.common : 
				{
					frameImgCtrl.color = itemMeta.commonItemFrameColor;
					break;		
				}
				case ItemRarity.rare :
				{
					frameImgCtrl.color = itemMeta.rareItemFrameColor;
					break;
				}
				case ItemRarity.unique :
				{
					frameImgCtrl.color = itemMeta.uniqueItemFrameColor;
					break;
				}
			}
		}
	}
}