using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
    [Serializable]
    public class ExpItem : ItemBase
    {
        [SerializeField] private ExpItemId id;
        private ItemType itemType = ItemType.expendableSingle;
        [SerializeField] private PlayerExpChangeInfo statType;
        [SerializeField] private double expIncrement;

        public override ItemId GetId { get { return (ItemId)id; } }
        public override ItemType GetItemType { get { return ItemType.expendableSingle; } }
        public PlayerExpChangeInfo GetStatType { get { return statType; } }
        public double GetExpIncrement { get { return expIncrement; } }
        public override string GetDescription
        {
            get
            {
                switch(statType)
                {
                    case PlayerExpChangeInfo.creditExp:
                        {
                            return string.Format("{0}\n\n신용등급 경험치 증가량 : {1}", base.GetDescription, expIncrement);
                        }
                    case PlayerExpChangeInfo.fitnessExp:
                        {
                            return string.Format("{0}\n\n체력 경험치 증가량 : {1}", base.GetDescription, expIncrement);
                        }
                    default:
                        {
                            return base.GetDescription;
                        }
                }                
            }
        }

        public override bool UseItem(uint num)
        {
            switch(statType)
            {
                case PlayerExpChangeInfo.fitnessExp:
                    {
                        LinkContainer.Instance.localStorage.playerStat.ChangeExp(PlayerExpChangeInfo.fitnessExp, ValueChangeInfo.increase, expIncrement);
                        break;
                    }
                case PlayerExpChangeInfo.creditExp:
                    {
                        LinkContainer.Instance.localStorage.playerStat.ChangeExp(PlayerExpChangeInfo.creditExp, ValueChangeInfo.increase, expIncrement);
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            return true;
        }
    }
}