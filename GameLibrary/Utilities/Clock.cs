using System.Diagnostics;

namespace GameLibrary.Utilities;

public class Clock
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    private long _lastTicks;

    public float RealDeltaTime { get; private set; }
    public float TotalRealTime { get; private set; }

    public void Update()
    {
        var ticks = _stopwatch.ElapsedTicks;
        var deltaTicks = ticks - _lastTicks;

        _lastTicks = ticks;

        RealDeltaTime = deltaTicks / (float)Stopwatch.Frequency;

        TotalRealTime += RealDeltaTime;
    }
}
