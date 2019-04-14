using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
    public class BankManager : MonoSingletonBase<BankManager>
    {
        // 돈 빌리기(신용등급 차등)
        // 돈 값기
        // 예금하기
        // 출금하기
        // 예금 이자(신용등급  차등)
        // 빌린돈 이자

        private LocalStorage localStorage;
        private BankMetadata bankMetadata;

        //private ItemBase itemRefDic;

        private const decimal maxDepositInterValue = 10000000;

        //public uint maxDepositDefault;
        //public uint maxDepositPerCredit;
        //[SerializeField] private uint maxLoanDefault;

        // Use this for initialization
        void Start()
        {
            bankMetadata = LinkContainer.Instance.bankMetadata;
            localStorage = LinkContainer.Instance.localStorage;

            //RenewInterest();
            //StockManager.Instance.GetStockPriceDatas();
            //LinkContainer.Instance.storeMetadata.storeNpcDic[StoreNpcId.MineAreaStoreNpc].RenewItemList();
        }

        public void RenewInterest()
        {
            int changeNum = GLAPI.GetTimeChangeAmount(localStorage.recentTimeData.GetBankRecentTime, bankMetadata.GetInterRenewTime, bankMetadata.GetMaxInterAmount);

            DEBUG.Log(changeNum);

            if (changeNum > 0)
            {
                decimal curDepositValue;

                if (localStorage.playerProperty.GetDeposit < maxDepositInterValue)
                {
                    curDepositValue = localStorage.playerProperty.GetDeposit;
                }
                else
                {
                    curDepositValue = maxDepositInterValue;
                }

                if (localStorage.playerProperty.GetDeposit > 0)
                {
                    decimal interest = changeNum * curDepositValue * (decimal)bankMetadata.depositInterPerCredit[localStorage.playerStat.GetCredit - 1];
                    double exp = decimal.ToDouble(interest / (decimal)bankMetadata.GetExpDivAmountPerInter);

                    if (exp > 0)
                    {
                        localStorage.playerStat.ChangeExp(PlayerExpChangeInfo.creditExp, ValueChangeInfo.increase, exp);
                    }

                    localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.deposit, ValueChangeInfo.increase,
                        interest);
                }

                if (localStorage.playerProperty.GetLoan > 0)
                {
                    decimal interest = changeNum * localStorage.playerProperty.GetLoan * (decimal)bankMetadata.depositInterPerCredit[localStorage.playerStat.GetCredit - 1];
                    double exp = decimal.ToDouble(interest / (decimal)bankMetadata.GetExpDivAmountPerInter);

                    if (exp > 0)
                    {
                        localStorage.playerStat.ChangeExp(PlayerExpChangeInfo.fitnessExp, ValueChangeInfo.increase, exp);
                    }

                    localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.loan, ValueChangeInfo.increase,
                        changeNum * localStorage.playerProperty.GetLoan * (decimal)bankMetadata.depositInterPerCredit[localStorage.playerStat.GetCredit]);
                }

                GLAPI.StatExpInc(PlayerExpChangeInfo.creditExp);

            }
        }

        public bool RepayMoney(uint money)
        {
            if (money > localStorage.playerProperty.GetLoan)
            {
                UIManager.Instance.exeRequestToasting("대출 중인 금액보다 많습니다");
                return false;
            }

            if (localStorage.playerProperty.GetCash >= money)
            {
                localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, money);
                localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.loan, ValueChangeInfo.decrease, money);
                UIManager.Instance.exeRequestToasting(string.Format("금액 {0}을 현금으로 지불했습니다", GLAPI.convertCashToWon(money)));
            }
            else if (localStorage.playerProperty.GetDeposit >= money)
            {
                localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.deposit, ValueChangeInfo.decrease, money);
                localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.loan, ValueChangeInfo.decrease, money);
                UIManager.Instance.exeRequestToasting(string.Format("금액 {0}을 계좌 잔액으로 지불했습니다", GLAPI.convertCashToWon(money)));
            }
            else
            {
                UIManager.Instance.exeRequestToasting("현금 또는 예금액이 부족합니다");
                return false;
            }

            return true;
        }

        public ulong GetLoanableAmount()
        {
            ulong loneableAmount;

            if (decimal.ToUInt64(localStorage.playerProperty.GetLoan) > bankMetadata.maxLoanPerCredit[(int)localStorage.playerStat.GetCredit - 1])
            {
                loneableAmount = 0;
            }
            else
            {
                loneableAmount = bankMetadata.maxLoanPerCredit[(int)localStorage.playerStat.GetCredit - 1] - decimal.ToUInt64(localStorage.playerProperty.GetLoan);
            }

            return loneableAmount;
        }

        public bool LoanMoney(uint money)
        {
            if (money > GetLoanableAmount())
            {
                UIManager.Instance.exeRequestToasting("대출 가능한 금액이 아닙니다");
                return false;
            }

            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.loan, ValueChangeInfo.increase, money);
            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.deposit, ValueChangeInfo.increase, money);
            UIManager.Instance.exeRequestToasting(string.Format("금액 {0}이 계좌에 지급됐습니다", GLAPI.convertCashToWon(money)));
            return true;
        }

        public bool WithdrawMoney(decimal deposit)
        {
            if (localStorage.playerProperty.GetDeposit < deposit)
            {
                UIManager.Instance.exeRequestToasting("예금액이 부족합니다");
                return false;
            }

            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.deposit, ValueChangeInfo.decrease, deposit);
            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.increase, deposit);
            UIManager.Instance.exeRequestToasting(string.Format("금액 {0}을 출금했습니다", GLAPI.convertCashToWon(deposit)));
            return true;
        }

        public bool DepositMoney(decimal cash)
        {
            if (localStorage.playerProperty.GetCash < cash)
            {
                UIManager.Instance.exeRequestToasting("현금이 부족합니다");
                return false;
            }

            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, cash);
            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.deposit, ValueChangeInfo.increase, cash);
            UIManager.Instance.exeRequestToasting(string.Format("금액 {0}을 예금했습니다", GLAPI.convertCashToWon(cash)));
            return true;
        }
    }
}