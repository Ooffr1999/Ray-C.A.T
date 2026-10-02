using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Custom;
using RayCat.Data;

public class Calculate
{
    public Player player;
    public int FOV;
    public Map MAP;
    public Vector2i GAME_RES;
    public Vector2i SCREEN_RES;

    //Walls
    public Ray.Raydata[] rayData;
    public Ray.LineData[] lineData;

    //Floor
    public int[,] floorPlanePoints;
    public float [,] floorPlanePointsDistance;
    public Vector2[,] floorPlaneTexturePosition;

    public Calculate(Player _player, int _FOV, Map _MAP, Vector2i _GAME_RES, Vector2i _SCREEN_RES)
    {
        player = _player;
        FOV = _FOV;
        MAP = _MAP;
        GAME_RES = _GAME_RES;
        SCREEN_RES = _SCREEN_RES;

        rayData = new Ray.Raydata[GAME_RES.X];
        lineData = new Ray.LineData[GAME_RES.X];

        floorPlanePoints = new int[GAME_RES.X, GAME_RES.Y];
        floorPlanePointsDistance = new float[GAME_RES.X, GAME_RES.Y];
        floorPlaneTexturePosition = new Vector2[GAME_RES.X, GAME_RES.Y];
    }
    public void Walls()
    {
        float InitialCastDirection = player.direction - FOV / 2;
        float CastFieldOfViewPerCycleIncrement = (float)FOV / (float)GAME_RES.X;

        for (int r = 0; r < GAME_RES.X; r++)
        {
            //Raycast and get data
            rayData[r] = Ray.WallCast(player.position, InitialCastDirection + CastFieldOfViewPerCycleIncrement * r, MAP, MAP.resolution);

            lineData[r].lineWidth = SCREEN_RES.X / GAME_RES.X;                                  //Set width of line
            lineData[r].lineHeight = SCREEN_RES.Y / rayData[r].distance;                        //Set length of line

            lineData[r].drawStart = -lineData[r].lineHeight / 2 + SCREEN_RES.Y / 2;             //Set the position the line is drawn from
            lineData[r].drawEnd = lineData[r].lineHeight / 2 + SCREEN_RES.Y / 2;                //Set the position the line is drawn to

            double wallX;
            
            if (rayData[r].hitSide == 0)
                wallX = player.position.Y + rayData[r].distance * rayData[r].direction.Y;
            else wallX = player.position.X + rayData[r].distance * rayData[r].direction.X;
            
            wallX -= Math.Floor(wallX);

            lineData[r].texX = wallX * MAP.cells[rayData[r].hitData].tex.Width;
            
            if (rayData[r].hitSide == 0 && rayData[r].direction.X > 0)
                lineData[r].texX = MAP.cells[rayData[r].hitData].tex.Width - lineData[r].texX - 1;
            if (rayData[r].hitSide == 1 && rayData[r].direction.Y < 0)
                lineData[r].texX = MAP.cells[rayData[r].hitData].tex.Width - lineData[r].texX - 1;

            lineData[r].layerDepth = rayData[r].distance / 1000.0f;
        }
    }
    
    public void Floor()
    {
        float InitialCastDirection = player.direction - FOV / 2f;
        float CastFieldOfViewPerCycleIncrement = (float)FOV / (float)GAME_RES.X;

        for (int y = 0; y < GAME_RES.Y / 2; y++)
        {
            int p = y - GAME_RES.Y / 2;
            float posZ = player.height * GAME_RES.Y;
            double rowDistance = Math.Abs(posZ / p);
                
            for (int x = 0; x < GAME_RES.X; x++)
            {
                Ray.Raydata data = Ray.Cast(player.position, 
                                            InitialCastDirection + (CastFieldOfViewPerCycleIncrement * x), 
                                            rowDistance, 
                                            MAP, MAP.resolution);

                floorPlanePoints[x, y] = data.hitData;
                floorPlanePointsDistance[x, y] = data.distance;
                
                Vector2 floorCheckCell = new Vector2(EMath.FloorToInt(data.hitPosition.X), 
                                                     EMath.FloorToInt(data.hitPosition.Y));

                Vector2 texturePosition = data.hitPosition - floorCheckCell;

                floorPlaneTexturePosition[x, y] = new Vector2((int)MAP.cells[data.hitData].tex.Width * (data.hitPosition.X - floorCheckCell.X),
                                                        (int)MAP.cells[data.hitData].tex.Height * (data.hitPosition.Y - floorCheckCell.Y));
            }
        }
    }
}