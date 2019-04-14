using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;
using System.Text;

namespace CityStage
{
    [Serializable]
    public class LotteryItem : ItemBase
    {
        [Serializable]
        public class LotteryData
        {
            [SerializeField] private uint prizeMoney;
            [SerializeField] private uint percentageValue;

            public uint GetPrizeMoney { get { return prizeMoney; } }
            public uint GetPercentage { get { return percentageValue; } }
        }

        public struct LotteryInfoData
        {
            public uint prize;
            public float percentage;
        }

        public struct LotteryResultData
        {
            public uint rank;
            public uint winNum;
            public decimal totalPrize;
        }

        public override string GetDescription
        {
            get
            {
                return String.Format("{0} \n\n {1}", base.GetDescription, GetLotteryInfoDatas());
            }
        }

        [SerializeField] private LotteryId id;
        private ItemType itemType = ItemType.expendable;

        public override ItemId GetId { get { return (ItemId)id; } }
		public override ItemType GetItemType { get { return ItemType.expendable; } }

		[SerializeField] private LotteryData[] lotteryDatas;

        private List<uint> accumulateValues = new List<uint>();

        public override bool UseItem(uint num)
        {
            LotteryResultData[] lotteryResultDatas = new LotteryResultData[lotteryDatas.Length];

            for (uint i = 0; i < lotteryResultDatas.Length; i++)
            {
                lotteryResultDatas[i].rank = i + 1;
                lotteryResultDatas[i].totalPrize = 0;
                lotteryResultDatas[i].winNum = 0;
            }

            for (int i = 0; i < num; i++)
            {
                uint preAccumulateVaule = 0;

                accumulateValues.Clear();

                for (int j = 0; j < lotteryDatas.Length; j++)
                {
                    accumulateValues.Add(preAccumulateVaule + lotteryDatas[j].GetPercentage);
                    preAccumulateVaule = accumulateValues[j];
                }

                if (preAccumulateVaule > 0)
                {
                    uint randomValue = (uint)UnityEngine.Random.Range(0, preAccumulateVaule + 1);

                    for (int k = 0; k < accumulateValues.Count; k++)
                    {
                        if ((accumulateValues[k] > 0) && (randomValue <= accumulateValues[k]))
                        {
                            LinkContainer.Instance.localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.increase, lotteryDatas[k].GetPrizeMoney);
                            lotteryResultDatas[k].totalPrize += lotteryDatas[k].GetPrizeMoney;

                            lotteryResultDatas[k].winNum++;
                            break;
                        }
                    }
                }
                else
                {
                    Debug.Log("Lottery Percentage Values Not Founded.");
                    return false;                    
                }
            }

            string lotteryResultString = "";

            foreach(LotteryResultData data in lotteryResultDatas)
            {
                if (data.winNum > 0)
                {
                    if(data.rank == lotteryDatas.Length)
                    {
                        lotteryResultString += string.Format("꽝 X {0} = {1}\n", data.winNum, GLAPI.convertCashToWon(data.totalPrize));
                    }
                    else
                    {
                        lotteryResultString += string.Format("{0}등 X {1} = {2}\n", data.rank, data.winNum, GLAPI.convertCashToWon(data.totalPrize));
                    }
                }
            }

            UIManager.Instance.exeOpenSimpleMessageBox("당첨 결과", lotteryResultString);

            return true;
        }

        public string GetLotteryInfoDatas()
        {
            List<LotteryInfoData> lotteryPrizeDatas = new List<LotteryInfoData>();

            uint totalPercentage = 0;
            string lotteryDescription = null;

            foreach (LotteryData data in lotteryDatas)
            {
                totalPercentage += data.GetPercentage;
            }

            for(int i=0; i<lotteryDatas.Length; i++)
            {
                if (i == (lotteryDatas.Length - 1))
                {

                    lotteryDescription += string.Format("꽝 : {0} [{1:p}]\n", GLAPI.convertCashToWon(lotteryDatas[i].GetPrizeMoney), (float)lotteryDatas[i].GetPercentage / (float)totalPercentage);
                    break;
                }

                lotteryDescription += string.Format("{0} 등 : {1} [{2:p}]\n", (i + 1), GLAPI.convertCashToWon(lotteryDatas[i].GetPrizeMoney), (float)lotteryDatas[i].GetPercentage / (float)totalPercentage);
            }

            return lotteryDescription;
        } 
    }
}