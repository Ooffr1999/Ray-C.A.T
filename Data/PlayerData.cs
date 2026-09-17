using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Player
{
    public float height;
    public Vector2 position;
    public float direction;
    public Vector2 forward;
    public Vector2 right;

    public Sprite sprite;

    public Player(Sprite _sprite)
    {
        position = new Vector2(0, 0);
        direction = 0f;
        height = 0.5f;

        sprite = _sprite;
        forward = EMath.getDirection(MathHelper.ToRadians(direction));
        right = EMath.getDirection(MathHelper.ToRadians(direction + 90));
    }

    public Player()
    {
        position = new Vector2(0, 0);
        direction = 0f;

        forward = EMath.getDirection(MathHelper.ToRadians(direction));
    }

    public void Draw(SpriteBatch _batch, Vector2 pos, float dir, Vector2 scale)
    {
        sprite.Draw(_batch, pos, Color.White, dir, scale);
    }

    public void Draw(SpriteBatch _batch, Vector2 pos, Color color, float dir, Vector2 scale)
    {
        sprite.Draw(_batch, pos, color, dir, scale);
    }

    public void Update()
    {
        forward = EMath.getDirection(MathHelper.ToRadians(direction));
        right = EMath.getDirection(MathHelper.ToRadians(direction + 90));
    }
}