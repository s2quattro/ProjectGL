using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;
using System;
using UnityEngine.UI;

namespace CityStage
{
    [CreateAssetMenu(fileName = "IconImageMetadata(CityStage)", menuName = "Storages/IconImageMetadata(CityStage)")]
    public class IconImageMetadata : ScriptableObject
    {
        [SerializeField] private List<IconImageData> iconImageDatas;
        public Dictionary<IconImageId, Sprite> iconImageDic = new Dictionary<IconImageId, Sprite>();

        public void SetDicData()
        {
            foreach (IconImageData data in iconImageDatas)
            {
                iconImageDic.Add(data.GetIconImageId, data.GetIconImage);
            }
        }
    }

    [Serializable]
    public class IconImageData
    {
        [SerializeField] private IconImageId iconImageId;
        [SerializeField] private Sprite iconImage;

        public IconImageId GetIconImageId { get { return iconImageId; } }
        public Sprite GetIconImage { get { return iconImage; } }
    }

}