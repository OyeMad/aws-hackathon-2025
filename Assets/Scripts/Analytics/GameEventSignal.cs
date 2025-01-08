using Amazon.MobileAnalytics.MobileAnalyticsManager;
using Supyrb;

/// <summary>
/// Game event for the game analytics data to be sent across for the game
/// </summary>
public class GameEventSignal : Signal<CustomEvent>
{
    protected override void Invoke(int index)
    {
        base.Invoke(index);
    }
}