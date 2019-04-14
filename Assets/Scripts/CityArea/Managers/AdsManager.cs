using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using GLCore;
using System;

namespace CityStage
{
    public class AdsManager : MonoSingletonBase<AdsManager>
    {
        public void Show()
        {
            Advertisement.Show();
        }

        public void ShowRewardedAd()
        {            
            if (Advertisement.IsReady("rewardedVideo"))
            {
                var options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show("rewardedVideo", options);
            }
        }

        private void HandleShowResult(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    Debug.Log("The ad was successfully shown.");
                    //
                    // YOUR CODE TO REWARD THE GAMER
                    // Give coins etc.
                    break;
                case ShowResult.Skipped:
                    Debug.Log("The ad was skipped before reaching the end.");
                    break;
                case ShowResult.Failed:
                    Debug.LogError("The ad failed to be shown.");
                    break;
            }
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}