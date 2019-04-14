using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class MessagePopUpPanelBehavior : WindowBoxUI
	{
		//public Text _titleTxtCtrl;
		//public Text _contentTxtCtrl;

		public TextMeshProUGUI titleTextCtrl;
		public TextMeshProUGUI contentTextCtrl;
		public ScrollRect scrollRectCtrl;

		public Button btn_Confirm;  //Same As Close

		//Inner Field

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		void Start()
		{
			register();			
		}
		void OnEnable()
		{
			scrollRectCtrl.verticalNormalizedPosition = 1f;
		}

		public void setContext(string title, string contents)
		{
			//print("Contents Deleted");
			//Initialize
			//_titleTxtCtrl.text = null;
			//_contentTxtCtrl.text = null;

			//titleTextCtrl.text = null;
			//contentTextCtrl.text = null;


			//_titleTxtCtrl.text = title;
			//_contentTxtCtrl.text = contents;			

			titleTextCtrl.text = title;
			contentTextCtrl.text = contents;
		}		

		//Called by Confirm Button
		public override void exeClosePanel()
		{
			//print("Simple Message was Closed");
			//UIManager.instance.treatDisabledSimplePopUp(this);
			//UIManager.instance.exeCloseMessageBox(this);
		}

		public override void exeSetGRaycastActive(bool flag)
		{
			btn_Confirm.image.raycastTarget = flag;
		}
	}
}
