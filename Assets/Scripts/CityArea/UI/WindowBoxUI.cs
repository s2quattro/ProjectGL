using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;



namespace CityStage
{	
	public class WindowBoxUI : UIBase
	{
		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		protected override void register()
		{			
			UIManager.Instance.exeRegisterObject(this);
		}
		public override void exeSetActive(bool flag)
		{
			gameObject.SetActive(flag);
		}

		public virtual void exeSetGRaycastActive(bool flag)
		{
			print("This UI has no Interaction");
		}

		//Called by Open request
		public virtual void exeOpenPanel()
		{
			UIManager.Instance.exeOpenMessageBox(this);
		}
		/*
		//Called by Open request
		public virtual void exeOpenPanel<T>(T arg)
		{
			UIManager.instance.exeOpenMessageBox(this);
		}*/
		//Called by Cancel request
		public virtual void exeClosePanel()
		{
			UIManager.Instance.exeCloseMessageBox(this);
		}
	}
}
