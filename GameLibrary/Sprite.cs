using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary;

public class Sprite
{
    public TextureRegion Region { get; set; }

    public Color Color { get; set; } = Color.White;

    public float Rotation { get; set; } = 0.0f;
    
    public Vector2 Scale { get; set; } = Vector2.One;
    
    /// <summary>
    /// Gets or Sets the xy-coordinate origin point, relative to top-left corner, of this sprite
    /// </summary>
    /// <remarks>
    /// Default value is Vector2.Zero
    /// </remarks>
    public Vector2 Origin { get; set; } = Vector2.Zero;

    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    
    /// <summary>
    /// Determines the Layer depth which is used for the layer ordering
    /// </summary>
    public float LayerDepth { get; set; } = 0.0f;
    
    /// <summary>
    /// Gets the Width (in pixels)
    /// </summary>
    /// <remarks>
    /// Width is calculated by multiplying the width of a source texture by the x-axis scale factor
    /// </remarks>
    public float Width => Region.Width * Scale.X;
    
        /// <summary>
    /// Gets the Height (in pixels)
    /// </summary>
    /// <remarks>
    /// Height is calculated by multiplying the height of a source texture by the y-axis scale factor
    /// </remarks>
    public float Height => Region.Height * Scale.Y;
        
    public Sprite() { }

    public Sprite(TextureRegion region)
    {
        this.Region = region;
    }

    public void CenterOrigin()
    {
        this.Origin = new Vector2(Region.Width, Region.Height) * 0.5f;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        this.Region.Draw(spriteBatch, position, Color, Rotation, Origin, Scale, Effects, LayerDepth);
    }
}