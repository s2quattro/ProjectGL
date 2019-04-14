using UnityEngine;
using System.Collections;



namespace CityStage
{
	public abstract class UIBase : MonoBehaviour
	{
		protected abstract void register();
		public abstract void exeSetActive(bool flag);
	}
}
