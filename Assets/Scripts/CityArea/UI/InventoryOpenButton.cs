using UnityEngine.UI;



namespace CityStage
{
	public class InventoryOpenButton : MainInterfaceUI
	{
		public RawImage imgCtrl;
		public Button btnCtrl;

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		// Use this for initialization
		void Start()
		{
			register();
		}

		public void exeSetInteraction(bool flag)
		{
			btnCtrl.interactable = flag;
		}
	}
}