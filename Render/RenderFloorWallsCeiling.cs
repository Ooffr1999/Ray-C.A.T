using Custom;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Render
{
    public float lightStrength {get; set;}

    SpriteBatch batch;

    public Render (SpriteBatch _batch)
    {
        batch = _batch;
        lightStrength = 0.5f;
    }

    public void Floors(Map MAP, Vector2i Game_Res, Vector2i Screen_Res, int[,] floorCheck, Vector2[,] floorTexturePosition, float[,] floorPointDistance)
    {
        for (int y = 0; y < Game_Res.Y / 2; y++)
        {
            for (int x = 0; x < Game_Res.X; x++)
            {   
                if (MAP.cells[floorCheck[x, y]].cellType != CellType.Wall)
                {
                    Texture2D tex = MAP.cells[floorCheck[x, y]].tex;
                    Color colorShade = MAP.cells[floorCheck[x, y]].color;
                    
                    //Get distance shade   
                    Color floorShade = (colorShade * lightStrength) * (1 / floorPointDistance[x, y]);
                    floorShade.A = 255;

                    //Texture floor
                    
                    Primitives.DrawTexturedBox(batch, 
                                                    tex, 
                                                    new EMath.Vector2i((int)floorTexturePosition[x, y].X, (int)floorTexturePosition[x, y].Y), 
                                                    new EMath.Vector2i(1, 1), 
                                                    new Vector2(x * 6f,
                                                    Screen_Res.Y - y * 4.5f),
                                                    Vector2.One * 0.5f,
                                                    Vector2.One * 6,
                                                    floorShade);
                                                    
                                                    
                    
                    if (MAP.cells[floorCheck[x, y]].cellType == CellType.FloorCeil)
                    {
                    //Texture ceiling
                    Primitives.DrawTexturedBox(batch, 
                                                    tex, 
                                                    new EMath.Vector2i((int)floorTexturePosition[x, y].X, (int)floorTexturePosition[x, y].Y), 
                                                    new EMath.Vector2i(1, 1), 
                                                    new Vector2(x * 6f,
                                                    0 + y * 4.5f),
                                                    Vector2.One * 0.5f,
                                                    Vector2.One * 6,
                                                    floorShade);
                    }
                }
            }
        }
    }

    public void Walls(Map MAP, Ray.Raydata[] rayData, Ray.LineData[] lineData, int xRes)
    {
        for (int r = 0; r < xRes; r++)
        {
            Cell cell = MAP.cells[rayData[r].hitData];

            Color color = (cell.color * lightStrength) * (1 / rayData[r].distance);
            color.A = 255;

            Primitives.DrawTexturedLine(batch, cell.tex, (int)lineData[r].texX, 0,
                                new Vector2(lineData[r].lineWidth * r, lineData[r].drawStart), 
                                new Vector2(lineData[r].lineWidth * r, lineData[r].drawEnd), 
                                lineData[r].lineWidth, 
                                color,
                                0);
                                
        }
    }
}