using UnityEngine;
using System.Collections;



namespace TestZone
{
	public class IMGUISimpleTrigger : MonoBehaviour
	{
		// Refs
		public RenewToastTester control;

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		private void OnGUI()
		{

			if (GUI.Button(new Rect(10, 110, 350, 200), "Generate"))
			{   //Exe OnPressed
				control.exeGenerateToast("WOW AWESOME!!");
			}

			/*
			if (GUI.Button(new Rect(10, 360, 350, 200), "Reverse"))
			{   //Exe OnPressed
				control.reverseTweening();
			}

			if (GUI.Button(new Rect(10, 610, 350, 200), "Replay"))
			{   //Exe OnPressed
				control.rePlayTween();
			}

			if (GUI.Button(new Rect(10, 860, 350, 200), "Stop"))
			{   //Exe OnPressed
				control.resetTweening();
			}
			*/

			/*
			if (GUI.Button(new Rect(10, 110, 70, 30), "ST. up"))
			{//Exe OnPressed
				staminaBarCtrl.exeRefreshBar(0.8f);
			}

			if (GUI.Button(new Rect(10, 150, 70, 30), "ST. down"))
			{//Exe OnPressed
				staminaBarCtrl.exeRefreshBar(0.4f);
			}
			*/

			/*
			if (GUI.Button(new Rect(10, 110, 70, 30), "Buy"))
			{//Exe OnPressed
				toast.exeGenerateToast("구매에 성공하였습니다");
			}

			if (GUI.Button(new Rect(10, 150, 70, 30), "Sell"))
			{//Exe OnPressed
				toast.exeGenerateToast("판매에 성공하였습니다");
			}

			if (GUI.Button(new Rect(10, 190, 70, 30), ""))
			{//Exe OnPressed
				print(Mathf.Clamp01(Time.time));
			}
			*/
		}
	}
}