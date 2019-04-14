using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GLCore;

namespace CityStage
{
    public class StockManager : MonoSingletonBase<StockManager>
    {
        public struct StockPriceData
        {
            public StockId stockType;
            //public string stockName;
            //public uint stockPrice;
            public int stockPriceChange;
            public bool isBackruptcy;
        }

        private StockMetadata stockMetadata;
        private LocalStorage localStorage;

        public uint changeTimeSecond;
        public uint maxChangeNum;

        private void Start()
        {
            stockMetadata = LinkContainer.Instance.stockMetadata;
            localStorage = LinkContainer.Instance.localStorage;
        }

        public bool PurchaseStock(StockId stockType, uint value)
        {
            if (localStorage.playerProperty.GetCash >= (stockMetadata.stockDic[stockType].GetCurPrice * value))
            {
                localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease,
                    (decimal)stockMetadata.stockDic[stockType].GetCurPrice * value);

                stockMetadata.stockDic[stockType].ChangeStockData(StockChangeInfo.playerHave, ValueChangeInfo.increase, value);
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool SellStock(StockId stockType, uint value)
        {
            if (stockMetadata.stockDic[stockType].GetPlayerHave >= value)
            {
                localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.increase,
                    (decimal)stockMetadata.stockDic[stockType].GetCurPrice * value);

                stockMetadata.stockDic[stockType].ChangeStockData(StockChangeInfo.playerHave, ValueChangeInfo.decrease, value);
            }
            else
            {
                return false;
            }

            return true;
        }

        //View Part
        public List<StockPriceData> GetStockPriceDatas()
        {
            List<StockPriceData> stockPriceDatas = new List<StockPriceData>();

            int changeNum = GLAPI.GetTimeChangeAmount(localStorage.recentTimeData.GetStockRecentTime, changeTimeSecond, maxChangeNum);

            if (changeNum > 0)
            {
                for (StockId id = StockId.none+1; id < StockId.last; id++)
                {
                    StockData stockData = stockMetadata.stockDic[id];

                    StockPriceData stockPriceData;

                    stockPriceData.stockType = id;
                    stockPriceData.isBackruptcy = false;

                    ulong prevStockPrice = stockData.GetCurPrice;

                    for (int j = 0; j < changeNum; id++)
                    {
                        uint randomValue = (uint)UnityEngine.Random.Range(stockData.GetMinChangePrice, stockData.GetMaxChangePrice + 1);

                        if ((int)UnityEngine.Random.Range(0, 2) == 0)
                        {
                            stockData.ChangeStockData(StockChangeInfo.curPrice, ValueChangeInfo.increase, randomValue);
                        }
                        else
                        {
                            if ((randomValue > stockData.GetCurPrice)
                                || ((stockData.GetCurPrice - randomValue) < stockData.GetLowerBoundPrice))
                            {
                                stockData.ChangeStockData(StockChangeInfo.curPrice, ValueChangeInfo.set, stockData.GetStartPrice);

                                if (stockData.GetPlayerHave > 0)
                                {
                                    stockPriceData.isBackruptcy = true;
                                    stockData.ChangeStockData(StockChangeInfo.playerHave, ValueChangeInfo.set, 0);
                                    //DataManager.GetInstance().localStorage.playerProperty.ChangeStock(stockData.GetStockType, ValueChangeInfo.set, 0);
                                }

                                break;
                            }

                            stockData.ChangeStockData(StockChangeInfo.curPrice, ValueChangeInfo.decrease, randomValue);
                        }
                    }

                    //stockPriceData.stockType = stockData.GetStockType;
                    //stockPriceData.stockName = stockData.GetStockName;
                    //stockPriceData.stockPrice = stockData.GetCurPrice;
                    stockPriceData.stockPriceChange = (int)stockData.GetCurPrice - (int)prevStockPrice;
                    stockPriceDatas.Add(stockPriceData);
                }
            }

            return stockPriceDatas;
        }
    }
}