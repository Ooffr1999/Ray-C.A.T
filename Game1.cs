using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WolfenStein_like;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    EMath.Vector2i MAP_RESOLUTION = new EMath.Vector2i(8, 8);
    
    EMath.Vector2i GAME_RESOLUTUION = new EMath.Vector2i(512, 270);
    EMath.Vector2i SCREEN_RESOLUTION = new EMath.Vector2i(960, 540);

    static int[] MAP = 
    {
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 0, 1, 0, 0, 0, 0, 1,
        1, 0, 1, 0, 0, 0, 0, 1,
        1, 0, 1, 0, 0, 0, 0, 1,
        1, 0, 0, 0, 0, 0, 0, 1,
        1, 0, 0, 0, 0, 1, 0, 1,
        1, 0, 0, 0, 0, 0, 0, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
    };

    const int FOV = 90;
    const int CELLSIZE = 32;
    const float PLAYER_SPEED = 20;
    const float PLAYER_ROT_SPEED = 70;

    Sprite player;
    Vector2 pos = new Vector2(1.5f, 1.5f);

    float dir = 0;
    Vector2 forward = Vector2.Zero;
    
    Vector2 screenBounds;

    float accumulatedDeltaTime = 0;
    int FPS = 0;

    Ray.Raydata[] rayData;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        Window.Title = "Coco 2: Electric boogaloo";
        Window.AllowUserResizing = true;

        
        _graphics.PreferredBackBufferWidth = SCREEN_RESOLUTION.X;
        _graphics.PreferredBackBufferHeight = SCREEN_RESOLUTION.Y;
        //_graphics.IsFullScreen = true;

        _graphics.ApplyChanges();

        rayData = new Ray.Raydata[GAME_RESOLUTUION.X];

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        player = new Sprite(Content.Load<Texture2D>("textures/teodor.jpg"));

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        GetFPS(delta);
        
        //Get forward vector based on rotation
        forward = EMath.getDirection(MathHelper.ToRadians(dir));

        //Get size of the screen
        screenBounds = 
            new Vector2(
                            GraphicsDevice.PresentationParameters.BackBufferWidth,
                            GraphicsDevice.PresentationParameters.BackBufferHeight);

        
        float startCastDir = dir - FOV / 2;
        double FOVincrement = (float)FOV / (float)GAME_RESOLUTUION.X;

        for (int r = 0; r < GAME_RESOLUTUION.X; r++)
            rayData[r] = Ray.Cast(pos, startCastDir + FOVincrement * r, MAP, MAP_RESOLUTION);
        
        #region Keyboard
        KeyboardState state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.Escape))
            Exit();

        if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up))
            pos += forward * delta * PLAYER_SPEED / CELLSIZE;



        MouseInput minput = new MouseInput();

        if (minput.wasMoved)
            dir += minput.XDelta * delta;

        if (dir > 360)
            dir = 0;
        else if (dir < 0)
            dir = 360;
        #endregion

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        /*

        for (int y = 0; y < MAP_RESOLUTION.Y; y++)
        {
            for (int x = 0; x < MAP_RESOLUTION.X; x++)
            {
                Color col = new Color();
                col = Color.White;

                switch(MAP[y * MAP_RESOLUTION.Y + x])
                {
                    case 0: 
                        col = Color.White;
                        break;
                    case 1: 
                        col = Color.Aquamarine;
                        break;

                    default: 
                        col = Color.Beige;
                        break;
                }
                Primitives.DrawBox(_spriteBatch, (new Vector2(x, y) * CELLSIZE), col, Vector2.One * (CELLSIZE - 1));
            }
        }

        player.Draw(_spriteBatch, pos * CELLSIZE, Color.White, dir, Vector2.One / 10);

        for (int r = 0; r < GAME_RESOLUTUION.X; r++)
            Primitives.DrawLine(_spriteBatch, pos * CELLSIZE, (pos + rayData[r].hitPosition) * CELLSIZE, 1f, Color.Red);
        */
        
        float lineWidth = screenBounds.X / GAME_RESOLUTUION.X;

        for (int r = 0; r < GAME_RESOLUTUION.X; r++)
        {
            float lineHeight = screenBounds.Y / rayData[r].distance;

            float drawStart = -lineHeight / 2 + screenBounds.Y / 2;
            if (drawStart < 0)
                drawStart = 0;
            float drawEnd = lineHeight / 2 + screenBounds.Y / 2;
            if (drawEnd > screenBounds.Y)
                drawEnd = screenBounds.Y;

            Color lineColor = Color.White;
            if (rayData[r].hitSide == 1)
                lineColor = Color.Gray;

            Primitives.DrawLine(_spriteBatch, 
                                new Vector2(lineWidth * r, drawStart), 
                                new Vector2(lineWidth * r, drawEnd), 
                                lineWidth, 
                                lineColor);
        }
        

        _spriteBatch.End();

        Console.Clear();

        base.Draw(gameTime);
    }

    void GetFPS(float delta)
    {
        accumulatedDeltaTime += delta;
        FPS += 1;

        if (accumulatedDeltaTime >= 1)
        {
            Console.WriteLine(FPS);
            accumulatedDeltaTime = 0;
            FPS = 0;
        }
    }
}
