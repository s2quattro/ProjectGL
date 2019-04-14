using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class BackBlurController : MonoBehaviour, IPointerDownHandler
	{
		//public RawImage imgCtrl;
		
		public void exeBlurSetUp(bool flag)
		{
			if (flag)
				gameObject.SetActive(true);
			else
				gameObject.SetActive(false);
		}

		//Linked Screen is touched and then activate
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			UIManager.Instance.exeCloseUppermostMessageBox();
		}
	}
}