using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
    public class CardGameManager : MonoSingletonBase<CardGameManager>
    {
        private List<CardId> cardList = new List<CardId>();
        private List<CardId> cardIdList;
        private CardGameMetadata cardGameMetadata;
        private LocalStorage localStorage;
        
        private int randomNumer = 0;
        private int gameRound = 0;
        private float dividendRate = 1;
        private decimal putMoney;
        
        private const int NumberOfCards = 3;
        private const int FirstNumber = 1;
        private const int LastNumber = 50;
                
        public int GetCurRandomNumber { get { return randomNumer; } }
        public int GetCurGameRound { get { return gameRound; } }
        public float GetCurDividendRate { get { return dividendRate; } }

        // Use this for initialization
        void Start()
        {
            cardGameMetadata = LinkContainer.Instance.cardGameMetadata;
            localStorage = LinkContainer.Instance.localStorage;
        }

        /* 카드게임 순서
        1. StartCardGame
        2. GetResult
        3. 이겼을 경우 일단 Continue 부르고 이후 Stop 선택 가능
        4. 2번, 3번을 반복
        */

        public List<CardId> StartCardGame(decimal putMoney)
        {
            if(localStorage.playerProperty.CompareCashToPrice(putMoney))
            {
                this.putMoney = putMoney;
            }
            else
            {
                UIManager.Instance.exeRequestToasting( "현금이 부족합니다");
                return null;
            }

            dividendRate = 1f;
            gameRound = 0;          

            return GetRandomCardId();
        }

        private List<CardId> GetRandomCardId()
        {
            cardList.Clear();
            cardIdList = cardGameMetadata.GetCardIdList();
            randomNumer = UnityEngine.Random.Range(FirstNumber, LastNumber);

            for (int i = 0; i < NumberOfCards; i++)
            {
                CardId randomCardId = (CardId)UnityEngine.Random.Range((int)cardIdList[0], (int)cardIdList[cardIdList.Count - 1]);
                cardList.Add(randomCardId);
                cardIdList.Remove(randomCardId);
            }

            return cardIdList;
        }

        public List<CardId> ContinueCardGame()
        {
            gameRound++;

            return GetRandomCardId();
        }

        public void StopCardGame()
        {
            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.increase, putMoney * (decimal)dividendRate - putMoney);
        }

        public bool GetResult(CardId selectCardId)
        {
            foreach(int number in cardGameMetadata.cardDataDic[selectCardId].hitNums)
            {
                if(randomNumer == number)
                {
                    dividendRate *= cardGameMetadata.cardDataDic[selectCardId].GetDividendRate;
                    return true;
                }
            }

            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, putMoney);
            return false;
        }
    }
}