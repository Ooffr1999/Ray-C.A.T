namespace Custom;

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
}