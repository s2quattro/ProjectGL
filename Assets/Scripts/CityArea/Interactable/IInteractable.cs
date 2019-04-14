using GLCore;
using UnityEngine;

public interface IInteractable
{    
	InteractionType interactType
	{		
		get;
	}

	bool isHighLight
	{
		set;
		get;
	}

	//method triggerd when interact	
	void exeInteract();
}