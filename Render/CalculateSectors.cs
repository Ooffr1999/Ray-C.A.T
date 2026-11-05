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

public class SectorBuilder
{
    List<Point> points = new List<Point>();
    SpriteBatch batch;
    Camera cam;

    VertexPositionColor[] vertexPositionColor;
    VertexBuffer vertexBuffer;

    public SectorBuilder(SpriteBatch _batch, Camera _cam)
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

    public void RenderSector(Sector sector)
    {
        RenderSector(GetVertexFromScreenVertices(sector.vertices), sector.height);
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

        short[] indices = new short[(sectorVertices.Length) * 6];

        for (int i = 0; i < indices.Length; i += 6)
        {
            indices[i] = (Int16)(0 + indicesIndex);
            indices[i + 1] = (Int16)(1 + indicesIndex);
            indices[i + 2] = (Int16)(2 + indicesIndex);
            indices[i + 3] = (Int16)(1 + indicesIndex);
            indices[i + 4] = (Int16)(3 + indicesIndex);
            indices[i + 5] = (Int16)(2 + indicesIndex);

            indicesIndex += 2;
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

    public static Vector3[] GetVertexFromScreenVertices(List<MapSectorVertice> _mapVertices)
    {
        Vector3[] vertices = new Vector3[_mapVertices.Count + 1]; 

        for (int i = 0; i < vertices.Length - 1; i++)
        {
            int index = i / 2;

            vertices[i] = new Vector3(_mapVertices[i].GetPosition().X, 0, _mapVertices[i].GetPosition().Y) / 10;
        }

        vertices[vertices.Length - 1] = vertices[0];

        return vertices;
    }
}

public struct Sector
{
    public List<MapSectorVertice> vertices;
    public float height;
    public BooleanType booleanType;
}

public enum BooleanType
{
    Union,
    Difference,

}