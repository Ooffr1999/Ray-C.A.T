using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class PlayerInput
{
    InputManager input;
    float speed {get; set;}
    float runModifier {get; set;}

    public PlayerInput(InputManager _input, float _moveSpeed, float _runModifier)
    {
        input = _input;

        speed = _moveSpeed;
        runModifier = _runModifier;
    }

    public void Move(Player player, float delta, Map MAP)
    {
        float _speed = speed;

        if (input.Keyboard.IsKeyDown(Keys.LeftShift))
            _speed *= 2.5f;

        Vector2 nextFramePosition = input.Keyboard.GetAxis(Keys.S, Keys.W) * player.forward +
                                    input.Keyboard.GetAxis(Keys.A, Keys.D) * player.right;

        nextFramePosition *= delta;
        nextFramePosition *= _speed;

        EMath.Vector2i mapCheck = EMath.FloorVector(player.position + nextFramePosition);

        if (MAP.cells[MAP.data[mapCheck.X, mapCheck.Y]].cellType != CellType.Wall)
            player.position += nextFramePosition;
    }

    public void Turn(Player player, float delta, int xRes)
    {
        player.direction += input.Mouse.XDelta * delta * 10;
        
        if (input.Mouse.position.X < 20)
            input.Mouse.SetPosition(xRes - 20, input.Mouse.position.Y);
        if (input.Mouse.position.X >= xRes - 5)
            input.Mouse.SetPosition(5, input.Mouse.position.Y);

        if (player.direction > 360)
            player.direction = 0;
        else if (player.direction < 0)
            player.direction = 360;
    }
}