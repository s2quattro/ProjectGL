using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
    [Serializable]
    public class BuffItem : ItemBase
    {
        [SerializeField] private BuffId id;
        [SerializeField] private float durationTime;
        [SerializeField] private float remainTime;
        [SerializeField] private ItemStatFactor itemStatFactor;

        public override ItemId GetId { get { return (ItemId)id; } }
        public override ItemType GetItemType { get { return ItemType.other; } }

        private LocalStorage localStorage;

        public override string GetDescription
        {
            get
            {
                return String.Format("{0} \n\n 남은 지속시간: {1} 초\n\n {2}",
                    base.GetDescription, ((uint)remainTime).ToString(), itemStatFactor.GetStatFactorDescription());
            }
        }

        public override ItemStatFactor GetItemStatFactor { get { return itemStatFactor; } }        

        public void InitRemainTime()
        {
            remainTime = durationTime;
        }

        public void UpdateRemainTime()
        {
            if(remainTime > 0f)
            {
                remainTime -= Time.deltaTime;
            }
            else
            {
                // 해당 버프아이템 제거
            }
        }
    }
}