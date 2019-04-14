using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;

namespace CityStage
{
    public class StaminaManager : MonoSingletonBase<StaminaManager>
    {
        private LocalStorage localStorage;
        private CharActionMetadata charActionMetadata;

        //[SerializeField] public const double DefaultMaxStamina = 100d;
        //[SerializeField] private double FactorMaxStaminaPerFitness;
        //[SerializeField] [Range(0f, 10f)] private float defaultStaminaRecoveryDelay;

        // Use this for initialization
        void Start()
        {
            localStorage = LinkContainer.Instance.localStorage;
            charActionMetadata = LinkContainer.Instance.charActionMetadata;
        }

        public double GetStaminaUsage(CharActionType charActionType)
        {
            double resultStaminaUsage = 0d;
            
            if (StatFactorManager.Instance.GetStaminaDecrement < StatFactorManager.Instance.GetMaxStaminaDecrement)
            {
                resultStaminaUsage = charActionMetadata.charActionDic[charActionType].GetStaminaUsage * (1d - StatFactorManager.Instance.GetStaminaDecrement);                
            }
            else
            {
                resultStaminaUsage = charActionMetadata.charActionDic[charActionType].GetStaminaUsage * (1d - StatFactorManager.Instance.GetMaxStaminaDecrement); 
            }

            return resultStaminaUsage;
        }

        public void UseStamina(CharActionType charActionType)
        {            
            double resultStaminaUsage = GetStaminaUsage(charActionType);

            if (charActionMetadata.charActionDic[charActionType].GetFintnessExpInc > 0d)
            {
                localStorage.playerStat.ChangeExp(PlayerExpChangeInfo.fitnessExp, ValueChangeInfo.increase, charActionMetadata.charActionDic[charActionType].GetFintnessExpInc);

                GLAPI.StatExpInc(PlayerExpChangeInfo.fitnessExp);
            }

            if (Random.Range(0f, 1f) <= StatFactorManager.Instance.GetNoStaminaConsumtionChance)
            {
                return;
            }

            // 현재 스태미나에서 스태미너 감소량만큼 뺀 값이 음수가 아니어야 한다
            if ((localStorage.playerStat.GetCurStamina - resultStaminaUsage) >= 0)
            {
                localStorage.playerStat.ChangeCurStamina(ValueChangeInfo.decrease, resultStaminaUsage);
            }
            else
            {
                localStorage.playerStat.ChangeCurStamina(ValueChangeInfo.set, 0d);
            }           
            
            // UI에 스태미너 증가치 표시하는 요청함수 추가할 것
        }

        public double GetMaxStamina()
        {
            return CharActionMetadata.DefaultMaxStamina + (localStorage.playerStat.GetFitness) * charActionMetadata.FactorMaxStaminaPerFitness;
        }

        public bool RecoveryStamina(float recoveryAmount)
        {
            bool isStaminaMaximum = false;

            double curStamina = localStorage.playerStat.GetCurStamina;
            double maxStamina = GetMaxStamina();
            double RecoveryAmount = recoveryAmount * (1d + StatFactorManager.Instance.GetStaminaIncrement);

            if ((curStamina + RecoveryAmount) >= maxStamina)
            {
                //RecoveryAmount = maxStamina - curStamina;
                isStaminaMaximum = true;
            }

            localStorage.playerStat.ChangeCurStamina(ValueChangeInfo.increase, RecoveryAmount);
            // UI에 스태미너 증가치 표시하는 요청함수 추가할 것

            return isStaminaMaximum;
        }

        public float GetRecoveryStaminaDelay()
        {
            float delay;

            delay = charActionMetadata.defaultStaminaRecoveryDelay * (1- + StatFactorManager.Instance.GetStaminaRecoverySpeed);

            return delay;
        }
    }
}