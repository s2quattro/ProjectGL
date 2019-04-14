using UnityEngine;
using GLCore;



namespace CityStage
{
	public class ServiceNPCBehavior : EntityBehavior, IInteractable
	{
		//refs		
		private LocalProperties localProperties;
		private LocalStorage storageCtrl;
		public SpriteRenderer rendererCtrl;
		public Sprite2DOutlineHighLighter highLightModule;


		[Header("NPC Option")]
		public WindowBoxUI serveThing;

		private bool _isHighLight;
		public bool isHighLight
		{
			get
			{
				return _isHighLight;
			}
			set
			{
				_isHighLight = value;
				highLightModule.exeToggleOutline(_isHighLight);

			}
		}

		private InteractionType _interactType = InteractionType.interactNPC;
		public InteractionType interactType
		{
			get
			{
				return _interactType;
			}
		}

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		// Use this for initialization
		void Start()
		{
            localProperties = LinkContainer.Instance.localProperties;
			storageCtrl = LinkContainer.Instance.localStorage;

			register();
			highLightModule.initShader(rendererCtrl, localProperties.hightLightColor);
		}

		public void exeInteract()
		{   // May I help U?

			// Save data is exist
			if (storageCtrl.horseRaceSaveDatas.Count > 0)
			{
				// Load previous racing game
				if (ModuleTrigger.Instance.triggerRacingGame())
					// True : Load Success
					return;
			}				
			serveThing.exeOpenPanel();
		}
	}
}

