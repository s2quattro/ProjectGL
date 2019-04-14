using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLCore;

namespace CityStage
{
    public class BuninessEntity : MonoBehaviour
    {
        [SerializeField] private BusinessId businessId;

        private BusinessMetadata businessMetadata;

        // Use this for initialization
        void Start()
        {
            businessMetadata = LinkContainer.Instance.businessMetadata;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}