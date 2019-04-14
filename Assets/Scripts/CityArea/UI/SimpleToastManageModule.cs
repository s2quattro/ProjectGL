using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public partial class SimpleToastManageModule : MonoBehaviour
{
	//refs
	public GameObject targetTweener;
	private SimpleToastBehavior tweenerCtrl;
	
	/*
	[Header("Option")]
	[Range(0f, 10f)]
	public float toastExposureTime = 1.5f;
	[Range(0f, 10f)]
	public float toastScalingDuration = 0.5f;
	public float toastFadeDuration = 0.5f;
	public bool ignoreTimeScale = true;
	*/

	// Inner field

	//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

	void Start()
	{
		// Link tweener
		tweenerCtrl = targetTweener.GetComponent<SimpleToastBehavior>();
	}


	public void exeRequestToast(string message)
	{
		tweenerCtrl.exeGenerateToast(message);
	}
}