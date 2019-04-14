using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using GLCore;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class RacingTicketBehavior : MonoBehaviour
	{
		//ref
		public RacingPanelBehavior parentPanel;

		public Button btnCtrl;
		public HorseNum myHorseNum;

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		void Start()
		{
			// Reset Callback
			btnCtrl.onClick.RemoveAllListeners();

			btnCtrl.onClick.AddListener(
				
				() => { parentPanel.setRacer(myHorseNum); }

			);
		}		

		public void exePlaySelectAnimation()
		{   // play Tween
			//print(name + " Me. me");
		}

		public void exePlayDeselectAnimation()
		{   // play Tween
			//print(name + "Okay bye");
		}

		public void exeDeselectWithoutAni()
		{	// Set deselect flag For Init state

		}
	}
}
