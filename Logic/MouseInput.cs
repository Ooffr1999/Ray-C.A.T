using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class MouseInput 
{
    public MouseState previousState {get; private set;}
    public MouseState currentState {get; private set;}

    public Point position
    {
        get => currentState.Position;
        set => SetPosition(value.X, value.Y); 
    }

    public int X
    {
        get => currentState.X;
        set => SetPosition(value, currentState.Y);
    }

    public int Y
    {
        get => currentState.Y;
        set => SetPosition(currentState.X, value);
    }

    public Point positionDelta => currentState.Position - previousState.Position;

    public int XDelta => currentState.Position.X - previousState.Position.X;

    public int YDelta => currentState.Position.Y - previousState.Position.Y;

    public bool wasMoved => positionDelta != Point.Zero;

    public int ScrollWheelValue => currentState.ScrollWheelValue;

    public int ScrollWheelDelta => currentState.ScrollWheelValue - previousState.ScrollWheelValue;

    public MouseInput() 
    {
        previousState = new MouseState();
        currentState = Mouse.GetState();
    }
    
    public void Update()
    {
        previousState = currentState;
        currentState = Mouse.GetState();
    }

    public bool IsButtonDown(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.left:
                return currentState.LeftButton == ButtonState.Pressed;
            case MouseButton.middle:
                return currentState.MiddleButton == ButtonState.Released;
            case MouseButton.right:
                return currentState.RightButton == ButtonState.Released;
            case MouseButton.xButton1:
                return currentState.XButton1 == ButtonState.Released;
            case MouseButton.xButton2:
                return currentState.XButton2 == ButtonState.Released;
            default:
                return false;   
        }
    }

    public bool IsButtonJustPressed(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.left:
                return currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released;
            case MouseButton.middle:
                return currentState.MiddleButton == ButtonState.Pressed && previousState.MiddleButton == ButtonState.Released;
            case MouseButton.right:
                return currentState.RightButton == ButtonState.Pressed && previousState.RightButton == ButtonState.Released;
            case MouseButton.xButton1:
                return currentState.XButton1 == ButtonState.Pressed && previousState.XButton1 == ButtonState.Released;
            case MouseButton.xButton2:
                return currentState.XButton2 == ButtonState.Pressed && previousState.XButton2 == ButtonState.Released;
            default:
                return false;
        }
    }

    public bool IsButtonJustReleased(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.left:
                return currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed;
            case MouseButton.middle:
                return currentState.MiddleButton == ButtonState.Released && previousState.MiddleButton == ButtonState.Pressed;
            case MouseButton.right:
                return currentState.RightButton == ButtonState.Released && previousState.RightButton == ButtonState.Pressed;
            case MouseButton.xButton1:
                return currentState.XButton1 == ButtonState.Released && previousState.XButton1 == ButtonState.Pressed;
            case MouseButton.xButton2:
                return currentState.XButton2 == ButtonState.Released && previousState.XButton2 == ButtonState.Pressed;
            default:
                return false;
        }
    }

    public void SetPosition(int x, int y)
    {
        Mouse.SetPosition(x, y);
        currentState = new MouseState(
            x,
            y,
            currentState.ScrollWheelValue,
            currentState.LeftButton,
            currentState.MiddleButton,
            currentState.RightButton,
            currentState.XButton1,
            currentState.XButton2
        );
    }

    public enum MouseButton
    {
        left,
        middle,
        right,
        xButton1,
        xButton2
    }
}

   