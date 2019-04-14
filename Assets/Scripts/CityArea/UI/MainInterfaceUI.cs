using UnityEngine;
using System.Collections;



namespace CityStage
{
	public class MainInterfaceUI : UIBase
	{

		protected override void register()
		{
			UIManager.Instance.exeRegisterMainInterface(this);
		}
		public override void exeSetActive(bool flag)
		{
			gameObject.SetActive(flag);
		}
	}
}