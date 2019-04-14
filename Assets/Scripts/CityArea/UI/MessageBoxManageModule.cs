using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GLCore;



namespace CityStage
{
	public class MessageBoxManageModule : MonoBehaviour
	{
		//refs
		public MessagePopUpPanelBehavior messagePanelCtrl;
		public GameObject backBlur;

		private bool _hasMessage = false;
		public bool hasMessage
		{
			get
			{
				return _hasMessage;
			}
			private set
			{
				if (_hasMessage != value)
				{
					//false, All Message Checked
					if (value == false)
						allMessageChecked.Invoke();

					_hasMessage = value;
				}
			}
		}

		//Event : All Message Checked
		[System.Serializable] public class onEmpty : UnityEvent { }
		public onEmpty allMessageChecked;

		//Inner Field
		private Stack<MessagePopUpInfo> messageList = new Stack<MessagePopUpInfo>();  //Message Waiting stack

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		void Awake()
		{
			allMessageChecked.RemoveAllListeners();
			allMessageChecked.AddListener(CloseSimpleMesageBox);
		}

		//Calling When UI Manager receive messaging request
		public void receiveMessage(string title, string content)
		{
			messageList.Push(new MessagePopUpInfo(title, content));
			OpenSimpleMessageBox(title, content);
			hasMessage = true;				
		}

		//Calling When Message Box Confirm
		public void checkMessage()
		{
			messageList.Pop();
			try
			{
				MessagePopUpInfo pickedMessage = messageList.Peek();
				OpenSimpleMessageBox(pickedMessage);
			}
			catch(System.InvalidOperationException)
			{	//Stack is Empty				
				hasMessage = false;
			}				
		}

		//Open Message PopUp Box like a Toast
		private void OpenSimpleMessageBox(MessagePopUpInfo message)
		{
			//Set Text
			messagePanelCtrl.setContext(message.title, message.content);

			//Enable
			backBlur.SetActive(true);
			messagePanelCtrl.gameObject.SetActive(true);
		}
		private void OpenSimpleMessageBox(string title, string contents)
		{
			//Set Text
			messagePanelCtrl.setContext(title, contents);

			//Enable
			backBlur.SetActive(true);
			messagePanelCtrl.gameObject.SetActive(true);
		}

		//Close Message Box & Disable backBlur
		public void CloseSimpleMesageBox()
		{
			//print("Close Simple PopUP");
			//▨□▨ Option
			clearText();

			//Disable
			backBlur.SetActive(false);
			messagePanelCtrl.gameObject.SetActive(false);
		}

		//Reset Text
		private void clearText()
		{
			messagePanelCtrl.setContext("Untitled", "New TxT");
		}
	}
}
