using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using GLCore;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class InventoryPanelBehavior : WindowBoxUI
	{
		// Refs		
		public Button btn_Close;
		public Button btn_Expand;
		public Transform itemSlotGroup;
		public Transform packSlotGroup;
		public Transform gearSlotGroup;
		public AdditiveTabButtonGroup packGroupCtrl;
		private LocalStorage storageCtrl;
		private ItemMetadata itemListDic;
		

		public InventoryItemInfoPanelBehavior subPanel_Info;


		// Inner Field			
		private List<InventorySlotBehavior> itemSlotCtrl;
		private List<EquipmentSlotBehavior> gearSlotCtrl;
		
		private int eachPackSlotMaxCount;
		private int PackSlotMaxCount;
		private int gearSlotMaxCount;

		private int selectedItemIndex = -1;
		private ItemId selectedItem = ItemId.NotExistId;
		private EquipmentType selectedGear = EquipmentType.non;
		private uint countSetValue = 0;
		private ItemType sortingType = ItemType.nonSelect;

		private List<InventoryItemListStruct> inventoryRef;  // inventoryList in backPack
		private EquipmentItemListStruct equipmentRef;  // EquipmentList
		private int selectedPackIndex;  // first is 0

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
		
		void Start()
		{
			register();
		}

		#region Initializers

		private void Firstloader()
		{
			//print("Loaded!!");

			storageCtrl = LinkContainer.Instance.localStorage;
			itemListDic = LinkContainer.Instance.itemMetadata;

			linkAllSlot();
			//inventoryRef = new List<InventoryItemListStruct>(eachPackSlotMaxCount);
		}
		private void linkAllSlot()
		{
			//Child Counter
			eachPackSlotMaxCount = itemSlotGroup.childCount;
			PackSlotMaxCount = packSlotGroup.childCount;
			gearSlotMaxCount = gearSlotGroup.childCount;

			//List Instance Maker
			itemSlotCtrl = new List<InventorySlotBehavior>(eachPackSlotMaxCount);
			gearSlotCtrl = new List<EquipmentSlotBehavior>(gearSlotMaxCount);

			//Item Slot Linker
			for (int i = 0; i < eachPackSlotMaxCount; i++)
			{
				Button eachCtrl = itemSlotGroup.GetChild(i).GetComponent<Button>();
				//print(i);
				eachCtrl.onClick.AddListener
				(
					() =>
					{
						exeReportSelectedItemSlot(eachCtrl.transform.GetSiblingIndex());
					}
				);

				//print(itemSlotGroup.GetChild(i).gameObject.name);
				itemSlotCtrl.Add(itemSlotGroup.GetChild(i).GetComponent<InventorySlotBehavior>());
			}
			//Gear Slot Linker
			for (int i = 0; i < gearSlotMaxCount; i++)
			{
				Button eachCtrl = gearSlotGroup.GetChild(i).GetComponent<Button>();
				//print(i);
				eachCtrl.onClick.AddListener
				(
					() =>
					{
						exeReportSelectedGearSlot(eachCtrl.name);
					}
				);

				gearSlotCtrl.Add(gearSlotGroup.GetChild(i).GetComponent<EquipmentSlotBehavior>());
			}
		}
		private void initializePanel()
		{
			// Reset backpack Selection
			selectedPackIndex = 0;
			packGroupCtrl.forceUpdateView(0);
			selectedItem = ItemId.NotExistId;
		}

		#endregion

		#region Refreshers

		// Request a List to Inventory Manager As a refresher
		public void exeRefreshInventoryList()
		{			
			// Calculate capacity of current selected pack
			int curPackCapacity = Mathf.Clamp((int)storageCtrl.playerStat.GetInventoryMaxAmount - eachPackSlotMaxCount * selectedPackIndex, 5, eachPackSlotMaxCount);			
			//print("Current Pack : " + curPackCapacity);


			// As a Refresher
			inventoryRef = InventoryManager.Instance.GetInventoryItems((uint)(selectedPackIndex * eachPackSlotMaxCount), sortingType);	

			// Empty Checker
			if(inventoryRef == null)
			{
				for (int y = 0; y < eachPackSlotMaxCount; y++)
				{
					itemSlotCtrl[y].exeSetEmpty();
				}

				// Clamp slots
				int k;
				for (k = 0; k < curPackCapacity; k++)
				{
					itemSlotCtrl[k].gameObject.SetActive(true);
				}
				for (; k < eachPackSlotMaxCount; k++)
				{
					itemSlotCtrl[k].gameObject.SetActive(false);
				}
				return;	// Skip
			}

			//print(string.Format("{0} To {1} || count : {2}", (uint)(selectedPackIndex * eachPackSlotCount), storageCtrl.playerStat.GetInventoryCurAmount - 1, inventoryRef.Count));

			// Contents refreshing
			int i;
			for (i = 0; i < inventoryRef.Count; i++)
			{
				ItemId selectedItem = inventoryRef[i].id;
				ItemBase pickedItemData = itemListDic.itemDataDic[selectedItem];


				itemSlotCtrl[i].exeSwapImg(pickedItemData.GetSprite, inventoryRef[i].curAmount, pickedItemData.GetItemRarity);
			}		
			// Treat empty slots
			for(; i < eachPackSlotMaxCount; i++)
			{
				itemSlotCtrl[i].exeSetEmpty();
			}
			
			// Clamp slots
			int a;
			for (a = 0; a < curPackCapacity; a++)
			{
				itemSlotCtrl[a].gameObject.SetActive(true);
			}
			for(; a < eachPackSlotMaxCount; a++)
			{
				itemSlotCtrl[a].gameObject.SetActive(false);
			}
		}

		// Request a Equipment List
		public void exeRefreshEquipmentList()
		{  // As a Refresher
			equipmentRef = InventoryManager.Instance.GetEquipmentItems();

			// set the head Gear
			setItemEachGearSlot(equipmentRef.head, 0);

			// set the pickaxe Gear
			setItemEachGearSlot(equipmentRef.pickax, 1);

			// set the shoes Gear
			setItemEachGearSlot(equipmentRef.shoes, 2);
		}
		private void setItemEachGearSlot(ItemId id, int slotNum)
		{
			if(id == ItemId.NotExistId)
			{
				gearSlotCtrl[slotNum].exeSetEmpty();
				return;
			}
			Sprite pickedImg = itemListDic.itemDataDic[id].GetSprite;

			gearSlotCtrl[slotNum].exeSwapImg(pickedImg);
		}
		
		// Refresh backpacks list
		public void exeRefreshPackList()
		{
			int havePackCount = ((int)storageCtrl.playerStat.GetInventoryMaxAmount - 5) / eachPackSlotMaxCount + 1;
			//print("we have packs : " + havePackCount);

			packGroupCtrl.syncRealUnlock(havePackCount);

			//int packCount = (int)Mathf.Ceil((int)storageCtrl.playerStat.GetInventoryMaxAmount / eachPackSlotMaxCount);
			//print("Test Pack(s) : " + packCount);			
		}

		// Refresh Expandable
		public void exeRefreshIsExpandable()
		{
			int confirmCost = InventoryManager.Instance.GetInventoryExpansionCost();

			// Check Expandable
			if(confirmCost == InventoryManager.InventoryExpansionMax)
			{   // Disable Button
				btn_Expand.interactable = false;
			}
			else
			{   // Enable Button
				btn_Expand.interactable = true;
			}
		}

		#endregion

		// Called by Open Request
		public override void exeOpenPanel()
		{
			//First Time Loader
			if (storageCtrl == null)
				Firstloader();
			
			// Refreshers
			exeRefreshInventoryList();
			exeRefreshEquipmentList();
			exeRefreshPackList();
			exeRefreshIsExpandable();

			base.exeOpenPanel();
		}
		// Called by Close Request
		public override void exeClosePanel()
		{
			initializePanel();
			base.exeClosePanel();
		}

		#region Linked with the Inventory panel button

		// Called by Item Slot Button
		public void exeReportSelectedItemSlot(int index)
		{
			if (index >= inventoryRef.Count)
				return;


			// print(index);
			// Pick Item Data
			selectedItemIndex = index;
			selectedItem = inventoryRef[index].id;
			ItemBase pickedItemMeta = itemListDic.itemDataDic[selectedItem];


			// Set Info Panel's Info
			subPanel_Info.showInfo(
				pickedItemMeta.GetSprite,
				pickedItemMeta.GetName,
				pickedItemMeta.GetDescription,
				pickedItemMeta.GetItemType,
				pickedItemMeta.GetItemRarity
			);
			// Open Info Panel
			UIManager.Instance.exeOpenMessageBox(subPanel_Info);
		}		
	
		// Called by Gear Slot Button
		public void exeReportSelectedGearSlot(string name)
		{
			switch (name)
			{
				case "head":
				{
						ItemBase pickedItemMeta = itemListDic.itemDataDic[equipmentRef.head];
						selectedGear = EquipmentType.head;

						subPanel_Info.showGearInfo(
							pickedItemMeta.GetSprite,
							pickedItemMeta.GetName,
							pickedItemMeta.GetDescription,
							pickedItemMeta.GetItemRarity
						);

						//Open Info Panel
						UIManager.Instance.exeOpenMessageBox(subPanel_Info);
						break;
				}
				case "pickax":
				{
						ItemBase pickedItemMeta = itemListDic.itemDataDic[equipmentRef.pickax];
						selectedGear = EquipmentType.pickax;

						subPanel_Info.showGearInfo(
							pickedItemMeta.GetSprite,
							pickedItemMeta.GetName,
							pickedItemMeta.GetDescription,
							pickedItemMeta.GetItemRarity
						);

						//Open Info Panel
						UIManager.Instance.exeOpenMessageBox(subPanel_Info);
						break;
				}
				case "shoes":
				{
						ItemBase pickedItemMeta = itemListDic.itemDataDic[equipmentRef.shoes];
						selectedGear = EquipmentType.shoes;

						subPanel_Info.showGearInfo(
							pickedItemMeta.GetSprite,
							pickedItemMeta.GetName,
							pickedItemMeta.GetDescription,
							pickedItemMeta.GetItemRarity
						);

						//Open Info Panel
						UIManager.Instance.exeOpenMessageBox(subPanel_Info);
						break;
				}
			}
		}

		#endregion


		#region Linked with the Item info panel button

		// Called by Consume Button
		public void exeConsumeItem()
		{
			uint consumeAmount = inventoryRef[selectedItemIndex].curAmount;
			ItemType consumeType = itemListDic.itemDataDic[inventoryRef[selectedItemIndex].id].GetItemType;

			// Only a single expendable???
			if (consumeType == ItemType.expendableSingle)
			{	// Skip CounterSetter
				UIManager.Instance.exeOpenDecisionReminder(decisionType.use, consumeItem);
				return;				
			}
			// Or Only One expendable???
			if(consumeAmount == 1)
			{
				// Skip CounterSetter
				UIManager.Instance.exeOpenDecisionReminder(decisionType.use, consumeItem, 1);
				return;
			}
			UIManager.Instance.exeOpenCountSetter(consumeAmount, consumeItem);
		}
		private void consumeItem(int itemCount)
		{
			InventoryManager.Instance.UseItem(selectedItem, (uint)itemCount);
			//print(selectedItem);

			// Refresh & Initialize
			selectedItem = ItemId.NotExistId;
			exeRefreshInventoryList();

			subPanel_Info.exeClosePanel();
		}
		// Using Only one item
		private void consumeItem()
        {
            InventoryManager.Instance.UseItem(selectedItem);
			//print(selectedItem);

			// Refresh & Initialize
			selectedItem = ItemId.NotExistId;
			exeRefreshInventoryList();

			subPanel_Info.exeClosePanel();
		}

		// Called by Discard Button
		public void exeDiscardItem()
		{
			if (inventoryRef[selectedItemIndex].curAmount > 1)
				UIManager.Instance.exeOpenCountSetter(inventoryRef[selectedItemIndex].curAmount, exeSetCountDiscardValue);
			else
			{	// Discard Only 'One' item
				countSetValue = 1;
				UIManager.Instance.exeOpenDecisionReminder(decisionType.discard, discardItem);
			}
		}
		// Called by Count Setter
		public void exeSetCountDiscardValue(int countValue)
		{
			//print("Value is Set : " + countValue);
			countSetValue = (uint)countValue;
			UIManager.Instance.exeOpenDecisionReminder(decisionType.discard, discardItem);
		}
		private void discardItem(int itemCount)
		{
			InventoryManager.Instance.RemoveItem(selectedItem, (uint)itemCount);
			//print(selectedItem);

			// Refresh & Initialize
			selectedItem = ItemId.NotExistId;
			exeRefreshInventoryList();

			subPanel_Info.exeClosePanel();
		}
		private void discardItem()
		{
			InventoryManager.Instance.RemoveItem(selectedItem, countSetValue);
			//print(selectedItem);

			// Refresh & Initialize
			selectedItem = ItemId.NotExistId;
			exeRefreshInventoryList();

			subPanel_Info.exeClosePanel();
		}

		// Called by Equip Button
		public void exeEquipItem()
		{
			InventoryManager.Instance.EquipItem(selectedItem);
			//print(selectedItem);

			// Refresh & Initialize
			selectedItem = ItemId.NotExistId;
			exeRefreshInventoryList();
			exeRefreshEquipmentList();

			subPanel_Info.exeClosePanel();
		}

		// Called by UnEquip Button
		public void exeUnEquipItem()
		{
			InventoryManager.Instance.UnEquipItem(selectedGear);
			//print(selectedItem);

			// Refresh & Initialize
			selectedGear = EquipmentType.non;
			exeRefreshInventoryList();
			exeRefreshEquipmentList();

			subPanel_Info.exeClosePanel();
		}

		#endregion

		// Called by Slot expand Button
		public void exeExpandSlot()
		{
			int confirmCost = InventoryManager.Instance.GetInventoryExpansionCost();

			// Message Script Adder
			if(confirmCost >= InventoryManager.InventoryExpansionCostIsCash)
			{   // Only Cash
				UIManager.Instance.exeOpenDecisionReminder(decisionType.expandInvSlot, () => { InventoryManager.Instance.InventoryExpansion(); forceRefreshAfterExpand(); },
					"확장 비용 : " + confirmCost + " 만 원", false);
			}
			else if(confirmCost <= InventoryManager.InventoryExpansionCostIsGold)
			{   // Only Gold
				UIManager.Instance.exeOpenDecisionReminder(decisionType.expandInvSlot, () => { InventoryManager.Instance.InventoryExpansion(); forceRefreshAfterExpand(); },
					"확장 비용 : <#ffbb00>" + Mathf.Abs(confirmCost) + " 금</color>", false);
			}
		}
		private void forceRefreshAfterExpand()
		{
			exeRefreshIsExpandable();
			exeRefreshInventoryList();
			exeRefreshPackList();
		}

		// Called by Sorting tab Group
		public void exeSortTabChanged(int tabIndex)
		{
			switch(tabIndex)
			{
				case 0:
					{
						sortingType = ItemType.nonSelect;
						exeRefreshInventoryList();
						break;
					}
				case 1:
					{
						sortingType = ItemType.expendable;
						exeRefreshInventoryList();
						break;
					}
				case 2:
					{
						sortingType = ItemType.equipment;
						exeRefreshInventoryList();
						break;
					}
				case 3:
					{
						sortingType = ItemType.expendableSingle;
						exeRefreshInventoryList();
						break;
					}
				case 4:
					{
						sortingType = ItemType.other;
						exeRefreshInventoryList();
						break;
					}
			}
		}

		// Called by Pack tab Group
		public void exePackTabChanged(int tabIndex)
		{
			//print(tabIndex);
			selectedPackIndex = tabIndex;
			exeRefreshInventoryList();			
		}
	}
}