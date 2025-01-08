using System;
using System.Collections.Generic;

namespace GameFramework.Madhur.Analytics
{
    [Serializable]
    public class GameEventData
    {
        public string EventName;

        public Dictionary<string , string> Attributes = new Dictionary<string, string>();

        public Dictionary<string, double> Metrics = new Dictionary<string, double>();
        
        public GameEventData()
        {
            Attributes = new Dictionary<string, string>();
            Metrics = new Dictionary<string, double>();
        }
    }
}
