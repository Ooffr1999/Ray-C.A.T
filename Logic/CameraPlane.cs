using System;
using Microsoft.Xna.Framework;

public class CameraPlane
{
    public Vector2 leftPosition {get; set;}
    public Vector2 rightPosition {get; set;}

    float distanceFromPlayer = 0;

    public CameraPlane(Vector2 playerPosition, float direction, float _distanceFromPlayer, int FieldOfView)
    {
        float fov = FieldOfView / 100.000f;
        distanceFromPlayer = _distanceFromPlayer;

        Vector2 _forward = EMath.getDirection(MathHelper.ToRadians(direction));
        Vector2 _right = EMath.getDirection(MathHelper.ToRadians(direction + 90));

        Vector2 planePosition = playerPosition + _forward * distanceFromPlayer;

        leftPosition = planePosition + -_right * (fov * distanceFromPlayer);
        rightPosition = planePosition + _right * (fov * distanceFromPlayer);
    }

    public Vector2 XlerpValueAcrossPlane(float value)
    {
        return Vector2.Lerp(leftPosition, rightPosition, value);

    }
}