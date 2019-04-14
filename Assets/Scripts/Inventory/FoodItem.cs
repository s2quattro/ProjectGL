using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
    [Serializable]
    public class FoodItem : ItemBase
    {
        [SerializeField] private FoodId id;
        [SerializeField] private ItemType itemType = ItemType.expendable;
        [SerializeField] private uint staminaRecoveryAmount;

        public override ItemId GetId { get { return (ItemId)id; } }
		public override ItemType GetItemType { get { return itemType; } }
        public override string GetDescription
        {
            get
            {
                return string.Format("{0}\n\n스태미너 회복량 : {1}", base.GetDescription, staminaRecoveryAmount);
            }
        }

        public override bool UseItem(uint num)
        {
            StaminaManager.Instance.RecoveryStamina(staminaRecoveryAmount * num);

            return true;
        }
    }
}