using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WolfenStein_like;

namespace RayCat.Data;

public class Entity
{
    public Vector2 position {get; set;}
    
    public float direction {get; set;}

    public float deltaTime;

    //Get transform directions
    Vector2 _forward {get; set;}
    public Vector2 forward;
    Vector2 _right{get; set;}
    public Vector2 right;

    //Draw
    public Sprite sprite;

    public Entity (GameServiceContainer service)
    {
        position = Vector2.Zero;
        direction = 0;

        service.GetService<List<Entity>>().Add(this);

        UpdateDirections();
    }

    public virtual void Update(float _deltaTime)
    {
        deltaTime = _deltaTime;
        UpdateDirections();
    }

    public void Draw(SpriteBatch _batch, Vector2 pos, float dir, Vector2 scale)
    {
        sprite.Draw(_batch, pos, Color.White, dir, scale);
    }

    public void Draw(SpriteBatch _batch, Vector2 pos, Color color, float dir, Vector2 scale)
    {
        if (sprite != null)
            sprite.Draw(_batch, pos, color, dir, scale);
    }

    void UpdateDirections()
    {
        _forward = EMath.getDirection(MathHelper.ToRadians(direction));
        _right = EMath.getDirection(MathHelper.ToRadians(direction + 90));

        forward = _forward;
        right = _right;
    }
}