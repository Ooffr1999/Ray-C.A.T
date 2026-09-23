using System;
using System.Collections.Generic;
using Custom;
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
    public Color spriteColor;
    public Vector2 spriteScale;

    public Entity (GameServiceContainer service)
    {
        service.GetService<List<Entity>>().Add(this);

        UpdateDirections();
        spriteColor = Color.White;
        spriteScale = Vector2.One;
    }

    public virtual void Update(float _deltaTime)
    {
        deltaTime = _deltaTime;
        UpdateDirections();
    }

    #region DrawToScreen
    public void DrawToScreen(SpriteBatch _batch, Player player, Vector2i WindowResolution)
    {
        DrawToScreen(_batch, player, WindowResolution, Vector2.Zero, spriteScale);
    }

    public void DrawToScreen(SpriteBatch _batch, Player player, Vector2i WindowResolution, Vector2 offset, Vector2 scale)
    {
        float positionToScreenModifier = float.RadiansToDegrees((float)Math.Atan2(position.Y - player.position.Y, position.X - player.position.X));

        Draw(_batch,
            new Vector2(WindowResolution.X / 2 - (player.direction - positionToScreenModifier) * 21.5f, WindowResolution.Y / 2) + offset,
            spriteColor,
            scale / Vector2.Distance(player.position, position)
            );
    }
    #endregion

    #region Draw
    public void Draw(SpriteBatch _batch, Vector2 pos, Color color, Vector2 scale)
    {
        sprite.Draw(_batch, pos, color, 0, scale);
    }

    public void Draw(SpriteBatch _batch, Vector2 pos, Vector2 scale)
    {
        sprite.Draw(_batch, pos, Color.White, 0, scale);
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
    #endregion

    void UpdateDirections()
    {
        _forward = EMath.getDirection(MathHelper.ToRadians(direction));
        _right = EMath.getDirection(MathHelper.ToRadians(direction + 90));

        forward = _forward;
        right = _right;
    }
}