using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GLCore;
using TMPro;



namespace CityStage
{
	[DisallowMultipleComponent]
	public class StoreSlotBehavior : MonoBehaviour
	{
		public Image itemImgCtrl;
		public Image frameImgCtrl;
		public TextMeshProUGUI nameTextCtrl;
		public TextMeshProUGUI capacityTextCtrl;
		public TextMeshProUGUI priceTextCtrl;

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		public void exeDisplay(Sprite itemImg, string itemName, uint capacity, int price)
		{			
			itemImgCtrl.sprite = itemImg;
			nameTextCtrl.text = itemName;
			capacityTextCtrl.text = capacity.ToString();
			priceTextCtrl.text = price.ToString();
		}
		
		public void exeSetEmpty()
		{
			itemImgCtrl.sprite = null;
			nameTextCtrl.text = null;
			capacityTextCtrl.text = null;
			priceTextCtrl.text = null;
		}	
	}
}