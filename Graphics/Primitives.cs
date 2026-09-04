using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class Primitives{

    public static void DrawBox(SpriteBatch batch, Vector2 location, Color color, float rotation, Vector2 scale, float layer)
    {
        Texture2D texture = getEmptyTexture(batch);

        Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

        batch.Draw(texture, location, null, color, rotation, origin, scale, SpriteEffects.None, layer);
    }

    public static void DrawBox(SpriteBatch batch, Vector2 location, Color color, Vector2 scale)
    {
        DrawBox(batch, location, color, 0.0f, scale, 0.0f);
    }

    public static void DrawLine(SpriteBatch batch, Vector2 start, Vector2 end, float thickness, Color color, float layer)
    {
        Texture2D texture = getEmptyTexture(batch);

        float distance = Vector2.Distance(start, end);
        Vector2 direction = end - start;
        float angle = (float)Math.Atan2(direction.Y, direction.X);

        batch.Draw(
            texture, 
            start, 
            null, 
            color, 
            angle, 
            Vector2.Zero, 
            new Vector2(distance, thickness), 
            SpriteEffects.None, 
            layer
        );
    }

    public static void DrawLine(SpriteBatch batch, Vector2 start, Vector2 end, Color color, float layer)
    {
        DrawLine(batch, start, end, 1, color, layer);
    }

    public static void DrawLine(SpriteBatch batch, Vector2 start, Vector2 end, float thickness, Color color)
    {
        DrawLine(batch, start, end, thickness, color, 0.0f);
    }

    public static void DrawLine(SpriteBatch batch, Vector2 start, Vector2 end, Color color)
    {
        DrawLine(batch, start, end, 1, color, 0.0f);
    }

    static Texture2D getEmptyTexture(SpriteBatch batch)
    {
         Texture2D _pixel = new Texture2D(batch.GraphicsDevice, 1, 1);
        _pixel.SetData(new[] {Color.White});
        
        return _pixel;
    }
}