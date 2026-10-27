using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class Camera
{
    //Movement
    Vector3 position {get; set;}
    Vector3 forward;
    Vector3 right;
    float movementSpeed;

    //Rotation
    float rotation {get; set;}
    float turnSpeed;

    //Camera settings
    Matrix viewMatrix;

    float fieldOfView;
    float nearPlaneDistance;
    float farPlaneDistance;

    public Camera()
    {
        position = new Vector3(0, 1.5f, 0);

        fieldOfView = 90f;
        nearPlaneDistance = 0.1f;
        farPlaneDistance = 100;

        movementSpeed = 5f;
        turnSpeed = 10;
    }

    public void Update(InputManager _input, float deltaTime)
    {
        Move(_input, deltaTime);
        Turn(_input, deltaTime);
    }

    public Matrix GetViewMatrix()
    {
        //float yaw = float.DegreesToRadians(rotation);

        viewMatrix = Matrix.CreateLookAt(position, getCameraTarget(), new Vector3(0, 1, 0));

        return viewMatrix;
    }

    public Matrix getPerspectiveMatrix()
    {
        return Matrix.CreatePerspectiveFieldOfView(float.DegreesToRadians(fieldOfView), 16 / 9, nearPlaneDistance, farPlaneDistance);
    }

    public Matrix getWorldMatrix()
    {
        return Matrix.CreateWorld(getCameraTarget(), getForward(), Vector3.Up);
    }

    public void setFOV(float _fieldOfView)
    {
        fieldOfView = _fieldOfView;
    }

    public void setNearPlaneDistance(float _nearPlaneDistance)
    {
        nearPlaneDistance = _nearPlaneDistance;
    }

    public void setFarPlaneDistance(float _farPlaneDistance)
    {
        farPlaneDistance = _farPlaneDistance;
    }

    public void Move(InputManager _input, float _deltaTime)
    {
        position -= _input.Keyboard.GetAxis(Keys.S, Keys.W) * (getForward() * _deltaTime * movementSpeed);
        position -= _input.Keyboard.GetAxis(Keys.A, Keys.D) * (getRight() * _deltaTime * movementSpeed);

        position += _input.Keyboard.GetAxis(Keys.LeftShift, Keys.Space) * Vector3.Up * _deltaTime;
    }

    public Vector3 getForward()
    {
        Quaternion cameraRotation = Quaternion.CreateFromYawPitchRoll(getRotationInRadians(), 0, 0);
        Vector3 rotatedForward = Vector3.Transform(new Vector3(0, 0, 1), cameraRotation);
        rotatedForward = Vector3.Normalize(rotatedForward);

        forward = rotatedForward;

        return rotatedForward;
    }

    Vector3 getRight()
    {
        Vector3 cameraRight = Vector3.Cross(getForward(), new Vector3(0, 1, 0));
        cameraRight = Vector3.Normalize(cameraRight);

        right = cameraRight;

        return cameraRight;
    }

    public void setPosition(Vector3 _position)
    {
        position = _position;
    }

    public Vector3 getPosition()
    {
        return position;
    }

    public void Turn(InputManager _input, float _deltaTime)
    {
        rotation -= _input.Mouse.XDelta * _deltaTime * turnSpeed;                               //Add mouse input to direction.

        if (_input.Mouse.position.X >= 1915)                                                                                        
            _input.Mouse.SetPosition(10, _input.Mouse.position.Y);
        else if (_input.Mouse.position.X <= 5)
            _input.Mouse.SetPosition(1910, _input.Mouse.position.Y);
        
        if (rotation > 180)
            rotation = -180;
        else if (rotation < -180)
            rotation = 180;
    }

    public void setRotationDegrees(float _rotation)
    {
        rotation = _rotation;
    }

    public void setRotationRadians(float _rotation)
    {
        rotation = float.DegreesToRadians(_rotation);
    }

    public float getRotationInRadians()
    {
        return float.DegreesToRadians(rotation);
    }

    public float getRotationInDegrees()
    {
        return rotation;
    }

    public Vector3 getCameraTarget()
    {
        return position - getForward();
    }
}