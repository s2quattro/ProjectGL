using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using GLCore;
using TMPro;



namespace CityStage
{
	[DisallowMultipleComponent]
	public partial class StorePanelBehavior : GenericWindowUI<StoreNpcId>
	{
		// Refs
		public Button btn_Close;
		public TextMeshProUGUI cashViewerCtrl;

		// MetaData Storages
		private LocalStorage storageCtrl;
		private Dictionary<ItemId, ItemBase> itemListDic;
		private Dictionary<StoreNpcId, StoreData> storeListDic;

		private List<SellItemInfo> productListRef;  // inventoryList in backPack

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		// Use this for initialization
		void Start()
		{
			register();
		}

		#region Initializers

		private void Firstloader()
		{			
			LinkContainer linker = LinkContainer.Instance;

			storageCtrl = linker.localStorage;
			itemListDic = linker.itemMetadata.itemDataDic;
			storeListDic = linker.storeMetadata.storeNpcDic;

			linkOriginalCase();
		}

		#endregion

		#region Refreshers

		// Request a List to Store Module As a refresher
		public void exeRefreshStoreList(StoreNpcId storeNPC)
		{
			/*
			if(productListRef == storeListDic[storeNPC].GetItemList())
			{   // Same List
				return;  // Skip refresh
			}*/

			// Clear the display case


			// Import a Goods list
			productListRef = storeListDic[storeNPC].GetItemList();

			// Force display
			displayGoods(productListRef.Count);
		}

		#endregion

		// Called by Open Request
		public override void exeOpenPanel(StoreNpcId arg)
		{			
			// First Time loader
			if (storageCtrl == null)
				Firstloader();
	
			// Refresh shop list
			exeRefreshStoreList(arg);
			// Refresh cash
			updateCashViewer();


			base.exeOpenPanel();
		}
		// Called by Close Request
		public override void exeClosePanel()
		{
			base.exeClosePanel();
		}

		// Force update cash value
		public void updateCashViewer()
		{
			cashViewerCtrl.text = GLAPI.DecimalToWon(storageCtrl.playerProperty.GetCash);
		}
	}


	// Flexible list pooling Parts
	public partial class StorePanelBehavior : GenericWindowUI<StoreNpcId>
	{
		// Refs
		public GameObject listElement;
		public Transform layoutList;

		int poolCapacity = 3;
		int activeBorderIndex;  // Last index of active element	

		List<StoreSlotBehavior> displayCase = new List<StoreSlotBehavior>();

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		// Called by Refresher
		void displayGoods(int listCount)
		{
			// Set Active Limit
			//activeBorderIndex = listCount - 1;

			// Need extra case?
			if (listCount > poolCapacity)
			{
				int extraCount = listCount - poolCapacity;

				// Make extra case
				for(int i = 0; i < extraCount; i++)
				{
					GameObject newCase = Instantiate(listElement, layoutList);
					displayCase.Add(newCase.GetComponent<StoreSlotBehavior>());					
				}
				poolCapacity = listCount;
			}

			// Now Display Shop
			for (int i = 0; i < listCount; i++)
			{
				// Activate the case
				displayCase[i].gameObject.SetActive(true);

				// Pick raw goods
				SellItemInfo pickedGoods = productListRef[i];

				// Pick goods
				ItemBase pickedItemMeta = itemListDic[productListRef[i].GetItemId];

				// Display
				displayCase[i].exeDisplay(
					pickedItemMeta.GetSprite,
					pickedItemMeta.GetName,
					pickedGoods.curAmount,
					pickedGoods.purchasePrice
				);
			}

			// Deactivate the rest
			for(int i = listCount; i < poolCapacity; i++)
			{
				displayCase[i].gameObject.SetActive(false);
			}
		}

		void deActiveAllCase()
		{

		}

		// Called by Initializer
		void linkOriginalCase()
		{
			// Goods list linker
			for (int i = 0; i < poolCapacity; i++)
			{
				/*
				Button eachCtrl = layoutList.GetChild(i).GetComponent<Button>();
				//print(i);
				eachCtrl.onClick.AddListener
				(
					() =>
					{
						exeReportSelectedItemSlot(eachCtrl.transform.GetSiblingIndex());
					}
				);*/

				//print(itemSlotGroup.GetChild(i).gameObject.name);
				displayCase.Add(layoutList.GetChild(i).GetComponent<StoreSlotBehavior>());
			}
		}
	}
}