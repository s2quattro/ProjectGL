using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;
using System.Text;

namespace CityStage
{
    [Serializable]
    public class RandomBoxItem : ItemBase
    {
        [SerializeField] private RandomBoxId id;
        private ItemType itemType = ItemType.expendableSingle;

        public override ItemId GetId { get { return (ItemId)id; } }
        public override ItemType GetItemType { get { return ItemType.expendableSingle; } }

        [SerializeField] private uint itemSlotCount;
        [SerializeField] private List<RandomItemData> randomItemList;

        private List<ItemId> randomItemSlotList = new List<ItemId>();
        private List<RandomItemData> tmpRandomItemIdList = new List<RandomItemData>();
        private List<float> accumulateProbabilityNumList = new List<float>();
        private float accumulateProbabilityNum;

        public List<RandomItemData> GetRandomItemList { get { return randomItemList; } }

        public override string GetDescription
        {
            get
            {
                string commonColor = string.Format("<#{0}>", ColorUtility.ToHtmlStringRGB(LinkContainer.Instance.itemMetadata.commonItemFrameColor));
                string rareColor = string.Format("<#{0}>", ColorUtility.ToHtmlStringRGB(LinkContainer.Instance.itemMetadata.rareItemFrameColor));
                string uniqueColor = string.Format("<#{0}>", ColorUtility.ToHtmlStringRGB(LinkContainer.Instance.itemMetadata.uniqueItemFrameColor));
                string colorHtmlEnd = "</color>";

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(base.GetDescription);
                stringBuilder.Append("\n획득 가능한 아이템 목록\n");

                ItemMetadata itemMetadata = LinkContainer.Instance.itemMetadata;

                foreach(RandomItemData data in randomItemList)
                {
                    stringBuilder.Append("\n");
                    switch(itemMetadata.itemDataDic[data.GetItemId].GetItemRarity)
                    {
                        case ItemRarity.common:
                            {
                                stringBuilder.Append(commonColor);
                                break;
                            }
                        case ItemRarity.rare:
                            {
                                stringBuilder.Append(rareColor);
                                break;
                            }
                        case ItemRarity.unique:
                            {
                                stringBuilder.Append(uniqueColor);
                                break;
                            }
                    }

                    stringBuilder.Append(itemMetadata.itemDataDic[data.GetItemId].GetName);
                    stringBuilder.Append(colorHtmlEnd);
                }

                return stringBuilder.ToString();
            }
        }

        [Serializable]
        public struct RandomItemData
        {
            [SerializeField] private ItemId itemId;
            [SerializeField] private float relativeProbability;

            public ItemId GetItemId { get { return itemId; } }
            public float GetRelativeProbability { get { return relativeProbability; } }
        }

        public override bool UseItem()
        {
            if (!InventoryManager.Instance.IsEnoughInventorySpace())
            {
                UIManager.Instance.exeRequestToasting("인벤토리 공간이 부족합니다");
                return false;
            }

            float randomValue;
            ItemId randomSelectItemId;

            randomItemSlotList.Clear();
            tmpRandomItemIdList.Clear();

            foreach (RandomItemData data in randomItemList)
            {
                tmpRandomItemIdList.Add(data);
            }

            for (int i = 0; i < itemSlotCount; i++)
            {
                accumulateProbabilityNum = 0f;
                accumulateProbabilityNumList.Clear();

                foreach (RandomItemData data in tmpRandomItemIdList)
                {
                    accumulateProbabilityNum += data.GetRelativeProbability;
                    accumulateProbabilityNumList.Add(accumulateProbabilityNum);
                }

                randomValue = UnityEngine.Random.Range(0f, accumulateProbabilityNum);

                for (int j = 0; j < accumulateProbabilityNumList.Count; j++)
                {
                    if (randomValue <= accumulateProbabilityNumList[j])
                    {
                        randomItemSlotList.Add(tmpRandomItemIdList[j].GetItemId);
                        tmpRandomItemIdList.Remove(tmpRandomItemIdList[j]);
                        break;
                    }
                }
            }

            int randomIndex = UnityEngine.Random.Range(0, randomItemSlotList.Count);

            randomSelectItemId = randomItemSlotList[randomIndex];

            if (!InventoryManager.Instance.GetItem(randomSelectItemId))
            {
                return false;
            }

            // UI 매니저 함수 호출. 매개변수 randomItemSlotList, randomSelectItemId

            return true;
        }
    }
}