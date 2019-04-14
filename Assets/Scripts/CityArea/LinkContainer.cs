using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;

namespace CityStage
{
    public class LinkContainer : MonoSingletonBase<LinkContainer>
    {
        public LocalProperties localProperties;
        public LocalStorage localStorage;
        public CharActionMetadata charActionMetadata;
        public StockMetadata stockMetadata;
        public ItemMetadata itemMetadata;
        public BankMetadata bankMetadata;
        public StoreMetadata storeMetadata;
        public HouseMetadata houseMetadata;
        public BusinessMetadata businessMetadata;
        public IconImageMetadata iconImageMetadata;
        public CardGameMetadata cardGameMetadata;
        
        // 클램프 처리, 예외 처리
    }
}