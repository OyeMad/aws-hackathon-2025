using Supyrb;

public class GameProgressSignal : Signal<string>
{
    protected override void Invoke(int index)
    {
        base.Invoke(index);
    }
}