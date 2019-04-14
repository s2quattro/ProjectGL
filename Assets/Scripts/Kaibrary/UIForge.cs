using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;


namespace Kaibrary.UIForge
{
	public static class UIForge
	{		
		/// <summary>
		/// (Image alpha value) fade Out Routine
		/// </summary>
		/// <param name="targetImg">Target Image refs</param>
		/// <param name="duration">Fading duration</param>
		/// <param name="Interval">Fading Interval</param>
		/// <returns>plz NEVER MIND XD</returns>
		public static IEnumerator alphaFadeOut(Image targetImg, float duration = 2f, float Interval = 0.05f)
		{			
			int fadingCount = (int)(1.0f / Interval);
			if (1.0f % Interval != 0)
				fadingCount += 1;

			WaitForSeconds delayRoutine = new WaitForSeconds(duration / fadingCount);

			for (int i = 0; i < fadingCount; i++)
			{
				Color curColor = targetImg.color;

				if (curColor.a == 0)
					break;

				curColor.a -= Interval;
				curColor.a = Mathf.Clamp(curColor.a, 0, 1f);
				targetImg.color = curColor;
				yield return delayRoutine;
			}
			yield return null;
		}

		/// <summary>
		/// (Image alpha value) fade In Routine
		/// </summary>
		/// <param name="targetImg">Target Image refs</param>
		/// <param name="duration">Fading duration</param>
		/// <param name="Interval">Fading Interval</param>
		/// <returns>plz NEVER MIND XD</returns>
		public static IEnumerator alphaFadeIn(Image targetImg, float duration = 2f, float Interval = 0.05f)
		{
			int fadingCount = (int)(1.0f / Interval);
			if (1.0f % Interval != 0)
				fadingCount += 1;

			WaitForSeconds delayRoutine = new WaitForSeconds(duration / fadingCount);

			for (int i = 0; i < fadingCount; i++)
			{
				Color curColor = targetImg.color;

				if (curColor.a == 1f)
					break;

				curColor.a += Interval;
				curColor.a = Mathf.Clamp(curColor.a, 0, 1f);
				targetImg.color = curColor;
				yield return delayRoutine;
			}
			yield return null;
		}

		/// <summary>
		/// (RAWImage alpha value) fade Out Routine
		/// </summary>
		/// <param name="targetImg">Target RAWImage refs</param>
		/// <param name="duration">Fading duration</param>
		/// <param name="Interval">Fading Interval</param>
		/// <returns>plz NEVER MIND XD</returns>
		public static IEnumerator alphaFadeOut(RawImage targetImg, float duration = 2f, float Interval = 0.05f)
		{		
			int fadingCount = (int)(1.0f / Interval);
			if (1.0f % Interval != 0)
				fadingCount += 1;

			WaitForSeconds delayRoutine = new WaitForSeconds(duration / fadingCount);

			for (int i = 0; i < fadingCount; i++)
			{
				Color curColor = targetImg.color;

				if (curColor.a == 0)
					break;

				curColor.a -= Interval;
				curColor.a = Mathf.Clamp(curColor.a, 0, 1f);
				targetImg.color = curColor;
				yield return delayRoutine;
			}
			yield return null;
		}

		/// <summary>
		/// (RAWImage alpha value) fade In Routine
		/// </summary>
		/// <param name="targetImg">Target RAWImage refs</param>
		/// <param name="duration">Fading duration</param>
		/// <param name="Interval">Fading Interval</param>
		/// <returns>plz NEVER MIND XD</returns>
		public static IEnumerator alphaFadeIn(RawImage targetImg, float duration = 2f, float Interval = 0.05f)
		{
			int fadingCount = (int)(1.0f / Interval);
			if (1.0f % Interval != 0)
				fadingCount += 1;

			WaitForSeconds delayRoutine = new WaitForSeconds(duration / fadingCount);

			for (int i = 0; i < fadingCount; i++)
			{
				Color curColor = targetImg.color;

				if (curColor.a == 1f)
					break;

				curColor.a += Interval;
				curColor.a = Mathf.Clamp(curColor.a, 0, 1f);
				targetImg.color = curColor;
				yield return delayRoutine;
			}
			yield return null;
		}


		/// <summary>
		/// (Text alpha value) fade In Routine
		/// </summary>
		/// <param name="targetImg">Target Text refs</param>
		/// <param name="duration">Fading duration</param>
		/// <param name="Interval">Fading Interval</param>
		/// <returns>plz NEVER MIND XD</returns>
		public static IEnumerator alphaFadeOut(Text targetImg, float duration = 2f, float Interval = 0.05f)
		{
			int fadingCount = (int)(1.0f / Interval);
			if (1.0f % Interval != 0)
				fadingCount += 1;

			WaitForSeconds delayRoutine = new WaitForSeconds(duration / fadingCount);

			for (int i = 0; i < fadingCount; i++)
			{
				Color curColor = targetImg.color;

				if (curColor.a == 0)
					break;

				curColor.a -= Interval;
				curColor.a = Mathf.Clamp(curColor.a, 0, 1f);
				targetImg.color = curColor;
				yield return delayRoutine;
			}
			yield return null;
		}
	}
}