using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TextureRegion
{
    public Texture2D texture {get; set;}

    public Microsoft.Xna.Framework.Rectangle sourceRectangle {get; set;}

    public int Width => sourceRectangle.Width;

    public int Height => sourceRectangle.Height;

    public TextureRegion() {}

    public TextureRegion(Texture2D tex, int x, int y, int width, int height)
    {
        texture = tex;
        sourceRectangle = new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
    }

    public void Draw(SpriteBatch batch, Vector2 position, Color color)
    {
        Draw(batch, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
    }

    public void Draw(SpriteBatch batch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects)
    {
        Draw(batch,
            position,
            color,
            rotation,
            origin,
            scale,
            effects,
            0.0f);
    }

    public void Draw(SpriteBatch batch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layer)
    {
        batch.Draw(
            texture,
            position,
            sourceRectangle,
            color,
            rotation,
            origin,
            scale,
            effects,
            layer
        );
    }
}