using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
    [CreateAssetMenu(fileName = "StoreMetadata(CityStage)", menuName = "Storages/StoreMetadata(CityStage)")]
    public class StoreMetadata : ScriptableObject
    {
        [SerializeField] [Range(1f, 2f)] public List<float> affectionBonusPerCharm;
        [SerializeField] [Range(1f, 2f)] public List<float> maxAffectionPerCharm;
        [SerializeField] private List<StoreData> storeNpcList;
        public Dictionary<StoreNpcId, StoreData> storeNpcDic = new Dictionary<StoreNpcId, StoreData>();

        public void SetDicData()
        {
            foreach(StoreData data in storeNpcList)
            {
                storeNpcDic.Add(data.GetNpcId, data);
            }
        }     
    }

    [Serializable]
    public class SellItemInfo
    {
        [SerializeField] private ItemId itemId;
        public uint curAmount;
        public int purchasePrice;
        public int purchasePriceGold;
        private float incAffectionAmount;

        public SellItemInfo(ItemId itemId, uint curAmount, int purchasePrice, int purchasePriceGold, float incAffectionAmount)
        {
            this.itemId = itemId;
            this.curAmount = curAmount;
            this.purchasePrice = purchasePrice;
            this.purchasePriceGold = purchasePriceGold;
            this.incAffectionAmount = incAffectionAmount;
        }

        public ItemId GetItemId { get { return itemId; } }
        public float GetIncAffectionAmount { get { return incAffectionAmount; } }
    }

    [Serializable]
    public class AffectionDiscountData
    {
        [SerializeField] private float perAffection;
        [SerializeField] [Range(0f, 0.9f)] private float discountRateInc;
        const float maxDiscountRate = 0.1f;

        public double GetDiscountRate(double CurAffection)
        {
            double discountRate = 1d - (CurAffection / perAffection * discountRateInc);

            if (discountRate < maxDiscountRate)
            {
                discountRate = maxDiscountRate;
            }

            return discountRate;
        }
    }

    public struct SellItemRandomSelectList
    {
        public SellItemData data;
        public float accumulatePer;
    }

    public enum StoreNpcId
    {
        none=0, GoldSellerNpc, LotteryStoreNpc, MineAreaStoreNpc, last
    }

    [Serializable]
    public class SellItemData
    {
        [SerializeField] private ItemId itemId;
        [Range(0f, 1f)] [SerializeField] private float appearPercentage;
        [SerializeField] private float minRequireAffection;
        [SerializeField] private uint maxSellAmount;
        [SerializeField] private uint curRemainAmount;
        [SerializeField] private float incAffectionAmount;

        public ItemId GetItemId { get { return itemId; } }
        public float GetAppearPercentage { get { return appearPercentage; } }
        public uint GetMaxSellAmount { get { return maxSellAmount; } }
        public float GetMinRequireAffection { get { return minRequireAffection; } }
        public float GetIncAffectionAmount { get { return incAffectionAmount; } }
    }

    [Serializable]
    public class StoreData
    {
        [SerializeField] private StoreNpcId npcId;
        [SerializeField] private List<SellItemData> sellItemList;
        [SerializeField] private List<SellItemInfo> curSellItemList;
        [SerializeField] private AffectionDiscountData affectionDiscountDatas;
        [SerializeField] [Range(Constants.statMin, Constants.statMax)] private uint interReqiureCharmStat;
        [SerializeField] [Range(0f, 1000f)] private float maxAffection;
        [SerializeField] private uint maxSellItemAmount;

        private List<SellItemRandomSelectList> randomList = new List<SellItemRandomSelectList>();
        private List<SellItemData> randomSellItemList = new List<SellItemData>();

        public StoreNpcId GetNpcId { get { return npcId; } }
        public float GetMaxAffection { get { return maxAffection; } }
        public CustumDateTime GetRecentTime { get { return storeSaveData.recentTime; } }

        private LocalStorage localStorage;
        private StoreSaveData storeSaveData;
        public StoreSaveData SetStoreSaveData { set { storeSaveData = value; localStorage = LinkContainer.Instance.localStorage; itemDataDic = LinkContainer.Instance.itemMetadata.itemDataDic; } }

        private Dictionary<ItemId, ItemBase> itemDataDic;

        public const double MultAffectionIncPerPrice = 0.01d;
        public const double GoldPrice = 100d;

        private void RenewPurchasePrice()
        {
            foreach (SellItemInfo data in curSellItemList)
            {
                data.purchasePrice = (int)((double)LinkContainer.Instance.itemMetadata.itemDataDic[data.GetItemId].GetPurchasePrice * GetDiscountRate());                
            }
        }

        public bool PurchaseItem(int index, uint num)
        {
            if ((index < 0)
                || (index >= curSellItemList.Count)
                || (curSellItemList[index].curAmount < num))
            {
                UIManager.Instance.exeRequestToasting("아이템 구매 실패");
                return false;
            }

            if(curSellItemList[index].purchasePriceGold > 0)
            {
                if (!InventoryManager.Instance.PurchaseItem(ItemPurchaseType.gold, curSellItemList[index].GetItemId, num, (uint)curSellItemList[index].purchasePriceGold))
                {
                    UIManager.Instance.exeRequestToasting("아이템 구매 실패");
                    return false;
                }

                if (curSellItemList[index].GetIncAffectionAmount > 0)
                {
                    storeSaveData.CurAffection += curSellItemList[index].GetIncAffectionAmount
                        * LinkContainer.Instance.storeMetadata.affectionBonusPerCharm[(int)LinkContainer.Instance.localStorage.playerStat.GetCharm - 1];
                }
                else
                {
                    storeSaveData.CurAffection += (double)curSellItemList[index].purchasePriceGold * 100d * MultAffectionIncPerPrice;
                }
            }
            else if (curSellItemList[index].purchasePrice >= 0)
            {
                if (!InventoryManager.Instance.PurchaseItem(ItemPurchaseType.cash, curSellItemList[index].GetItemId, num, (uint)curSellItemList[index].purchasePrice))
                {
                    UIManager.Instance.exeRequestToasting("아이템 구매 실패");
                    return false;
                }

                if (curSellItemList[index].GetIncAffectionAmount > 0)
                {
                    storeSaveData.CurAffection += curSellItemList[index].GetIncAffectionAmount
                        * LinkContainer.Instance.storeMetadata.affectionBonusPerCharm[(int)LinkContainer.Instance.localStorage.playerStat.GetCharm - 1];
                }
                else
                {
                    storeSaveData.CurAffection += (double)curSellItemList[index].purchasePrice * MultAffectionIncPerPrice;
                }
            }

            curSellItemList[index].curAmount--;

            float totalMaxAffection = maxAffection * LinkContainer.Instance.storeMetadata.maxAffectionPerCharm[(int)LinkContainer.Instance.localStorage.playerStat.GetCharm - 1];

            if (storeSaveData.CurAffection > totalMaxAffection)
            {
                storeSaveData.CurAffection = totalMaxAffection;
            }

            RenewPurchasePrice();

            return true;
        }

        public void RenewItemList()
        {
            uint remainItemAmount = maxSellItemAmount;

            curSellItemList.Clear();
            randomSellItemList.Clear();

            double discountRate = GetDiscountRate();

            // 확률이 100%인 아이템부터 판매목록 리스트를 채운다
            foreach (SellItemData data in sellItemList)
            {
                if ((remainItemAmount > 0) && (storeSaveData.CurAffection >= data.GetMinRequireAffection))
                {
                    if (data.GetAppearPercentage >= 1f)
                    {
                        SellItemInfo sellItemInfo = new SellItemInfo(data.GetItemId, data.GetMaxSellAmount,
                            (int)((double)itemDataDic[data.GetItemId].GetPurchasePrice * GetDiscountRate()),
                            itemDataDic[data.GetItemId].GetPurchasePriceGold,
                            data.GetIncAffectionAmount);
                        curSellItemList.Add(sellItemInfo);
                        remainItemAmount--;
                    }
                    else if (data.GetAppearPercentage > 0f)
                    {
                        randomSellItemList.Add(data);
                    }
                }
                //else
                //{
                //    break;
                //}
            }

            // randomSellItemList = 남은 확률 0~1 사이의 아이템 리스트
            // randomList = 각각 아이템의 획득 확률과 데이터를 담은 리스트

            float accumulatePerTmp;

            for (int i = 0; i < remainItemAmount; i++)
            {
                if (randomSellItemList.Count <= 0)
                {
                    break;
                }

                accumulatePerTmp = 0f;
                randomList.Clear();

                foreach (SellItemData data in randomSellItemList)
                {
                    SellItemRandomSelectList randomListEntity;
                    randomListEntity.data = data;
                    accumulatePerTmp += data.GetAppearPercentage;
                    randomListEntity.accumulatePer = accumulatePerTmp;
                    randomList.Add(randomListEntity);
                }

                float randomPer = UnityEngine.Random.Range(0f, accumulatePerTmp);

                for (int j = 0; j < randomList.Count; j++)
                {
                    if (randomPer <= randomList[j].accumulatePer)
                    {
                        SellItemInfo sellItemInfo = new SellItemInfo(randomList[j].data.GetItemId, randomList[j].data.GetMaxSellAmount,
                            (int)((double)itemDataDic[randomList[j].data.GetItemId].GetPurchasePrice * GetDiscountRate()),
                            itemDataDic[randomList[j].data.GetItemId].GetPurchasePriceGold,
                            randomList[j].data.GetIncAffectionAmount);
                        curSellItemList.Add(sellItemInfo);
                        randomSellItemList.Remove(randomList[j].data);
                        break;
                    }
                }
            }
        }

        public double GetDiscountRate()
        {
            return affectionDiscountDatas.GetDiscountRate(storeSaveData.CurAffection);
        }

        public TimeSpan GetRemainTime()
        {
            try
            {
                DateTime curTime = GLAPI.GetNetworkTime();

                if ((storeSaveData.recentTime.month == 0)
                    || (storeSaveData.recentTime.day == 0)
                    || (storeSaveData.recentTime.year == 0))
                {
                    storeSaveData.recentTime.year = curTime.Year;
                    storeSaveData.recentTime.month = curTime.Month;
                    storeSaveData.recentTime.day = curTime.Day;
                    storeSaveData.recentTime.hour = curTime.Hour;
                    storeSaveData.recentTime.minute = curTime.Minute;
                    storeSaveData.recentTime.second = curTime.Second;

                    localStorage.SaveToLocal(FileSaveType.store);

                    return TimeSpan.Zero;
                }

                DateTime recentTmpTime = new DateTime(storeSaveData.recentTime.year,
                                storeSaveData.recentTime.month,
                                storeSaveData.recentTime.day,
                                storeSaveData.recentTime.hour,
                                storeSaveData.recentTime.minute,
                                storeSaveData.recentTime.second);

                TimeSpan remainTime = TimeSpan.FromDays(1d) - (curTime - recentTmpTime);

                if (remainTime < TimeSpan.Zero)
                {
                    remainTime = TimeSpan.Zero;
                }

                return remainTime;
            }
            catch
            {
                return TimeSpan.MaxValue;
            }
        }

        public List<SellItemInfo> GetItemList()
        {
            try
            {
                if (GetRemainTime() <= TimeSpan.Zero)
                {
                    DateTime curTime = GLAPI.GetNetworkTime();

                    storeSaveData.recentTime.year = curTime.Year;
                    storeSaveData.recentTime.month = curTime.Month;
                    storeSaveData.recentTime.day = curTime.Day;
                    storeSaveData.recentTime.hour = curTime.Hour;
                    storeSaveData.recentTime.minute = curTime.Minute;
                    storeSaveData.recentTime.second = curTime.Second;

                    localStorage.SaveToLocal(FileSaveType.store);

                    RenewItemList();
                }

                return curSellItemList;

                //DateTime curTime = GLAPI.GetNetworkTime();

                //if ((curTime.Year != storeSaveData.RecentTime.year)
                //    || (curTime.Month != storeSaveData.RecentTime.month)
                //    || ((curTime.Month == storeSaveData.RecentTime.month) && (curTime.Day != storeSaveData.RecentTime.day)))
                //{
                //    RenewItemList();

                //    storeSaveData.RecentTime.year = curTime.Year;
                //    storeSaveData.RecentTime.month = curTime.Month;
                //    storeSaveData.RecentTime.day = curTime.Day;
                //    storeSaveData.RecentTime.hour = curTime.Hour;
                //}

            }
            catch
            {
                return null;
            }
        }
    }
}