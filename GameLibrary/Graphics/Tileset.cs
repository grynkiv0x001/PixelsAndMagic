namespace GameLibrary.Graphics;

public class Tileset
{
    private readonly TextureRegion[] _tiles;

    public Tileset(TextureRegion textureRegion, int tileWidth, int tileHeight)
    {
        TileWidth = tileWidth;
        TileHeight = tileHeight;

        Columns = textureRegion.Width / tileWidth;
        Rows = textureRegion.Height / tileHeight;

        Count = Columns * Rows;

        _tiles = new TextureRegion[Count];

        for (var i = 0; i < Count; i++)
        {
            var x = i % Columns * TileWidth;
            var y = i / Columns * TileHeight;

            _tiles[i] = new TextureRegion(
                textureRegion.Texture,
                textureRegion.SourceRectangle.X + x,
                textureRegion.SourceRectangle.Y + y,
                tileWidth,
                tileHeight
            );
        }
    }

    public int TileWidth { get; }
    public int TileHeight { get; }

    public int Rows { get; }
    public int Columns { get; }

    public int Count { get; }

    public TextureRegion GetTile(int index)
    {
        return _tiles[index];
    }

    public TextureRegion GetTile(int row, int col)
    {
        var index = row * Columns + col;

        return GetTile(index);
    }
}
