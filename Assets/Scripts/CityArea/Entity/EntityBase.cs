using UnityEngine;
using System.Collections;
using GLCore;


public abstract class EntityBase : MonoBehaviour
{
	[Tooltip("If set true, Block Manager will manage this Entity")]
	public bool isAffectedBlockManager = true;
    public IconImageId iconImageId = IconImageId.none;

	protected abstract void register();

	public abstract void exeSpeakName();
}
