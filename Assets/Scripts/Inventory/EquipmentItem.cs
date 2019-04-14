using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;
using UnityEngine.EventSystems;
using TMPro;

namespace CityStage
{
    [Serializable]
    public class EquipmentItem : ItemBase
    {
        private ItemType itemType = ItemType.equipment;
        [SerializeField] private EquipmentId id;
        [SerializeField] private EquipmentType equipmentType;
        public EquipmentType GetEquipmentType { get { return equipmentType; } }
        public override ItemId GetId { get { return (ItemId)id; } }
        public override uint GetMaxAmount { get { return Constants.equipmentMaxAmount; } }
        [SerializeField] private ItemStatFactor itemStatFactor;
		public override ItemType GetItemType { get { return ItemType.equipment; } }

		public override ItemStatFactor GetItemStatFactor { get { return itemStatFactor; } }		 

        public override string GetDescription
        {
            get
            {
                return String.Format("{0} \n {1}", base.GetDescription, itemStatFactor.GetStatFactorDescription());
			}
        }

        public EquipmentItem(EquipmentType equipmentType)
        {
            this.equipmentType = equipmentType;
        }
	}
}