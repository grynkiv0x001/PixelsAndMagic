using System.Collections.Generic;
using System.Linq;

namespace GameLibrary.Utilities;
// Source - https://stackoverflow.com/a/20679895
// Posted by craftworkgames, modified by community. See post 'Timeline' for change history
// Retrieved 2026-01-19, License - CC BY-SA 4.0

public class FrameCounter
{
    public const int MaximumSamples = 100;

    private readonly Queue<float> _sampleBuffer = new();
    public long TotalFrames { get; private set; }
    public float TotalSeconds { get; private set; }
    public int AverageFramesPerSecond { get; private set; }
    public float CurrentFramesPerSecond { get; private set; }

    public void Update(float deltaTime)
    {
        CurrentFramesPerSecond = 1.0f / deltaTime;

        _sampleBuffer.Enqueue(CurrentFramesPerSecond);

        if (_sampleBuffer.Count > MaximumSamples)
        {
            _sampleBuffer.Dequeue();
            AverageFramesPerSecond = (int)_sampleBuffer.Average(i => i);
        }
        else
        {
            AverageFramesPerSecond = (int)CurrentFramesPerSecond;
        }

        TotalFrames++;
        TotalSeconds += deltaTime;
    }
}
