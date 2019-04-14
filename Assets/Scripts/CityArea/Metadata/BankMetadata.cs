using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;

namespace CityStage
{
    [CreateAssetMenu(fileName = "BankMetadata(CityStage)", menuName = "Storages/BankMetadata(CityStage)")]
    public class BankMetadata : ScriptableObject
    {        
        public ulong[] maxLoanPerCredit;
        [Range(0f, 1f)] public double[] depositInterPerCredit;
        [Range(0f, 1f)] public double[] loanInterPerCredit;
        public double[] demandExpPerCredit;

        [SerializeField] private double expDivAmountPerInter;
        [SerializeField] private uint interRenewTime;
        [SerializeField] private uint maxInterAmount;

        public double GetExpDivAmountPerInter { get { return expDivAmountPerInter; } }
        public uint GetInterRenewTime { get { return interRenewTime; } }
        public uint GetMaxInterAmount { get { return maxInterAmount; } }

        //public CustumDateTime recentTime;
    }
}