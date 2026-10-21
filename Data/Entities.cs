using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.X86;
using Custom;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WolfenStein_like;

namespace RayCat.Data;

public class Entity
{
    public Vector3 position {get; set;}
    
    public float direction {get; set;}

    public float deltaTime;

    //Get transform directions
    Vector2 _forward {get; set;}
    public Vector2 forward;
    Vector2 _right{get; set;}
    public Vector2 right;

    public Vector2 offset {get; set;}
    public Vector2 screenPosition;
    public float angleToPlayer;

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
        DrawToScreen(_batch, player, WindowResolution, offset, spriteScale);
    }

    public void DrawToScreen(SpriteBatch _batch, Player player, Vector2i WindowResolution, Vector2 offset, Vector2 scale)
    {
        angleToPlayer = float.RadiansToDegrees((float)Math.Atan2(position.Y - player.position.Y, position.X - player.position.X));

        float xPosition = WindowResolution.X / 2 - (player.direction - angleToPlayer) * 21.5f;

        screenPosition = new Vector2(xPosition, WindowResolution.Y / 2);

        if (player.direction >= -180 && player.direction <= -90 &&
            angleToPlayer <= 180 && angleToPlayer >= 90)
            screenPosition.X -= 7740;

        if (player.direction <= 180 && player.direction >= 90 &&
            angleToPlayer >= -180 && angleToPlayer <= -90)
            screenPosition.X += 7740;    

        Draw(_batch,
            player,
            screenPosition + offset,
            spriteColor,
            scale / Vector3.Distance(player.position, position)
            );
    }
    #endregion

    #region Draw
    public void Draw(SpriteBatch _batch, Player player, Vector2 pos, Color color, Vector2 scale)
    {
        sprite.Draw(_batch, pos, color, 0, scale, Vector3.Distance(position, player.position) / 100);
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