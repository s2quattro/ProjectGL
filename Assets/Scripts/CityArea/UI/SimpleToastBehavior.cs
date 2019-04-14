using UnityEngine;
using TMPro;
using uTools;



public class SimpleToastBehavior : MonoBehaviour
{
	public TextMeshProUGUI textCtrl;
	//public TweenScale scaleCtrl;
	public TweenAlpha alphaCtrl;
	//public TweenColor colorCtrl;

	private bool _isOperating = false;
	public bool isOperating
	{
		get
		{
			return _isOperating;
		}

		private set
		{
			_isOperating = value;
		}
	}

	// Inner field


	//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

	public void exeGenerateToast(string msg)
	{
		textCtrl.text = msg;

		if (isOperating)
		{   //receive message while operating

			//rewind Tweening			
			//scaleCtrl.ResetToBeginning();
			alphaCtrl.ResetToBeginning();
			//colorCtrl.ResetToBeginning();
		}
		else
		{
			isOperating = true;
			alphaCtrl.ResetToBeginning();
			//colorCtrl.ResetToBeginning();
			alphaCtrl.PlayForward();
			//colorCtrl.PlayForward();
		}
	}

	public void exeRemoveToastImmediate()
	{
		float first = alphaCtrl.duration;
		//float second = colorCtrl.duration;
		isOperating = false;
		alphaCtrl.ResetToBeginning();
		//colorCtrl.ResetToBeginning();
		alphaCtrl.SetStartToCurrentValue();
		alphaCtrl.SetEndToCurrentValue();
		//colorCtrl.SetStartToCurrentValue();
		//colorCtrl.SetEndToCurrentValue();
	}


	// Called by Tweener 's OnFinish event
	public void onTweeningEnd()
	{
		isOperating = false;
	}
}
