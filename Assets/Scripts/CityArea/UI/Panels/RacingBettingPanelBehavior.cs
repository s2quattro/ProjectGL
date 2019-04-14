using UnityEngine;
using UnityEngine.UI;
using GLCore;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class RacingBettingPanelBehavior : WindowBoxUI
	{
		//ref
		public RacingPanelBehavior parentPanel;

		public Button btn_Confirm;
		public Button btn_Close;
		public Button btn_Reset;

		public InputField inputBand;

		//Inner Field

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		void Start()
		{			
			register();

			initializeField();
		}

		/*
		//Called by NPC request
		public void exeOpenPanel()
		{
			UIManager.instance.exeOpenMessageBox(this);
		}
		//Called by Cancel Button
		public void exeClosePanel()
		{
			UIManager.instance.exeCloseMessageBox(this);
		}*/

		//Called by Confirm Button
		public void exeAcceptBetting()
		{
			//UIManager.instance.exeOpenMessageBox("sliejfsleijfse", "1. dlfihjseih\n2. alwijalwijgt");

			//Finally Set Value
			parentPanel.setBet(decimal.Parse(inputBand.text));
		}

		//Called by Reset Button
		public void exeInitializeField()
		{
			initializeField();
		}

		private void initializeField()
		{
			inputBand.text = "0";
		}
		private void checkValidInput()
		{
			//DataManager.GetInstance().localStorage.playerProperty.GetProperty()
		}
		public void clampMoney()
		{

		}
	}
}
