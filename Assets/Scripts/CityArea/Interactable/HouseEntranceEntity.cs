using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GLCore;

namespace CityStage
{
    [RequireComponent(typeof(Sprite2DOutlineHighLighter))]
    public class HouseEntranceEntity : TileEntityBehavior, IInteractable
    {
        // Use this for initialization
        void Start()
        {
            highLightModule = GetComponent<Sprite2DOutlineHighLighter>();
            localStorage = LinkContainer.Instance.localStorage;
            localProperties = LinkContainer.Instance.localProperties;
            highLightModule.initShader(GetComponent<SpriteRenderer>(), localProperties.hightLightColor);
            register();
        }

        [SerializeField] private HouseId houseId;
        [SerializeField] private Transform roomPos;

        private LocalStorage localStorage;
        private LocalProperties localProperties;

        public Sprite2DOutlineHighLighter highLightModule;

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
            HouseSaveData saveData = localStorage.playerHouseDic[houseId];

            if (saveData.PlayerHave)
            {
                localProperties.mainCharLoc.transform.position = roomPos.transform.position;
            }
            else
            {
                UIManager.Instance.exeRequestToasting("소유중인 집이 아닙니다");
            }
        }
    }
}