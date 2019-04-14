using UnityEngine;
using GLCore;
using UnityEngine.UI;
using UnityEngine.Events;

namespace CityStage
{
	public class OreEntityBehavior : TileEntityBehavior, IInteractable
	{
		//refs
		public LocalProperties localProperties;
        public LocalStorage localStorage;
		public SpriteRenderer rendererCtrl;
		public Sprite2DOutlineHighLighter highLightModule;
        public UnityAction takeOreAction;

        [Header("Ore Option")]
		public int oreNum;
		public string type;

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

		private InteractionType _interactType = InteractionType.mining;
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
            localStorage = LinkContainer.Instance.localStorage;
			localProperties = LinkContainer.Instance.localProperties;
            takeOreAction = () => OreManager.Instance.TakeOre(this);

            register();
            highLightModule.initShader(rendererCtrl, localProperties.hightLightColor);
		}


        public void exeInteract()
		{
            //print("Ore! ::" + gameObject.name);
            
            if (localStorage.playerStat.GetCurStamina < StaminaManager.Instance.GetStaminaUsage(CharActionType.OreMining))
            {
                UIManager.Instance.exeRequestToasting("스태미너가 부족합니다");
                return;
            }

            float minningTime = LinkContainer.Instance.charActionMetadata.defaultMinningTime * (1f - StatFactorManager.Instance.GetMiningSpeed);

            TopDown2DSpriteChanger.Instance.StartMinningAction(minningTime, takeOreAction, this.transform.position);
        }
	}
}