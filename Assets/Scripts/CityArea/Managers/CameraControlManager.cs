using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.Events;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class CameraControlManager : MonoBehaviour
	{
		public static CameraControlManager instance;

		public LocalProperties localProperties;

		//refs
		public CinemachineVirtualCamera targetVirtualCamera;
		//private CinemachineBasicMultiChannelPerlin noiseCtrl;


		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		void Awake()
		{
			instance = this;
		}

		// Use this for initialization
		void Start()
		{
			if (targetVirtualCamera == null)
			{
				this.enabled = false;
				throw new UnassignedReferenceException("Cannot find target virtual camera");
			}

			localProperties = LinkContainer.Instance.localProperties;
		}

		public void exeSwapFollowTarget(Transform newTarget)
		{
			targetVirtualCamera.Follow = newTarget;
		}

		public void exeFollowMainPlayer()
		{
			targetVirtualCamera.Follow = localProperties.mainCharLoc;
		}
	}
}
