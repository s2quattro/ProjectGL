using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GLCore;

namespace CityStage
{
    [RequireComponent(typeof(Sprite2DOutlineHighLighter))]
    public class HouseTransactionEntity : TileEntityBehavior, IInteractable
    {
        // Use this for initialization
        void Start()
        {
            highLightModule = GetComponent<Sprite2DOutlineHighLighter>();
            localStorage = LinkContainer.Instance.localStorage;
            houseMetadata = LinkContainer.Instance.houseMetadata;
            register();
        }

        [SerializeField] private HouseId houseId;

        private LocalStorage localStorage;
        private HouseMetadata houseMetadata;

        private Sprite2DOutlineHighLighter highLightModule;

        public bool GetPlayerHave { get { return localStorage.playerHouseDic[houseId].PlayerHave; } }

        private bool _isHighLight;
        public bool isHighLight
        {
            get
            {
                return _isHighLight;
            }
            set
            {
                _isHighLight = value;
                highLightModule.exeToggleOutline(_isHighLight);
            }
        }

        private InteractionType _interactType = InteractionType.teleport;
        public InteractionType interactType
        {
            get
            {
                return _interactType;
            }
        }

        public virtual void exeInteract()
        {            
            // ui 매니저 함수 호출
        }

        public bool PurchaseHouse()
        {
            ulong price = houseMetadata.houseDic[houseId].GetPurchasePrice;

            if (!localStorage.playerProperty.CompareCashToPrice(price))
            {
                UIManager.Instance.exeRequestToasting("현금이 부족합니다");
                return false;
            }

            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, price);
            localStorage.playerHouseDic[houseId].PlayerHave = true;

            return true;
        }

        public bool SellHouse()
        {         
            if (GetPlayerHave)
            {
                UIManager.Instance.exeRequestToasting("소유중인 집이 아닙니다");
                return false;
            }

            ulong price = houseMetadata.houseDic[houseId].GetPurchasePrice;

            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, price);
            localStorage.playerHouseDic[houseId].PlayerHave = false;

            return true;
        }
    }
}