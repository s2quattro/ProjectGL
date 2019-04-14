using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
    [CreateAssetMenu(fileName = "HouseMetadata(CityStage)", menuName = "Storages/HouseMetadata(CityStage)")]
    public class HouseMetadata : ScriptableObject
    {
        [SerializeField] private List<HouseData> houseDatas;
        [SerializeField] private List<HotelData> hotelDatas;
        [SerializeField] private float defaultRecoverySpeed = 1f;
        public Dictionary<HouseId, HouseData> houseDic = new Dictionary<HouseId, HouseData>();
        public Dictionary<HotelId, HotelData> hotelDic = new Dictionary<HotelId, HotelData>();

        public void SetDicData()
        {
            foreach (HouseData data in houseDatas)
            {
                houseDic.Add(data.GetHouseId, data);
            }
            foreach (HotelData data in hotelDatas)
            {
                hotelDic.Add(data.GetHotelId, data);
            }
        }
    }
}