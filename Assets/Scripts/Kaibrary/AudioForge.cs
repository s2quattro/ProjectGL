using UnityEngine;
using System.Collections;



namespace Kaibrary
{
	public static class AudioForge
	{
		/// <summary>
		/// Sound fade In Co-Routine
		/// </summary>
		/// <param name="musicPlayer">Target AudioSource ref</param>
		/// <param name="duration">Fading duration</param>
		/// <param name="Interval">Fading Interval</param>
		/// <returns>plz never mind</returns>
		public static IEnumerator volumeFadeIn(AudioSource musicPlayer, float duration = 2f, float Interval = 0.05f)
		{
			Debug.Log("Sound fade Out");

			int fadingCount = (int)(1.0f / Interval);
			if (1.0f % Interval != 0)
				fadingCount += 1;

			WaitForSeconds delayRoutine = new WaitForSeconds(duration / fadingCount);


			//Fader
			for (int i = 0; i < fadingCount; i++)
			{
				//checking already Done it
				if (musicPlayer.volume == 1f)
					break;

				musicPlayer.volume += Interval;
				musicPlayer.volume = Mathf.Clamp(musicPlayer.volume, 0, 1f);

				//Delaying
				yield return delayRoutine;
			}
			yield break;
		}

		/// <summary>
		/// Sound fade Out Co-Routine
		/// </summary>
		/// <param name="musicPlayer">Target AudioSource ref</param>
		/// <param name="duration">Fading duration</param>
		/// <param name="Interval">Fading Interval</param>
		/// <returns>plz never mind</returns>
		public static IEnumerator volumeFadeOut(AudioSource musicPlayer, float duration = 2f, float Interval = 0.05f)
		{
			Debug.Log("Sound fade Out");

			int fadingCount = (int)(1.0f / Interval);
			if (1.0f % Interval != 0)
				fadingCount += 1;

			WaitForSeconds delayRoutine = new WaitForSeconds(duration / fadingCount);


			//Fader
			for (int i = 0; i < fadingCount; i++)
			{
				//checking already Done it
				if (musicPlayer.volume == 0)
					break;

				musicPlayer.volume -= Interval;
				musicPlayer.volume = Mathf.Clamp(musicPlayer.volume, 0, 1f);

				//Delaying
				yield return delayRoutine;
			}
			yield break;
		}

		/// <summary>
		/// Sound fade Out N Out Co-Routine
		/// </summary>
		/// <param name="musicPlayer">Target AudioSource ref</param>
		/// <param name="duration">Fading duration</param>
		/// <param name="Interval">Fading Interval</param>
		/// <returns>plz never mind</returns>
		public static IEnumerator volumeFadeOutNStop(AudioSource musicPlayer, float duration = 2f, float Interval = 0.05f)
		{
			Debug.Log("Sound fade Out");

			int fadingCount = (int)(1.0f / Interval);
			if (1.0f % Interval != 0)
				fadingCount += 1;

			WaitForSeconds delayRoutine = new WaitForSeconds(duration / fadingCount);


			//Fader
			for (int i = 0; i < fadingCount; i++)
			{
				//checking already Done it
				if (musicPlayer.volume == 0)
					break;

				musicPlayer.volume -= Interval;
				musicPlayer.volume = Mathf.Clamp(musicPlayer.volume, 0, 1f);

				//Delaying
				yield return delayRoutine;
			}

			musicPlayer.Stop();

			yield break;
		}
	}
}