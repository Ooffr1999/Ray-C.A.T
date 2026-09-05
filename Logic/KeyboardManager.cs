using System.Numerics;
using Microsoft.Xna.Framework.Input;

public class KeyboardInput
{
    public KeyboardState previousState {get; private set;}
    public KeyboardState currentState {get; private set;}

    public KeyboardInput ()
    {
        previousState = new KeyboardState();
        currentState = Keyboard.GetState();
    }

    public void Update()
    {
        previousState = currentState;
        currentState = Keyboard.GetState();
    }

    public bool IsKeyDown(Keys key)
    {
        return currentState.IsKeyDown(key);
    }

    public bool IsKeyUp(Keys key)
    {
        return currentState.IsKeyUp(key);
    }

    public bool IsButtonJustPressed(Keys key)
    {
        return currentState.IsKeyDown(key) && previousState.IsKeyUp(key);
    }

    public bool IsButtonJustReleased(Keys key)
    {
        return currentState.IsKeyUp(key) && previousState.IsKeyDown(key);
    }

    public float GetAxis(Keys negative, Keys positive)
    {
        float axis = 0.0f;

        if (currentState.IsKeyDown(negative) && !currentState.IsKeyDown(positive))
            axis = -1f;
        else if (!currentState.IsKeyDown(negative) && currentState.IsKeyDown(positive))
            axis = 1f;
        else axis = 0;

        return axis;
    }

    public Vector2 GetVector(Keys Xnegative, Keys Xpositive, Keys YNegative, Keys YPositive)
    {
        return new Vector2(GetAxis(Xnegative, Xpositive), GetAxis(YNegative, YPositive));
    }
}