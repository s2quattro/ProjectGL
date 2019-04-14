using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public partial class ToastManageModule : MonoBehaviour
{
	//refs
	public GameObject [] toastPrefebs;

	[Header("Option")]
	[Range(0f, 10f)]
	public float toastExposureTime = 1f;
	[Range(0f, 10f)]
	public float toastSwapTime = 1f;
	public bool ignoreTimeScale = true;

	// Inner Field
	private Queue<string> messageWaitingQueue = new Queue<string>();

	private Vector3 leftCanvasBorderPos;
	private Vector3 rightCanvasBorderPos;

	// Temp
	private float swapWaitingTime = 0f;	

	private bool _isOperating = false;
	public bool isOperating
	{
		get
		{
			return _isOperating;
		}

		private set
		{
			if(_isOperating != value)
			{	// If changed
				_isOperating = value;
				enabled = value;  // control the update call
			}			
		}
	}

	private moduleState curState = moduleState.Idle;
	private enum moduleState
	{
		Idle, Operating, StandBy
	}

	//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

	#region Initializer

	void Start()
	{
		calculatePlan();
		linkTweenes();
	}	

	private void calculatePlan()
	{
		print(string.Format("Current Resolution : {0} X {1}", Screen.currentResolution.width, Screen.currentResolution.height));

		leftCanvasBorderPos = new Vector3(Screen.currentResolution.width / 2f + toastPrefebs[0].transform.position.x / 2f, 0f, 0f);
		rightCanvasBorderPos = -leftCanvasBorderPos;
	}	

	#endregion

	// Swap flow control
	void Update()
	{
		float timeDelta = ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
		swapWaitingTime -= timeDelta;

		// Do Swap
		if (swapWaitingTime <= 0f)
		{
			// disable next Update
			isOperating = false;
			// Init the timer
			swapWaitingTime = toastSwapTime;

			forceSwap();
		}
	}


	public void exeAddToast(string message)
	{
		// Accept a message
		messageWaitingQueue.Enqueue(message);
		
		if(curState != moduleState.Idle)
		{	// message received during operating
			return;	//just receive message
		}

		// First operate : First Pulse
		forceSwap();
		//initBeforeFirstSwap();
	}
	public void exeRemoveAllToast()
	{
		
	}



	void forceSwap()
	{		
		// Check and Change State
		if(curState == moduleState.Idle)
		{   // Idle
			curState = moduleState.Operating;

			//First swap
			int pickedFirst = getNewTweenerIndexAndCycling();

			//Set 
			//pickedFirst

		}
		else if(curState == moduleState.StandBy)
		{   // Stand By
			curState = moduleState.Operating;

			//Next swap
		}
	}


	void readyToNextSwap()
	{

	}

	private void initBeforeFirstSwap()
	{
		swapWaitingTime = toastSwapTime;
		isOperating = true;
	}
}


//-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-


// Tween Module Selector
public partial class ToastManageModule : MonoBehaviour
{
	//Tweener Set
	private List<uTools.TweenPosition> tweenModules;
	private List<TMPro.TextMeshProUGUI> tmpModules;
	
	//Set Indexer (Cyclic)
	private int _firstTweenerIndex = 0;
	public int firstTweenerIndex
	{
		get
		{
			return _firstTweenerIndex;
		}
		private set
		{
			if(value >= tweenerCount)
			{
				_firstTweenerIndex = 0;
			}
			else if(value < 0)
			{
				_firstTweenerIndex = tweenerCount - 1;
			}
			else
			{
				_firstTweenerIndex = value;
			}
		}
	}
	private int _middleTweenerIndex = 0;
	public int middleTweenerIndex
	{
		get
		{
			return _middleTweenerIndex;
		}
		private set
		{
			if (value >= tweenerCount)
			{
				_middleTweenerIndex = 0;
			}
			else if (value < 0)
			{
				_middleTweenerIndex = tweenerCount - 1;
			}
			else
			{
				_middleTweenerIndex = value;
			}
		}
	}
		
	private int tweenerCount;

	//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

	#region (partial) Initializer

	private void linkTweenes()
	{
		tweenerCount = toastPrefebs.Length;

		if(tweenerCount == 0)
		{
			Debug.LogError("No Tweener Module Set");
		}

		// Make set
		tweenModules = new List<uTools.TweenPosition>(tweenerCount);
		tmpModules = new List<TMPro.TextMeshProUGUI>(tweenerCount);		

		// And link
		for (int i = 0; i < tweenerCount; i++)
		{
			tweenModules.Add(toastPrefebs[i].GetComponent<uTools.TweenPosition>());
			tmpModules.Add(toastPrefebs[i].GetComponent<TMPro.TextMeshProUGUI>());
		}
	}

	#endregion

	private int getNewTweenerIndexAndCycling()
	{
		_middleTweenerIndex = firstTweenerIndex;
		firstTweenerIndex++;

		return _middleTweenerIndex;
	}

	private void setValueFirstTweener(int index)
	{
		uTools.TweenPosition targetTweener = tweenModules[index];

		targetTweener.from = leftCanvasBorderPos;
		targetTweener.to = Vector3.zero;

		//targetTweener.AddOnFinished();
	}

	private void setValueMidTweener(int index)
	{
		uTools.TweenPosition targetTweener = tweenModules[index];

		targetTweener.from = Vector3.zero;
		targetTweener.to = rightCanvasBorderPos;
	}
}
