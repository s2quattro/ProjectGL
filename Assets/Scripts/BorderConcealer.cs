using UnityEngine.Tilemaps;
using UnityEngine;



[DisallowMultipleComponent]
public class BorderConcealer : MonoBehaviour
{
	//Target renderer
	public TilemapRenderer tileRenderer;

	// Use this for initialization
	void Start()
	{
		tileRenderer.enabled = false;
	}
}
