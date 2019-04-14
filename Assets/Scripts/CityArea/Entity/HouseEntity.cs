using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GLCore;

namespace CityStage
{
    public enum HouseInteractType
    {
        none = 0, entrance, exit, transaction
    }

    public class HouseEntity : LodgingBase
    {
        [SerializeField] private HouseId houseId;
        [SerializeField] private HouseInteractType interactType;
        [SerializeField] private Transform movePos;

        [SerializeField] private Transform garageInnerPos;
        [SerializeField] private Transform garageOuterPos;

        private HouseSaveData saveData;

        private void Start()
        {
            register();

            houseMetadata = LinkContainer.Instance.houseMetadata;
            localStorage = LinkContainer.Instance.localStorage;
            localProperties = LinkContainer.Instance.localProperties;

            saveData = LinkContainer.Instance.localStorage.playerHouseDic[houseId];
        }

        private void Update()
        {
            if (isRecoveryStart)
            {
                CheckRecovery();
            }
        }

        public void EnterGarage()
        {
            // 주차장 이동 추가

        }

        public void LeaveGarage()
        {
            // 주차장 밖으로 이동 추가

        }

        public bool DriveOut(VehicleId vehicleId)
        {
            //VehicleManager.Instance.ChangeCurrentVehicle(vehicleId, houseId);

            LeaveGarage();

            return true;
        }

        public bool ParkingCurVehicle()
        {
            //VehicleManager.Instance.ParkingCurrentVehicle(houseId);

            EnterGarage();

            return true;
        }

        public bool PurchaseHouse()
        {
            if(houseId == HouseId.none)
            {
                UIManager.Instance.exeRequestToasting("집 정보가 올바르지 않습니다");
                return false;
            }
            if (saveData.PlayerHave)
            {
                UIManager.Instance.exeRequestToasting("이미 소유중인 집입니다");
                return false;
            }
            if (localStorage.playerProperty.GetCash < (decimal)houseMetadata.houseDic[houseId].GetPurchasePrice)
            {
                UIManager.Instance.exeRequestToasting("현금이 부족합니다");
                return false;
            }

            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, (decimal)houseMetadata.houseDic[houseId].GetPurchasePrice);
            saveData.PlayerHave = true;

            return true;
        }

        public bool SellHouse()
        {
            if (houseId == HouseId.none)
            {
                UIManager.Instance.exeRequestToasting("집 정보가 올바르지 않습니다");
                return false;
            }
            if (!saveData.PlayerHave)
            {
                UIManager.Instance.exeRequestToasting("현재 소유중인 집이 아닙니다");
                return false;
            }
            if(houseMetadata.houseDic[houseId].GetSellPrice == 0)
            {
                UIManager.Instance.exeRequestToasting("판매 가능한 집이 아닙니다");
                return false;
            }
            if (houseMetadata.houseDic[houseId].parkingVehicleList.Count > 0)
            {
                UIManager.Instance.exeRequestToasting("집 주차장에 차량이 있습니다");
                return false;
            }

            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, (decimal)houseMetadata.houseDic[houseId].GetSellPrice);
            saveData.PlayerHave = false;

            return true;
        }

        public bool EnterHouse()
        {
            if (houseId == HouseId.none)
            {
                UIManager.Instance.exeRequestToasting("집 정보가 올바르지 않습니다");
                return false;
            }
            if (!houseMetadata.houseDic[houseId].playerHave)
            {
                UIManager.Instance.exeRequestToasting("현재 소유중인 집이 아닙니다");
                return false;
            }

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
                StaminaManager.Instance.RecoveryStamina(houseMetadata.houseDic[houseId].GetStaminaRecoveryAmount);
            }
        }

        public override void exeInteract()
        {
            //Teleportation			
            EnterHouse();
        }
    }
}
