using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Xml.Linq;
using Custom;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Sector
{
    List<Point> points = new List<Point>();
    SpriteBatch batch;
    Camera cam;

    VertexPositionColor[] vertexPositionColor;
    VertexBuffer vertexBuffer;

    public Sector(SpriteBatch _batch, Camera _cam)
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
        batch = _batch;
        cam = _cam;
    }

    public void RenderSector(Vector3[] sectorVertices, float height)
    {
        RenderSector(sectorVertices, height, Vector3.Zero);
    }

    public void RenderSector(Vector3[] sectorVertices, float height, Vector3 offset)
    {
        //Testing the rendering and creation of 3d stuff using basiceffect
        //Set up Basic effect

        BasicEffect basicEffect = new BasicEffect(batch.GraphicsDevice);
        basicEffect.World = Matrix.Identity;
        basicEffect.View = cam.GetViewMatrix();
        basicEffect.Projection = cam.getPerspectiveMatrix();

        basicEffect.Alpha = 1.0f;
        basicEffect.VertexColorEnabled = true;
        basicEffect.LightingEnabled = false;

        //Create triangle
        vertexPositionColor = new VertexPositionColor[sectorVertices.Length * 2];

        for (int i = 0; i < vertexPositionColor.Length;)
        {
            vertexPositionColor[i].Position = sectorVertices[i / 2] + offset;
            vertexPositionColor[i].Color = Color.Red;
            vertexPositionColor[i +1].Position = sectorVertices[i / 2] + Vector3.Up * height + offset;
            vertexPositionColor[i +1].Color = Color.Green;

            i += 2;
        }

        short indicesIndex = 0;
        short[] indices = new short[(sectorVertices.Length - 1) * 6];

        for (int i = 0; i < indices.Length;)
        {
            indices[i] = (Int16)(0 + indicesIndex);
            indices[i + 1] = (Int16)(1 + indicesIndex);
            indices[i + 2] = (Int16)(2 + indicesIndex);
            indices[i + 3] = (Int16)(1 + indicesIndex);
            indices[i + 4] = (Int16)(3 + indicesIndex);
            indices[i + 5] = (Int16)(2 + indicesIndex);

            indicesIndex += 2;
            i += 6;
        }

        vertexBuffer = new VertexBuffer(batch.GraphicsDevice, typeof(VertexPositionColor), vertexPositionColor.Length, BufferUsage.WriteOnly);
        vertexBuffer.SetData<VertexPositionColor>(vertexPositionColor);

        IndexBuffer indexBuffer = new IndexBuffer(batch.GraphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.WriteOnly);
        indexBuffer.SetData(indices);

        batch.GraphicsDevice.SetVertexBuffer(vertexBuffer);
        batch.GraphicsDevice.Indices = indexBuffer;

        RasterizerState rasterizer = new RasterizerState();
        rasterizer.CullMode = CullMode.None;

        batch.GraphicsDevice.RasterizerState = rasterizer;

        foreach(EffectPass pass in basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            batch.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, (sectorVertices.Length - 1) * 2);
        }
    }

    public Vector3 ProjectVertexToScreenPosition(Vector3 vertex, Camera camera)
    {
        return batch.GraphicsDevice.Viewport.Project(vertex, camera.getPerspectiveMatrix(), camera.GetViewMatrix(), Matrix.Identity);
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
}