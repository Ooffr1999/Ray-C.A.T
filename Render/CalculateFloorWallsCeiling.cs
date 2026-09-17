using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Custom;

public class CalculateMap
{
    Player player;
    int FOV;
    Map MAP;
    Vector2i GAME_RES;
    Vector2i SCREEN_RES;

    public CalculateMap(Player _player, int _FOV, Map _MAP, Vector2i _GAME_RES, Vector2i _SCREEN_RES)
    {
        player = _player;
        FOV = _FOV;
        MAP = _MAP;
        GAME_RES = _GAME_RES;
        SCREEN_RES = _SCREEN_RES;
    }
    public void Walls(Player player, Map MAP, Ray.Raydata[] rayData, Ray.LineData[] lineData)
    {
        float InitialCastDirection = player.direction - FOV / 2;
        float CastFieldOfViewPerCycleIncrement = (float)FOV / (float)GAME_RES.X;

        for (int r = 0; r < GAME_RES.X; r++)
        {
            CameraPlane plane = new CameraPlane(player.position, player.direction, 0.25f, FOV);

            rayData[r] = Ray.WallCast(player.position, InitialCastDirection + CastFieldOfViewPerCycleIncrement * r, MAP, MAP.resolution);

            lineData[r].lineWidth = SCREEN_RES.X / GAME_RES.X;
            lineData[r].lineHeight = SCREEN_RES.Y / rayData[r].distance;

            lineData[r].drawStart = -lineData[r].lineHeight / 2 + SCREEN_RES.Y / 2;
            lineData[r].drawEnd = lineData[r].lineHeight / 2 + SCREEN_RES.Y / 2;

            double wallX;
            
            if (rayData[r].hitSide == 0)
                wallX = player.position.Y + rayData[r].distance * rayData[r].direction.Y;
            else wallX = player.position.X + rayData[r].distance * rayData[r].direction.X;
            wallX -= Math.Floor(wallX);

            lineData[r].texX = (int)(wallX * (double)MAP.cells[rayData[r].hitData].tex.Width);
            if (rayData[r].hitSide == 0 && rayData[r].direction.X > 0)
                lineData[r].texX = MAP.cells[rayData[r].hitData].tex.Width - lineData[r].texX - 1;
            if (rayData[r].hitSide == 1 && rayData[r].direction.Y < 0)
                lineData[r].texX = MAP.cells[rayData[r].hitData].tex.Width - lineData[r].texX - 1;
        }
    }
    
    public void Floor(Player player, Map MAP, int[,] floorPoints, float[,] floorPointsDistance, Vector2[,] floorPointTexture)
    {
        float InitialCastDirection = player.direction - FOV / 2f;
        float CastFieldOfViewPerCycleIncrement = (float)FOV / (float)GAME_RES.X;

        for (int y = 0; y < GAME_RES.Y / 2; y++)
        {
            int p = y - GAME_RES.Y / 2;
            float posZ = player.height * GAME_RES.Y;
            double rowDistance = Math.Abs(posZ / p);

            Console.WriteLine(posZ);
                
            for (int x = 0; x < GAME_RES.X; x++)
            {
                Ray.Raydata data = Ray.Cast(player.position, 
                                            InitialCastDirection + (CastFieldOfViewPerCycleIncrement * x), 
                                            rowDistance, 
                                            MAP, MAP.resolution);

                floorPoints[x, y] = data.hitData;
                floorPointsDistance[x, y] = data.distance;
                
                Vector2 floorCheckCell = new Vector2(EMath.FloorToInt(data.hitPosition.X), 
                                                     EMath.FloorToInt(data.hitPosition.Y));

                Vector2 texturePosition = data.hitPosition - floorCheckCell;

                floorPointTexture[x, y] = new Vector2((int)MAP.cells[data.hitData].tex.Width * (data.hitPosition.X - floorCheckCell.X),
                                                        (int)MAP.cells[data.hitData].tex.Height * (data.hitPosition.Y - floorCheckCell.Y));
                
                //Color[] colorArray = new Color[MAP.cells[data.hitData].tex.Width * MAP.cells[data.hitData].tex.Height];
                //MAP.cells[data.hitData].tex.GetData(colorArray);

                
            }
        }
    }
}