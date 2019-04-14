using UnityEngine;
using System.Collections.Generic;
using GLCore;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CityStage
{
    [CreateAssetMenu(fileName = "LocalStorage(CityStage)", menuName = "Storages/LocalStorage(CityStage)")]
    [Serializable]
    public class LocalStorage : ScriptableObject
    {
        public IdentificationData identificationData;
        public FieldData fieldData;
        public RecentTimeData recentTimeData;

        public PlayerStat playerStat;
        public PlayerProperty playerProperty;
        public PlayerAcievement playerAcievement;

        public List<InventoryItemList> inventoryItems;
        public EquipmentItemList equipmentItems;

        public Dictionary<BusinessId, BusinessSaveData> playerBusinessDic = new Dictionary<BusinessId, BusinessSaveData>();
        public Dictionary<HouseId, HouseSaveData> playerHouseDic = new Dictionary<HouseId, HouseSaveData>();
        public Dictionary<StockId, StockSaveData> playerStockDic = new Dictionary<StockId, StockSaveData>();
        public Dictionary<StoreNpcId, StoreSaveData> playerStoreDic = new Dictionary<StoreNpcId, StoreSaveData>();

        public List<HorseRaceSaveData> horseRaceSaveDatas;
        public int horseRaceSelectHorseNum;
        public string horseRacePutMoney;

        private BinaryFormatter bf;

        public void InitData()
        {
            fieldData = new FieldData();
            recentTimeData = new RecentTimeData();
            playerStat = new PlayerStat();
            playerProperty = new PlayerProperty();
            playerAcievement = new PlayerAcievement();
            inventoryItems = new List<InventoryItemList>();
            equipmentItems = new EquipmentItemList();

            BusinessMetadata businessMetadata = LinkContainer.Instance.businessMetadata;
            HouseMetadata houseMetadata = LinkContainer.Instance.houseMetadata;
            StockMetadata stockMetadata = LinkContainer.Instance.stockMetadata;
            StoreMetadata storeMetadata = LinkContainer.Instance.storeMetadata;

            //playerProperty.SetLocalStorage = LinkContainer.Instance.localStorage;
            //playerStat.SetLocalStorage = LinkContainer.Instance.localStorage;
            //equipmentItems.SetLocalStorage = LinkContainer.Instance.localStorage;

            for (StoreNpcId id = StoreNpcId.GoldSellerNpc; id < StoreNpcId.last; id++)
            {
                if (storeMetadata.storeNpcDic.ContainsKey(id))
                {
                    StoreSaveData saveData = new StoreSaveData(storeMetadata.storeNpcDic[id].GetNpcId);
                    playerStoreDic.Add(id, saveData);
                    storeMetadata.storeNpcDic[id].SetStoreSaveData = saveData;
                }
            }

            for (StockId id = StockId.none+1; id < StockId.last; id++)
            {
                if (stockMetadata.stockDic.ContainsKey(id))
                {
                    StockSaveData saveData = new StockSaveData(stockMetadata.stockDic[id].GetStockId);
                    playerStockDic.Add(id, saveData);
                    stockMetadata.stockDic[id].SetStockSaveData = saveData;
                }
            }

            for (HouseId id = HouseId.none+1; id < HouseId.lastIndex; id++)
            {
                if (houseMetadata.houseDic.ContainsKey(id))
                {
                    HouseSaveData saveData = new HouseSaveData(houseMetadata.houseDic[id].GetHouseId);
                    playerHouseDic.Add(id, saveData);
                }
            }
        }

        public bool SaveToLocal(FileSaveType type)
        {
            try
            {
                string fileName = string.Format("{0}/{1}", Application.persistentDataPath, Constants.SaveFileName[(int)type]);
                FileStream file = File.Open(fileName, FileMode.OpenOrCreate);

                bf = new BinaryFormatter();

                switch (type)
                {
                    case FileSaveType.identification:
                        {
                            bf.Serialize(file, identificationData);
                            break;
                        }
                    case FileSaveType.field:
                        {
                            bf.Serialize(file, fieldData);
                            break;
                        }
                    case FileSaveType.stat:
                        {
                            bf.Serialize(file, playerStat);
                            break;
                        }
                    case FileSaveType.property:
                        {
                            bf.Serialize(file, playerProperty);
                            break;
                        }
                    case FileSaveType.inventory:
                        {
                            bf.Serialize(file, inventoryItems);
                            break;
                        }
                    case FileSaveType.equipment:
                        {
                            bf.Serialize(file, equipmentItems);
                            break;
                        }
                    case FileSaveType.business:
                        {
                            bf.Serialize(file, playerBusinessDic);
                            break;
                        }
                    case FileSaveType.house:
                        {
                            bf.Serialize(file, playerHouseDic);
                            break;
                        }
                    case FileSaveType.stock:
                        {
                            bf.Serialize(file, playerStockDic);
                            break;
                        }
                    case FileSaveType.store:
                        {
                            bf.Serialize(file, playerStoreDic);
                            break;
                        }
                    case FileSaveType.recentTime:
                        {
                            bf.Serialize(file, recentTimeData);
                            break;
                        }
                    default:
                        {
                            Debug.Log("save type error");
                            return false;
                        }
                }

                Debug.Log(fileName + " file save success");
                file.Close();

                File.SetAttributes(fileName, FileAttributes.Hidden);

                return true;
            }
            catch
            {
                Debug.Log("serialize and file save failed");
                return false;
            }
        }

        public bool LoadFile()
        {
            string fileName;
            try
            {
                for (FileSaveType type = FileSaveType.identification; type < FileSaveType.last; type++)
                {
                    fileName = string.Format("{0}/{1}", Application.persistentDataPath, Constants.SaveFileName[(int)type]);

                    if (File.Exists(fileName))
                    {
                        FileStream stream = File.Open(fileName, FileMode.Open);
                        bf = new BinaryFormatter();

                        switch (type)
                        {
                            case FileSaveType.identification:
                                {
                                    identificationData = bf.Deserialize(stream) as IdentificationData;
                                    break;
                                }
                            case FileSaveType.field:
                                {
                                    fieldData = bf.Deserialize(stream) as FieldData;
                                    break;
                                }
                            case FileSaveType.stat:
                                {
                                    playerStat = bf.Deserialize(stream) as PlayerStat;
                                    break;
                                }
                            case FileSaveType.property:
                                {
                                    playerProperty = bf.Deserialize(stream) as PlayerProperty;
                                    break;
                                }
                            case FileSaveType.inventory:
                                {
                                    inventoryItems = bf.Deserialize(stream) as List<InventoryItemList>;
                                    break;
                                }
                            case FileSaveType.equipment:
                                {
                                    equipmentItems = bf.Deserialize(stream) as EquipmentItemList;
                                    break;
                                }
                            case FileSaveType.business:
                                {
                                    playerBusinessDic = bf.Deserialize(stream) as Dictionary<BusinessId, BusinessSaveData>;
                                    break;
                                }
                            case FileSaveType.house:
                                {
                                    playerHouseDic = bf.Deserialize(stream) as Dictionary<HouseId, HouseSaveData>;
                                    break;
                                }
                            case FileSaveType.stock:
                                {
                                    playerStockDic = bf.Deserialize(stream) as Dictionary<StockId, StockSaveData>;
                                    break;
                                }
                            case FileSaveType.store:
                                {
                                    playerStoreDic = bf.Deserialize(stream) as Dictionary<StoreNpcId, StoreSaveData>;
                                    break;
                                }
                            case FileSaveType.recentTime:
                                {
                                    recentTimeData = bf.Deserialize(stream) as RecentTimeData;
                                    break;
                                }
                            default:
                                {
                                    Debug.Log("load type error");
                                    return false;
                                }
                        }

                        Debug.Log(fileName + " file load success");
                        stream.Close();
                    }
                    else
                    {
                        Debug.Log(Constants.SaveFileName[(int)type] + " file is not exist");
                    }
                }

                return true;
            }
            catch
            {
                Debug.Log("file load failed");
                return false;
            }
        }
    }

    [Serializable]
    public class HorseRaceSaveData
    {
        public int horseNum;
        public int eventNum;
        public float durationTime;
        public float speed;

        public HorseRaceSaveData(int horseNum, int eventNum, float durationTime, float speed = 1f)
        {
            this.horseNum = horseNum;
            this.durationTime = durationTime;
            this.eventNum = eventNum;
            this.speed = speed;
        }
    }

    [Serializable]
    public class FieldData
    {
        public GameObject curVehicle;
        public VehicleId curVehicleId = VehicleId.none;
        public HouseId curVehicleParkingPlace = HouseId.none;
        public HouseId mainHouse = HouseId.houseDefault;

        public FieldData()
        {
            curVehicle = null;
            curVehicleId = VehicleId.none;
            curVehicleParkingPlace = HouseId.none;
            mainHouse = HouseId.houseDefault;
        }
    }

    [Serializable]
    public class IdentificationData
    {
        public float version;
        public string userId;
    }

    [Serializable]
    public class BusinessSaveData
    {
        public BusinessId id;
        public CustumDateTime recentTime;
        public bool playerHave;
        public string totalProfit;

        public BusinessSaveData(BusinessId id)
        {
            this.id = id;
        }
    }

    [Serializable]
    public class HouseSaveData
    {
        private HouseId id;
        public HouseId GetId { get { return id; } }
        private bool playerHave;
        public bool PlayerHave { get { return playerHave; } set { playerHave = value; LinkContainer.Instance.localStorage.SaveToLocal(FileSaveType.house); } }
        public List<VehicleId> parkingVehicleList;

        public HouseSaveData(HouseId id)
        {
            this.id = id;
            playerHave = false;
            parkingVehicleList = new List<VehicleId>();
        }
    }

    [Serializable]
    public class StockSaveData
    {
        //private LocalStorage localStorage;

        private StockId id;
        public StockId GetId { get { return id; } }
        private uint playerHave;
        public uint PlayerHave { get { return playerHave; } set { playerHave = value; LinkContainer.Instance.localStorage.SaveToLocal(FileSaveType.stock); } }
        private ulong curPrice;
        public ulong CurPrice { get { return curPrice; } set { curPrice = value; LinkContainer.Instance.localStorage.SaveToLocal(FileSaveType.stock); } }

        public StockSaveData(StockId id)
        {
            //localStorage = LinkContainer.Instance.localStorage;
            this.id = id;
            playerHave = 0;
            curPrice = LinkContainer.Instance.stockMetadata.stockDic[id].GetStartPrice;
        }
    }

    [Serializable]
    public class StoreSaveData
    {
        //private LocalStorage localStorage;

        private StoreNpcId id;
        public StoreNpcId GetId { get { return id; } }
        private double curAffection;
        public double CurAffection { get { return curAffection; } set { curAffection = value; LinkContainer.Instance.localStorage.SaveToLocal(FileSaveType.store); } }
        public CustumDateTime recentTime;

        public StoreSaveData(StoreNpcId id)
        {
            //localStorage = LinkContainer.Instance.localStorage;
            this.id = id;
            curAffection = 0d;
            recentTime = new CustumDateTime();
        }
    }

    [Serializable]
    public class RecentTimeData
    {
        [SerializeField] private CustumDateTime bankRecentTime;
        public CustumDateTime GetBankRecentTime { get { return bankRecentTime; } }
        [SerializeField] private CustumDateTime stockRecentTime;
        public CustumDateTime GetStockRecentTime { get { return stockRecentTime; } }
        [SerializeField] private CustumDateTime adsRecentTime;
        public CustumDateTime GetAdsRecentTime { get { return adsRecentTime; } }

        //public RecentTimeData()
        //{
        //    bankRecentTime = new CustumDateTime();
        //    stockRecentTime = new CustumDateTime();
        //}
    }

    [Serializable]
    public class PlayerStat : DataCalcEntity
    {
        [Header("Main Stat")]
        [Range((uint)Gender.Man, (uint)Gender.Woman)]
        [SerializeField] private uint gender;
        [Range((uint)StatRank.RankD, (uint)StatRank.RankSp)]
        [SerializeField] private uint charm;
        [Range((uint)StatRank.RankD, (uint)StatRank.RankSp)]
        [SerializeField] private uint fitness;
        [SerializeField] private double fitnessExp;
        [Range((uint)StatRank.RankD, (uint)StatRank.RankSp)]
        [SerializeField] private uint credit;
        [SerializeField] private double creditExp;

        [Header("Other")]
        [SerializeField] private double curStamina;
        [SerializeField] private uint inventoryMaxAmount;
        [SerializeField] private uint inventoryCurAmount;

        public PlayerStat()
        {
            gender = Constants.statMin;
            charm = Constants.statMin;
            credit = Constants.statMin;
            fitness = Constants.statMin;
            creditExp = 0d;
            fitnessExp = 0d;

            curStamina = CharActionMetadata.DefaultMaxStamina;
            inventoryMaxAmount = InventoryManager.DefaultInventoryCount;
            inventoryCurAmount = 0;
        }
        
        public uint GetGender { get { return gender; } }
        public uint GetCharm { get { return charm; } }
        public uint GetFitness { get { return fitness; } }
        public uint GetCredit { get { return credit; } }
        public double GetCurStamina { get { return curStamina; } }
        public double GetFitnessExp { get { return fitnessExp; } }
        public double GetCreditExp { get { return creditExp; } }
        public uint GetInventoryMaxAmount { get { return inventoryMaxAmount; } }
        public uint GetInventoryCurAmount { get { return inventoryCurAmount; } }

        public void ChangeCurStamina(ValueChangeInfo changeInfo, double value)
        {
            curStamina = CalcChangeValue(curStamina, changeInfo, value);

            LinkContainer.Instance.localStorage.SaveToLocal(FileSaveType.stat);
        }

        public void ChangeExp(PlayerExpChangeInfo playerExpChangeInfo, ValueChangeInfo changeInfo, double value)
        {
            switch (playerExpChangeInfo)
            {
                case PlayerExpChangeInfo.creditExp:
                    {
                        creditExp = CalcChangeValue(creditExp, changeInfo, value);
                        break;
                    }
                case PlayerExpChangeInfo.fitnessExp:
                    {
                        fitnessExp = CalcChangeValue(fitnessExp, changeInfo, value);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            LinkContainer.Instance.localStorage.SaveToLocal(FileSaveType.stat);
        }

        public void ChangeStat(PlayerStatChangeInfo playerStatInfo, ValueChangeInfo changeInfo, uint value)
        {
            switch (playerStatInfo)
            {
                case PlayerStatChangeInfo.gender:
                    {
                        gender = CalcChangeValue(gender, changeInfo, value);
                        break;
                    }
                case PlayerStatChangeInfo.charm:
                    {
                        charm = CalcChangeValue(charm, changeInfo, value);
                        break;
                    }
                case PlayerStatChangeInfo.credit:
                    {
                        credit = CalcChangeValue(credit, changeInfo, value);
                        break;
                    }
                case PlayerStatChangeInfo.fitness:
                    {
                        fitness = CalcChangeValue(fitness, changeInfo, value);
                        break;
                    }
                //case PlayerStatsChangeInfo.maxStamina:
                //    {
                //        maxStamina = CalcChangeValue(maxStamina, changeInfo, value);
                //        break;
                //    }
                case PlayerStatChangeInfo.inventoryMaxAmount:
                    {
                        inventoryMaxAmount = CalcChangeValue(inventoryMaxAmount, changeInfo, value);
                        break;
                    }
                case PlayerStatChangeInfo.inventoryCurAmount:
                    {
                        inventoryCurAmount = CalcChangeValue(inventoryCurAmount, changeInfo, value);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            LinkContainer.Instance.localStorage.SaveToLocal(FileSaveType.stat);
        }
    }

    [Serializable]
    public class PlayerProperty : DataCalcEntity
    {
        [SerializeField] private string cash;
        [SerializeField] private string deposit;
        [SerializeField] private string loan;
        [SerializeField] private ulong gold;

        public PlayerProperty()
        {
            string s = "0";
            cash = s;
            deposit = s;
            loan = s;
            gold = 0;
        }

        public decimal GetCash { get { if (cash == string.Empty) { return Convert.ToDecimal("0"); } else { return Convert.ToDecimal(cash); } } }
        public string GetCashToString { get { if (cash == string.Empty) { return "0"; } else { return cash; } } }
        public decimal GetDeposit { get { if (deposit == string.Empty) { return Convert.ToDecimal("0"); } else { return Convert.ToDecimal(deposit); } } }
        public string GetDepositToString { get { if (deposit == string.Empty) { return "0"; } else { return deposit; } } }
        public decimal GetLoan { get { if (loan == string.Empty) { return Convert.ToDecimal("0"); } else { return Convert.ToDecimal(loan); } } }
        public string GetLoanToString { get { if (loan == string.Empty) { return "0"; } else { return loan; } } }
        public ulong GetGold { get { return gold; } }

        public bool CompareCashToPrice(decimal price)
        {
            if (Convert.ToDecimal(cash) >= price)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ChangeProperty(PlayerPropertyChangeInfo playerMoneyInfo, ValueChangeInfo changeInfo, decimal value)
        {
            //save

            switch (playerMoneyInfo)
            {
                case PlayerPropertyChangeInfo.cash:
                    {
                        cash = CalcChangeValue(cash, changeInfo, value);

						UIManager.Instance.updateCashValue(cash);
						break;
                    }
                case PlayerPropertyChangeInfo.deposit:
                    {
                        deposit = CalcChangeValue(deposit, changeInfo, value);
                        break;
                    }
                case PlayerPropertyChangeInfo.loan:
                    {
                        loan = CalcChangeValue(loan, changeInfo, value);
                        break;
                    }
                case PlayerPropertyChangeInfo.gold:
                    {
                        gold = CalcChangeValue(gold, changeInfo, (ulong)value);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            LinkContainer.Instance.localStorage.SaveToLocal(FileSaveType.property);
        }
    }

    [Serializable]
    public class EquipmentItemList
    {
        public EquipmentItemList()
        {
            head = EquipmentId.NotExistId;
            pickax = EquipmentId.NotExistId;
            shoes = EquipmentId.NotExistId;
            buffs = new List<ItemId>();
        }

        [SerializeField] private EquipmentId pickax;
        [SerializeField] private EquipmentId head;
        [SerializeField] private EquipmentId shoes;
        public List<ItemId> buffs;

        public ItemId GetEquipmentId(EquipmentType equipmentType)
        {
            ItemId itemId = ItemId.NotExistId;

            switch (equipmentType)
            {
                case EquipmentType.head:
                    {
                        itemId = (ItemId)head;
                        break;
                    }
                case EquipmentType.pickax:
                    {
                        itemId = (ItemId)pickax;
                        break;
                    }
                case EquipmentType.shoes:
                    {
                        itemId = (ItemId)shoes;
                        break;
                    }
                default:
                    {
                        Debug.Log("equipment type is not correct");
                        break;
                    }
            }

            return itemId;
        }

        public bool ChangeEquipmentId(EquipmentType equipmentType, ItemId itemId)
        {
            switch (equipmentType)
            {
                case EquipmentType.head:
                    {
                        head = (EquipmentId)itemId;
                        break;
                    }
                case EquipmentType.pickax:
                    {
                        pickax = (EquipmentId)itemId;
                        break;
                    }
                case EquipmentType.shoes:
                    {
                        shoes = (EquipmentId)itemId;
                        break;
                    }
                default:
                    {
                        Debug.Log("Equipment type is not exist");
                        return false;
                    }
            }

            LinkContainer.Instance.localStorage.SaveToLocal(FileSaveType.equipment);

            return true;
        }
    }
}
