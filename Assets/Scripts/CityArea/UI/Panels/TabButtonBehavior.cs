using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



public class TabButtonBehavior : MonoBehaviour
{
	// Refs
	public Button btnCtrl;

	[HideInInspector]
	public int indexInGroup;

	//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

	public void linkMethodWithBtn(UnityAction callback)
	{
		btnCtrl.onClick.AddListener(callback);
	}

	public void tabButtonDeactivate()
	{
		btnCtrl.interactable = false;
	}
	public void tabButtonActivate()
	{
		btnCtrl.interactable = true;
	}
}
