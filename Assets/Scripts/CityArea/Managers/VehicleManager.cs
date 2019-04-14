using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;

namespace CityStage
{
    public class VehicleManager : MonoSingletonBase<VehicleManager>
    {
        //[SerializeField] private VehicleId curVehicle;

        //private LocalStorage localStorage;
        //private HouseMetadata houseMetadata;

        //// Use this for initialization
        //void Start()
        //{
        //    localStorage = LinkContainer.Instance.localStorage;
        //    houseMetadata = LinkContainer.Instance.houseMetadata;

        //    SetVehicles();
        //}

        //// 게임 초기에 주차장에 차량생성
        //private void SetVehicles()
        //{
        //    ParkingCurrentVehicle();
        //}

        //// 현재 차량을 변경
        //// 새로운 차량을 구입했을때
        //public bool ChangeCurrentVehicle(VehicleId vehicleId, HouseId houseId = HouseId.none)
        //{
        //    if (houseId == HouseId.none)
        //    {
        //        bool isGarageEmpty = true;
        //        int curVehicleFillAmount = 0;

        //        for (HouseId i = HouseId.houseDefault; i < HouseId.lastIndex; i++)
        //        {
        //            curVehicleFillAmount = 0;

        //            if ((localProperties.curVehicleId != VehicleId.none) && (localStorage.curVehicleParkingPlace == i))
        //            {
        //                curVehicleFillAmount = 1;
        //            }

        //            if ((houseMetadata.houseDic[i].parkingVehicleList.Count + curVehicleFillAmount) < houseMetadata.houseDic[i].GetMaxParkingVehicle)
        //            {
        //                houseId = i;
        //                isGarageEmpty = false;
        //                break;
        //            }
        //        }

        //        if (isGarageEmpty)
        //        {
        //            UIManager.Instance.exeOpenSimpleMessageBox("알림", "이용 가능한 차고가 없습니다");
        //            return false;
        //        }
        //    }

        //    if (localStorage.curVehicleId != VehicleId.none)
        //    {
        //        if (!ParkingCurrentVehicle())
        //        {
        //            UIManager.Instance.exeOpenSimpleMessageBox("알림", "기존 차량 주차 실패");
        //            return false;
        //        }
        //    }

        //    localStorage.curVehicle.SetActive(true);
        //    localStorage.curVehicleId = vehicleId;
        //    localStorage.curVehicleParkingPlace = houseId;

        //    return true;
        //}

        //// 현재 차량을 주차장으로 이동
        //// 새로운 차량을 구입했을때
        //// 차량을 꺼낸 채로 다른 차량을 꺼냈을때
        //public bool ParkingCurrentVehicle(HouseId houseId = HouseId.none)
        //{
        //    if (houseId == HouseId.none)
        //    {
        //        houseId = localStorage.curVehicleParkingPlace;
        //    }

        //    if (localStorage.curVehicleId == VehicleId.none)
        //    {
        //        UIManager.Instance.exeOpenSimpleMessageBox("알림", "현재 이용중인 차량이 없습니다");
        //        return false;
        //    }

        //    if (houseMetadata.houseDic[localStorage.curVehicleParkingPlace].parkingVehicleList.Count >= houseMetadata.houseDic[localStorage.curVehicleParkingPlace].GetMaxParkingVehicle)
        //    {
        //        UIManager.Instance.exeOpenSimpleMessageBox("알림", "차고가 가득 찼습니다");
        //        return false;
        //    }

        //    localStorage.curVehicle.SetActive(false);
        //    localStorage.curVehicleId = VehicleId.none;
        //    localStorage.curVehicleParkingPlace = HouseId.none;
        //    houseMetadata.houseDic[localStorage.curVehicleParkingPlace].parkingVehicleList.Add(localStorage.curVehicleId);
        //    return true;
        //}
    }
}