using UnityEngine;
using System.Collections;



namespace CityStage
{
	/// <summary>
	///		Same as WindowBoxUI But can receive One Argment
	/// </summary>
	/// <typeparam name="T">Argment Type of Open/Close method</typeparam>
	public class GenericWindowUI<T> : WindowBoxUI
	{
		//Called by Open request (One Arg)
		public virtual void exeOpenPanel(T arg)
		{
			UIManager.Instance.exeOpenMessageBox(this);
		}		
	}
}