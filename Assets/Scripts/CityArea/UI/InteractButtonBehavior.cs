using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using GLCore;



namespace CityStage
{
	public class InteractButtonBehavior : MainInterfaceUI
	{
		public Image imgCtrl;
		public Button btnCtrl;

		//Interaction img List
		public List<Sprite> imgList;

		//Inner Field

		private Dictionary<InteractionType, Sprite> interactionTypeIcon = new Dictionary<InteractionType, Sprite>();
		private Sprite originBtnImg;

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		// Use this for initialization
		void Start()
		{
			register();
			iconLinker();
		}
		//Interaction icon linker
		private void iconLinker()
		{
			originBtnImg = imgCtrl.sprite;

			int interactionTypeEnumMemberCount = System.Enum.GetNames(typeof(InteractionType)).Length;

			//print(string.Format("{0} : {1}", interactionTypeEnumMemberCount, imgList.Count));
			if(interactionTypeEnumMemberCount != imgList.Count)
			{
				throw new System.InvalidOperationException("Unbalance interaction type and icon img set");
			}

			for(int i = 0; i < imgList.Count; i++)
			{
				interactionTypeIcon.Add((InteractionType)i, imgList[i]);
			}
		}


		/*
		public void exeSwapBtnImage()
		{
			//imgCtrl.texture = ;
		}
		*/

		public void exeSetInteraction(bool flag)
		{
			btnCtrl.interactable = flag;

			imgCtrl.sprite = originBtnImg;
		}
		public void exeSetInteraction(bool flag, IInteractable interactable = null)
		{
			btnCtrl.interactable = flag;

			imgCtrl.sprite = interactionTypeIcon[interactable.interactType];
		}
	}
}
