using System;
using System.Collections.Generic;
using System.Security.Principal;
using Custom;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Sector
{
    List<Point> points = new List<Point>();
    SpriteBatch batch;

    Vector3 campos;

    public Sector(SpriteBatch _batch)
    {
        //VIKTIG: Jeg tar notater under arbeidsprosessen fordi det at JEG SPESSIFIKT skjønner dette er jævla viktig
        /*

        *-------------*
        |             |
        |             |
        |             |
        |             |
        |             | 
        *-------------*

        Funksjonen vi skriver tar fire punkter.
        Disse punktene er sortert slik at annethvert punkt er vertikalt med punktet som kom før.

        Dvs at firkanten over blir slik

        0-------------2
        |             |
        |             |
        |             |
        |             |
        |             | 
        1-------------3

        1 må alltid være vertikal med 0 osv.
        Dermed kan man tegne linjene mellom 0 og 2 veldig lett ved å bare ta i + 2 og få neste.
        Dette er jeg for meg vil forenkle denne prosessen og derfor må jeg prøve å ikke glemme at det er slik
        Points skal settes opp.

        Når det kommer til å fylle inn denne jævelen så tar den distansen mellom de to vertikale linjene.
        , så distansen mellom senteret på de vertikale linjene og tegner linjer som interpolerer.

        Fordi vi vet hva resolusjonen er, vet vi også bredden på hver linje. 

        I lengden ser jeg for meg at det skal gå ann å dele opp 
        */
        campos = new Vector3(0, 0, 0.1f);
        batch = _batch;
    }

    public void DrawSquare(SpriteBatch _batch)
    {
        float indexMax = points[2].X - points[0].X;

        for (int i = 0; i < indexMax; i++)
        {
            Primitives.DrawLine(batch,
                                Vector2.Lerp(points[0].ToVector2(), points[2].ToVector2(), (1 / indexMax) * i),
                                Vector2.Lerp(points[1].ToVector2(), points[3].ToVector2(), (1 / indexMax) * i),
                                Color.White);
        }    
    }

    public void DrawBox(SpriteBatch _spriteBatch, Player player, Vector3 playerPosition, Vector2 WindowResolution, InputManager input)
    {
        Matrix test = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 4f, 16 / 9, 0.1f, 300);

        Vector3 cameraUpVector = Vector3.Up;

        float yaw = float.DegreesToRadians(-player.direction);

        Quaternion cameraRotation = Quaternion.CreateFromYawPitchRoll(yaw, 0, 0);
        Vector3 rotatedForward = Vector3.Transform(new Vector3(0, 0, 1), cameraRotation);
        rotatedForward = Vector3.Normalize(rotatedForward);
        
        Vector3 cameraTarget = campos - rotatedForward;

        Vector3 cameraRight = Vector3.Cross(rotatedForward, cameraUpVector);
        cameraRight = Vector3.Normalize(cameraRight);

        //player.position += new Vector3(player.forward.X, 0, player.forward.Y) * input.Keyboard.GetAxis(Keys.Down, Keys.Up);
        campos += input.Keyboard.GetAxis(Keys.Left, Keys.Right) * cameraRight / 50;
        campos += input.Keyboard.GetAxis(Keys.Down, Keys.Up) * rotatedForward / 50;

        Matrix viewMatrix = Matrix.CreateLookAt(campos, cameraTarget, cameraUpVector);
        
        Vector3[] points = new Vector3[]
        {
            new Vector3(0, -1, 5),
            new Vector3(1, -1, 5),
            new Vector3(0, -1, 6),
            new Vector3(1, -1, 6), 
            new Vector3(0, 0, 5),
            new Vector3(1, 0, 5),
            new Vector3(0, 0, 6),
            new Vector3(1, 0, 6), 
        };
        
        Vector3[] screenPoints = new Vector3[points.Length];

        for (int i = 0; i < screenPoints.Length; i++)
        {
            screenPoints[i] = _spriteBatch.GraphicsDevice.Viewport.Project(points[i], test, viewMatrix, Matrix.Identity);

            if (screenPoints[i].Z > 0 && screenPoints[i].Z < 1f)
            {
                Primitives.DrawBox(_spriteBatch, new Vector2(screenPoints[i].X, screenPoints[i].Y), Color.Red, Vector2.One * 10);
            }
        };

        Console.WriteLine(screenPoints[0]);
        //Primitives.DrawBox(_spriteBatch, screenPoints[1], Color.Green, Vector2.One * 20);
        /*
        Primitives.DrawLine(_spriteBatch, WindowResolution / 2 + screenPoints[0], WindowResolution / 2 +  screenPoints[1], Color.Red);
        Primitives.DrawLine(_spriteBatch, WindowResolution / 2 + screenPoints[2], WindowResolution / 2 + screenPoints[3], Color.Red);
        Primitives.DrawLine(_spriteBatch, WindowResolution / 2 + screenPoints[0], WindowResolution / 2 + screenPoints[2], Color.Red);
        Primitives.DrawLine(_spriteBatch, WindowResolution / 2 + screenPoints[1], WindowResolution / 2 + screenPoints[3], Color.Red);

        Primitives.DrawLine(_spriteBatch, WindowResolution / 2 + screenPoints[4], WindowResolution / 2 +  screenPoints[5], Color.Green);
        Primitives.DrawLine(_spriteBatch, WindowResolution / 2 + screenPoints[6], WindowResolution / 2 + screenPoints[7], Color.Green);
        Primitives.DrawLine(_spriteBatch, WindowResolution / 2 + screenPoints[4], WindowResolution / 2 + screenPoints[6], Color.Green);
        Primitives.DrawLine(_spriteBatch, WindowResolution / 2 + screenPoints[5], WindowResolution / 2 + screenPoints[7], Color.Green);
        */
    }

    public float GetPlayerAngleToPoint(Player player, Point point)
    {
        //float angle = float.RadiansToDegrees((float)Math.Atan2(point.Y - player.position.Y, point.X - player.position.X));
        //return angle + player.direction;
        
        Vector2 targetDirection = point.ToVector2() - new Vector2(player.position.X, player.position.Z);
        targetDirection.Normalize();

        Vector2 lookDirection = player.forward;
        lookDirection.Normalize();

        float dot = lookDirection.X * targetDirection.X + lookDirection.Y * targetDirection.Y;
        float cross = lookDirection.X * targetDirection.Y - lookDirection.Y * targetDirection.X;

        return float.RadiansToDegrees((float)Math.Atan2(cross, dot));
    }

    public bool IsPointInView(Point point, Vector2i WindowResolution)
    {
        if (point.X < 0 || point.X > WindowResolution.X)
            return false;
        
        return true;
    }


    public void DrawPointToScreen(SpriteBatch _batch, Player player, Vector2 location, Vector2i WindowResolution)
    {
        DrawPointToScreen(_batch, player, location, WindowResolution, Vector2.Zero, Vector2.One);
    }

    public void DrawPointToScreen(SpriteBatch _batch, Player player, Vector2 location, Vector2i WindowResolution, Vector2 offset, Vector2 scale)
    {
        Color shade = Color.White * 1f * (1 / Vector2.Distance(new Vector2(player.position.X, player.position.Z), location));
        shade.A = 255;

        Primitives.DrawBox(_batch,
            GetPointToScreenPosition(player, location, WindowResolution).ToVector2() + offset,
            shade,
            scale
            );
    }

    public static Point GetPointToScreenPosition(Player player, Vector2 location, Vector2i WindowResolution)
    {
        float angleToPlayer = float.RadiansToDegrees((float)Math.Atan2(location.Y - player.position.Y, location.X - player.position.X));
        float xPosition = WindowResolution.X / 2 - (player.direction - angleToPlayer) * 21.5f;
        Vector2 screenPosition = new Vector2(xPosition, WindowResolution.Y / 2);

        /*
        if (player.direction >= -180 && player.direction <= -90 &&
            angleToPlayer <= 180 && angleToPlayer >= 90)
            screenPosition.X -= 7740;

        if (player.direction <= 180 && player.direction >= 90 &&
            angleToPlayer >= -180 && angleToPlayer <= -90)
            screenPosition.X += 7740;    
        */
        return new Point((int)screenPosition.X, (int)screenPosition.Y);
    }

    public static bool IsVectorBehindPlayer(Player player, Vector2 location)
    {
        Vector2 direction = location - new Vector2(player.position.X, player.position.Z);
        direction.Normalize();

        if (Vector2.Dot(player.forward, direction) < 0)
            return true;
        return false;
    }

    void DrawLine(int _start, int _end)
    {
        Primitives.DrawLine(batch, points[_start].ToVector2(), points[_end].ToVector2(), Color.White);
    }

    void DrawLine(int _start, int _end, Color _color)
    {
        Primitives.DrawLine(batch, points[_start].ToVector2(), points[_end].ToVector2(), _color);
    }
}