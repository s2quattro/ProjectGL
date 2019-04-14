using UnityEngine;
using GLCore;
using UnityEditor;

namespace CityStage
{
    public class SceneTrigger : MonoBehaviour
    {
        //refs
        public Transform mainCharTransform;
        public Transform interactionCenterTransform;

		private void Awake()
		{
			LinkContainer.Instance.localProperties.mainCharLoc = mainCharTransform;
			LinkContainer.Instance.localProperties.interactionCenterPoint = interactionCenterTransform;
		}

		//Primal Loading
		private void Start()
        {
            LinkContainer.Instance.stockMetadata.SetDicData();
            LinkContainer.Instance.itemMetadata.SetDicData();
            LinkContainer.Instance.storeMetadata.SetDicData();
            LinkContainer.Instance.houseMetadata.SetDicData();
            LinkContainer.Instance.iconImageMetadata.SetDicData();
            LinkContainer.Instance.charActionMetadata.SetDicData();
            LinkContainer.Instance.cardGameMetadata.SetDicData();

            LinkContainer.Instance.localStorage.InitData();
            LinkContainer.Instance.localStorage.LoadFile();

            UIManager.Instance.SetUp();
            MiniMapManager.Instance.SetUp();
            OreManager.Instance.SetUp();
            BlockManager.Instance.SetUp();
            EntityManager.Instance.SetUp();
            StatFactorManager.Instance.SetUp();
			SpritePackageManager.Instance.SetUp();
		}
    }
}