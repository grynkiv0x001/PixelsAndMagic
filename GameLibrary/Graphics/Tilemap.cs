using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Graphics;

public class Tilemap
{
    // TODO: Refactor

    private readonly int[] _tiles;
    private readonly Tileset _tileset;

    public Tilemap(Tileset tileset, int rows, int cols)
    {
        _tileset = tileset;

        Rows = rows;
        Columns = cols;
        Count = Rows * Columns;

        Scale = Vector2.One;

        _tiles = new int[Count];
    }

    public int Columns { get; }
    public int Rows { get; }

    public int Count { get; }

    public Vector2 Scale { get; set; }

    public float TileWidth => _tileset.TileWidth * Scale.X;
    public float TileHeight => _tileset.TileHeight * Scale.Y;

    public void SetTile(int index, int tilesetID)
    {
        _tiles[index] = tilesetID;
    }

    public void SetTile(int row, int col, int tilesetID)
    {
        var index = row * Columns + col;
        SetTile(index, tilesetID);
    }

    public TextureRegion GetTile(int index)
    {
        return _tileset.GetTile(_tiles[index]);
    }

    public TextureRegion GetTile(int row, int col)
    {
        var index = row * Columns + col;
        return GetTile(index);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (var i = 0; i < Count; i++)
        {
            var tilesetIndex = _tiles[i];
            var tile = _tileset.GetTile(tilesetIndex);

            var x = i % Columns;
            var y = i / Columns;

            var position = new Vector2(x * TileWidth, y * TileHeight);

            tile.Draw(
                spriteBatch,
                position,
                Color.White,
                0.0f,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                1.0f
            );
        }
    }

    /// <summary>
    ///     Creates a new tilemap based on a tilemap xml configuration file.
    /// </summary>
    /// <param name="content">The content manager used to load the texture for the tileset.</param>
    /// <param name="filename">The path to the xml file, relative to the content root directory.</param>
    /// <returns>The tilemap created by this method.</returns>
    public static Tilemap FromFile(ContentManager content, string filename)
    {
        var filePath = Path.Combine(content.RootDirectory, filename);

        using (var stream = TitleContainer.OpenStream(filePath))
        {
            using (var reader = XmlReader.Create(stream))
            {
                var doc = XDocument.Load(reader);
                var root = doc.Root;

                // The <Tileset> element contains the information about the tileset
                // used by the tilemap.
                //
                // Example
                // <Tileset region="0 0 100 100" tileWidth="10" tileHeight="10">contentPath</Tileset>
                //
                // The region attribute represents the x, y, width, and height
                // components of the boundary for the texture region within the
                // texture at the contentPath specified.
                //
                // the tileWidth and tileHeight attributes specify the width and
                // height of each tile in the tileset.
                //
                // the contentPath value is the contentPath to the texture to
                // load that contains the tileset
                var tilesetElement = root.Element("Tileset");

                var regionAttribute = tilesetElement.Attribute("region").Value;
                var split = regionAttribute.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var x = int.Parse(split[0]);
                var y = int.Parse(split[1]);
                var width = int.Parse(split[2]);
                var height = int.Parse(split[3]);

                var tileWidth = int.Parse(tilesetElement.Attribute("tileWidth").Value);
                var tileHeight = int.Parse(tilesetElement.Attribute("tileHeight").Value);
                var contentPath = tilesetElement.Value;

                // Load the texture 2d at the content path
                var texture = content.Load<Texture2D>(contentPath);

                // Create the texture region from the texture
                var textureRegion = new TextureRegion(texture, x, y, width, height);

                // Create the tileset using the texture region
                var tileset = new Tileset(textureRegion, tileWidth, tileHeight);

                // The <Tiles> element contains lines of strings where each line
                // represents a row in the tilemap.  Each line is a space
                // separated string where each element represents a column in that
                // row.  The value of the column is the id of the tile in the
                // tileset to draw for that location.
                //
                // Example:
                // <Tiles>
                //      00 01 01 02
                //      03 04 04 05
                //      03 04 04 05
                //      06 07 07 08
                // </Tiles>
                var tilesElement = root.Element("Tiles");

                // Split the value of the tiles data into rows by splitting on
                // the new line character
                var rows = tilesElement.Value.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);

                // Split the value of the first row to determine the total number of columns
                var columnCount = rows[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Length;

                // Create the tilemap
                var tilemap = new Tilemap(tileset, rows.Length, columnCount);

                // Process each row
                for (var row = 0; row < rows.Length; row++)
                {
                    // Split the row into individual columns
                    var columns = rows[row].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    // Process each column of the current row
                    for (var column = 0; column < columnCount; column++)
                    {
                        // Get the tileset index for this location
                        var tilesetIndex = int.Parse(columns[column]);

                        // Add that region to the tilemap at the row and column location
                        tilemap.SetTile(row, column, tilesetIndex);
                    }
                }

                return tilemap;
            }
        }
    }
}
