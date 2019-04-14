using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using GLCore;

namespace CityStage
{
	public class Horse_Controller : MonoSingletonBase<Horse_Controller>
	{
		[Serializable]
		public struct EventList
		{
			public float speed;
			public string massage;
		}

		[Serializable]
		public struct HorseList
		{
			public GameObject horse;
			public Transform startPos;
			public Transform destPos;
			public Text text;

			[HideInInspector]
			public float speed;
			[HideInInspector]
			public float curSpeed;
		}

        private LocalStorage localStorage;
        //private MessageMetadata messageMetadata;

		[SerializeField] private HorseList[] horseList = new HorseList[3];
        [SerializeField] private EventList[] eventList;
        [SerializeField] private uint minBattingPrice;
        [SerializeField] [Range(0.1f, 1f)] private float minDistToDest = 0.5f;
        [SerializeField] private float minStartSpeed = 1.2f;
        [SerializeField] private float maxStartSpeed = 1.8f;
        [SerializeField] private float lerpSpeed = 1f;
        [SerializeField] private float minEventTime = 1f;
        [SerializeField] private float maxEventTime = 3f;

        private List<HorseRaceSaveData> horseRaceSaveDatas = new List<HorseRaceSaveData>();

		private float delta = 0f;
		private bool isRaceStart = false;

		//private int selectHorseNum;
		//private uint putMoney;
		private int winHorseNum = -1;

        //private decimal putMoney;
        
        private const float SpeedConstValue = 0.5f;
        private const int StartSpeedNotEvent = -1;
        private const int OnlyEventDrationTime = -2;
        private const int BattingVictoryAmount = 3;

        private void Start()
        {
            localStorage = LinkContainer.Instance.localStorage;
            //messageMetadata = LinkContainer.GetInstance().messageMetadata;
        }

        // Update is called once per frame
        void Update()
		{
            if (isRaceStart)
			{
				CheckRace();
			}
		}

        public bool ContinueRace()
        {                
            if(localStorage.horseRaceSaveDatas.Count < 1)
            {
                UIManager.Instance.exeRequestToasting("이어할 수 있는 데이터가 없습니다");
                return false;
            }

            horseRaceSaveDatas = new List<HorseRaceSaveData>(localStorage.horseRaceSaveDatas);

            // 초기 말 속도 지정
            for (int i = 0; i < horseList.Length; i++)
            {
                horseList[i].text.text = "";
                horseList[i].horse.transform.position = horseList[i].startPos.position;
                horseList[i].speed = 0f;
                horseList[i].curSpeed = 0f;
            }

            delta = 0f;

            isRaceStart = true;
            return true;
        }

		public bool StartRace(HorseNum horseNum, decimal money)
        {
            if (localStorage.playerStat.GetCurStamina < StaminaManager.Instance.GetStaminaUsage(CharActionType.Racing))
            {
                UIManager.Instance.exeRequestToasting("스태미너가 부족합니다");
                return false;
            }

            if (localStorage.playerProperty.GetCash < money)
            {
                UIManager.Instance.exeRequestToasting("현금이 부족합니다");
                return false;
            }
            else if((money < minBattingPrice) || (money <= 0))
            {
                UIManager.Instance.exeRequestToasting("배팅 금액이 적습니다");
                return false;
            }            

			//selectHorseNum = (int)horseNum;
			//putMoney = money;

            localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.decrease, money);
            localStorage.horseRacePutMoney = money.ToString();
            localStorage.horseRaceSelectHorseNum = (int)horseNum;
            localStorage.horseRaceSaveDatas.Clear();

            // 초기 말 속도 지정
            for (int i = 0; i < horseList.Length; i++)
			{
				horseList[i].text.text = "";
				horseList[i].horse.transform.position = horseList[i].startPos.position;
				horseList[i].speed = UnityEngine.Random.Range(minStartSpeed, maxStartSpeed);
                horseList[i].curSpeed = 0f;

                localStorage.horseRaceSaveDatas.Add(new HorseRaceSaveData(i, StartSpeedNotEvent, 0f, horseList[i].speed));
            }

			// 이벤트 발생 시간 랜덤
			delta = UnityEngine.Random.Range(minEventTime, maxEventTime);

            localStorage.horseRaceSaveDatas.Add(new HorseRaceSaveData(0, OnlyEventDrationTime, delta));

            isRaceStart = true;

            return true;
        }

		private void CheckRace()
		{
			for (int i = 0; i < horseList.Length; i++)
			{
				if (Vector2.Distance(horseList[i].horse.transform.position, horseList[i].destPos.position) < minDistToDest)
				{
					isRaceStart = false;
					winHorseNum = i;
					FinishRace();

                    return;
				}
			}

			if (delta > 0f)
			{
				delta -= Time.deltaTime;

				for (int i = 0; i < horseList.Length; i++)
				{
					horseList[i].curSpeed = Mathf.Lerp(horseList[i].curSpeed, horseList[i].speed, Time.deltaTime);
					horseList[i].horse.transform.Translate(Vector2.right * Time.deltaTime * horseList[i].curSpeed * SpeedConstValue);
				}
			}
			else
			{
                if(horseRaceSaveDatas.Count > 0)
                {
                    HorseRaceSaveData data = horseRaceSaveDatas[0];

                    if (data.eventNum == StartSpeedNotEvent)
                    {
                        delta = data.durationTime;
                        horseList[data.horseNum].speed = data.speed;
                        horseList[data.horseNum].text.text = "";
                    }
                    else if (data.eventNum == OnlyEventDrationTime)
                    {
                        delta = data.durationTime;
                    }
                    else
                    {
                        delta = data.durationTime;
                        horseList[data.horseNum].speed = eventList[data.eventNum].speed;
                        horseList[data.horseNum].text.text = eventList[data.eventNum].massage;
                    }

                    horseRaceSaveDatas.Remove(data);
                }
                else
                {
                    int horseNum = UnityEngine.Random.Range(0, horseList.Length);
                    int eventNum = UnityEngine.Random.Range(0, eventList.Length);

                    delta = UnityEngine.Random.Range(minEventTime, maxEventTime);

                    horseList[horseNum].speed = eventList[eventNum].speed;
                    horseList[horseNum].text.text = eventList[eventNum].massage;

                    localStorage.horseRaceSaveDatas.Add(new HorseRaceSaveData(horseNum, eventNum, delta));
                }
            }
		}

		private void FinishRace()
		{
            if (localStorage.horseRaceSelectHorseNum == winHorseNum)
            {
                localStorage.playerProperty.ChangeProperty(PlayerPropertyChangeInfo.cash, ValueChangeInfo.increase, Convert.ToDecimal(localStorage.horseRacePutMoney) * BattingVictoryAmount);
                UIManager.Instance.exeOpenSimpleMessageBox("승리 했습니다!", string.Format("+ {0} X {1}", GLAPI.StringToWon(localStorage.horseRacePutMoney), BattingVictoryAmount));
			}
            else
            {
                UIManager.Instance.exeOpenSimpleMessageBox("패배 했습니다!", string.Format("- {0}", GLAPI.StringToWon(localStorage.horseRacePutMoney)));
            }

            localStorage.horseRaceSaveDatas.Clear();

			//Ended
			ModuleTrigger.Instance.finishRacingGame();
		}
	}
}