using GLCore;
using System.Collections.Generic;
using UnityEngine;

namespace CityStage
{
    public enum ItemPurchaseType
    {
        cash = 0, gold = 1
    }

    public class InventoryManager : MonoSingletonBase<InventoryManager>
    {
        private LocalStorage localStorage = null;
        private ItemMetadata itemMetadata = null;

        private decimal itemTotalPrice;

        public const int DefaultInventoryCount = 5;
        public const int InventoryExpansionCountInc = 5;
        public const int InventoryExpansionMaxNum = 14;
        public const int InventoryExpansionCostIsGold = -1;
        public const int InventoryExpansionCostIsCash = 1;
        public const int InventoryExpansionMax = 0;

        [SerializeField] private List<int> costsForInventoryExpansion;

        // 아이템 정보 불러오기(UI에서 호출하는 함수)
        // 아이템 획득, 사용

        private void Start()
        {
            localStorage = LinkContainer.Instance.localStorage;
            itemMetadata = LinkContainer.Instance.itemMetadata;            
        }

        public bool IsEnoughInventorySpace(uint num = 1)
        {
            if(localStorage.inventoryItems.Count+num <= localStorage.playerStat.GetInventoryMaxAmount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public uint GetItemCurAmount(ItemId itemId)
        {
            foreach(InventoryItemList item in localStorage.inventoryItems)
            {
                if(item.id == itemId)
                {
                    return item.curAmount;
                }
            }

            return 0;
        }

        public int GetInventoryExpansionCost()
        {
            int index = ((int)localStorage.playerStat.GetInventoryMaxAmount / InventoryExpansionCountInc) - 1;

            if (index >= 14)
            {
                return InventoryExpansionMax;
            }

            return costsForInventoryExpansion[index];
        }

        public bool InventoryExpansion()
        {
            int index = ((int)localStorage.playerStat.GetInventoryMaxAmount / InventoryExpansionCountInc) - 1;

            if (index >= InventoryExpansionMaxNum)
            {
                UIManager.Instance.exeRequestToasting("인벤토리를 더이상 확장할 수 없습니다");
                return false;
            }

            if(costsForInventoryExpansion[index] >= Constants.CostIsCash)
            {
                if (localStorage.playerProperty.GetCash < costsForInventoryExpansion[index])
                {
                    UIManager.Instance.exeRequestToasting("현금이 부족합니다");
                    return false;
                }
                else
                {
                    localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, costsForInventoryExpansion[index]);
                }
            }
            else if (costsForInventoryExpansion[index] <= Constants.CostIsGold)
            {
                if (localStorage.playerProperty.GetGold < (ulong)(-costsForInventoryExpansion[index]))
                {
                    UIManager.Instance.exeRequestToasting("금이 부족합니다");
                    return false;
                }
                else
                {
                    localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.gold, ValueChangeInfo.decrease, (ulong)(-costsForInventoryExpansion[index]));
                }
            }
        
            localStorage.playerStat.ChangeStat(PlayerStatChangeInfo.inventoryMaxAmount, ValueChangeInfo.increase, InventoryExpansionCountInc);

            return true;
        }

        public EquipmentItemListStruct GetEquipmentItems()
        {
            EquipmentItemListStruct equipmentList;

            equipmentList.head = localStorage.equipmentItems.GetEquipmentId(EquipmentType.head);
            equipmentList.pickax = localStorage.equipmentItems.GetEquipmentId(EquipmentType.pickax);
            equipmentList.shoes = localStorage.equipmentItems.GetEquipmentId(EquipmentType.shoes);

            return equipmentList;
        }

        public bool EquipItem(ItemId itemId)
        {
            if (itemId == ItemId.NotExistId)
            {
                UIManager.Instance.exeRequestToasting("올바르지 않은 아이템입니다");
                return false;
            }

            if (itemMetadata.itemDataDic[itemId].GetItemType != ItemType.equipment)
            {
                UIManager.Instance.exeRequestToasting("장비 아이템이 아닙니다");
                return false;
            }

            bool isItemExist = false;

            foreach (InventoryItemList data in localStorage.inventoryItems)
            {
                if (data.id == itemId)
                {
                    isItemExist = true;
                    break;
                }
            }

            if (!isItemExist)
            {
                UIManager.Instance.exeRequestToasting("아이템이 인벤토리에 존재하지 않습니다");
                return false;
            }

            EquipmentItem itemData = (EquipmentItem)itemMetadata.itemDataDic[itemId];

            ItemId id;

            id = localStorage.equipmentItems.GetEquipmentId(itemData.GetEquipmentType);
            id = CheckEquipEnable(id, itemId);

            if (id == ItemId.NotExistId)
            {
                return false;
            }

            localStorage.equipmentItems.ChangeEquipmentId(itemData.GetEquipmentType, id);

			// Refreshing
            StatFactorManager.Instance.RenewStatFactor();

            return true;
        }

        private ItemId CheckEquipEnable(ItemId equipmentItemId, ItemId inventorytItemId)
        {
            if (equipmentItemId == ItemId.NotExistId)
            {
                if (RemoveItem(inventorytItemId, 1))
                {
                    equipmentItemId = inventorytItemId;
                }
                else
                {
                    UIManager.Instance.exeRequestToasting("아이템 장착에 실패했습니다");
                    return ItemId.NotExistId;
                }
            }
            else
            {
                if (RemoveItem(inventorytItemId, 1))
                {
                    if (GetItem(equipmentItemId, 1))
                    {
                        equipmentItemId = inventorytItemId;
                    }
                    else
                    {
                        GetItem(equipmentItemId, 1);
                        UIManager.Instance.exeRequestToasting("아이템 장착에 실패했습니다");
                        return ItemId.NotExistId;
                    }
                }
                else
                {
                    UIManager.Instance.exeRequestToasting("아이템 장착에 실패했습니다");
                    return ItemId.NotExistId;
                }
            }

            return equipmentItemId;
        }

        public bool UnEquipItem(EquipmentType equipmentType)
        {
            if (localStorage.equipmentItems.GetEquipmentId(equipmentType) == ItemId.NotExistId)
            {
                UIManager.Instance.exeRequestToasting("장착중인 아이템이 없습니다");
                return false;
            }

            if (GetItem(localStorage.equipmentItems.GetEquipmentId(equipmentType), 1))
            {
                localStorage.equipmentItems.ChangeEquipmentId(equipmentType, ItemId.NotExistId);
            }
            else
            {
                UIManager.Instance.exeRequestToasting("아이템 장착해제에 실패했습니다");
                return false;
            }

			// Refreshing
			StatFactorManager.Instance.RenewStatFactor();

            return true;
        }

        private void UpdateInventoryCurAmount()
        {
            uint inventoryCurAmount = 0;

            int count = localStorage.inventoryItems.Count;

            for (int i = 0; i < count; i++)
            {
                if (localStorage.inventoryItems[i].curAmount > 0)
                {
                    uint itemSpace = localStorage.inventoryItems[i].curAmount / itemMetadata.itemDataDic[localStorage.inventoryItems[i].id].GetMaxAmount;

                    if ((localStorage.inventoryItems[i].curAmount % itemMetadata.itemDataDic[localStorage.inventoryItems[i].id].GetMaxAmount) > 0)
                    {
                        itemSpace++;
                    }

                    inventoryCurAmount += itemSpace;
                }
                else
                {
                    localStorage.inventoryItems.Remove(localStorage.inventoryItems[i]);
                    count--;
                    i--;
                }
            }

            if (localStorage.playerStat.GetInventoryCurAmount != inventoryCurAmount)
            {
                localStorage.playerStat.ChangeStat(PlayerStatChangeInfo.inventoryCurAmount, ValueChangeInfo.set, inventoryCurAmount);
            }
        }

        public List<InventoryItemListStruct> GetInventoryItems(uint startIndex, ItemType itemType = ItemType.nonSelect)
        {
            UpdateInventoryCurAmount();

            if (startIndex > (localStorage.playerStat.GetInventoryCurAmount - 1))
            {
                Debug.Log("index out of range");
                return null;
            }
            else if (localStorage.playerStat.GetInventoryCurAmount < 1)
            {
                Debug.Log("have no item");
                return null;
            }

            List<InventoryItemListStruct> itemDatas = new List<InventoryItemListStruct>();

            if (itemType != ItemType.nonSelect)
            {
                foreach (InventoryItemList data in localStorage.inventoryItems)
                {
                    if (itemMetadata.itemDataDic[data.id].GetItemType == itemType)
                    {
                        uint div = data.curAmount / itemMetadata.itemDataDic[data.id].GetMaxAmount;
                        uint rem = data.curAmount % itemMetadata.itemDataDic[data.id].GetMaxAmount;

                        InventoryItemListStruct itemList;

                        for (int i = 0; i < div; i++)
                        {
                            itemList.id = data.id;
                            itemList.curAmount = itemMetadata.itemDataDic[data.id].GetMaxAmount;
                            itemDatas.Add(itemList);
                        }

                        if (rem > 0)
                        {
                            itemList.id = data.id;
                            itemList.curAmount = rem;
                            itemDatas.Add(itemList);
                        }
                    }
                }
            }

            foreach (InventoryItemList data in localStorage.inventoryItems)
            {
                if (itemMetadata.itemDataDic[data.id].GetItemType != itemType)
                {
                    uint div = data.curAmount / itemMetadata.itemDataDic[data.id].GetMaxAmount;
                    uint rem = data.curAmount % itemMetadata.itemDataDic[data.id].GetMaxAmount;

                    InventoryItemListStruct itemList;

                    for (int i = 0; i < div; i++)
                    {
                        itemList.id = data.id;
                        itemList.curAmount = itemMetadata.itemDataDic[data.id].GetMaxAmount;
                        itemDatas.Add(itemList);
                    }

                    if (rem > 0)
                    {
                        itemList.id = data.id;
                        itemList.curAmount = rem;
                        itemDatas.Add(itemList);
                    }
                }
            }

            List<InventoryItemListStruct> itemDatasCopy = new List<InventoryItemListStruct>();

            uint lastIndex = startIndex + 14;

            if (itemDatas.Count - 1 < lastIndex)
            {
                lastIndex = (uint)itemDatas.Count - 1;
            }

            for (int i = (int)startIndex; i <= lastIndex; i++)
            {
                itemDatasCopy.Add(itemDatas[i]);
            }

            return itemDatasCopy;
        }

        public bool GetItem(ItemId itemId, uint num = 1)
        {
            if (itemId == ItemId.NotExistId)
            {
                UIManager.Instance.exeRequestToasting("올바르지 않은 아이템입니다");
                return false;
            }

            UpdateInventoryCurAmount();

            uint itemCurAmount = 0;

            foreach (InventoryItemList data in localStorage.inventoryItems)
            {
                if (data.id == itemId)
                {
                    itemCurAmount = data.curAmount;
                    break;
                }
            }

            uint curInventorySpace = itemCurAmount / itemMetadata.itemDataDic[itemId].GetMaxAmount;

            if ((itemCurAmount % itemMetadata.itemDataDic[itemId].GetMaxAmount) > 0)
            {
                curInventorySpace++;
            }

            uint combInventorySpace = (itemCurAmount + num) / itemMetadata.itemDataDic[itemId].GetMaxAmount;

            if (((itemCurAmount + num) % itemMetadata.itemDataDic[itemId].GetMaxAmount) > 0)
            {
                combInventorySpace++;
            }

            if ((localStorage.playerStat.GetInventoryCurAmount + (combInventorySpace - curInventorySpace)) > localStorage.playerStat.GetInventoryMaxAmount)
            {
                UIManager.Instance.exeRequestToasting("인벤토리 공간 부족");
                return false;
            }

            foreach (InventoryItemList list in localStorage.inventoryItems)
            {
                if (list.id == itemId)
                {
                    list.curAmount += num;
                    UpdateInventoryCurAmount();

                    localStorage.SaveToLocal(FileSaveType.inventory);
                    return true;                    
                }
            }

            localStorage.inventoryItems.Add(new InventoryItemList(itemId, num));
            UpdateInventoryCurAmount();

            localStorage.SaveToLocal(FileSaveType.inventory);
            return true;
        }

        public bool RemoveItem(ItemId itemId, uint num)
        {
            if (itemId == ItemId.NotExistId)
            {
                UIManager.Instance.exeRequestToasting("올바르지 않은 아이템입니다");
                return false;
            }

            foreach (InventoryItemList list in localStorage.inventoryItems)
            {
                if ((list.id == itemId) && (list.curAmount >= num))
                {
                    list.curAmount -= num;
                    UpdateInventoryCurAmount();

                    localStorage.SaveToLocal(FileSaveType.inventory);

                    return true;
                }
            }

            UpdateInventoryCurAmount();

            UIManager.Instance.exeRequestToasting("아이템 부족");
                        
            return false;
        }

        public bool UseItem(ItemId itemId)
        {
            if (itemId == ItemId.NotExistId)
            {
                UIManager.Instance.exeRequestToasting("올바르지 않은 아이템입니다");
                return false;
            }

            foreach (InventoryItemList list in localStorage.inventoryItems)
            {
                if (list.id == itemId)
                {
                    if (itemMetadata.itemDataDic[itemId].UseItem())
                    {
                        list.curAmount --;
                        UpdateInventoryCurAmount();
                        localStorage.SaveToLocal(FileSaveType.inventory);
                        return true;
                    }
                }
            }

            UIManager.Instance.exeRequestToasting("아이템 부족");
            return false;
        }

        public bool UseItem(ItemId itemId, uint num)
        {
            if (itemId == ItemId.NotExistId)
            {
                UIManager.Instance.exeRequestToasting("올바르지 않은 아이템입니다");
                return false;
            }

            foreach (InventoryItemList list in localStorage.inventoryItems)
            {
                if ((list.id == itemId) && (list.curAmount >= num))
                {
                    if (itemMetadata.itemDataDic[itemId].UseItem(num))
                    {
                        list.curAmount -= num;
                        UpdateInventoryCurAmount();                        
                        localStorage.SaveToLocal(FileSaveType.inventory);
                        return true;
                    }
                }
            }

            UIManager.Instance.exeRequestToasting("아이템 부족");
            return false;
        }
        
        public bool PurchaseItem(ItemPurchaseType itemPurchaseType, ItemId itemId, uint num, ulong itemPrice)
        {
            if (itemId == ItemId.NotExistId)
            {
                UIManager.Instance.exeRequestToasting("올바르지 않은 아이템입니다");
                return false;
            }

            itemTotalPrice = itemPrice * num;

            if (itemId == ItemId.OtherGold)
            {
                if (localStorage.playerProperty.CompareCashToPrice(itemTotalPrice))
                {
                    localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, itemTotalPrice);
                    localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.gold, ValueChangeInfo.increase, num);
                }
                else
                {
                    UIManager.Instance.exeRequestToasting("재화 부족");
                    return false;
                }
            }
            else
            {
                switch (itemPurchaseType)
                {
                    case ItemPurchaseType.cash:
                        {
                            if (itemMetadata.itemDataDic[itemId].GetPurchasePrice < 0)
                            {
                                UIManager.Instance.exeRequestToasting("구매가 불가능한 아이템입니다");
                                return false;
                            }

                            if (localStorage.playerProperty.CompareCashToPrice(itemTotalPrice))
                            {
                                if (GetItem(itemId, num))
                                {
                                    localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, itemTotalPrice);
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                UIManager.Instance.exeRequestToasting("재화 부족");
                                return false;
                            }
                            break;
                        }
                    case ItemPurchaseType.gold:
                        {
                            if (itemMetadata.itemDataDic[itemId].GetPurchasePriceGold < 0)
                            {
                                UIManager.Instance.exeRequestToasting("구매가 불가능한 아이템입니다");
                                return false;
                            }

                            if (localStorage.playerProperty.GetGold >= itemTotalPrice)
                            {
                                if (GetItem(itemId, num))
                                {
                                    localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.gold, ValueChangeInfo.decrease, itemTotalPrice);
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                UIManager.Instance.exeRequestToasting("재화 부족");
                                return false;
                            }
                            break;
                        }
                    default:
                        {
                            return false;
                        }
                }
            }

            localStorage.SaveToLocal(FileSaveType.inventory);
            return true;
        }

        public bool SellItem(ItemId itemId, uint num)
        {
            if (itemId == ItemId.NotExistId)
            {
                UIManager.Instance.exeRequestToasting("올바르지 않은 아이템입니다");
                return false;
            }

            if (itemMetadata.itemDataDic[itemId].GetSellPrice < 0)
            {
                UIManager.Instance.exeRequestToasting("판매할 수 없는 아이템입니다");
                return false;
            }

            if (RemoveItem(itemId, num))
            {
                itemTotalPrice = (decimal)itemMetadata.itemDataDic[itemId].GetSellPrice * num;
                localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.increase, itemTotalPrice);
                return true;
            }
            else
            {
                UIManager.Instance.exeRequestToasting("아이템 부족");
                return false;
            }
        }

        //public bool PurchaseItem(ItemId itemId, uint num)
        //{
        //    if (itemId == ItemId.NotExistId)
        //    {
        //        UIManager.Instance.exeRequestToasting("올바르지 않은 아이템입니다");
        //        return false;
        //    }

        //    if (itemMetadata.itemDataDic[itemId].GetPurchasePrice < 0)
        //    {
        //        UIManager.Instance.exeRequestToasting("구매가 불가능한 아이템입니다");
        //        return false;

        //    }

        //    itemTotalPrice = (decimal)itemMetadata.itemDataDic[itemId].GetPurchasePrice * num;

        //    if (localStorage.playerProperty.GetCash >= itemTotalPrice)
        //    {
        //        if (GetItem(itemId, num))
        //        {
        //            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, itemTotalPrice);
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        UIManager.Instance.exeRequestToasting("재화 부족");
        //        return false;
        //    }

        //    localStorage.SaveToLocal(FileSaveType.inventory);
        //    return true;
        //}
    }
}