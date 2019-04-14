using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GLCore;



namespace CityStage
{
	[DisallowMultipleComponent]
	public partial class SimpleMessageManageModule : MonoBehaviour
	{

		//Prefeb
		public GameObject simplePopUIPrefeb;
		public bool hasActivePopUp
		{
			get
			{
				if (simplePopUIList.Count == curLoadedPopUpCount)
					return false;
				else
					return true;
			}
		}

		public int curLoadedPopUpCount
		{
			get
			{
				return allList.Count;
			}
		}



		//Inner Field		
		//Disabled PopUp Waiting Queue
		private Stack<MessagePopUpPanelBehavior> simplePopUIList = new Stack<MessagePopUpPanelBehavior>();
		private List<MessagePopUpPanelBehavior> allList = new List<MessagePopUpPanelBehavior>();
		


		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		// Use this for initialization
		void Start()
		{
			createOnePopUp();
		}
		private void createOnePopUp()
		{
			GameObject pickedPopUp = Instantiate(simplePopUIPrefeb, this.transform);
			pickedPopUp.SetActive(false);  //Force Disable

			//Regist
			MessagePopUpPanelBehavior anyPopUP = pickedPopUp.GetComponent<MessagePopUpPanelBehavior>();
			simplePopUIList.Push(anyPopUP);
			allList.Add(anyPopUP);			
		}

		//Called by UI Manager : give one Pop Up (Request)
		public MessagePopUpPanelBehavior exeTransferOnePopUp()
		{
			//Has no available Pop
			if(simplePopUIList.Count == 0)
			{
				//Create One
				createOnePopUp();
				//And Send it
				return simplePopUIList.Pop();
			}
			else
			{//Has a Pop & Send it
				return simplePopUIList.Pop();
			}
		}

		//Called by UI Manager : take one Pop Up (Order)
		public void exeReturnOnePopUp(MessagePopUpPanelBehavior popUp)
		{
			simplePopUIList.Push(popUp);
		}
		//Called by UI Manager : Close All UI (Order)
		public void exeReturnAllPopUp()
		{
			//Copy AllList and paste to stack
			foreach(MessagePopUpPanelBehavior eachPopUP in allList)
			{
				simplePopUIList.Push(eachPopUP);
			}
		}
	}
}