using UnityEngine;
using UnityEngine.UI;
using GLCore;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class EquipmentSlotBehavior : MonoBehaviour
	{
		// Refs
		public Image imgCtrl;		
		public Image frameImgCtrl;
		public ItemMetadata itemMeta;

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		public void exeSwapImg(Sprite itemImg)
		{
			imgCtrl.enabled = true;
			imgCtrl.sprite = itemImg;
			frameImgCtrl.raycastTarget = true;
		}
		public void exeSwapImg(Sprite itemImg, ItemRarity rarity)
		{
			imgCtrl.enabled = true;
			imgCtrl.sprite = itemImg;
			setFrameColorRarity(rarity);

			frameImgCtrl.raycastTarget = true;
		}

		public void exeSetEmpty()
		{
			imgCtrl.enabled = false;
			imgCtrl.sprite = null;		
			frameImgCtrl.raycastTarget = false;
		}

		void setFrameColorRarity(ItemRarity rarity)
		{
			switch (rarity)
			{
				case ItemRarity.common:
					{
						frameImgCtrl.color = itemMeta.commonItemFrameColor;
						break;
					}
				case ItemRarity.rare:
					{
						frameImgCtrl.color = itemMeta.rareItemFrameColor;
						break;
					}
				case ItemRarity.unique:
					{
						frameImgCtrl.color = itemMeta.uniqueItemFrameColor;
						break;
					}
			}
		}
	}
}