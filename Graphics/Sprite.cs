using System.Threading.Tasks.Dataflow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Sprite
{
    public Texture2D texture  {get; set;}
    public SpriteEffects spriteEffects {get; set;}

    public Sprite() { }

    public Sprite(Texture2D Texture)
    {
        texture = Texture;
    }

    public void Draw(SpriteBatch batch, Vector2 location, Color color)
    {
        batch.Draw(texture, location, color);
    }

    public void Draw(SpriteBatch batch, Vector2 location, Color color, float rotation, Vector2 scale)
    {
        Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
        batch.Draw(texture, location, null, color, MathHelper.ToRadians(rotation), origin, scale, spriteEffects, 0.0f);
    }

    public void Draw(SpriteBatch batch, Vector2 location, Color color, float rotation, Vector2 scale, float layer)
    {
        Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
        batch.Draw(texture, location, null, color, MathHelper.ToRadians(rotation), origin, scale, spriteEffects, layer);
    }

    public void DrawEmpty(SpriteBatch batch, Vector2 location, Color color, float rotation, Vector2 scale)
    {
        Texture2D _pixel = new Texture2D(batch.GraphicsDevice, 1, 1);
        _pixel.SetData(new[] {Color.White});
        texture = _pixel;

        Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

        batch.Draw(texture, location, null, color, rotation, origin, scale, spriteEffects, 0.0f);
    }

    public Texture2D getEmptyTexture(SpriteBatch batch)
    {
         Texture2D _pixel = new Texture2D(batch.GraphicsDevice, 1, 1);
        _pixel.SetData(new[] {Color.White});
        
        return _pixel;
    }
}