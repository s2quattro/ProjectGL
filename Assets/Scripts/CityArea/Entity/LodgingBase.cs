using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;

namespace CityStage
{
    public abstract class LodgingBase : TileEntityBehavior, IInteractable
    {
        protected float delta = 0f;
        protected bool isRecoveryStart = false;

        protected HouseMetadata houseMetadata;
        protected LocalStorage localStorage;
        protected LocalProperties localProperties;
        
        public void StartStaminaRecovery()
        {
            //localProperties.mainCharLoc.position = innerPos.position;
            isRecoveryStart = true;
        }

        public void EndStaminaRecovery()
        {
            //localProperties.mainCharLoc.position = outerPos.position;
            delta = 0f;
            isRecoveryStart = false;
        }

        protected virtual void CheckRecovery()
        {
        }

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
        }
    }
}