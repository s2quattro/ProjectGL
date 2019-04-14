using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;

namespace CityStage
{
	public class OreManager : MonoSingletonBase<OreManager>
	{
		[Serializable]
		public class OrePercentageData
		{
			[SerializeField] private OreId oreId;
			[SerializeField] private uint percentageValue;

            public OreId GetOreId { get { return oreId; } }
            public uint GetPercentageValue { get { return percentageValue; } }
        }

		public uint maxOreCount;
		public uint curOreCount = 0;
		public float minOreSpawnTime;
		public float maxOreSpawnTime;

		[SerializeField] private List<OrePercentageData> orePercentages;

		private List<OreEntityBehavior> oreList = new List<OreEntityBehavior>();
		private List<int> nonactivatedOreList = new List<int>();
		private List<int> activatedOreList = new List<int>();
		private float delta = 0f;

		private List<uint> accumulateValues = new List<uint>();

        private const int SpecificOreDoubleChanceMagnification = 2;

        private LocalStorage localStorage;

		// Use this for initialization
		public void SetUp()
		{
			SetOreLists();

            localStorage = LinkContainer.Instance.localStorage;
		}

		// Update is called once per frame
		void Update()
		{
			CheckOreSpawn();
		}

		public void TakeOre(OreEntityBehavior oreEntity)
		{
            if(localStorage.playerStat.GetCurStamina < StaminaManager.Instance.GetStaminaUsage(CharActionType.OreMining))
            {
                UIManager.Instance.exeRequestToasting("스태미너가 부족합니다");
                return;
            }

			curOreCount--;

			BlockManager.Instance.DeleteObjectToBlock(oreEntity);

			activatedOreList.Remove(oreEntity.oreNum);
			nonactivatedOreList.Add(oreEntity.oreNum);

			oreEntity.transform.SetParent(transform);

			oreEntity.gameObject.SetActive(false);

            MiniMapManager.Instance.DisenableIcon(oreEntity);

            if (InventoryManager.Instance.GetItem(GetRandomOre(), 1))
            {
                StaminaManager.Instance.UseStamina(CharActionType.OreMining);

                float randomValue = UnityEngine.Random.Range(0, 1f);

                if (randomValue <= StatFactorManager.Instance.GetMiningDoubleChance)
                {
                    InventoryManager.Instance.GetItem(GetRandomOre(), 1);
                }
            }
		}

		private void SetOreLists()
		{
			int oreNum = 0;

			foreach (OreEntityBehavior ore in GetComponentsInChildren<OreEntityBehavior>())
			{
				nonactivatedOreList.Add(oreNum);
				ore.oreNum = oreNum;
				oreNum++;
				oreList.Add(ore);
				ore.gameObject.SetActive(false);
                MiniMapManager.Instance.DisenableIcon(ore);
            }
		}

		private void CheckOreSpawn()
		{
			if ((curOreCount < maxOreCount) && (curOreCount < oreList.Count))
			{
				if (delta > 0f)
				{
					delta -= Time.deltaTime;
				}
				else
				{
					delta = UnityEngine.Random.Range(minOreSpawnTime, maxOreSpawnTime);

					SpawnOre();
				}
			}
		}

		private void SpawnOre()
		{
			curOreCount++;

			int randomSelectOreNum = nonactivatedOreList[UnityEngine.Random.Range(0, nonactivatedOreList.Count)];

			activatedOreList.Add(randomSelectOreNum);
			nonactivatedOreList.Remove(randomSelectOreNum);

			BlockManager.Instance.AddObjectToBlock(oreList[randomSelectOreNum]);

			oreList[randomSelectOreNum].gameObject.SetActive(true);

            if (BlockManager.Instance.IsEntityInBlock(oreList[randomSelectOreNum]))
            {
                MiniMapManager.Instance.EnableIcon(oreList[randomSelectOreNum]);
            }
            else
            {
                MiniMapManager.Instance.DisenableIcon(oreList[randomSelectOreNum]);
            }
        }

		private ItemId GetRandomOre()
		{
            ItemId randomOreId = ItemId.NotExistId;

			uint preAccumulateVaule = 0;

			accumulateValues.Clear();          

			for (int i = 0; i < orePercentages.Count; i++)
			{
                if(orePercentages[i].GetOreId == StatFactorManager.Instance.GetSpecificOreDoubleChance)
                {
                    accumulateValues.Add(preAccumulateVaule + orePercentages[i].GetPercentageValue * SpecificOreDoubleChanceMagnification);
                }
                else
                {
                    accumulateValues.Add(preAccumulateVaule + orePercentages[i].GetPercentageValue);
                }
				
				preAccumulateVaule = accumulateValues[i];
			}

			if (preAccumulateVaule > 0)
			{
				uint randomValue = (uint)UnityEngine.Random.Range(0, preAccumulateVaule + 1);

				for (int i = 0; i < accumulateValues.Count; i++)
				{
					if ((accumulateValues[i] > 0) && (randomValue <= accumulateValues[i]))
					{
						randomOreId = (ItemId)orePercentages[i].GetOreId;
						break;
					}
				}
			}
			else
			{
				Debug.Log("Ore Percentage Values not founded.");
			}

			if (randomOreId == ItemId.NotExistId)
			{
				throw new OreTypeNotFoundException("Random Ore Type Not Founded");
			}

			return randomOreId;
		}
	}
}
