using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GLCore;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class InventoryItemInfoPanelBehavior : WindowBoxUI
	{
		// Refs
		public Image itemImgCtrl;
		public TextMeshProUGUI itemNameTextCtrl;		
		public TextMeshProUGUI descriptionTextCtrl;		
		public ScrollRect scrollCtrl;
		public ItemMetadata itemMeta;

		public Button btn_Consume;
		public Button btn_Equip;
		public Button btn_UnEquip;
		public Button btn_Discard;
		public Button btn_Close;

		// Inner Field

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		void Start()
		{
			register();			
		}
		void OnEnable()
		{
			scrollCtrl.verticalNormalizedPosition = 1f;
		}

		public void showGearInfo(Sprite itemImg, string itemName, string description)
		{
			itemImgCtrl.sprite = itemImg;
			itemNameTextCtrl.text = itemName;
			descriptionTextCtrl.text = description.Replace("\\n", "\n");						


			btn_Equip.gameObject.SetActive(false);
			btn_Consume.gameObject.SetActive(false);
			btn_Discard.gameObject.SetActive(false);
			btn_UnEquip.gameObject.SetActive(true);
		}
		public void showGearInfo(Sprite itemImg, string itemName, string description, ItemRarity rarity)
		{
			itemImgCtrl.sprite = itemImg;
			itemNameTextCtrl.text = itemName;			
			descriptionTextCtrl.text = description.Replace("\\n", "\n");


			setFrameColorRarity(rarity);

			btn_Equip.gameObject.SetActive(false);
			btn_Consume.gameObject.SetActive(false);
			btn_Discard.gameObject.SetActive(false);
			btn_UnEquip.gameObject.SetActive(true);
		}		
		
		public void showInfo(Sprite itemImg, string itemName, string description, ItemType itemType)
		{		
			itemImgCtrl.sprite = itemImg;
			itemNameTextCtrl.text = itemName;
			descriptionTextCtrl.text = description.Replace("\\n", "\n");			

			appearProperButton(itemType);
		}
		public void showInfo(Sprite itemImg, string itemName, string description, ItemType itemType, ItemRarity rarity)
		{			
			itemImgCtrl.sprite = itemImg;
			itemNameTextCtrl.text = itemName;
			descriptionTextCtrl.text = description.Replace("\\n", "\n");

			setFrameColorRarity(rarity);
			appearProperButton(itemType);			
		}

		private void appearProperButton(ItemType itemType)
		{
			btn_Equip.gameObject.SetActive(false);
			btn_UnEquip.gameObject.SetActive(false);
			btn_Consume.gameObject.SetActive(false);			
			btn_Discard.gameObject.SetActive(true);

			if (itemType == ItemType.equipment)
			{
				btn_Equip.gameObject.SetActive(true);
			}
			else if (itemType == ItemType.expendable || itemType == ItemType.expendableSingle)
			{
				btn_Consume.gameObject.SetActive(true);
			}
		}

		private void setFrameColorRarity(ItemRarity rarity)
		{
			switch (rarity)
			{
				case ItemRarity.common:
					{
						itemNameTextCtrl.color = itemMeta.commonItemFrameColor;
						break;
					}
				case ItemRarity.rare:
					{
						itemNameTextCtrl.color = itemMeta.rareItemFrameColor;
						break;
					}
				case ItemRarity.unique:
					{
						itemNameTextCtrl.color = itemMeta.uniqueItemFrameColor;
						break;
					}
			}
		}
	}
}
