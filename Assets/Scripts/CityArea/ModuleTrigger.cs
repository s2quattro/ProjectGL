using UnityEngine;
using System.Collections;
using GLCore;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class ModuleTrigger : MonoSingletonBase<ModuleTrigger>
	{
		//refs
		public Horse_Controller racingGame;
		public Transform raceTrack;

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		// New Racing Game
		public void triggerRacingGame(HorseNum racer, decimal bet)
		{
			// Restrict UI elements
			UIManager.Instance.exeCloseAllMessageBox();
			UIManager.Instance.exeSetActiveInputModules(false);

			// Mini Game Start!
			if (racingGame.StartRace(racer, bet))
			{
				// Attention : Moving the camera
				CameraControlManager.instance.exeSwapFollowTarget(raceTrack);
			}
			else
			{
				finishRacingGame();
			}
		}
		// Load Racing Game
		public bool triggerRacingGame()
		{
			// Restrict UI elements
			UIManager.Instance.exeCloseAllMessageBox();
			UIManager.Instance.exeSetActiveInputModules(false);

			// Mini Game Start!			
			if (racingGame.ContinueRace())
			{
				// Attention : Moving the camera
				CameraControlManager.instance.exeSwapFollowTarget(raceTrack);
				return true;
			}
			else
			{
				finishRacingGame();
				return false;
			}
		}
		
		public void finishRacingGame()
		{
			//Camera Back
			CameraControlManager.instance.exeFollowMainPlayer();
			UIManager.Instance.exeSetActiveInputModules(true);
		}
	}
}