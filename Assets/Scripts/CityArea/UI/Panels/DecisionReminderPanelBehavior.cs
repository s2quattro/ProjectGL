using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using GLCore;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class DecisionReminderPanelBehavior : WindowBoxUI
	{
		//refs
		public Button btn_Confirm;
		public Button btn_Cancel;

		public TextMeshProUGUI panelExplanation;

		//Inner Field
		private UnityAction callbackMethod = null;
		private UnityAction<int> intCallbackMethod = null;

		const string discardExplanation = "해당 아이템을 버리시겠습니까?\n(신중히 결정하시길)";
		const string buyExplanation = "정말 구매 하시겠습니까?";
		const string sellExplanation = "정말 판매 하시겠습니까?";
		const string expandInvSlotExplanation = "인벤토리를 확장하시겠습니까?";
		const string useExplanation = "정말 사용 하시겠습니까?";

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		void Start()
		{
			register();
		}

		// Called by Requester [ Class ]
		public void exeReceiveRequest(decisionType type, UnityAction callback)
		{
			callbackMethod = callback;
			panelExplanation.text = selectProperDescription(type);
		}
		// Called by Requester [ Class ]
		public void exeReceiveRequest(decisionType type, UnityAction<int> callback, int count)
		{
			intCallbackMethod = callback;
			panelExplanation.text = selectProperDescription(type);
		}
		// Called by Requester (Extra script) [ Class ]
		public void exeReceiveRequest(decisionType type, UnityAction callback, string extraScript, bool isAppend = true)
		{
			callbackMethod = callback;

			if(isAppend)
			{	// Add Behind
				panelExplanation.text = selectProperDescription(type) + "\n" + extraScript;
			}
			else
			{   // Add Front
				panelExplanation.text = extraScript + "\n" + selectProperDescription(type);
			}
			
		}
		// Called by Confirm Button
		public void exeReConfirm()
		{
			if(callbackMethod != null)
			{
				callbackMethod();
				callbackMethod = null;
			}				
			if (intCallbackMethod != null)
			{
				intCallbackMethod(1);
				intCallbackMethod = null;
			}

			exeClosePanel();
		}
		// Called by Cancel Button
		public override void exeClosePanel()
		{			
			base.exeClosePanel();
		}


		private string selectProperDescription(decisionType type)
		{
			switch(type)
			{
				case decisionType.discard:
					return discardExplanation;
				case decisionType.buy:
					return buyExplanation;
				case decisionType.sell:
					return sellExplanation;
				case decisionType.expandInvSlot:
					return expandInvSlotExplanation;
				case decisionType.use:
					return useExplanation;
			}
			return null;
		}
	}
}

