using System;
using Microsoft.Xna.Framework;

public static class Ray
{
    public static Raydata WallCast(Vector2 pos, double direction, int[] MAP, EMath.Vector2i MAP_RESOLUTION)
        {
            Raydata data = new Raydata();

            data.direction = EMath.getDirection(MathHelper.ToRadians((float)direction));

            EMath.Vector2i step = new EMath.Vector2i(Math.Sign(data.direction.X), Math.Sign(data.direction.Y));
            EMath.Vector2i mapCheck = EMath.FloorVector(pos);
            new Vector2((int)Math.Floor(pos.X), (int)Math.Floor(pos.Y));

            Vector2 roFract = new Vector2(pos.X - mapCheck.X, pos.Y - mapCheck.Y);

            Vector2 rayLength = Vector2.Zero;
            Vector2 rayUnitStepSize = new Vector2((float)Math.Sqrt(1 + (data.direction.Y / data.direction.X) * (data.direction.Y / data.direction.X)), 
                                                (float)Math.Sqrt(1 + (data.direction.X / data.direction.Y) * (data.direction.X / data.direction.Y)));
            
            if (data.direction.X < 0)
                rayLength.X = roFract.X * rayUnitStepSize.X;
            else rayLength.X = (1 - roFract.X) * rayUnitStepSize.X;

            if (data.direction.Y < 0)
                rayLength.Y = roFract.Y * rayUnitStepSize.Y;
            else rayLength.Y = (1 - roFract.Y) * rayUnitStepSize.Y;

            //Console.WriteLine(mapCheck);

            for (int i = 0; i < 50; i++)
            {
                if (rayLength.X < rayLength.Y)
                {
                    mapCheck.X += step.X;
                    data.distance = rayLength.X;
                    data.hitSide = 0;
                    rayLength.X += rayUnitStepSize.X;
                }

                else
                {
                    mapCheck.Y += step.Y;
                    data.distance = rayLength.Y;
                    data.hitSide = 1;
                    rayLength.Y += rayUnitStepSize.Y;
                }

                data.hitData = MAP[mapCheck.Y * MAP_RESOLUTION.Y + mapCheck.X];
                if (data.hitData == 1)
                    break;
            }

            data.hitPosition = data.direction * data.distance;

            return data;
        }

    public static Raydata Cast(Vector2 pos, double direction, double distance, int [] MAP, EMath.Vector2i MAP_RESOLUTION)
    {
        Raydata data = new Raydata();

        data.direction = EMath.getDirection(MathHelper.ToRadians((float)direction));

        data.distance = (float)distance;

        data.hitPosition = pos + (data.direction * data.distance);

        if (
            data.hitPosition.X >= 0 &&
            data.hitPosition.X < MAP_RESOLUTION.X &&
            data.hitPosition.Y >= 0 &&
            data.hitPosition.Y < MAP_RESOLUTION.Y
        )
        {
            data.hitData = MAP[MAP_RESOLUTION.Y * (int)data.hitPosition.Y + (int)data.hitPosition.X];
        }
        return data;
    }
    public struct Raydata
    {
        public float distance;
        public Vector2 direction;
        public int hitData;
        public int hitSide;
        public Vector2 hitPosition;
    }

    public struct LineData
    {
        public float lineWidth;
        public float lineHeight;
        public float drawStart;
        public float drawEnd;
        public Color color;

        public double texX;
    }
}