using UnityEngine;
using GLCore;



namespace CityStage
{
	public class InteractablePortal : TileEntityBehavior, IInteractable
	{
		//refs
		public LocalProperties localProperties;
		public SpriteRenderer rendererCtrl;
		public Sprite2DOutlineHighLighter highLightModule;


		[Header("Portal Option")]
		public Transform destination;

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

		private InteractionType _interactType = InteractionType.teleport;
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
            //Teleportation			
            localProperties.mainCharLoc.position = destination.position;
		}
	}
}