using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GLCore;

namespace CityStage
{
    [RequireComponent(typeof(Sprite2DOutlineHighLighter))]
    public class HotelManagerEntity : TileEntityBehavior, IInteractable
    {
        // Use this for initialization
        void Start()
        {
            highLightModule = GetComponent<Sprite2DOutlineHighLighter>();
            localStorage = LinkContainer.Instance.localStorage;
            localProperties = LinkContainer.Instance.localProperties;
            houseMetadata = LinkContainer.Instance.houseMetadata;
            register();
        }

        [SerializeField] private HotelId hotelId;
        [SerializeField] private Transform roomPos;

        private LocalStorage localStorage;
        private LocalProperties localProperties;
        private HouseMetadata houseMetadata;

        private Sprite2DOutlineHighLighter highLightModule;

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

        public bool PayLodging()
        {
            uint price = houseMetadata.hotelDic[hotelId].GetPrice;

            if (!localStorage.playerProperty.CompareCashToPrice(price))
            {
                UIManager.Instance.exeRequestToasting("현금이 부족합니다");
                return false;
            }

            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, price);
            localProperties.mainCharLoc.transform.position = roomPos.transform.position;
            return true;
        }
    }
}