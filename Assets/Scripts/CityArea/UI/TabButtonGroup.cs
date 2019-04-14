using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;



public class TabButtonGroup : MonoBehaviour
{
	// Refs
	public List<TabButtonBehavior> groupMemberList;
	public int defaultIndex = 0;

	// Inner field
	protected TabButtonBehavior curSelectedTab;

	// Custom event
	[System.Serializable]
	public class TabReportEvent : UnityEvent<int> { }
	[Header("Events")]
	[Space]
	// Event : The tab touched
	public TabReportEvent onTouchTab;

	//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

	// Called by Operator panel
	protected virtual void Start()
	{
		registerTabBtn();
	}
	// Set member's index
	protected virtual void registerTabBtn()
	{
		// Record the frist selection
		curSelectedTab = groupMemberList[defaultIndex];

		int i = 0;
		foreach(TabButtonBehavior eachBtn in groupMemberList)
		{
			Button eachCtrl = eachBtn.GetComponent<Button>();
			
			// Set index 
			eachBtn.indexInGroup = i++;

			// Set callback
			eachCtrl.onClick.AddListener
			(
				() =>
				{
					exeReportSelectedTab(eachBtn.indexInGroup);
				}
			);
		}

		//print("Link Completed");

		// Warning : No memeber
		if (groupMemberList.Count == 0)
			Debug.LogWarning("No one Licked in Tab Group. Be not useful");
	}
	// First default select
	void reSelectFirstTab()
	{
		// Deactivate All
		foreach(TabButtonBehavior eachBtn in groupMemberList)
		{
			eachBtn.tabButtonDeactivate();
		}
	}


	// Called by certain tab
	public void exeReportSelectedTab(int index)
	{
		TabButtonBehavior touchedTab = groupMemberList[index];

		// Update view
		curSelectedTab.tabButtonActivate();
		curSelectedTab = touchedTab;
		curSelectedTab.tabButtonDeactivate();

		// And trigger event
		onTouchTab.Invoke(index);
	}

	// Just update view via script
	public void forceUpdateView(int index)
	{
		TabButtonBehavior touchedTab = groupMemberList[index];

		// Update view
		curSelectedTab.tabButtonActivate();
		curSelectedTab = touchedTab;
		curSelectedTab.tabButtonDeactivate();
	}
	
	public void registerMember(TabButtonBehavior newbie)
	{
		//if(!groupMemberList.Contains(newbie))
		groupMemberList.Add(newbie);
	}
	/*
	public bool DiscardMemeber(TabButtonBehavior newbie)
	{
		if (memberList.Contains(newbie))
		{
			memberList.Remove(newbie);
			return true;
		}
		else
			return false;		
	}
	

	// For Debug
	public void curMemberCount()
	{
		print("Member : " + groupMemberList.Count);
	}
	*/
}
