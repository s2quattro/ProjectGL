using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;



public class AdditiveTabButtonGroup : TabButtonGroup
{
	// Refs
	public Sprite normalTabSprite;
	public Sprite selectedTabSprite;
	public Sprite lockedTabSprite;

	// Unlock Counter
	public int unlockCount = 1;

	//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

	protected override void Start()
	{
		registerTabBtn();
	}
	// Set member's index
	protected override void registerTabBtn()
	{
		// Record the frist selection
		curSelectedTab = groupMemberList[defaultIndex];

		// Warning : No memeber
		if (groupMemberList.Count < unlockCount)
			Debug.LogWarning("Warning : Out of Index");

		for (int i = 0; i < unlockCount; i++)
		{
			TabButtonBehavior eachBtn = groupMemberList[i];

			Button eachCtrl = eachBtn.GetComponent<Button>();

			// Set index 
			eachBtn.indexInGroup = i;

			// Set callback
			eachCtrl.onClick.AddListener
			(
				() =>
				{
					exeReportSelectedTab(eachBtn.indexInGroup);
				}
			);
		}

		//print("Alright Linking Completed");

		// Warning : No memeber
		if (groupMemberList.Count == 0)
			Debug.LogWarning("No one Licked in Tab Group. Be not useful");
	}

	/*
	public void unlockTabContinual()
	{
		if (groupMemberList.Count == unlockCount)
		{			
			print("Not enough space");
			return;
		}

		TabButtonBehavior eachBtn = groupMemberList[unlockCount];

		Button eachCtrl = eachBtn.GetComponent<Button>();

		// Set index 
		eachBtn.indexInGroup = unlockCount++;

		// Set callback
		eachCtrl.onClick.AddListener
		(
			() =>
			{
				exeReportSelectedTab(eachBtn.indexInGroup);
			}
		);

		// Set new sprite
		swapSprite(eachCtrl);
	}*/

	public void syncRealUnlock(int packCount)
	{
		if (packCount == unlockCount)
		{
			//print("Alreay done");
			return;
		}

		int unlocking = packCount - unlockCount;

		if(unlocking < 0)
		{
			decreaser(Mathf.Abs(unlocking));
			Debug.LogWarning("Decreasing Inventory is not possible!!");
			return;
		}

		for (int i = 0; i < unlocking; i++)
		{
			TabButtonBehavior eachBtn = groupMemberList[unlockCount];

			Button eachCtrl = eachBtn.GetComponent<Button>();

			// Set index 
			eachBtn.indexInGroup = unlockCount++;

			// Set callback
			eachCtrl.onClick.AddListener
			(
				() =>
				{
					exeReportSelectedTab(eachBtn.indexInGroup);
				}
			);

			// Set new sprite
			swapSprite(eachCtrl);

			// And Activated
			eachCtrl.interactable = true;
		}
		//print("Good Expanding Completed");
	}

	private void swapSprite(Button btnCtrl)
	{
		SpriteState spriteState = new SpriteState
		{
			disabledSprite = selectedTabSprite
		};
		//spriteState = btnCtrl.spriteState;

		btnCtrl.spriteState = spriteState;
	}

	// For Test
	private void decreaser(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			TabButtonBehavior eachBtn = groupMemberList[--unlockCount];			

			Button eachCtrl = eachBtn.GetComponent<Button>();

			// Set index 
			//eachBtn.indexInGroup = unlockCount;

			// Reset callback
			eachCtrl.onClick.RemoveAllListeners();

			// Reset new sprite
			SpriteState spriteState = new SpriteState
			{
				disabledSprite = lockedTabSprite
			};
			//spriteState = btnCtrl.spriteState;
			eachCtrl.spriteState = spriteState;

			// And Dectivated
			eachCtrl.interactable = false;
		}
	}
}


/*
SpriteState spriteState = new SpriteState();
spriteState = button.spriteState;
if (enabled)
{
    spriteState.pressedSprite = redSprite;
}
else
{
    spriteState.pressedSprite = greenSprite;
}
button.spriteState = spriteState;
*/
