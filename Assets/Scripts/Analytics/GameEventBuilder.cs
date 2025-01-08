using Amazon.MobileAnalytics.MobileAnalyticsManager;

namespace GameFramework.Madhur.Analytics
{

    /// <summary>
    /// Game Event Builder
    /* 
    var customEvent = new GameEventBuilder("level_complete")
        .AddAttribute("LevelName", "Level1")
        .AddAttribute("CharacterClass", "Warrior")
        .AddAttribute("Successful", "True")
        .AddMetric("Score", 12345)
        .AddMetric("TimeInLevel", 64)
        .Build();
    */
    /// </summary>
    public class GameEventBuilder
    {
        private GameEventData _eventData;

        public GameEventBuilder(string eventName)
        {
            _eventData = new GameEventData { EventName = eventName };
        }

        public GameEventBuilder AddAttribute(string key, string value)
        {
            _eventData.Attributes[key] = value;
            return this;
        }

        public GameEventBuilder AddMetric(string key, double value)
        {
            _eventData.Metrics[key] = value;
            return this;
        }

        public CustomEvent Build()
        {
            return EventConverter.ToCustomEvent(_eventData);
        }
    }

    // Helper class to convert the event data to CustomEvent
    public class EventConverter
    {
        public static CustomEvent ToCustomEvent(GameEventData eventData)
        {
            CustomEvent customEvent = new CustomEvent(eventData.EventName);

            foreach (var attribute in eventData.Attributes)
            {
                customEvent.AddAttribute(attribute.Key, attribute.Value);
            }

            foreach (var metric in eventData.Metrics)
            {
                customEvent.AddMetric(metric.Key, metric.Value);
            }

            return customEvent;
        }
    }

}