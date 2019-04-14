using ScottGarland;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace GLCore
{
    public static class Constants
    {
        public const int diceStatGender = 0;
        public const int diceStatCharm = 1;
        public const int diceStatFitness = 2;
        public const int diceStatCredit = 3;
        public const int diceStatCount = 4;

        public const int statMin = 1;
        public const int statMax = 10;

        //public const int DefaultInventoryCount = 5;
        //public const int ExpansionInventoryCountInc = 5;
        //public const int ExpansionInventoryMaxNum = 14;

        public const int genderMan = 0;
        public const int genderWoman = 1;
        //public const int diceTextStat4 = 4;

        public const int equipmentMaxAmount = 1;

        //public const uint cashMaxValue = 3000000000;
        //public const uint cashExchangeValue = 1000000000;
        //public const uint MaxTransactionCost = 1000000000;

        public const int CostIsCash = 1;
        public const int CostIsGold = -1;

        public static string[] RankString = new string[] { "D", "D+", "C", "C+", "B", "B+", "A", "A+", "S", "S+" };

        public static string[] SaveFileName = new string[] {"Identification", "Field", "Stat", "Property", "Inventory", "Equipment",
        "Business", "House", "Stock", "Store", "RecentTime"};
    }

    public enum FileSaveType
    {
        identification = 0, field, stat, property, inventory, equipment, business, house, stock, store, recentTime, last
    }

    #region Item Id

    [Serializable]
    public enum ItemId
    {
        NotExistId = -1,

        // Lottery Ids
        LotteryNormal = 100, LotterySuper,

        // Ore Ids
        OreCopper = 200, OreTin, OreIron, OreRuby, OreSapphire, OreEmerald, OreDiamond, OreOrichalcon, OreMithril, OreAdamantium,

        // Equipment Ids
        EquipHeadUnique = 300,
        EquipPickaxOld = 400, EquipPickaxUseful, EquipPickaxSolid, EquipPickaxIron, EquipPickaxAlloy, EquipPickaxHardMetal, EquipPickaxTitanium, EquipPickaxCarbon,
        EquipPickaxRuby, EquipPickaxSapphire, EquipPickaxEmerald, EquipPickaxDiamond, EquipPickaxOrihalcon, EquipPickaxMithril, EquipPickaxAdamantium,
        EquipShoesUnique = 500,

        // Random Box Ids
        RandomBoxCommon = 600, RandomBoxRare, RandomBoxUnique,

        // Other Ids
        OtherGold = 700,

        // ExpItem Ids
        ExpFitnessSmall = 800, ExpFitnessMiddle = 801, ExpFitnessLarge = 802,

        // Food Ids
        FoodWater = 900, FoodSoda, FoodJuice, FoodPowerDrink, FoodBread, FoodRiceBall, FoodBeefJerky, FoodEelLunchBox,
    }

    public enum LotteryId
    {
        NotExistId = -1,
        LotteryNormal = 100, LotterySuper,
    }

    public enum OreId
    {
        NotExistId = -1,
        OreCopper = 200, OreTin, OreIron, OreRuby, OreSapphire, OreEmerald, OreDiamond, OreOrichalcon, OreMithril, OreAdamantium,
    }

    [Serializable]
    public enum EquipmentId
    {
        NotExistId = -1,
        EquipHeadOld = 300,
        EquipPickaxOld = 400, EquipPickaxUseful, EquipPickaxSolid, EquipPickaxIron, EquipPickaxAlloy, EquipPickaxHardMetal, EquipPickaxTitanium, EquipPickaxCarbon,
        EquipPickaxRuby, EquipPickaxSapphire, EquipPickaxEmerald, EquipPickaxDiamond, EquipPickaxOrihalcon, EquipPickaxMithril, EquipPickaxAdamantium,
        EquipShoesUnique = 500,
    }

    public enum RandomBoxId
    {
        NotExistId = -1,
        RandomBoxCommon = 600, RandomBoxRare, RandomBoxUnique,
    }

    public enum OtherId
    {
        NotExistId = -1,
        OtherGold = 700,
    }

    public enum ExpItemId
    {
        NotExistId = -1,
        ExpFitnessSmall = 800, ExpFitnessMiddle = 801, ExpFitnessLarge = 802,
    }

    public enum FoodId
    {
        NotExistId = -1,
        FoodWater = 900, FoodSoda, FoodJuice, FoodPowerDrink, FoodBread, FoodRiceBall, FoodBeefJerky, FoodEelLunchBox
    }

    public enum BuffId
    {
        NotExistId = -1,
    }



    #endregion

    public enum entityLookDir
    {
        up,
        down,
        left,
        right
    }

    public enum StatRank
    {
        RankD = 1,
        RankDp,
        RankC,
        RankCp,
        RankB,
        RankBp,
        RankA,
        RankAp,
        RankS,
        RankSp,
        Last
    }

    public enum Gender
    {
        Man = 1,
        Woman
    }

    public enum HorseNum
    {
        none = -1,
        horse1,
        horse2,
        horse3
    }

    public enum CharActionType
    {
        TypeNotFounded, Moving, OreMining, Racing,
    }

    #region for Changeable Vars

    public enum PlayerStatChangeInfo
    {
        gender = 1, charm, fitness, credit, inventoryCurAmount, inventoryMaxAmount
    }

    public enum PlayerExpChangeInfo
    {
        fitnessExp = 1, creditExp
    }

    public enum PlayerPropertyChangeInfo
    {
        cash, deposit, loan, gold
    }

    //public enum CashExchangeInfo
    //{
    //    toUpper, toLower
    //}

    [Serializable]
    public class PlayerAcievement
    {
        [SerializeField] private string obtainCash;
        [SerializeField] private string useCash;
        [SerializeField] private ulong useStamina;
        [SerializeField] private ulong obtainOre;
        [SerializeField] private uint purchaseCarAmount;
        [SerializeField] private uint purchaseHouseAmount;

        public PlayerAcievement()
        {
            obtainCash = "0";
            useCash = "0";
            useStamina = 0;
            obtainOre = 0;
            purchaseCarAmount = 0;
            purchaseHouseAmount = 0;
        }
    }

    public enum StockId
    {
        none=0, stockNGElectronics, stockOSUNGHeavyIndustries, stockWhiteMessa, stockMiraeMotors, stockHotelGoguryeo, stockOSUNGElectronics, stockSTTelecom, stockScratchbackEntertainment
            ,MersLife, GigaStudy, last
    }

    public enum StockChangeInfo
    {
        curPrice, playerHave
    }

    #endregion

    #region for Item Def

    [Serializable]
    public class CharAction
    {
        [SerializeField] private CharActionType charActionType;
        [SerializeField] private double staminaUsage;
        [SerializeField] private double fitnessExpInc;

        public CharActionType GetCharActionType { get { return charActionType; } }
        public double GetStaminaUsage { get { return staminaUsage; } }
        public double GetFintnessExpInc { get { return fitnessExpInc; } }
    }
    
    public enum ValueChangeInfo
    {
        increase, decrease, set
    }

    [Serializable]
    public class DataCalcEntity
    {
        //protected BigInteger CalcChangeValue(BigInteger memberData, ValueChangeInfo changeInfo, BigInteger value)
        //{
        //    switch (changeInfo)
        //    {
        //        case ValueChangeInfo.increase:
        //            {
        //                memberData += value;
        //                break;
        //            }
        //        case ValueChangeInfo.decrease:
        //            {
        //                if(memberData >= value)
        //                {
        //                    memberData -= value;
        //                }
        //                break;
        //            }
        //        case ValueChangeInfo.set:
        //            {
        //                if (value >= 0)
        //                {
        //                    memberData = value;
        //                }
        //                break;
        //            }
        //        default:
        //            {
        //                break;
        //            }
        //    }

        //    return memberData;
        //}

        protected ulong CalcChangeValue(ulong memberData, ValueChangeInfo changeInfo, ulong value)
        {
            ulong changeValue = memberData;

            switch (changeInfo)
            {
                case ValueChangeInfo.increase:
                    {
                        if (value <= (ulong.MaxValue - memberData))
                        {
                            changeValue = memberData + value;
                        }
                        else
                        {
                            Debug.Log("result value is max of uint");
                        }

                        break;
                    }
                case ValueChangeInfo.decrease:
                    {
                        if (memberData >= value)
                        {
                            changeValue = memberData - value;
                        }
                        else
                        {
                            Debug.Log("result value is negative number");
                        }

                        break;
                    }
                case ValueChangeInfo.set:
                    {
                        changeValue = value;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return changeValue;
        }

        protected uint CalcChangeValue(uint memberData, ValueChangeInfo changeInfo, uint value)
        {
            uint changeValue = memberData;

            switch (changeInfo)
            {
                case ValueChangeInfo.increase:
                    {
                        if(value <= (uint.MaxValue - memberData))
                        {
                            changeValue = memberData + value;
                        }
                        else
                        {
                            Debug.Log("result value is max of uint");
                        }

                        break;
                    }
                case ValueChangeInfo.decrease:
                    {
                        if (memberData >= value)
                        {
                            changeValue = memberData - value;
                        }
                        else
                        {
                            Debug.Log("result value is negative number");
                        }

                        break;
                    }
                case ValueChangeInfo.set:
                    {
                        changeValue = value;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return changeValue;
        }

        protected string CalcChangeValue(string memberData, ValueChangeInfo changeInfo, decimal value)
        {
            if (memberData == string.Empty)
            {
                memberData = "0";
            }

            string changeValue = memberData;

            switch (changeInfo)
            {
                case ValueChangeInfo.increase:
                    {
                        if (value >= 0m)
                        {
                            if (value <= (decimal.MaxValue - Convert.ToDecimal(memberData)))
                            {
                                changeValue = Convert.ToString(Convert.ToDecimal(memberData) + value);
                            }
                            else
                            {
                                Debug.Log("result value is max of float");
                            }
                        }
                        else
                        {
                            Debug.Log("value is negative number");
                        }

                        break;
                    }
                case ValueChangeInfo.decrease:
                    {
                        if (value >= 0m)
                        {
                            if (Convert.ToDecimal(memberData) >= value)
                            {
                                changeValue = changeValue = Convert.ToString(Convert.ToDecimal(memberData) - value);
                            }
                            else
                            {
                                Debug.Log("result value is negative number");
                            }
                        }
                        else
                        {
                            Debug.Log("value is negative number");
                        }

                        break;
                    }
                case ValueChangeInfo.set:
                    {
                        if (value >= 0m)
                        {
                            changeValue = Convert.ToString(value);
                        }
                        else
                        {
                            Debug.Log("value is negative number");
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return changeValue;
        }

        protected double CalcChangeValue(double memberData, ValueChangeInfo changeInfo, double value)
        {
            double changeValue = memberData;

            switch (changeInfo)
            {
                case ValueChangeInfo.increase:
                    {
                        if (value >= 0f)
                        {
                            if (value <= (double.MaxValue - memberData))
                            {
                                changeValue = memberData + value;
                            }
                            else
                            {
                                Debug.Log("result value is max of float");
                            }
                        }
                        else
                        {
                            Debug.Log("value is negative number");
                        }

                        break;
                    }
                case ValueChangeInfo.decrease:
                    {
                        if (value >= 0f)
                        {
                            if (memberData >= value)
                            {
                                changeValue = memberData - value;
                            }
                            else
                            {
                                Debug.Log("result value is negative number");
                            }
                        }
                        else
                        {
                            Debug.Log("value is negative number");
                        }

                        break;
                    }
                case ValueChangeInfo.set:
                    {
                        if (value >= 0f)
                        {
                            changeValue = value;
                        }
                        else
                        {
                            Debug.Log("value is negative number");
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return changeValue;
        }

        protected float CalcChangeValue(float memberData, ValueChangeInfo changeInfo, float value)
        {
            float changeValue = memberData;

            switch (changeInfo)
            {
                case ValueChangeInfo.increase:
                    {
                        if (value >= 0f)
                        {
                            if (value <= (float.MaxValue - memberData))
                            {
                                changeValue = memberData + value;
                            }
                            else
                            {
                                Debug.Log("result value is max of float");
                            }
                        }
                        else
                        {
                            Debug.Log("value is negative number");
                        }

                        break;
                    }
                case ValueChangeInfo.decrease:
                    {
                        if (value >= 0f)
                        {
                            if (memberData >= value)
                            {
                                changeValue = memberData - value;
                            }
                            else
                            {
                                Debug.Log("result value is negative number");
                            }
                        }
                        else
                        {
                            Debug.Log("value is negative number");
                        }

                        break;
                    }
                case ValueChangeInfo.set:
                    {
                        if (value >= 0f)
                        {
                            changeValue = value;
                        }
                        else
                        {
                            Debug.Log("value is negative number");
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return changeValue;
        }
    }   

    public enum ItemType
    {
        nonSelect=-1, expendable=0, equipment, other, expendableSingle,
    }

    public enum ItemRarity
    {
        common, rare, unique 
    }

    [Serializable]
    public class InventoryItemList
    {
        public InventoryItemList(ItemId itemId, uint num)
        {
            id = itemId;
            curAmount = num;
        }

        public InventoryItemList()
        {
            id = ItemId.NotExistId;
            curAmount = 0;
        }

        public ItemId id;
        public uint curAmount;
    }

    public struct InventoryItemListStruct
    {
        public ItemId id;
        public uint curAmount;
    }

    public enum EquipmentType
    {
		non = -1, head, pickax, shoes
    }

    public struct EquipmentItemListStruct
    {
        public ItemId pickax;
        public ItemId head;
        public ItemId shoes;
    }

    #endregion

    #region other

    //public enum MessageBoxInfo
    //{
    //    lackOfMoney, 
    //}

    //[Serializable]
    //public class MessageBox
    //{
    //    [SerializeField] private MessageBoxInfo messageBoxInfo;
    //    [SerializeField] private string title;
    //    [SerializeField] private string contents;

    //    public string GetTitle { get { return title; } }
    //    public string GetContents { get { return contents; } }
    //    public MessageBoxInfo GetMessageBoxInfo { get { return messageBoxInfo; } }
    //}

    [Serializable]
    public class CustumDateTime
    {
        

        public CustumDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }

        public CustumDateTime()
        {
            year = 0;
            month = 0;
            day = 0;
            hour = 0;
            minute = 0;
            second = 0;
        }

        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
        public int second;
    }

    #endregion

    [Serializable]
    public enum HouseId
    {
        none=0, houseDefault, house1, house2, house3, house4, house5, lastIndex
    }

    public enum HotelId
    {
        none=0, hotel1, hotel2
    }

    [Serializable]
    public class HouseData
    {
        [SerializeField] private HouseId houseId;
        [SerializeField] private string name;
        [SerializeField] private ulong purchasePrice;
        [SerializeField] private ulong sellPrice;
        [SerializeField] private float staminaRecoveryAmount;
        [SerializeField] public bool playerHave = false;
        [SerializeField] public List<VehicleId> parkingVehicleList;
        [SerializeField] private uint maxParkingVehicle;

        public HouseId GetHouseId { get { return houseId; } }
        public float GetStaminaRecoveryAmount { get { return staminaRecoveryAmount; } }
        public ulong GetPurchasePrice { get { return purchasePrice; } }
        public ulong GetSellPrice { get { return sellPrice; } }
        public string GetName { get { return name; } }
        public uint GetMaxParkingVehicle { get { return maxParkingVehicle; } }
    }

    [Serializable]
    public class HotelData
    {
        [SerializeField] private HotelId hotelId;
        [SerializeField] private string name;
        [SerializeField] private uint price;
        [SerializeField] private float staminaRecoveryAmount;

        public HotelId GetHotelId { get { return hotelId; } }
        public float GetStaminaRecoveryAmount { get { return staminaRecoveryAmount; } }
        public uint GetPrice { get { return price; } }
        public string GetName { get { return name; } }
    }

    [Serializable]
    public enum VehicleId
    {
        none = 0, vehicle1, vehicle2
    }

    [Serializable]
    public class VehicleData
    {
        [SerializeField] private VehicleId vehicleId;
        [SerializeField] private string name;
        [SerializeField] private ulong purchasePrice;
        [SerializeField] private ulong sellPrice;
        [SerializeField] private float speed;
        [SerializeField] private float rotateSpeed;

        public VehicleId GetVehicleId { get { return vehicleId; } }
        public string GetName { get { return name; } }
        public ulong GetPurchasePrice { get { return purchasePrice; } }
        public ulong GetSellPrice { get { return sellPrice; } }
        public float GetSpeed { get { return speed; } }
        public float GetRotateSpeed { get { return rotateSpeed; } }
    }

    public enum IconImageId
    {
        none=0, hotel,
    }
}