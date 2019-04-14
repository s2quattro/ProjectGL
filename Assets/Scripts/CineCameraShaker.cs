using UnityEngine;
using Cinemachine;
using UnityEngine.Events;



[DisallowMultipleComponent]
public class CineCameraShaker : MonoBehaviour
{
	//refs
	public CinemachineVirtualCamera targetVirtualCamera;
	private CinemachineBasicMultiChannelPerlin noiseCtrl;

	[Header("Option")]
	[Space]
	[Range(0f, 100f)]	public float duration;
	[Range(0f, 5f)]		public float amplification = 0.1f;
	[Range(0f, 20f)]	public float frequency = 4.84f;

	private float elapsedTime = 0f;
	private bool isShaking = false;
	private UnityAction endedCallback;
	

	[Header("Events")]
	[Space]

	//Event : shaking is Ended
	public UnityEvent onShakingEnd;

	//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

	// Use this for initialization
	void Start()
	{
		if (targetVirtualCamera != null)
			noiseCtrl = targetVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

		if (targetVirtualCamera == null || noiseCtrl == null)
		{
			this.enabled = false;
			throw new UnassignedReferenceException("Cannot find target virtual camera");
		}
	}

	void Update()
	{
		/*
		// TODO: Replace with your trigger
		if (Input.GetKey(KeyCode.S))
		{
			//Start Shaking
			elapsedTime = duration;
			isShaking = true;
		}
		*/

		// If shaking flag is set, do shaking
		if (isShaking)
		{
			// If Camera Shake effect is still playing
			if (elapsedTime > 0)
			{
				// Set Cinemachine Camera Noise parameters
				noiseCtrl.m_AmplitudeGain = amplification;
				noiseCtrl.m_FrequencyGain = frequency;

				// Update Shake Timer
				elapsedTime -= Time.deltaTime;
			}
			else
			{
				// If Camera Shake effect is over, reset variables
				noiseCtrl.m_AmplitudeGain = 0f;
				elapsedTime = 0f;
				isShaking = false;
				onShakingEnd.Invoke();
				if (endedCallback != null)
				{
					endedCallback();
					endedCallback = null;
				}					
			}
		}
	}

	/// <summary>
	///		Camera Shaking based cinemachine
	/// </summary>
	/// <param name = "duration" > Duration of shaking</param>
	public void exeShaking(float duration = 0.8f)
	{
		// Shaking trigger
		if (isShaking == false && duration > 0f)
		{
			//Start Shaking
			elapsedTime = duration;
			isShaking = true;
		}
	}

	/// <summary>
	///		Camera Shaking based cinemachine
	/// </summary>
	/// <param name="callback">Called when shaking end</param>
	/// <param name="duration">Duration of shaking</param>
	public void exeShaking(UnityAction callback, float duration = 0.8f)
	{
		// Shaking trigger
		if (isShaking == false && duration > 0f)
		{
			//Start Shaking
			elapsedTime = duration;
			isShaking = true;
			endedCallback += callback;
		}
	}
}
