using UnityEngine;
using System.Collections;
using TMPro;
using uTools;



public class RenewToastTester : MonoBehaviour
{
	// Refs
	public TweenScale scaleTweener;
	public TextMeshProUGUI textViewCtrl;

	// Option
	[Header("Option")]
	[Range(0f, 10f)]
	public float duration = 3f;
	
	
	// Flag
	private bool _isOperating = false;
	public bool isOperating
	{
		set
		{
			// Changed
			if(_isOperating != value)
			{
				// True -> False
				if(value == false)
				{
					// OFF
				}
				// False -> True
				else
				{
					// ON
				}
			}

			_isOperating = value;
		}

		get
		{
			return _isOperating;
		}
	}

	//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-	

	// Called by Requester
	public void exeGenerateToast(string msg)
	{
		print("GENERATED");
		// Set the message
		textViewCtrl.text = msg;

		// Control the tween module
		if(isOperating)
		{   // Already tween is operating
			//print("REPLAY TWEEN");
			resetTweening();
			scaleTweener.PlayForward();
		}
		else
		{	// Idle
			isOperating = true;
			scaleTweener.PlayForward();
		}

		scaleTweener.RemoveAllOnFinished();
		scaleTweener.AddOnFinished(() => { StartCoroutine(viewStasisToast()); });
	}

	// Called by Requester
	public void exeGenerateToast(string msg, float duration)
	{
		//print("GENERATED");
		// Set the message
		textViewCtrl.text = msg;

		// Control the tween module
		if (isOperating)
		{   // Already tween is operating
			//print("REPLAY TWEEN");
			resetTweening();
			scaleTweener.PlayForward();
		}
		else
		{   // Idle
			isOperating = true;
			scaleTweener.PlayForward();
		}

		scaleTweener.RemoveAllOnFinished();
		scaleTweener.AddOnFinished(() => { StartCoroutine(viewStasisToast(duration)); });
	}



	// For Once Ping Pong
	public void reverseTweening()
	{
		scaleTweener.PlayReverse();
		
		// Init OnFinished callbacks & set a callback
		scaleTweener.RemoveAllOnFinished();
		scaleTweener.AddOnFinished(() => { print("Pong"); resetReversedTween(); isOperating = false; scaleTweener.RemoveAllOnFinished(); });

		//isOperating = false;
	}

	// Immediately reset the tweening
	public void resetTweening()
	{
		//enabled = false;
		//scaleTweener.ResetToBeginning();

		//scaleTweener.SetCurrentValueToStart();
		//scaleTweener.SetCurrentValueToEnd();
		if(scaleTweener.direction == Direction.Forward)
		{
			scaleTweener.ResetToBeginning();
		}			
		else if(scaleTweener.direction == Direction.Reverse)
		{
			scaleTweener.reverseDirection();
			scaleTweener.ResetToBeginning();
		}

		StopAllCoroutines();
	}

	public void resetReversedTween()
	{
		scaleTweener.reverseDirection();
	}

	// Waiting Coroutine
	private IEnumerator viewStasisToast()
	{		
		// Timer
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			elapsedTime += Time.unscaledDeltaTime;
			yield return null;
		}

		// And Do it
		reverseTweening();
	}

	// Waiting Coroutine
	private IEnumerator viewStasisToast(float _duration)
	{
		// Timer
		float elapsedTime = 0f;
		while (elapsedTime < _duration)
		{
			elapsedTime += Time.unscaledDeltaTime;
			yield return null;
		}

		// And Do it
		reverseTweening();
	}
}
