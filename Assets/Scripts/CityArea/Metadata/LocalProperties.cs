using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;

namespace CityStage
{
    [CreateAssetMenu(fileName = "LocalProperties(CityStage)", menuName = "Storages/LocalProperties(CityStage)")]

    public class LocalProperties : ScriptableObject
    {
        [HideInInspector]
        public Transform mainCharLoc;
        [HideInInspector]
        public Transform interactionCenterPoint;

        public Color hightLightColor;

        public Dictionary<int, List<EntityBase>> blockDic = new Dictionary<int, List<EntityBase>>();
    }
}