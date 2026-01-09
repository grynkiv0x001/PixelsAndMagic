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
    private readonly Dictionary<string, Animation> _animations;
    private readonly Dictionary<string, TextureRegion> _regions;

    public SpriteSheet()
    {
        _regions = new Dictionary<string, TextureRegion>();
        _animations = new Dictionary<string, Animation>();
    }

    public SpriteSheet(Texture2D texture)
    {
        Texture = texture;

        _regions = new Dictionary<string, TextureRegion>();
        _animations = new Dictionary<string, Animation>();
    }

    /// <summary>
    ///     Gets or Sets the source texture represented by this texture atlas.
    /// </summary>
    public Texture2D Texture { get; set; }

    public void AddRegion(string name, int x, int y, int width, int height)
    {
        var region = new TextureRegion(Texture, x, y, width, height);

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
        var region = GetRegion(regionName);

        return new Sprite(region);
    }

    public AnimatedSprite CreateAnimatedSprite(string animationName)
    {
        var animation = GetAnimation(animationName);

        return new AnimatedSprite(animation);
    }

    /// <summary>
    ///     Creates a new SpriteSheet (TextureAtlas) based on an XML configuration file.
    /// </summary>
    /// <param name="content">The ContentManager.</param>
    /// <param name="filename">The path to the XML file, relative to the content root dir.</param>
    /// <returns>The SpriteSheet created by this method.</returns>
    public static SpriteSheet FromFile(ContentManager content, string filename)
    {
        var spriteSheet = new SpriteSheet();

        var path = Path.Combine(content.RootDirectory, filename);

        using (var stream = TitleContainer.OpenStream(path))
        {
            using (var reader = XmlReader.Create(stream))
            {
                var document = XDocument.Load(reader);
                var root = document.Root;

                // The <Texture> element contains the content path for the Texture2D to load.
                // We retrieve that value then use the content manager to load the texture.
                if (root != null)
                {
                    var texturePath = root.Element("Texture")?.Value;
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
                    foreach (var region in regions)
                    {
                        var name = region.Attribute("name")?.Value;

                        var x = int.Parse(region.Attribute("x")?.Value ?? "0");
                        var y = int.Parse(region.Attribute("y")?.Value ?? "0");
                        var width = int.Parse(region.Attribute("width")?.Value ?? "0");
                        var height = int.Parse(region.Attribute("height")?.Value ?? "0");

                        if (!string.IsNullOrEmpty(name)) spriteSheet.AddRegion(name, x, y, width, height);
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
                    foreach (var animationElement in animationElements)
                    {
                        var name = animationElement.Attribute("name")?.Value;
                        var delayInMilliseconds = float.Parse(animationElement.Attribute("delay")?.Value ?? "0");

                        var delay = TimeSpan.FromMilliseconds(delayInMilliseconds);

                        var frames = new List<TextureRegion>();

                        var frameRegions = animationElement.Elements("Frame");

                        if (frameRegions != null)
                            foreach (var frameRegion in frameRegions)
                            {
                                var regionName = frameRegion.Attribute("region")?.Value;

                                var region = spriteSheet.GetRegion(regionName);
                                frames.Add(region);
                            }

                        var animation = new Animation(frames, delay);

                        spriteSheet.AddAnimation(name, animation);
                    }

                return spriteSheet;
            }
        }
    }
}
