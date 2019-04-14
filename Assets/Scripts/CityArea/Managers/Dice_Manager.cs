using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GLCore;
using TMPro;

namespace CityStage
{
    public class Dice_Manager : MonoBehaviour
    {
        public TextMeshProUGUI[] diceText = new TextMeshProUGUI[Constants.diceStatCount];

        [Range(Constants.statMin, Constants.statMax-1)]
        public int maxStat;
        [Range(5, 9)] public int maxTotalStat;
        private int randomPlusTotalStat;    // 현재는 사용하지 말 것

        private int sumStat = 0;   // 총합 계산용
        private int remainStat;    // 남은 스탯량

        private List<int> statList = new List<int>();
        private int sizeofStatList;
        private List<uint> stats = new List<uint>();

        private void Start()
        {
            // 기본값 설정
            stats.Add(Constants.genderMan);
            stats.Add(Constants.statMin);
            stats.Add(Constants.statMin);
            stats.Add(Constants.statMin);
        }

        public void PushDiceButton()
        {
            sumStat = 0;

            switch (Random.Range(0, 2))  // 일단 성별을 결정
            {
                case 0:
                    {
                        diceText[Constants.diceStatGender].text = "Man";
                        stats[Constants.diceStatGender] = Constants.genderMan;
                        break;
                    }
                case 1:
                    {
                        diceText[Constants.diceStatGender].text = "Woman";
                        stats[Constants.diceStatGender] = Constants.genderWoman;
                        break;
                    }
            }

            statList.Add(Constants.diceStatCharm);
            statList.Add(Constants.diceStatFitness);
            statList.Add(Constants.diceStatCredit);
            //statList.Add(Constants.diceTextStat4);

            remainStat = maxTotalStat + Random.Range(0, randomPlusTotalStat + 1); // 추가 스탯 랜덤 결정

            sizeofStatList = statList.Count;

            for (int i = 0; i < sizeofStatList; i++)
            {
                int random = Random.Range(0, statList.Count); // 0~3 사이 랜덤

                RandomStat(statList[random]);

                statList.Remove(statList[random]);
            }

            //diceText[diceText.Length-1].text = sumStat.ToString();  // 스탯 총합 표시        
        }

        public void SetStatButton()
        {
            LinkContainer.Instance.localStorage.playerStat.ChangeStat(PlayerStatChangeInfo.gender, ValueChangeInfo.set, stats[Constants.diceStatGender]);
            LinkContainer.Instance.localStorage.playerStat.ChangeStat(PlayerStatChangeInfo.charm, ValueChangeInfo.set, stats[Constants.diceStatCharm]);
            LinkContainer.Instance.localStorage.playerStat.ChangeStat(PlayerStatChangeInfo.fitness, ValueChangeInfo.set, stats[Constants.diceStatFitness]);
            LinkContainer.Instance.localStorage.playerStat.ChangeStat(PlayerStatChangeInfo.credit, ValueChangeInfo.set, stats[Constants.diceStatCredit]);

            // 다이스 매니저 종료후 게임시작
        }

        private void RandomStat(int diceNum)
        {
            int random;

            if (statList.Count == 1)
            {
                random = remainStat;
            }
            else if ((remainStat - statList.Count + 1) >= maxStat)
            {
                random = Random.Range(1, maxStat + 1);
            }
            else
            {
                random = Random.Range(1, (remainStat - statList.Count + 2));
            }

            remainStat -= random;
            sumStat += random;

            stats[diceNum] = (uint)random;

            diceText[diceNum].text = Constants.RankString[random - 1].ToString();
        }
    }
}