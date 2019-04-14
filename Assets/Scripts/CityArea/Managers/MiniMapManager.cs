using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GLCore;

namespace CityStage
{        
    public class MiniMapManager : MonoSingletonBase<MiniMapManager>
    {
        [SerializeField] private Vector2 RadiusOfIconImage;

        private IconImageMetadata iconImageMetadata;

        public RectTransform miniMapMaskTrans;
        public Transform miniMapTrans;
        public Transform playerTrans;

        [SerializeField] private Vector2 smallMaskPosition;
        [SerializeField] private Vector2 smallMaskSize;
        [SerializeField] private Vector2 largeMaskPosition;
        [SerializeField] private Vector2 largeMaskSize;

        private bool isMaskLarge = false;

        private Dictionary<EntityBase, GameObject> iconObjectDic = new Dictionary<EntityBase, GameObject>();

        [SerializeField] private float mapAccumulation;

        private Vector3 IconScaleFactor = new Vector3(1f, 1f, 1f);

        // Use this for initialization
        void Start()
        {
            iconImageMetadata = LinkContainer.Instance.iconImageMetadata;
        }

        private void FixedUpdate()
        {
            RenewMinimapTrans();
        }

        public void SetUp()
        {
            SetMinimapIcon();
        }

        public void ChnageMaskSize()
        {
            if (isMaskLarge)
            {
                UIManager.Instance.exeSetActiveInputModules(true);
                miniMapMaskTrans.localPosition = smallMaskPosition;
                miniMapMaskTrans.sizeDelta = smallMaskSize;
                isMaskLarge = false;
            }
            else
            {
                UIManager.Instance.exeSetActiveInputModules(false);
                miniMapMaskTrans.localPosition = largeMaskPosition;
                miniMapMaskTrans.sizeDelta = largeMaskSize;
                isMaskLarge = true;
            }



            RenewMinimapTrans();
        }

        public void EnableIcon(EntityBase entity)
        {
            if (iconObjectDic.ContainsKey(entity))
            {
                iconObjectDic[entity].SetActive(true);
            }
        }

        public void DisenableIcon(EntityBase entity)
        {
            if (iconObjectDic.ContainsKey(entity))
            {
                iconObjectDic[entity].SetActive(false);
            }
        }

        public void RenewMinimapTrans()
        {
            Vector2 v = playerTrans.position;

            miniMapTrans.transform.position = (Vector2)miniMapMaskTrans.position;
            miniMapTrans.transform.localPosition = -v * mapAccumulation;
        }

        public void SetMinimapIcon()
        {
            List<EntityBase> entityList = EntityManager.Instance.GetEntityList();

            iconObjectDic.Clear();

            foreach (EntityBase entity in entityList)
            {
                if (entity.iconImageId != IconImageId.none)
                {
                    GameObject iconImage = Instantiate(Resources.Load("Prefabs/IconImage") as GameObject);
                    iconImage.GetComponent<Image>().sprite = iconImageMetadata.iconImageDic[entity.iconImageId];
                    iconImage.transform.SetParent(miniMapTrans);
                    iconImage.transform.localPosition = -RadiusOfIconImage + (Vector2)entity.transform.position * mapAccumulation;
                    iconImage.transform.localScale = IconScaleFactor;
                    iconObjectDic.Add(entity, iconImage);
                    iconImage.SetActive(false);
                }
            }
        }
    }
}