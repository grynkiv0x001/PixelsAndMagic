using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Graphics;

public class SpriteSheet
{
    private Dictionary<string, TextureRegion> _regions;
    private Dictionary<string, Animation> _animations;

    /// <summary>
    /// Gets or Sets the source texture represented by this texture atlas.
    /// </summary>
    public Texture2D Texture { get; set; }

    public SpriteSheet()
    {
        _regions = new Dictionary<string, TextureRegion>();
        _animations = new Dictionary<string, Animation>();
    }

    public SpriteSheet(Texture2D texture)
    {
        this.Texture = texture;
        
        _regions = new Dictionary<string, TextureRegion>();
        _animations = new Dictionary<string, Animation>();
    }

    public void AddRegion(string name, int x, int y, int width, int height)
    {
        TextureRegion region = new TextureRegion(Texture, x, y, width, height);
        
        _regions.Add(name, region);
    }

    public TextureRegion GetRegion(string name)
    {
        return _regions[name];
    }

    public bool RemoveRegion(string name)
    {
        return _regions.Remove(name);
    }

    public void AddAnimation(string name, Animation animation)
    {
        _animations.Add(name, animation);
    }

    public Animation GetAnimation(string name)
    {
        return _animations[name];
    }

    public bool RemoveAnimation(string name)
    {
        return _animations.Remove(name);
    }

    public void Clear()
    {
        _regions.Clear();
        _animations.Clear();
    }
    
    public Sprite CreateSprite(string regionName)
    {
        TextureRegion region = GetRegion(regionName);
        
        return new Sprite(region);
    }

    public AnimatedSprite CreateAnimatedSprite(string animationName)
    {
        Animation animation = GetAnimation(animationName);
        
        return new AnimatedSprite(animation);
    }

    /// <summary>
    /// Creates a new SpriteSheet (TextureAtlas) based on an XML configuration file.
    /// </summary>
    /// <param name="content">The ContentManager.</param>
    /// <param name="filename">The path to the XML file, relative to the content root dir.</param>
    /// <returns>The SpriteSheet created by this method.</returns>
    public static SpriteSheet FromFile(ContentManager content, string filename)
    {
        SpriteSheet spriteSheet = new SpriteSheet();
        
        string path = Path.Combine(content.RootDirectory, filename);

        using (Stream stream = TitleContainer.OpenStream(path))
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                var document = XDocument.Load(reader);
                var root = document.Root;
                
                // The <Texture> element contains the content path for the Texture2D to load.
                // We retrieve that value then use the content manager to load the texture.
                if (root != null)
                {
                    string texturePath = root.Element("Texture")?.Value;
                    spriteSheet.Texture = content.Load<Texture2D>(texturePath);
                }
                
                // The <Regions> element contains individual <Region> elements, each one describing
                // a different texture region within the sprite sheet.  
                //
                // Example:
                // <Regions>
                //      <Region name="spriteOne" x="0" y="0" width="32" height="32" />
                //      <Region name="spriteTwo" x="32" y="0" width="32" height="32" />
                // </Regions>
                //
                // So we retrieve all the <Region> elements then loop through each one
                // and generate a new TextureRegion instance from it and add it to this sprite sheet.
                var regions = root.Element("Regions")?.Elements("Region");

                if (regions != null)
                {
                    foreach (var region in regions)
                    {
                        string name = region.Attribute("name")?.Value;
                        
                        int x = int.Parse(region.Attribute("x")?.Value ?? "0");
                        int y = int.Parse(region.Attribute("y")?.Value ?? "0");
                        int width = int.Parse(region.Attribute("width")?.Value ?? "0");
                        int height = int.Parse(region.Attribute("height")?.Value ?? "0");

                        if (!string.IsNullOrEmpty(name))
                        {
                            spriteSheet.AddRegion(name, x, y, width, height);
                        }
                    }
                }
                
                // The <Animations> element contains individual <Animation> elements, each one describing
                // a different animation within the atlas.
                //
                // Example:
                // <Animations>
                //      <Animation name="animation" delay="100">
                //          <Frame region="spriteOne" />
                //          <Frame region="spriteTwo" />
                //      </Animation>
                // </Animations>
                //
                // So we retrieve all the <Animation> elements then loop through each one
                // and generate a new Animation instance from it and add it to this atlas.
                var animationElements = root.Element("Animations")?.Elements("Animation");

                if (animationElements != null)
                {
                    foreach (var animationElement in animationElements)
                    {
                        string name = animationElement.Attribute("name")?.Value;
                        float delayInMilliseconds = float.Parse(animationElement.Attribute("delay")?.Value ?? "0");
                        
                        TimeSpan delay = TimeSpan.FromMilliseconds(delayInMilliseconds);
                        
                        List<TextureRegion> frames = new List<TextureRegion>();
                        
                        var frameRegions = animationElement.Elements("Frame");

                        if (frameRegions != null)
                        {
                            foreach (var frameRegion in frameRegions)
                            {
                                string regionName = frameRegion.Attribute("region")?.Value;
                                
                                TextureRegion region = spriteSheet.GetRegion(regionName);
                                frames.Add(region);
                            }
                        }

                        Animation animation = new Animation(frames, delay);
                        
                        spriteSheet.AddAnimation(name, animation);
                    }
                }
                
                return spriteSheet;
            }
        }
    }
}