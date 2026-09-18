using Custom;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Map
{
    public int[,] data {get; set;}
    public Vector2i resolution {get; set;}

    public float cellSize {get; set;}

    public Map (int[,] _data, float _cellSize)
    {
        data = _data;

        resolution = new Vector2i(1, 1);

        resolution.X = (int)_data.GetLength(0);
        resolution.Y = (int)_data.GetLength(1);
        cellSize = _cellSize;
    }

    public Cell[] cells {get; set;}
}

public enum CellType {Floor, FloorCeil, Wall, Sprite}

public class Cell
{
    public CellType cellType {get; set;}
    public Color color {get; set;}
    public Texture2D tex {get; set;}

    public Cell(CellType _cellType, Color _color, Texture2D _tex)
    {
        cellType = _cellType;
        color = _color;
        tex = _tex;
    }
    
}