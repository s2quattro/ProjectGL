using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace TestZone
{
	public class Changer2DTester : MonoBehaviour
	{
		// Renderer Refs
		public SpriteRenderer bodyParts;

		// Inner
		private int index;

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		public void exeBodyChange()
		{
			Sprite pickedImg = SpriteSetManager.Instance.getBodySprite(index);
			bodyParts.sprite = pickedImg;
		}

		public void exeCyclingIndex()
		{
			index++;

			if (index >= SpriteSetManager.Instance.bodyList.Count)
				index = 0;
		}
	}
}