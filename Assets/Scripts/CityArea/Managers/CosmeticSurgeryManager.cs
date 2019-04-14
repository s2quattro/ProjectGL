using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
    public class CosmeticSurgeryManager : MonoSingletonBase<CosmeticSurgeryManager>
    {
        [SerializeField] private List<CosmeticCostData> cosmeticCostDatas;

        private LocalStorage localStorage;

        private void Start()
        {
            localStorage = LinkContainer.Instance.localStorage;
        }

        public int GetCosmeticCost()
        {
            int cost;

            if(localStorage.playerStat.GetCharm >= Constants.statMax)
            {
                cost = 0;
            }
            else
            {
                cost = cosmeticCostDatas[(int)localStorage.playerStat.GetCharm - 1].GetCost;
            }

            return cost;
        }

        public float GetCosmeticProbability()
        {
            float probability;

            if (localStorage.playerStat.GetCharm >= Constants.statMax)
            {
                probability = 0;
            }
            else
            {
                probability = cosmeticCostDatas[(int)localStorage.playerStat.GetCharm - 1].GetProbability;
            }

            return probability;
        }

        public bool StartCosmeticSurgery()
        {
            int cost = GetCosmeticCost();

            if(cost >= Constants.CostIsCash)
            {
                if(localStorage.playerProperty.GetCash < cost)
                {
                    UIManager.Instance.exeRequestToasting("현금이 부족합니다");
                    return false;
                }

                localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, cost);
            }
            else if(cost <= Constants.CostIsGold)
            {
                if (localStorage.playerProperty.GetGold < (ulong)(-cost))
                {
                    UIManager.Instance.exeRequestToasting("금이 부족합니다");
                    return false;
                }

                localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.gold, ValueChangeInfo.decrease, (ulong)(-cost));
            }
            else
            {
                UIManager.Instance.exeRequestToasting("더 이상 성형을 진행할 수 없습니다");
                return false;
            }

            if(UnityEngine.Random.Range(0f, 1f) <= GetCosmeticProbability())
            {
                localStorage.playerStat.ChangeStat(PlayerStatChangeInfo.charm, ValueChangeInfo.increase, 1);
                UIManager.Instance.exeOpenSimpleMessageBox("알림", string.Format("성형 수술이 성공했습니다\n성형 등급 {0} -> {1}"
                    , Constants.RankString[localStorage.playerStat.GetCharm - 2]
                    , Constants.RankString[localStorage.playerStat.GetCharm - 1]));
                return true;
            }
            else
            {
                if(localStorage.playerStat.GetCharm > Constants.statMin)
                {
                    localStorage.playerStat.ChangeStat(PlayerStatChangeInfo.charm, ValueChangeInfo.decrease, 1);
                    UIManager.Instance.exeOpenSimpleMessageBox("알림", string.Format("성형 수술이 실패했습니다\n성형 등급 {0} -> {1}"
                        , Constants.RankString[localStorage.playerStat.GetCharm]
                        , Constants.RankString[localStorage.playerStat.GetCharm - 1]));
                }
                else
                {
                    UIManager.Instance.exeOpenSimpleMessageBox("알림", string.Format("성형 수술이 실패했습니다"));
                }

                return true;
            }
        }
    }

    [Serializable]
    public class CosmeticCostData
    {
        [SerializeField] private int cost;
        [SerializeField] [Range(0f, 1f)] private float probability;

        public int GetCost { get { return cost; } }
        public float GetProbability { get { return probability; } }
    }
}