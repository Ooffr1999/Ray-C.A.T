using System;
using System.Xml.Serialization;
using Custom;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Render
{
    public float lightStrength {get; set;}
    
    SpriteBatch batch;
    RaycastCalculate calcMap;
    Vector2i WindowResolution;
    Vector2i GameResolution;

    float xModifier = 0;
    float yModifier = 0;


    public Render (GameServiceContainer serviceContainer, RaycastCalculate _calcMap)
    {
        batch = serviceContainer.GetService<SpriteBatch>();
        calcMap = _calcMap;
        lightStrength = 1f;

        WindowResolution = calcMap.SCREEN_RES;
        GameResolution = calcMap.GAME_RES;

        xModifier = (float)WindowResolution.X / (float)GameResolution.X; 
        yModifier = (float)WindowResolution.Y / (float)GameResolution.Y;
    }

    public void Floors()
    {
        Texture2D tex = calcMap.MAP.cells[0].tex;

        for (int y = 0; y < GameResolution.Y / 2; y++)
        {
            for (int x = 0; x < GameResolution.X; x++)
            {   
                if (calcMap.floorPlanePoints[x, y] != 0)
                {
                if (calcMap.MAP.cells[calcMap.floorPlanePoints[x, y]].cellType != CellType.Wall)
                {
                    Color colorShade = calcMap.MAP.cells[calcMap.floorPlanePoints[x, y]].color;
                    
                    //Get distance shade   
                    Color floorShade = (colorShade * lightStrength) * (1 / calcMap.floorPlanePointsDistance[x, y]);
                    floorShade.A = 255;

                    //Texture floor
                    Primitives.DrawTexturedBox(batch, 
                                                    tex, 
                                                    new Vector2i((int)calcMap.floorPlaneTexturePosition[x, y].X, 
                                                                       (int)calcMap.floorPlaneTexturePosition[x, y].Y), 
                                                    new Vector2i(1, 1), 
                                                    new Vector2(x * xModifier,
                                                    WindowResolution.Y - y * yModifier),
                                                    Vector2.One * 0.5f,
                                                    Vector2.One * xModifier,
                                                    floorShade,
                                                    0.0f);
                                                    
                                                    
                    
                    if (calcMap.MAP.cells[calcMap.floorPlanePoints[x, y]].cellType == CellType.FloorCeil)
                    {
                    //Texture ceiling
                    Primitives.DrawTexturedBox(batch, 
                                                    tex, 
                                                    new Vector2i( (int)calcMap.floorPlaneTexturePosition[x, y].X, 
                                                                        (int)calcMap.floorPlaneTexturePosition[x, y].Y), 
                                                    new Vector2i(1, 1), 
                                                    new Vector2(x * xModifier,
                                                    0 + y * yModifier),
                                                    Vector2.One * 0.5f,
                                                    Vector2.One * xModifier,
                                                    floorShade,
                                                    0.0f);
                    }
                }
                }
            }
        }
    }

    public void Walls()
    {
        Texture2D tex = AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/Stone/Stone01.png");

        for (int r = 0; r < GameResolution.X; r++)
        {
            Cell cell = calcMap.MAP.cells[calcMap.rayData[r].hitData];

            Color color = (cell.color * lightStrength) * (1 / calcMap.rayData[r].distance);
            color.A = 255;

            Primitives.DrawTexturedLine(batch, tex, (int)calcMap.lineData[r].texX, 0,
                                new Vector2((calcMap.lineData[r].lineWidth * r), calcMap.lineData[r].drawStart), 
                                new Vector2((calcMap.lineData[r].lineWidth * r), calcMap.lineData[r].drawEnd), 
                                calcMap.lineData[r].lineWidth / xModifier, 
                                color,
                                calcMap.lineData[r].layerDepth); 
        }
    }
}