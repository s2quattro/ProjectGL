using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
    [CreateAssetMenu(fileName = "CardGameMetadata(CityStage)", menuName = "Storages/CardGameMetadata(CityStage)")]
    public class CardGameMetadata : ScriptableObject
    {
        [SerializeField] private List<CardData> cardDatas;
        public Dictionary<CardId, CardData> cardDataDic = new Dictionary<CardId, CardData>();
        public int GetTotalNumberOfCards { get { return cardDatas.Count; } }

        private List<CardId> cardIdList = new List<CardId>();

        public List<CardId> GetCardIdList()
        {
            cardIdList.Clear();

            foreach (CardData data in cardDatas)
            {
                cardIdList.Add(data.GetCardId);
            }

            return cardIdList;
        }

        public void SetDicData()
        {
            foreach (CardData data in cardDatas)
            {
                cardDataDic.Add(data.GetCardId, data);
            }

            for (CardId i = CardId.none + 1; i < CardId.last; i++)
            {
                if (!cardDataDic.ContainsKey(i))
                {
                    UIManager.Instance.exeRequestToasting("모든 카드 ID가 딕셔너리에 제대로 등록되지 않았습니다");
                }
            }
        }
    }

    [Serializable]
    public class CardData
    {
        [SerializeField] private CardId cardId;
        [SerializeField] private string description;
        [SerializeField] [Range(1f, 50f)] private float dividendRate;
        public List<uint> hitNums;

        public CardId GetCardId { get { return cardId; } }
        public string GetDescription { get { return description; } }
        public float GetDividendRate { get { return dividendRate; } }
    }

    public enum CardId
    {
        none, multipleOfTwo, multipleOfTree, multipleOfFive, multipleOfSeven, last
    }
}