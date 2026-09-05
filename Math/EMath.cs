using System;
using Microsoft.Xna.Framework;

public static class EMath
{
    public static Vector2 getDirection(float dir)
    {
        return new Vector2((float)Math.Cos(dir), (float)Math.Sin(dir));
    }

    public static float DegToRad(float degree)
    {
        return (float)(degree * (Math.PI * 180));
    }

    public static Vector2i FloorVector(Vector2 pos)
    {
        return new Vector2i(FloorToInt(pos.X), FloorToInt(pos.Y));
    } 

    public static int FloorToInt(float n)
    {
        return (int)Math.Floor(n);
    }
    
    public static int CeilToInt(float n)
    {
        return (int)Math.Floor(n);
    }

    public struct Vector2i
    {
        public int X;
        public int Y;

        public Vector2i(int x, int y)
        {
            X = x;
            Y = y;
        } 
    }
}