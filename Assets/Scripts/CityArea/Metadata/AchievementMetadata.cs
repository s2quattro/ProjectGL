using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
    [CreateAssetMenu(fileName = "AchievementMetadata(CityStage)", menuName = "Storages/AchievementMetadata(CityStage)")]
    public class AchievementMetadata : ScriptableObject
    {

    }

    public enum RewardId
    {
        none=0, cash, item1, 
    }

    public enum AchievementId
    {
        none = 0, obtainCash, useCash, useStamina, obtainOre, purchaseCarAmount, purchaseHouseAmount
    }
}