using TMPro;
using GLCore;



namespace CityStage
{
	public class CashMainViewer : MainInterfaceUI
	{
		public TextMeshProUGUI viewerCtrl;

		//private const string viewTemplete = "{0}만 원";

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		void Start()
		{
			register();
		}

		public void exeRefreshCashValue(decimal newValue)
		{
            viewerCtrl.text = (GLAPI.DecimalToWon(newValue));

            //viewerCtrl.text = string.Format(viewTemplete, GLAPI.convertCashToTenThouWon(newValue, 4, null));
			
		}
		public void exeRefreshCashValue(string newValue)
		{
            viewerCtrl.text = (GLAPI.StringToWon(newValue));

            //viewerCtrl.text = string.Format(viewTemplete, GLAPI.convertCashToTenThouWon(newValue, 4, null));
		}
	}
}