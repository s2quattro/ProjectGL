using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class UIFadeble : UIBase
{
	public uTools.TweenAlpha alphaCtrl;
	UnityAction onFadingFinish;
	


	private void Awake()
	{
		alphaCtrl.onFinished.AddListener(onFadingFinish);
	}

	public void fadeIn(UnityAction call)
	{
		open();
		onFadingFinish = call;
		alphaCtrl.from = 0f;
		alphaCtrl.to = 1f;
		alphaCtrl.ResetToBeginning();
		alphaCtrl.PlayForward();
	}
	
	public void fadeOut(UnityAction call)
	{
		close();
		onFadingFinish = call;
		alphaCtrl.from = 1f;
		alphaCtrl.to = 0f;
		alphaCtrl.ResetToBeginning();
		alphaCtrl.PlayForward();
	}
}
