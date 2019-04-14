using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System.Text;
using System;

namespace CityStage
{
    public class StatFactorManager : MonoSingletonBase<StatFactorManager>
    {
        ItemMetadata itemMetadata;
        LocalStorage localStorage;

        [SerializeField] private ItemStatFactor itemStatFactor;

        [SerializeField] [Range(0f, 0.9f)] private float maxStaminaRecoverySpeed;
        [SerializeField] [Range(0f, 0.9f)] private float maxStaminaIncrement;
        [SerializeField] [Range(0f, 0.9f)] private float maxMiningSpeed;
        [SerializeField] [Range(0f, 0.9f)] private float maxMiningDoubleChance;
        [SerializeField] [Range(0f, 0.9f)] private float maxMoveSpeed;
        [SerializeField] [Range(0f, 0.9f)] private float maxStaminaDecrement;
        [SerializeField] [Range(0f, 0.9f)] private float maxNoStaminaConsumtionChance;

        public float GetMaxStaminaRecoverySpeed { get { return maxStaminaRecoverySpeed; } }
        public float GetMaxStaminaIncrement { get { return maxStaminaIncrement; } }
        public float GetMaxMiningSpeed { get { return maxMiningSpeed; } }
        public float GetMaxMiningDoubleChance { get { return maxMiningDoubleChance; } }
        public float GetMaxMoveSpeed { get { return maxMoveSpeed; } }
        public float GetMaxStaminaDecrement { get { return maxStaminaDecrement; } }
        public float GetMaxNoStaminaConsumtionChance { get { return maxNoStaminaConsumtionChance; } }

        public uint GetCharm { get { return localStorage.playerStat.GetCharm; } }
        public float GetAffectionBous { get { return LinkContainer.Instance.storeMetadata.affectionBonusPerCharm[(int)(localStorage.playerStat.GetCharm - 1)]; } }
        public float GetMaxAffectionBonus { get { return LinkContainer.Instance.storeMetadata.maxAffectionPerCharm[(int)(localStorage.playerStat.GetCharm - 1)]; } }
        public uint GetFitness { get { return localStorage.playerStat.GetFitness; } }
        public double GetFitnessExp { get { return localStorage.playerStat.GetFitnessExp / LinkContainer.Instance.charActionMetadata.demandExpPerFitness[(int)(localStorage.playerStat.GetFitness - 1)]; } }
        public uint GetCredit { get { return localStorage.playerStat.GetCredit; } }
        public double GetCreditExp { get { return localStorage.playerStat.GetCreditExp / LinkContainer.Instance.bankMetadata.demandExpPerCredit[(int)(localStorage.playerStat.GetCredit - 1)]; } }
        public decimal GetDepositValue { get { return localStorage.playerProperty.GetDeposit; } }
        public double GetDepositInter { get { return LinkContainer.Instance.bankMetadata.depositInterPerCredit[(int)(localStorage.playerStat.GetCredit - 1)]; } }
        public decimal GetLoanValue { get { return localStorage.playerProperty.GetLoan; } }
        public double GetMaxLoanValue { get { return LinkContainer.Instance.bankMetadata.maxLoanPerCredit[(int)(localStorage.playerStat.GetCredit - 1)]; } }
        public double GetLoanInter { get { return LinkContainer.Instance.bankMetadata.loanInterPerCredit[(int)(localStorage.playerStat.GetCredit - 1)]; } }
        public double GetCurStamina { get { return localStorage.playerStat.GetCurStamina; } }
        public double GetMaxStamina { get { return StaminaManager.Instance.GetMaxStamina(); } }

        public float GetStaminaRecoverySpeed { get { float value = itemStatFactor.GetStaminaRecoverySpeed; if (value > maxStaminaRecoverySpeed) { value = maxStaminaRecoverySpeed; } return value; } }
        public float GetStaminaIncrement { get { float value = itemStatFactor.GetStaminaIncrement; if (value > maxStaminaIncrement) { value = maxStaminaIncrement; } return value; } }
        public float GetAutoStaminaRecoveryAmount { get { return itemStatFactor.GetAutoStaminaRecoveryAmount; } }
        public float GetMiningSpeed { get { float value = itemStatFactor.GetMiningSpeed; if (value > maxMiningSpeed) { value = maxMiningSpeed; } return value; } }
        public float GetMiningDoubleChance { get { float value = itemStatFactor.GetMiningDoubleChance; if (value > maxMiningDoubleChance) { value = maxMiningDoubleChance; } return value; } }
        public OreId GetSpecificOreDoubleChance { get { return itemStatFactor.GetSpecificOreDoubleChance; } }
        public float GetMoveSpeed { get { float value = itemStatFactor.GetMoveSpeed; if (value > maxMoveSpeed) { value = maxMoveSpeed; } return value; } }
        public float GetStaminaDecrement { get { float value = itemStatFactor.GetStaminaDecrement; if (value > maxStaminaDecrement) { value = maxStaminaDecrement; } return value; } }
        public float GetNoStaminaConsumtionChance { get { float value = itemStatFactor.GetNoStaminaConsumtionChance; if (value > maxNoStaminaConsumtionChance) { value = maxNoStaminaConsumtionChance; } return value; } }
        public bool GetNoStaminaConsumtionWhenMoving { get { return itemStatFactor.GetNoStaminaConsumtionWhenMoving; } }
        
        public void SetUp()
        {
            itemMetadata = LinkContainer.Instance.itemMetadata;
            localStorage = LinkContainer.Instance.localStorage;

            RenewStatFactor();
        }

        public void RenewStatFactor()
        {
            itemStatFactor.InitStats();
                       
            for(EquipmentType i=EquipmentType.head; i<EquipmentType.shoes+1; i++)
            {
                if (localStorage.equipmentItems.GetEquipmentId(i) > ItemId.NotExistId)
                {
                    itemStatFactor += itemMetadata.itemDataDic[localStorage.equipmentItems.GetEquipmentId(i)].GetItemStatFactor;
                }
            }

            foreach(ItemId id in localStorage.equipmentItems.buffs)
            {
                itemStatFactor += itemMetadata.itemDataDic[id].GetItemStatFactor;
            }
        }        
    }

    [Serializable]
    public class ItemStatFactor
    {
        [Header("Head Main Option")]
        [SerializeField] [Range(0f, 0.9f)] private float staminaRecoverySpeed;
        [Header("Head Additional Option")]
        [SerializeField] [Range(0f, 0.9f)] private float staminaIncrement;
        [SerializeField] private float autoStaminaRecoveryAmount;
        [Header("Pickax Main Option")]
        [SerializeField] [Range(0f, 0.9f)] private float miningSpeed;
        [Header("Pickax Additional Option")]
        [SerializeField] [Range(0f, 0.9f)] private float miningDoubleChance;
        [SerializeField] private OreId specificOreDoubleChance = OreId.NotExistId;
        [Header("Shoes Main Option")]
        [SerializeField] [Range(0f, 0.9f)] private float moveSpeed;
        [Header("Shoes Additional Option")]
        [SerializeField] [Range(0f, 0.9f)] private float staminaDecrement;
        [SerializeField] [Range(0f, 0.9f)] private float noStaminaConsumtionChance;
        [SerializeField] private bool noStaminaConsumtionWhenMoving = false;

        public float GetStaminaRecoverySpeed { get { return staminaRecoverySpeed; } }
        public float GetStaminaIncrement { get { return staminaIncrement; } }
        public float GetAutoStaminaRecoveryAmount { get { return autoStaminaRecoveryAmount; } }
        public float GetMiningSpeed { get { return miningSpeed; } }
        public float GetMiningDoubleChance { get { return miningDoubleChance; } }
        public OreId GetSpecificOreDoubleChance { get { return specificOreDoubleChance; } }
        public float GetMoveSpeed { get { return moveSpeed; } }
        public float GetStaminaDecrement { get { return staminaDecrement; } }
        public float GetNoStaminaConsumtionChance { get { return noStaminaConsumtionChance; } }
        public bool GetNoStaminaConsumtionWhenMoving { get { return noStaminaConsumtionWhenMoving; } }

        public void InitStats()
        {
            staminaRecoverySpeed = 0f;
            staminaIncrement = 0f;
            autoStaminaRecoveryAmount = 0f;
            miningSpeed = 0f;
            miningDoubleChance = 0f;
            specificOreDoubleChance = OreId.NotExistId;
            moveSpeed = 0f;
            staminaDecrement = 0f;
            noStaminaConsumtionChance = 0f;
            noStaminaConsumtionWhenMoving = false;
        }

        public string GetStatFactorDescription()
        {
            StringBuilder description = new StringBuilder();

            if (GetStaminaRecoverySpeed > 0)
            {
                description.Append(String.Format("스태미너 회복속도 + {0:p}\n", GetStaminaRecoverySpeed));
            }
            if (GetStaminaIncrement > 0)
            {
                description.Append(String.Format("스태미너 증가량 + {0:p}\n", GetStaminaIncrement));
            }
            if (GetAutoStaminaRecoveryAmount > 0)
            {
                description.Append(String.Format("초당 스태미너 자동 회복량 + {0}\n", autoStaminaRecoveryAmount));
            }
            if (GetMiningSpeed > 0)
            {
                description.Append(String.Format("채광속도 + {0:p}\n", GetMiningSpeed));
            }
            if (GetMiningDoubleChance > 0)
            {
                description.Append(String.Format("채광량 2배 확률 + {0:p}\n", miningDoubleChance));
            }
            if (GetSpecificOreDoubleChance > OreId.NotExistId)
            {
                description.Append(String.Format("{0} 채광률 2배\n", LinkContainer.Instance.itemMetadata.itemDataDic[(ItemId)specificOreDoubleChance].GetName));
            }
            if (GetMoveSpeed > 0)
            {
                description.Append(String.Format("이동속도 + {0:p}\n", GetMoveSpeed));
            }
            if (GetStaminaDecrement > 0)
            {
                description.Append(String.Format("스태미너 감소량 + {0:p}\n", GetStaminaDecrement));
            }
            if (GetNoStaminaConsumtionChance > 0)
            {
                description.Append(String.Format("스태미너가 소모되지 않을 확률 + {0:p}\n", noStaminaConsumtionChance));
            }
            if (GetNoStaminaConsumtionWhenMoving)
            {
                description.Append(String.Format("이동중에 스태미너가 소모되지 않음"));
            }

            return description.ToString();
        }

        public static ItemStatFactor operator +(ItemStatFactor leftSide, ItemStatFactor rightSide)
        {
            leftSide.staminaRecoverySpeed += rightSide.staminaRecoverySpeed;
            leftSide.staminaIncrement += rightSide.staminaIncrement;
            leftSide.autoStaminaRecoveryAmount += rightSide.autoStaminaRecoveryAmount;
            leftSide.miningSpeed += rightSide.miningSpeed;
            leftSide.miningDoubleChance += rightSide.miningDoubleChance;
            leftSide.specificOreDoubleChance = rightSide.specificOreDoubleChance;
            leftSide.moveSpeed += rightSide.moveSpeed;
            leftSide.staminaDecrement += rightSide.staminaDecrement;
            leftSide.noStaminaConsumtionChance += rightSide.noStaminaConsumtionChance;
            leftSide.noStaminaConsumtionWhenMoving = rightSide.noStaminaConsumtionWhenMoving;

            return leftSide;
        }
    }
}