using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using GLCore;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class RacingPanelBehavior : WindowBoxUI
	{
		public Button btn_Select;
		public Button btn_Close;
		public Button btn_Question;

		public WindowBoxUI subPanel_Question;
		public WindowBoxUI subPanel_Betting;

		public List<RacingTicketBehavior> ticketCtrls;

		// Ticket Selection Manager
		private HorseNum _selectedHorse = HorseNum.none;
		public HorseNum selectedHorse
		{
			get
			{
				return _selectedHorse;
			}

			set
			{
				// Update view parts
				if(_selectedHorse != value && gameObject.activeSelf)  // && value != HorseNum.none
				{   // Actually changed
					// deselect previous
					if(_selectedHorse != HorseNum.none)
						ticketCtrls[(int)_selectedHorse].exePlayDeselectAnimation();
					// select current
					if(value != HorseNum.none)
						ticketCtrls[(int)value].exePlaySelectAnimation();
				}

				// Set Confirm Btn interaction
				if (value != HorseNum.none)
					btn_Select.interactable = true;
				else
					btn_Select.interactable = false;

				_selectedHorse = value;
			}
		}

		public decimal bettedMoney;

		//Inner Field

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		void Start()
		{
			register();
		}


		// Called by Question Button
		public void exeOpenQuestionPanel()
		{
			UIManager.Instance.exeOpenMessageBox(subPanel_Question);
		}

		// Called by betting Button
		public void exeOpenBettingPanel()
		{
			UIManager.Instance.exeOpenMessageBox(subPanel_Betting);
		}

		// Called by Close Request
		public override void exeClosePanel()
		{
			// Init Flags
			selectedHorse = HorseNum.none;
			bettedMoney = 0;

			// Init Interactions

			base.exeClosePanel();
		}
		private void setDeselectAllTicket()
		{
			foreach(RacingTicketBehavior eachTicket in ticketCtrls)
			{
				// eachTicket
			}
		}

		// Called by Ticket onClick Button event
		public void setRacer(HorseNum horse)
		{
			selectedHorse = horse;
		}
		// Called by Confirm of Betting Panel
		public void setBet(decimal money)
		{
			bettedMoney = money;

			//string.Format("Racer : {0} || Money : {1}", selectedHorse, bettedMoney);

			//report Parts
			ModuleTrigger.Instance.triggerRacingGame(selectedHorse, bettedMoney);			

			selectedHorse = HorseNum.none;
			bettedMoney = 0;
		}
	}
}
