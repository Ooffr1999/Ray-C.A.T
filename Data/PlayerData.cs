using System;
using System.Buffers.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RayCat.Data;

public class Player : Entity
{
    public float height {get; set;}                             //Additional modifier for height
                                                                //This is used in the Rendering. Later it will be used to have variable height positions
    public float speed {get; set;}                              //Movement speed
    public float runModifier {get; set;}                        //Modifier that increases speed when character is running

    public float turnSpeedModifier {get; set;}                  //Turn speed modifier

    InputManager input;
    Map MAP;

    //Passes in the Servicemanager for the Entity it inherits from
    public Player(GameServiceContainer service, Map _MAP) : base(service)
    {
        position = new Vector2(5, 2);                            //Set initial position
        direction = 90;                                          //Set initial direction
        height = 0.5f;                                           //Set initial height
        speed = 1;                                               //Set initial speed           
        runModifier = 3;                                         //set runmodifier                   
        turnSpeedModifier = 10;

        input = RayCatCore._input;                              //Get reference to the input manager directly from the Core.
                                                                //The input is static. That's what makes it possible to do this.

        MAP = _MAP;                                             //Get a copy of the map for collision detection
    }

    public override void Update(float _deltaTime)               //Run move and turn functions in the Update
    {
        Move();                                         
        Turn();

        base.Update(_deltaTime);
    }

    public void Move()
    {
        float _speed = speed;                                   //Get a copy of the speed we can modify for running

        if (input.Keyboard.IsKeyDown(Keys.LeftShift))           //Check if the left-shift button is down
            _speed *= 2.5f;                                     //If it is, we multiply the speed by the runningmodifier

        Vector2 nextFramePosition = input.Keyboard.GetAxis(Keys.S, Keys.W) * forward +      //Add WASD movement
                                    input.Keyboard.GetAxis(Keys.A, Keys.D) * right;

        nextFramePosition *= deltaTime;                         //Multiply movement by delta 
        nextFramePosition *= _speed;                            //And speed

        EMath.Vector2i mapCheck = EMath.FloorVector(position + nextFramePosition);      

        if (MAP.cells[MAP.data[mapCheck.X, mapCheck.Y]].cellType != CellType.Wall)      //Check if collision is possible, by checking the future position and moving there if it is open
            position += nextFramePosition;
    }

    public void Turn()
    {
        direction += input.Mouse.XDelta * deltaTime * turnSpeedModifier;                               //Add mouse input to direction.
        
        if (input.Mouse.position.X >= 1915)                                                                                        
            input.Mouse.SetPosition(10, input.Mouse.position.Y);
        else if (input.Mouse.position.X <= 5)
            input.Mouse.SetPosition(1910, input.Mouse.position.Y);

        if (direction > 180)
            direction = -180;
        else if (direction < -180)
            direction = 180;
    }
    
}