using UnityEngine;
using System.Collections;



public class UIBase : MonoBehaviour
{
	public void open()
	{
		this.gameObject.SetActive(true);
	}

	public void close()
	{
		this.gameObject.SetActive(false);
	}

	public bool isOpen
	{
		get
		{
			return this.gameObject.activeSelf;
		}
	}
}
