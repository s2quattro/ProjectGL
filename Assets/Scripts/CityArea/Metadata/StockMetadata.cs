using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
    [CreateAssetMenu(fileName = "StockMetadata(CityStage)", menuName = "Storages/StockMetadata(CityStage)")]
    public class StockMetadata : ScriptableObject
    {
        [SerializeField]
        private List<StockData> stockData;
        public Dictionary<StockId, StockData> stockDic = new Dictionary<StockId, StockData>();

        public void SetDicData()
        {
            foreach (StockData data in stockData)
            {
                stockDic.Add(data.GetStockId, data);
            }
        }
    }

    [Serializable]
    public class StockData : DataCalcEntity
    {
        [SerializeField] private StockId stockId;
        [SerializeField] private string stockName;
        [SerializeField] private uint startPrice;
        [SerializeField] private uint minChangePrice;
        [SerializeField] private uint maxChangePrice;
        [SerializeField] private uint lowerBoundPrice;

        private StockSaveData stockSaveData;
        public StockSaveData SetStockSaveData { set { stockSaveData = value; } }

        public StockId GetStockId { get { return stockId; } }
        public string GetStockName { get { return stockName; } }
        public ulong GetCurPrice { get { return stockSaveData.CurPrice; } }
        public uint GetStartPrice { get { return startPrice; } }
        public uint GetMinChangePrice { get { return minChangePrice; } }
        public uint GetMaxChangePrice { get { return maxChangePrice; } }
        public uint GetLowerBoundPrice { get { return lowerBoundPrice; } }
        public uint GetPlayerHave { get { return stockSaveData.PlayerHave; } }

        public void ChangeStockData(StockChangeInfo stockChangeInfo, ValueChangeInfo changeInfo, uint value)
        {
            switch (stockChangeInfo)
            {
                case StockChangeInfo.curPrice:
                    {
                        stockSaveData.CurPrice = CalcChangeValue(stockSaveData.CurPrice, changeInfo, value);
                        break;
                    }
                case StockChangeInfo.playerHave:
                    {
                        stockSaveData.PlayerHave = CalcChangeValue(stockSaveData.PlayerHave, changeInfo, value);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}