using System;
using System.Collections.Generic;

namespace GameLibrary.Graphics;

public class Animation
{
    /// <summary>
    /// The list of textures that determines in which order the frames should be displayed in
    /// </summary>
    public List<TextureRegion> Frames { get; set; }
    
    /// <summary>
    /// The amount of time between frame change
    /// </summary>
    public TimeSpan Delay { get; set; }

    public Animation()
    {
        Frames = new List<TextureRegion>();
        Delay = TimeSpan.FromMilliseconds(100);
    }

    public Animation(List<TextureRegion> frames, TimeSpan delay)
    {
        this.Frames = frames;
        this.Delay = delay;
    }
}