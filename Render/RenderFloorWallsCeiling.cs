using Custom;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Render
{
    public float lightStrength {get; set;}
    
    SpriteBatch batch;
    Calculate calcMap;

    public Render (GameServiceContainer serviceContainer, Calculate _calcMap)
    {
        batch = serviceContainer.GetService<SpriteBatch>();
        calcMap = _calcMap;
        lightStrength = 0.5f;
    }

    public void Floors()
    {
        for (int y = 0; y < calcMap.GAME_RES.Y / 2; y++)
        {
            for (int x = 0; x < calcMap.GAME_RES.X; x++)
            {   
                if (calcMap.MAP.cells[calcMap.floorPlanePoints[x, y]].cellType != CellType.Wall)
                {
                    Texture2D tex = calcMap.MAP.cells[calcMap.floorPlanePoints[x, y]].tex;
                    Color colorShade = calcMap.MAP.cells[calcMap.floorPlanePoints[x, y]].color;
                    
                    //Get distance shade   
                    Color floorShade = (colorShade * lightStrength) * (1 / calcMap.floorPlanePointsDistance[x, y]);
                    floorShade.A = 255;

                    //Texture floor
                    
                    Primitives.DrawTexturedBox(batch, 
                                                    tex, 
                                                    new EMath.Vector2i((int)calcMap.floorPlaneTexturePosition[x, y].X, 
                                                                       (int)calcMap.floorPlaneTexturePosition[x, y].Y), 
                                                    new EMath.Vector2i(1, 1), 
                                                    new Vector2(x * 6f,
                                                    calcMap.SCREEN_RES.Y - y * 4.5f),
                                                    Vector2.One * 0.5f,
                                                    Vector2.One * 6,
                                                    floorShade);
                                                    
                                                    
                    
                    if (calcMap.MAP.cells[calcMap.floorPlanePoints[x, y]].cellType == CellType.FloorCeil)
                    {
                    //Texture ceiling
                    Primitives.DrawTexturedBox(batch, 
                                                    tex, 
                                                    new EMath.Vector2i( (int)calcMap.floorPlaneTexturePosition[x, y].X, 
                                                                        (int)calcMap.floorPlaneTexturePosition[x, y].Y), 
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

    public void Walls()
    {
        for (int r = 0; r < calcMap.GAME_RES.X; r++)
        {
            Cell cell = calcMap.MAP.cells[calcMap.rayData[r].hitData];

            Color color = (cell.color * lightStrength) * (1 / calcMap.rayData[r].distance);
            color.A = 255;

            Primitives.DrawTexturedLine(batch, cell.tex, (int)calcMap.lineData[r].texX, 0,
                                new Vector2(calcMap.lineData[r].lineWidth * r, calcMap.lineData[r].drawStart), 
                                new Vector2(calcMap.lineData[r].lineWidth * r, calcMap.lineData[r].drawEnd), 
                                calcMap.lineData[r].lineWidth, 
                                color,
                                0);
                                
        }
    }

    public void Sprite(Sprite sprite, Vector2 pos)
    {

    }
}