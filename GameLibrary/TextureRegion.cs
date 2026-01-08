using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary;

/// <summary>
/// Represents a rectangular region within a texture
/// </summary>
public class TextureRegion
{
    public Texture2D Texture { get; set; }
    
    public Rectangle SourceRectangle { get; set; }
    
    /// <summary>
    /// Gets width and height of the texture in pixels
    /// </summary>
    public int Width => SourceRectangle.Width;
    public int Height => SourceRectangle.Height;

    public TextureRegion(Texture2D texture, int x, int y, int width, int height)
    {
        Texture = texture;
        SourceRectangle = new Rectangle(x, y, width, height);
    }
}