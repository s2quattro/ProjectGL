using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
    [Serializable]
    public class OtherItem : ItemBase
    {
        private ItemType itemType = ItemType.other;
        [SerializeField] private OtherId id;

        public override ItemId GetId { get { return (ItemId)id; } }
        public override ItemType GetItemType { get { return ItemType.other; } }
    }
}
