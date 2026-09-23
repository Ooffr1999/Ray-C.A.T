using System.Diagnostics;
using Custom;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RayCat.Draw;

public class Minimap
{
    public bool Hide {get; set;}

    SpriteBatch spriteBatch;
    Map MAP;

    Vector2 offset;

    public Minimap(SpriteBatch _spriteBatch, Map _MAP)
    {
        Hide = true;

        spriteBatch = _spriteBatch; 
        MAP = _MAP;
        offset = new Vector2(0, 0);
    }

    public void DrawMap()
    {
        DrawMap(Vector2.Zero);
    }

    public void DrawMap(Vector2 _offset)
    {
        offset = _offset;

        if (Hide)
            return;

        for (int y = 0; y < MAP.resolution.Y; y++)                  //TODO: Make a better looking map
        {
            for (int x = 0; x < MAP.resolution.X; x++)
            {
                Color col = new Color();
                col = Color.White;

                switch(MAP.data[x, y])
                {
                    case 0: 
                        col = Color.White;
                        break;
                    case 1: 
                        col = Color.Aquamarine;
                        break;

                    default: 
                        col = Color.Beige;
                        break;
                }
                Primitives.DrawBox(spriteBatch, ((new Vector2(x, y) + offset) * MAP.cellSize), col, Vector2.One * (MAP.cellSize - 1));
            }
        }
    }

    public void DrawPlayer(Player player, int FOV)
    {
        if (Hide)
            return;
        
        //for (int r = 0; r < GameResolution.X; r++)
        //    Primitives.DrawLine(spriteBatch, player.position * MAP.cellSize, (player.position + _calculateMap.rayData[r].hitPosition) * MAP.cellSize, 1f, Color.Red);
        
        //Draw player on the minimap
        player.Draw(spriteBatch, (player.position + offset) * MAP.cellSize, Color.White, player.direction, Vector2.One / 10);   
    }    
}