namespace Custom;

using Microsoft.Xna.Framework;

public class Vector2i
{
    public int X {get; set;}
    public int Y {get; set;}

    public Vector2i(int _x, int _y)
    {
        X = _x;
        Y = _y;        
    }

    public void One()
    {
        new Vector2i(1, 1);
    }

    public void Zero()
    {
        new Vector2i(0, 0);
    }

    public Vector2 toVector2()
    {
        return new Vector2(X, Y);
    }
}