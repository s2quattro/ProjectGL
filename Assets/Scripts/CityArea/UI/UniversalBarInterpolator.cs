using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class UniversalBarInterpolator : MainInterfaceUI
	{
		// Refs
		public Image barCtrl; 

		[Header("Option")]
		public bool ignoreTimeScale = true;
		[Range(.01f, 10f)]
		public float speedFactor = 1f;

		private bool _isChanging = false;
		public bool isChanging
		{
			get
			{
				return _isChanging;
			}

			private set
			{
				if (_isChanging != value)
				{   // Changed
					_isChanging = value;
					enabled = value;  // Control Update method
				}
			}
		}

		// Inner field
		private float previousRatio;
		private float targetRatio;
		private float shellFactor = 0f;

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		void Start()
		{
			register();

			// Default : Disable update
			if (enabled)
			{
				enabled = false;
			}
		}		

		// Interpolator
		void Update()
		{			
			float timeDelta = ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
			shellFactor += timeDelta * speedFactor;

			barCtrl.fillAmount = Mathf.Lerp(previousRatio, targetRatio, shellFactor);

			if (shellFactor >= 1f)
			{
				isChanging = false;
			}
		}

		// Called by Outer Initializer
		/// <summary>
		///		Instantly Set value of Bar
		/// </summary>
		/// <param name="ratio">Current value / Max value</param>
		public void exeSetBarValue(float ratio)
		{
			barCtrl.fillAmount = ratio;
		}

		// Called by Requestor
		/// <summary>
		///		Smoothly Set value of Bar
		/// </summary>
		/// <param name="ratio">Current value / Max value</param>
		public void exeRefreshBar(float ratio)
		{
			// Stop interpolation
			isChanging = false;

			// Skip
			if (barCtrl.fillAmount == ratio)
				return;

			shellFactor = 0f;
			targetRatio = ratio;
			previousRatio = barCtrl.fillAmount;

			isChanging = true;
		}
	}
}