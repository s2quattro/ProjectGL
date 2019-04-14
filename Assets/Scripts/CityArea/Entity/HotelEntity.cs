using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GLCore;

namespace CityStage
{
    public class HotelEntity : LodgingBase
    {
        [SerializeField] private HotelId hotelId;

        private void Start()
        {
            register();

            houseMetadata = LinkContainer.Instance.houseMetadata;
            localStorage = LinkContainer.Instance.localStorage;
            localProperties = LinkContainer.Instance.localProperties;
        }

        // Update is called once per frame
        void Update()
        {
            if (isRecoveryStart)
            {
                CheckRecovery();
            }
        }

        public bool PayHotelExpenses()
        {
            if (hotelId == HotelId.none)
            {
                UIManager.Instance.exeRequestToasting("호텔 정보가 올바르지 않습니다");
                return false;
            }
            if (localStorage.playerProperty.GetCash < (decimal)houseMetadata.hotelDic[hotelId].GetPrice)
            {
                UIManager.Instance.exeRequestToasting("현금이 부족합니다");
                return false;
            }

            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, (decimal)houseMetadata.hotelDic[hotelId].GetPrice);

            StartStaminaRecovery();

            return true;
        }

        protected override void CheckRecovery()
        {
            if (delta > 0f)
            {
                delta -= Time.deltaTime;
            }
            else
            {
                delta = StaminaManager.Instance.GetRecoveryStaminaDelay();

                if(StaminaManager.Instance.RecoveryStamina(houseMetadata.hotelDic[hotelId].GetStaminaRecoveryAmount))
                {
                    EndStaminaRecovery();
                }
            }
        }

        public override void exeInteract()
        {
            //Teleportation		
            StartStaminaRecovery();
        }
    }
}