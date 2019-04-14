using UnityEngine;
using GLCore;



namespace CityStage
{
	[RequireComponent(typeof(Sprite2DOutlineHighLighter))]
	public class SimpleInteractableBehavior : EntityBehavior, IInteractable
	{
		//refs
		public LocalProperties localProperties;
		public SpriteRenderer rendererCtrl;
		public Sprite2DOutlineHighLighter highLightModule;

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
			UIManager.Instance.exeOpenSimpleMessageBox("Att", "coasfawsf\nselijgszelij\nsleijgsleijg\nlisejg\nlisweajg\nLSEIjg\nlsiejg");
		}
	}
}
