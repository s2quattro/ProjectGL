using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;

namespace CityStage
{
    [CreateAssetMenu(fileName = "CharActionMetadata(CityStage)", menuName = "Storages/CharActionMetadata(CityStage)")]
    public class CharActionMetadata : ScriptableObject
    {
        public List<double> demandExpPerFitness;
        public List<CharAction> charAction;
        public Dictionary<CharActionType, CharAction> charActionDic = new Dictionary<CharActionType, CharAction>();

        [Header("About Move")]
        public float defaultMoveSpeed;
        public float moveSpeedMultWhenStaminaZero;

        [Header("About Stamina")]
        public double FactorMaxStaminaPerFitness;
        public float defaultStaminaRecoveryDelay;
        public const double DefaultMaxStamina = 100d;

        [Header("About Minning")]
        public float defaultMinningTime;
        
        
        //[SerializeField] [Range(0f, 10f)] private float defaultStaminaRecoveryDelay;
        //[SerializeField] private float defaultMaxStamina;
        //[SerializeField] private float FactorMaxStaminaPerFitness;

        public void SetDicData()
        {
            foreach (CharAction data in charAction)
            {
                charActionDic.Add(data.GetCharActionType, data);
            }
        }
    }
}