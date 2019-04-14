using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
    [CreateAssetMenu(fileName = "BusinessMetadata(CityStage)", menuName = "Storages/BusinessMetadata(CityStage)")]
    public class BusinessMetadata : ScriptableObject
    {
        [SerializeField] private List<BusinessData> businessDatas; 
        public Dictionary<BusinessId, BusinessData> businessDic = new Dictionary<BusinessId, BusinessData>();
    }

    public enum BusinessId
    {
        none = 0, hotel, race, stock, last
    }

    [Serializable]
    public class BusinessData
    {
        [SerializeField] private BusinessId businessId;
        [SerializeField] private bool playerHave = false;
        [SerializeField] private ulong purchasePrice;
        [SerializeField] private ulong sellPrice;
        [SerializeField] private uint renewTime;
        [SerializeField] private uint maxRenewNum;
        [SerializeField] private uint profitPerHour;
        [SerializeField] private decimal totalProfit;
        [SerializeField] private CustumDateTime recentTime;

        public void RenewProfit()
        {
            int changeNum = GLAPI.GetTimeChangeAmount(recentTime, renewTime, maxRenewNum);

            totalProfit += profitPerHour * changeNum;
        }

        public bool PurchaseBuilding()
        {
            if (businessId == BusinessId.none)
            {
                UIManager.Instance.exeRequestToasting("사업장 정보가 올바르지 않습니다");
                return false;
            }
            if (playerHave)
            {
                UIManager.Instance.exeRequestToasting("이미 소유중인 사업장 입니다");
                return false;
            }
            if (LinkContainer.Instance.localStorage.playerProperty.GetCash < purchasePrice)
            {
                UIManager.Instance.exeRequestToasting("현금이 부족합니다");
                return false;
            }

            LinkContainer.Instance.localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, purchasePrice);
            playerHave = true;

            return true;
        }

        public bool SellBuilding()
        {
            if (businessId == BusinessId.none)
            {
                UIManager.Instance.exeRequestToasting("사업장 정보가 올바르지 않습니다");
                return false;
            }
            if (!playerHave)
            {
                UIManager.Instance.exeRequestToasting("소유중인 사업장이 아닙니다");
                return false;
            }
            if (sellPrice <= 0)
            {
                UIManager.Instance.exeRequestToasting("판매 가능한 사업장이 아닙니다");
                return false;
            }
            if (totalProfit > 0)
            {
                UIManager.Instance.exeRequestToasting("수익금이 남아있습니다");
                return false;
            }

            LinkContainer.Instance.localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.increase, sellPrice);
            playerHave = false;

            return true;
        }
    }
}