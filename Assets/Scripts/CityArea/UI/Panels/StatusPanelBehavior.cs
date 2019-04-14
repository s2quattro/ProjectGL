using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using TMPro;
using GLCore;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class StatusPanelBehavior : WindowBoxUI
	{
		// Refs		
		// Buttons
		public Button btn_Close;
		public ScrollRect scrollRectCtrl;
		public List<RectTransform> contentList;		
		// Status texts
		[Space]
		public TextMeshProUGUI statu_Charm;
		public TextMeshProUGUI statu_Fitness;
		public TextMeshProUGUI statu_FitnessExp;
		public TextMeshProUGUI statu_Affection;
		public TextMeshProUGUI statu_Credit;
		public TextMeshProUGUI statu_CreditExp;
		public TextMeshProUGUI statu_DepositValue;
		public TextMeshProUGUI statu_DepositInter;
		public TextMeshProUGUI statu_LoanValue;		
		public TextMeshProUGUI statu_LoanInter;
		[Space]
		// Item Optoion
		public TextMeshProUGUI staminaRecoverySpeed;
		public TextMeshProUGUI staminaIncrement;
		public TextMeshProUGUI staminaAutoRecoveryAmount;
		public TextMeshProUGUI staminaDecrement;
		public TextMeshProUGUI staminaNoConsumtionChance;
		public TextMeshProUGUI staminaNoConsumtionWhenMoving;
		public TextMeshProUGUI moveSpeed;
		public TextMeshProUGUI miningSpeed;
		public TextMeshProUGUI miningDoubleChance;
		public TextMeshProUGUI miningSpecificOreDoubleChance;


		// Inner Field
		//const string curMaxTextTemplete = "{0} / {1}";
		//const string curMaxPercentageTextTemplete = "{0:1P} / {1:1P}";
		//const string percentageTextTemplete = "{0:1P}";
		const string middleSlashString = " / ";
		const string trueYesString = "Yes";
		const string falseNoString = "No";
		const string expMaxString = "Max";
		const string singlePercentFormat = "p0";
		const string singleOnePercentFormat = "p1";

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		// Use this for initialization
		void Start()
		{
			register();
		}

		#region Initializer

		private void Firstloader()
		{
			//print("Loaded!!");
		}

		#endregion


		#region Refresher

		// Request a List from StatFactorManager As a refresher
		public void exeRefreshPanel()
		{
			// Set Linker
			StatFactorManager statFactors = StatFactorManager.Instance;

			// Update View : Status
			statu_Charm.SetText(curMaxBuilder(Constants.RankString[statFactors.GetCharm - 1], Constants.RankString[Constants.statMax - 1]));				

			statu_Affection.SetText(curMaxPercentBuilder(statFactors.GetAffectionBous, statFactors.GetMaxAffectionBonus));

			statu_Fitness.SetText(curMaxBuilder(Constants.RankString[statFactors.GetFitness - 1], Constants.RankString[Constants.statMax - 1]));				

			expStatFillter(statu_FitnessExp, statFactors.GetFitnessExp);

			statu_Credit.SetText(curMaxBuilder(Constants.RankString[statFactors.GetCredit - 1], Constants.RankString[Constants.statMax - 1]));				

			expStatFillter(statu_CreditExp, statFactors.GetCreditExp);			
			
			statu_DepositValue.text = GLAPI.viewBiggestScaleValue(statFactors.GetDepositValue);

			statu_DepositInter.text = statFactors.GetDepositInter.ToString(singlePercentFormat);
			
			
			statu_LoanValue.text = " Safety First";

			statu_LoanInter.text = statFactors.GetLoanInter.ToString(singlePercentFormat);


			
			// Update View : Item Optoion
			staminaRecoverySpeed.text = statFactors.GetStaminaRecoverySpeed.ToString(singlePercentFormat);

			staminaIncrement.text = statFactors.GetStaminaIncrement.ToString(singlePercentFormat);

			staminaAutoRecoveryAmount.text = statFactors.GetAutoStaminaRecoveryAmount.ToString();

			staminaDecrement.text = statFactors.GetStaminaDecrement.ToString(singlePercentFormat);

			staminaNoConsumtionChance.text = statFactors.GetNoStaminaConsumtionChance.ToString(singlePercentFormat);

			boolStatChooser(staminaNoConsumtionWhenMoving, statFactors.GetNoStaminaConsumtionWhenMoving);			

			moveSpeed.text = statFactors.GetMoveSpeed.ToString(singlePercentFormat);

			miningSpeed.text = statFactors.GetMiningSpeed.ToString(singlePercentFormat);

			miningDoubleChance.text = statFactors.GetMiningDoubleChance.ToString(singlePercentFormat);

			miningSpecificOreDoubleChance.text = statFactors.GetSpecificOreDoubleChance.ToString("G");

			// For Test
			//print(string.Format("{0:P}", 0.125f));
		}

		private void boolStatChooser(TextMeshProUGUI tmpText, bool value)
		{			
			if(value)
			{   //Yes
				tmpText.text = trueYesString;				
			}
			else
			{
				tmpText.text = falseNoString;
			}			
		}
		
		private void expStatFillter(TextMeshProUGUI tempText, double value)
		{
			if(value == -1d)
			{
				tempText.text = expMaxString;				
			}
			else
			{
				tempText.text = value.ToString(singleOnePercentFormat);
			}
		}

		private StringBuilder curMaxPercentBuilder(double curValue, double maxValue)
		{
			StringBuilder pickedBuilder = new StringBuilder(curValue.ToString(singleOnePercentFormat), 20);
			pickedBuilder.Append(middleSlashString);
			pickedBuilder.Append(maxValue.ToString(singleOnePercentFormat));

			// Debug
			//print(string.Format("Capacity : {0} || Length : {1} || MaxCap : {2} /.", pickedBuilder.Capacity, pickedBuilder.Length, pickedBuilder.MaxCapacity));

			return pickedBuilder;
		}

		private StringBuilder curMaxBuilder(string curValue, string maxValue)
		{
			StringBuilder pickedBuilder = new StringBuilder(curValue, 8);
			pickedBuilder.Append(middleSlashString);
			pickedBuilder.Append(maxValue);

			// Debug
			//print(string.Format("Capacity : {0} || Length : {1} || MaxCap : {2} /.", pickedBuilder.Capacity, pickedBuilder.Length, pickedBuilder.MaxCapacity));

			return pickedBuilder;
		}

		#endregion


		// Called by Open Request
		public override void exeOpenPanel()
		{
			//First Time Loader			
			//Firstloader();

			//Request Inv List
			exeRefreshPanel();
		
			base.exeOpenPanel();
		}

		// Called by Custom tab Group
		public void exeTabChanged(int tabIndex)
		{			
			RectTransform previousContent = scrollRectCtrl.content;
			previousContent.gameObject.SetActive(false);
			contentList[tabIndex].gameObject.SetActive(true);
			scrollRectCtrl.content = contentList[tabIndex];
		}
	}
}