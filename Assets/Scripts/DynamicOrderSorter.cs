using UnityEngine;
using System.Collections.Generic;



public class DynamicOrderSorter : MonoBehaviour
{
	// Refs
	public SpriteRenderer [] renderers;

	[Header("Option")]
	public float yOffset = 0;

	// Inner field
	float layer;
	SpriteRenderer rend;
	Vector3 centerBottom;

	// Use this for initialization
	void Start()
	{
		if(renderers.Length == 0)
			renderers = GetComponentsInChildren<SpriteRenderer>();

		//rend = GetComponent<SpriteRenderer>();
	}

	void FixedUpdate()
	{
		foreach(SpriteRenderer eachRenderer in renderers)
		{
			centerBottom = transform.TransformPoint(eachRenderer.sprite.bounds.min);

			layer = centerBottom.y + yOffset;

			eachRenderer.sortingOrder -= (int)(layer * 100);
			//eachRenderer.sortingOrder = - (int)(layer * 100);
		}
	}
}
