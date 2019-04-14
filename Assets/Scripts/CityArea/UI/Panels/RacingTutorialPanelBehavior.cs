using UnityEngine;
using UnityEngine.UI;
using System.Collections;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class RacingTutorialPanelBehavior : WindowBoxUI
	{
		public Button btn_Next;
		public Button btn_Previous;
		public Button btn_Close;

		//Inner Field

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		void Start()
		{
			register();
		}

		/*
		//Called by Racing Panel Question Button
		public void exeOpenPanel()
		{
			UIManager.instance.exeOpenMessageBox(this);
		}
		//Called by Cancel Button
		public void exeClosePanel()
		{
			UIManager.instance.exeCloseMessageBox(this);
		}	*/
		
	}
}
