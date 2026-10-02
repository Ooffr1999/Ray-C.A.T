using System;
using System.Data;
using System.Net.NetworkInformation;
using Custom;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GUIObject
{
    public InputManager input;
    Rectangle area {get; set;}

    Vector2 position { get; set; }
    int Width;
    int Height;
    
    public Color standardColor {get; set;}
    public Color hoverColor {get; set;}
    public Color clickColor {get; set;}
    public Color color;

    public GUIObject()
    {
        SetPosition(new Vector2(100, 100));
        SetWidth(200); 
        SetHeight(50);
        area = new Rectangle((int)position.X, (int)position.Y, Width, Height);
        color = Color.White;
        standardColor = Color.White;
        hoverColor = Color.Gray;
        clickColor = Color.Black;
    }

    public GUIObject(InputManager _input)
    {
        input = _input;
        SetPosition(new Vector2(100, 100));
        SetWidth(200); 
        SetHeight(100);
        area = new Rectangle((int)position.X, (int)position.Y, Width, Height);
        color = Color.White;
        standardColor = Color.White;
        hoverColor = Color.Gray;
        clickColor = Color.Black;
    }

    public GUIObject(InputManager _input, Vector2 position, int Width, int Height)
    {
        input = _input;
        SetPosition(position);
        SetWidth(Width); 
        SetHeight(Height);
        area = new Rectangle((int)position.X, (int)position.Y, Width, Height);
        color = Color.White;
        standardColor = Color.White;
        hoverColor = Color.Gray;
        clickColor = Color.Black;
    }

    public virtual void Update()
    {
        
    }

    public virtual void Draw(SpriteBatch _batch)
    {
        Primitives.DrawBox(_batch, 
                            position, 
                            color, 
                            new Vector2(Width, Height));
    }

    public bool IsMouseInConfines()
    {
        return area.Contains(input.Mouse.position);
    }

    public bool GetMouseClick()
    {
        return input.Mouse.IsButtonJustPressed(MouseInput.MouseButton.left) && IsMouseInConfines();
    }

    public virtual void SetPosition(Vector2 _position)
    {
        position = _position;
        SetArea();
    }

    public virtual Vector2 GetPosition()
    {
        return position;
    }

    public virtual void SetWidth(int _Width)
    {
        Width = _Width;
        SetArea();
    }

    public virtual void SetHeight(int _Height)
    {
        Height = _Height;
        SetArea();
    }

    public virtual int GetWidth()
    {
        return Width;
    }

    public virtual int GetHeight()
    {
        return Height;
    }

    public void SetArea()
    {
        area = new Rectangle((int)position.X, (int)position.Y, Width, Height);
    }

    public void SetArea(Vector2 _position, int _Width, int _Height)
    {
        area = new Rectangle((int)_position.X, (int)_position.Y, _Width, _Height);
    }

    public virtual Rectangle GetArea()
    {
        return area;
    }
}