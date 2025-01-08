using Amazon;
using Amazon.CognitoIdentity;
using Amazon.MobileAnalytics.MobileAnalyticsManager;
using Supyrb;
using UnityEngine;

namespace GameFramework.Madhur.Analytics
{


public class GameAnalytics : MonoBehaviour
{
        public string IdentityPoolId = "YourIdentityPoolId";

        public string appId = "YourAppId";

        public string CognitoIdentityRegion = RegionEndpoint.USEast1.SystemName;

        private RegionEndpoint _CognitoIdentityRegion
        {
            get { return RegionEndpoint.GetBySystemName(CognitoIdentityRegion); }
        }

        public string AnalyticsRegion = RegionEndpoint.USEast1.SystemName;

        private RegionEndpoint _AnalyticsRegion
        {
            get { return RegionEndpoint.GetBySystemName(AnalyticsRegion); }
        }

        private MobileAnalyticsManager analyticsManager;

        private CognitoAWSCredentials _credentials;


        void Start()
        {
            UnityInitializer.AttachToGameObject(this.gameObject);

            _credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoIdentityRegion);
            analyticsManager = MobileAnalyticsManager.GetOrCreateInstance(appId, _credentials,_AnalyticsRegion);

        }

        void OnApplicationFocus(bool focus)
        {
            if(focus)
            {
                analyticsManager.ResumeSession();
            }
            else
            {
                analyticsManager.PauseSession();
            }
        }

        void OnEnable()
        {
            Signals.Get<GameEventSignal>().AddListener(CustomEventHandler);
        }

        void OnDisable()
        {
            Signals.Get<GameEventSignal>().RemoveListener(CustomEventHandler);
        }


        void CustomEventHandler(CustomEvent customEvent )
        {
            analyticsManager.RecordEvent(customEvent);
        }

}

}