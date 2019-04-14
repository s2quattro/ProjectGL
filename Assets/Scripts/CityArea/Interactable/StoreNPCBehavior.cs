using UnityEngine;
using GLCore;



namespace CityStage
{
	public class StoreNPCBehavior : EntityBehavior, IInteractable
	{
		//refs
		private LocalProperties localProperties;
		public SpriteRenderer rendererCtrl;
		public Sprite2DOutlineHighLighter highLightModule;
		public StorePanelBehavior serveThing;

		[Header("NPC Option")]
		public StoreNpcId storeNpcId;

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
            register();
			highLightModule.initShader(rendererCtrl, localProperties.hightLightColor);
		}

		public void exeInteract()
		{
			//May I help U?
			serveThing.exeOpenPanel(storeNpcId);
		}
	}
}
