using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
    [CreateAssetMenu(fileName = "ItemMetadata(CityStage)", menuName = "Storages/ItemMetadata(CityStage)")]
    public class ItemMetadata : ScriptableObject
    {
        [SerializeField] private List<EquipmentItem> equipmentHeadDatas;
        [SerializeField] private List<EquipmentItem> equipmentPickaxDatas;
        [SerializeField] private List<EquipmentItem> equipmentShoesDatas;
        [SerializeField] private List<LotteryItem> lotteryItemDatas;
        [SerializeField] private List<OreItem> oreItemDatas;
        [SerializeField] private List<RandomBoxItem> randomBoxDatas;
        [SerializeField] private List<FoodItem> foodItemDatas;
        [SerializeField] private List<BuffItem> buffItemDatas;
        [SerializeField] private List<OtherItem> otherItemDatas;
        [SerializeField] private List<ExpItem> expItemDatas;

        public Dictionary<ItemId, ItemBase> itemDataDic = new Dictionary<ItemId, ItemBase>();

        public void SetDicData()
        {
            foreach (ItemBase data in equipmentHeadDatas)
            {
                itemDataDic.Add(data.GetId, data);
            }
            foreach (ItemBase data in equipmentPickaxDatas)
            {
                itemDataDic.Add(data.GetId, data);
            }
            foreach (ItemBase data in equipmentShoesDatas)
            {
                itemDataDic.Add(data.GetId, data);
            }
            foreach (ItemBase data in lotteryItemDatas)
            {
                itemDataDic.Add(data.GetId, data);
            }
            foreach (ItemBase data in randomBoxDatas)
            {
                itemDataDic.Add(data.GetId, data);
            }
            foreach (ItemBase data in oreItemDatas)
            {
                itemDataDic.Add(data.GetId, data);
            }
            foreach (ItemBase data in foodItemDatas)
            {
                itemDataDic.Add(data.GetId, data);
            }
            foreach (ItemBase data in buffItemDatas)
            {
                itemDataDic.Add(data.GetId, data);
            }
            foreach (ItemBase data in otherItemDatas)
            {
                itemDataDic.Add(data.GetId, data);
            }
            foreach (ItemBase data in expItemDatas)
            {
                itemDataDic.Add(data.GetId, data);
            }
        }

		public Color commonItemFrameColor;
		public Color rareItemFrameColor;
		public Color uniqueItemFrameColor;
	}

    [Serializable]
    public abstract class ItemBase
    {
        private ItemType itemTypeDefault = ItemType.other;
        [SerializeField] private ItemRarity itemRarity;
        [SerializeField] private string name;
        [SerializeField] private uint maxAmount;
        [SerializeField] private int purchasePrice;
        [SerializeField] private int purchasePriceGold;
        [SerializeField] private int sellPrice;
        [SerializeField] private string description;
        [SerializeField] private Sprite itemImg;

        public virtual ItemType GetItemType { get { return itemTypeDefault; } }
        public virtual ItemId GetId { get { return ItemId.NotExistId; } }
        public virtual uint GetMaxAmount { get { return maxAmount; } }
        public int GetPurchasePrice { get { return purchasePrice; } }
        public int GetPurchasePriceGold { get { return purchasePriceGold; } }
        public int GetSellPrice { get { return sellPrice; } }
        public string GetName { get { return name; } }
        public Sprite GetSprite { get { return itemImg; } }
        public virtual string GetDescription { get { return description; } }
        public virtual ItemStatFactor GetItemStatFactor { get { return null; } }
        public ItemRarity GetItemRarity { get { return itemRarity; } }

        public virtual bool UseItem(uint num)
        {
            return false;
        }

        public virtual bool UseItem()
        {
            return false;
        }
    }
}