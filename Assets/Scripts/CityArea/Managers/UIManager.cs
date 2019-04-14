using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Kaibrary;
using GLCore;



namespace CityStage
{
	[DisallowMultipleComponent]
	public partial class UIManager : MonoSingletonBase<UIManager>
	{
		// Refs
		private LocalStorage storageCtrl;

		public InteractButtonBehavior interactBtnCtrl;
		public BackBlurController blurCtrl;
		//public MessagePopUpPanelBehavior simplePopUpCtrl;
		//public SimpleMessageManageModule simplePopUpManageModule;
		public MessageBoxManageModule messageBoxPopUpModule;
		public CountSetterPanelBehavior countSetterCtrl;
		public DecisionReminderPanelBehavior decisionReminderCtrl;
		


		//Inner member
		private bool _hasPopUpBox;
		public bool hasPopUpBox
		{
			get
			{
				return _hasPopUpBox;
			}
			set
			{
				if (_hasPopUpBox != value)
				{
					blurCtrl.exeBlurSetUp(value);
					exeSetActiveInputModules(!value);
				}
				_hasPopUpBox = value;
			}
		}



		//Dynamic Ui Object List
		private List<WindowBoxUI> popUpList = new List<WindowBoxUI>();  //Message PopUp List			
		private List<UIBase> uiObjectList = new List<UIBase>();  //General UI List
		private List<MainInterfaceUI> mainInterfaceUIList = new List<MainInterfaceUI>();  //Main Interface UI List (joystick, interact Btn...		

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		// Init All UI element or Manager Init
		void Start()
        {
            // Manager
            storageCtrl = LinkContainer.Instance.localStorage;
        }

        public void SetUp()
        {
            // UIs
            initCashViewer();
        }

        //Register and Discard
        public void exeRegisterObject(UIBase uiObject)
		{
			uiObjectList.Add(uiObject);
			//print("Current Object count : " + uiObjectList.Count);
		}
		public bool exeDiscardObject(UIBase uiObject)
		{
			if (uiObjectList.Contains(uiObject))
			{
				uiObjectList.Remove(uiObject);
				return true;
			}
			else
				return false;
			//print("interaction of Object has been removed");
		}
		public void exeDiscardObjectAll()
		{
			uiObjectList.Clear();
		}
		//For MainInterface
		public void exeRegisterMainInterface(MainInterfaceUI uiObject)
		{
			mainInterfaceUIList.Add(uiObject);
			//print("Current Object count : " + uiObjectList.Count);
		}
		public bool exeDiscardMainInterface(MainInterfaceUI uiObject)
		{
			if (mainInterfaceUIList.Contains(uiObject))
			{
				mainInterfaceUIList.Remove(uiObject);
				return true;
			}
			else
				return false;
			//print("interaction of Object has been removed");
		}
		public void exeDiscardMainInterfaceAll()
		{
			mainInterfaceUIList.Clear();
		}
		//UI activator
		public void exeSetActiveAllUI(bool flag)
		{
			foreach (UIBase eachUiCtrl in uiObjectList)
			{
				eachUiCtrl.exeSetActive(flag);
			}

			exeSetActiveInputModules(flag);
		}
		public void exeSetActiveInputModules(bool flag)
		{
			foreach (MainInterfaceUI eachUiCtrl in mainInterfaceUIList)
			{
				eachUiCtrl.exeSetActive(flag);
			}
		}




		//For Interaction Button
		public void exeChangeInteractBtnState(IInteractable interactTarget)
		{
			if (interactTarget == null)
			{
				interactBtnCtrl.exeSetInteraction(false);
			}
			else
			{
				interactBtnCtrl.exeSetInteraction(true, interactTarget);
			}
		}		
	}

	//-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-	



	//Window UI managing method parts
	public partial class UIManager : MonoSingletonBase<UIManager>
    {
		#region PopUp Box Opener

		// Open Message Box or Window UI
		public void exeOpenMessageBox(WindowBoxUI uiObject)
		{
			popUpListAdd(uiObject);

			// Rearrange
			blurCtrl.transform.SetAsLastSibling();
			uiObject.transform.SetAsLastSibling();
		}
		// Open Message PopUp Box like a Toast
		public void exeOpenSimpleMessageBox(string title, string contents)
		{
			messageBoxPopUpModule.receiveMessage(title, contents);
		}


		// Open Count Setter PopUp Box
		public void exeOpenCountSetter(int max, UnityAction<int> callback)
		{
			popUpListAdd(countSetterCtrl);
			countSetterCtrl.exeSetRequestLink(max, callback);

			// Rearrange
			blurCtrl.transform.SetAsLastSibling();
			countSetterCtrl.transform.SetAsLastSibling();
		}
		// Open Count Setter PopUp Box (Overload : for uint)
		public void exeOpenCountSetter(uint max, UnityAction<int> callback)
		{			
			popUpListAdd(countSetterCtrl);
			countSetterCtrl.exeSetRequestLink((int)max, callback);

			// Rearrange
			blurCtrl.transform.SetAsLastSibling();
			countSetterCtrl.transform.SetAsLastSibling();
		}


		// Open Decision Reminder PopUp Box
		public void exeOpenDecisionReminder(decisionType type, UnityAction callback)
		{
			popUpListAdd(decisionReminderCtrl);
			decisionReminderCtrl.exeReceiveRequest(type, callback);

			// Rearrange
			blurCtrl.transform.SetAsLastSibling();
			decisionReminderCtrl.transform.SetAsLastSibling();
		}
		// Open Decision Reminder PopUp Box
		public void exeOpenDecisionReminder(decisionType type, UnityAction<int> callback, int count)
		{
			popUpListAdd(decisionReminderCtrl);
			decisionReminderCtrl.exeReceiveRequest(type, callback, count);

			// Rearrange
			blurCtrl.transform.SetAsLastSibling();
			decisionReminderCtrl.transform.SetAsLastSibling();
		}
		// Open Decision Reminder PopUp Box (Extra script as param)
		public void exeOpenDecisionReminder(decisionType type, UnityAction callback, string extraScript, bool isAppend = true)
		{
			popUpListAdd(decisionReminderCtrl);
			decisionReminderCtrl.exeReceiveRequest(type, callback, extraScript, isAppend);

			// Rearrange
			blurCtrl.transform.SetAsLastSibling();
			decisionReminderCtrl.transform.SetAsLastSibling();
		}


		#endregion

		#region PopUp Box Closer

		//Close Uppermost Message Box or Window UI
		public void exeCloseUppermostMessageBox()
		{
			if (hasPopUpBox)
			{
				//print("Force Closee Uppermost Box");
				//Last Pop
				popUpList[popUpList.Count - 1].exeClosePanel();
			}
			else
				print("No Message Box Here");
		}
		//Close Specific Message Box or Window UI
		public void exeCloseMessageBox(WindowBoxUI uiObject)
		{
			if (hasPopUpBox)
			{
				//print("Force Close Message Box");
				popUpListPop(uiObject);

				int lastIndex = popUpList.Count - 1;
				//Rearrange
				if (lastIndex >= 0)
					popUpList[lastIndex].transform.SetAsLastSibling();

				//print(popUpList[lastIndex].gameObject.name);
			}
			else
				print("No Message Box Here");
		}
		//Close All Active Message Box or Window UI
		public void exeCloseAllMessageBox()
		{
			if (hasPopUpBox)
			{
				print("Close ALL Message Box!! DO IT!!");

				//#1 deactivate
				foreach (WindowBoxUI eachPopUp in popUpList)
				{
					eachPopUp.exeSetActive(false);
				}
				//Clear List
				popUpList.Clear();

				//Set Flag
				hasPopUpBox = false;
			}
			else
				print("No Message Box Here");
		}

		#endregion

		//PopUp Stack Setter : Push
		private void popUpListAdd(WindowBoxUI uiObject)
		{
			//Add
			popUpList.Add(uiObject);
			//activate
			uiObject.exeSetActive(true);

			if (popUpList.Count > 0)
			{
				hasPopUpBox = true;
			}
		}
		//PopUp Stack Setter : Pop
		private WindowBoxUI popUpListPop()
		{
			//Last Pop
			WindowBoxUI uiObject = popUpList[popUpList.Count - 1];
			popUpList.RemoveAt(popUpList.Count - 1);
			//deactivate
			uiObject.exeSetActive(false);

			if (popUpList.Count == 0)
			{
				hasPopUpBox = false;
			}

			return uiObject;
		}
		//PopUp Stack Setter : Specific Pop
		private void popUpListPop(WindowBoxUI uiObject)
		{
			//Pop
			popUpList.Remove(uiObject);

			//deactivate
			uiObject.exeSetActive(false);

			if (popUpList.Count == 0)
			{
				hasPopUpBox = false;				
			}
		}
	}

	//-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-



	// Toast Module
	public partial class UIManager : MonoSingletonBase<UIManager>
    {
		// Refs
		public RenewToastTester toastTweenerCtrl;

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		public void exeRequestToasting(string message)
		{
			toastTweenerCtrl.exeGenerateToast(message);
		}		
	}



	//-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-



	// Main UI Manage Module
	public partial class UIManager : MonoSingletonBase<UIManager>
	{
		// Refs
		public CashMainViewer cashViewerCtrl;

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
		
		private void initCashViewer()
		{
			cashViewerCtrl.exeRefreshCashValue(storageCtrl.playerProperty.GetCash);
		}

		public void updateCashValue(string cashString)
		{
			cashViewerCtrl.exeRefreshCashValue(cashString);
		}
	}
}
