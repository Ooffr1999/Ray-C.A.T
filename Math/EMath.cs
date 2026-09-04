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