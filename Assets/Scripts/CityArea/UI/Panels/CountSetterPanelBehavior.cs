using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class CountSetterPanelBehavior : WindowBoxUI
	{
		// Refs
		public Button btn_Confirm;
		public Button btn_Cancel;
		public Button btn_Max;

		public Button btn_Increaser;
		public Button btn_Decreaser;

		public TextMeshProUGUI countViewer;
		public TextMeshProUGUI panelExplanation;

		// Inner Field
		private int maxClampCount = 1;

		private int _countValue = 1;
		public int countValue  // Include a Clamper
		{
			get
			{
				return _countValue;
			}
			private set
			{
				
				// Has not changed
				if (_countValue == value)
					return;
				
				// Changed
				
				// Clamping part
				if (value < 1)
					_countValue = 1;
				else if (value > maxClampCount)
					_countValue = maxClampCount;
				else
					_countValue = value;

				// Update view
                //print(_countValue);
				countViewer.text = _countValue.ToString();
			}
		}

		
		// Callback
		private UnityAction<int> valueSetCallback = null;

		const string defaultExplanation = "개수 입력\n( 최대 : {0} )";
		//const string discardExplanation = "버릴 개수 입력\n(최대 : {0} )";
		//const string useExplanation = "사용 개수 입력\n(최대 : {0} )";
		//const string buyExplanation = "구입 개수 입력\n(최대 : {0} )";
		//const string sellExplanation = "판매 개수 입력\n(최대 : {0} )";

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		void Start()
		{
			register();

			// Init only view value, not all
			initializeViewValue();
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

		// Called by Requester
		public void exeSetRequestLink(int max, UnityAction<int> callback)
		{
			maxClampCount = max;
			valueSetCallback = callback;
			panelExplanation.SetText(defaultExplanation, max);
		}

		// Called by Confirm Button
		public void exeTransferCountValue()
		{
			//print("Transfer Value : " + countValue);
			valueSetCallback(countValue);
			exeClosePanel();
		}
		// Called by Cancel Button
		public override void exeClosePanel()
		{   //Init all value
			initializeViewer();
			base.exeClosePanel();
		}

		// Called by Max Button
		public void exeSetMaxValue()
		{
			countValue = maxClampCount;
		}
		// Called by Increaser
		public void exeIncreaseValue()
		{
			countValue++;
		}
		// Called by Decreaser
		public void exeDecreaseValue()
		{
			countValue--;
		}



		private void initializeViewValue()
		{
			countValue = 1;
		}
		private void initializeViewer()
		{
			countValue = 1;
			maxClampCount = 1;
			valueSetCallback = null;
		}
	}
}

