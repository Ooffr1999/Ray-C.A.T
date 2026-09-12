using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WolfenStein_like;

public class Game1 : Game
{
    public static GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    EMath.Vector2i MAP_RESOLUTION = new EMath.Vector2i(8, 8);
    
    EMath.Vector2i GAME_RESOLUTUION = new EMath.Vector2i(640, 480);
    EMath.Vector2i SCREEN_RESOLUTION = new EMath.Vector2i(1920, 1080);

    static int[] MAP = 
    {
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 0, 2, 0, 0, 0, 0, 1,
        1, 2, 0, 0, 0, 0, 0, 1,
        1, 0, 0, 0, 0, 0, 0, 1,
        1, 0, 0, 0, 0, 0, 0, 1,
        1, 2, 0, 2, 0, 1, 0, 1,
        1, 0, 0, 0, 0, 0, 0, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
    };

    const int FOV = 90;
    const int CELLSIZE = 32;
    const float PLAYER_SPEED = 20;
    const float PLAYER_ROT_SPEED = 70;

    Sprite player;

    float height = 0.5f;

    Vector2 pos = new Vector2(1.5f, 1.5f);
    Vector2 lastPos = new Vector2(0, 0);

    float dir = 90;
    Vector2 forward = Vector2.Zero;
    Vector2 right = Vector2.Zero;
    
    bool showMap = true;

    float accumulatedDeltaTime = 0;
    int FPS = 0;

    Ray.Raydata[] rayData;
    Ray.LineData[] lineData;

    InputManager input;

    Texture2D wall;
    Texture2D brickWall;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        Window.Title = "Teo...";
        Window.AllowUserResizing = true;

        _graphics.PreferredBackBufferWidth = SCREEN_RESOLUTION.X;
        _graphics.PreferredBackBufferHeight = SCREEN_RESOLUTION.Y;
        //_graphics.IsFullScreen = true;
        
        _graphics.ApplyChanges();

        rayData = new Ray.Raydata[GAME_RESOLUTUION.X];
        lineData = new Ray.LineData[GAME_RESOLUTUION.X];

        input = new InputManager();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        player = new Sprite(AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/teodor.jpg"));
        wall = AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/redbrick.png");
        brickWall = AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/redbrick.png");
    }

    protected override void Update(GameTime gameTime)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        input.Update(gameTime);
        input.Keyboard.Update();
        
        //GetFPS(delta);
        
        //Get forward vector based on rotation
        forward = EMath.getDirection(MathHelper.ToRadians(dir));
        right = EMath.getDirection(MathHelper.ToRadians(dir + 90));
        
        #region Calculate Raycast
        float startCastDir = dir - FOV / 2;
        double FOVincrement = (float)FOV / (float)GAME_RESOLUTUION.X;

        for (int r = 0; r < GAME_RESOLUTUION.X; r++)
        {
            #region Calculate Line size
            rayData[r] = Ray.Cast(pos, startCastDir + FOVincrement * r, MAP, MAP_RESOLUTION);

            lineData[r].lineWidth = SCREEN_RESOLUTION.X / GAME_RESOLUTUION.X;
            lineData[r].lineHeight = SCREEN_RESOLUTION.Y / rayData[r].distance;

            lineData[r].drawStart = -lineData[r].lineHeight / 2 + SCREEN_RESOLUTION.Y / 2;
            lineData[r].drawEnd = lineData[r].lineHeight / 2 + SCREEN_RESOLUTION.Y / 2;
            #endregion

            #region Calculate Texture position
            double wallX;
            
            if (rayData[r].hitSide == 0)
                wallX = pos.Y + rayData[r].distance * rayData[r].direction.Y;
            else wallX = pos.X + rayData[r].distance * rayData[r].direction.X;
            wallX -= Math.Floor(wallX);

            lineData[r].texX = (int)(wallX * (double)wall.Width);
            if (rayData[r].hitSide == 0 && rayData[r].direction.X > 0)
                lineData[r].texX = wall.Width - lineData[r].texX - 1;
            if (rayData[r].hitSide == 1 && rayData[r].direction.Y < 0)
                lineData[r].texX = wall.Width - lineData[r].texX - 1;
            
            #endregion

            switch (rayData[r].hitSide)
            {
                case 0:
                    lineData[r].color = Color.LightBlue;
                    break;
                case 1: 
                    lineData[r].color = Color.Blue;
                    break;
            }
        }
        #endregion
        
        #region Keyboard
        KeyboardState state = Keyboard.GetState();

        if (input.Keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
            Environment.Exit(0);
        }

        float speed = PLAYER_SPEED;

        if (input.Keyboard.IsKeyDown(Keys.LeftShift))
            speed *= 2.5f;

        Vector2 nextFramePosition = input.Keyboard.GetAxis(Keys.S, Keys.W) * forward * delta * speed / CELLSIZE +
                                    input.Keyboard.GetAxis(Keys.A, Keys.D) * right * delta * speed / CELLSIZE;

        EMath.Vector2i mapCheck = EMath.FloorVector(pos + nextFramePosition);
        
        if (MAP[MAP_RESOLUTION.Y * mapCheck.Y + mapCheck.X] != 1)
            pos += nextFramePosition;

        if (input.Keyboard.IsButtonJustPressed(Keys.Tab))
            showMap = !showMap;

        #region Mouse
        dir += input.Mouse.XDelta * delta * 10;
        
        if (input.Mouse.position.X < 20)
            input.Mouse.SetPosition(SCREEN_RESOLUTION.X - 20, input.Mouse.position.Y);
        if (input.Mouse.position.X >= SCREEN_RESOLUTION.X - 5)
            input.Mouse.SetPosition(5, input.Mouse.position.Y);

        if (dir > 360)
            dir = 0;
        else if (dir < 0)
            dir = 360;
        #endregion
        #endregion

        lastPos = pos;

        //base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap);

        #region Floorcast attempt DELETE LATER OR MOVE TO UPDATE

        CameraPlane plane = new CameraPlane(pos, dir, -0.20f, FOV);

        int mult = 3;
        int floorWidth = SCREEN_RESOLUTION.X / mult;
        int floorHeight = (SCREEN_RESOLUTION.Y) / mult;

        int halfHeight = floorHeight / 2;
        
        for (int y = 0; y < halfHeight; y++)
        {
            int p = y - halfHeight;
            float posZ = height * halfHeight;
            float rowDistance = -posZ / p;;

            Vector2 floor = plane.XlerpValueAcrossPlane(0);
            
            //floor.X += rowDistance * forward.X / floorWidth;
            //floor.X += rowDistance * plane.XlerpValueAcrossPlane((1f / halfHeight) * y).X - MathHelper.Lerp(-rowDistance * 0.5f, rowDistance * 0.5f, (1f / halfHeight) * y);
            //floor.Y += rowDistance * plane.XlerpValueAcrossPlane((1f / floorWidth) * y).Y - MathHelper.Lerp(-rowDistance * 0.5f, rowDistance * 0.5f, (1f / halfHeight) * y);
            floor.Y += rowDistance;
            //floor.Y /= floorWidth;
            for (int x = 0; x < floorWidth; x++)
            {   
                float widthLerped = (1f / floorWidth) * x;

                //floor.X += rowDistance * plane.XlerpValueAcrossPlane((1f / floorWidth) * x).X - MathHelper.Lerp(-rowDistance * 0.5f, rowDistance * 0.5f, (1f / floorWidth) * x);
                //floor.Y += rowDistance * plane.XlerpValueAcrossPlane((1f / floorWidth) * x).Y - MathHelper.Lerp(-rowDistance * 0.5f, rowDistance * 0.5f, (1f / floorWidth) * x);
            
                //floor.X /= floorWidth;
                

                floor.X = (plane.XlerpValueAcrossPlane(widthLerped).X - 
                          MathHelper.Lerp(-rowDistance, rowDistance, widthLerped)) * forward.Y;
                //floor.Y = plane.XlerpValueAcrossPlane(widthLerped).Y - 
                //          MathHelper.Lerp(-rowDistance, rowDistance, widthLerped) * -forward.X;
                
               
                
                Color color = Color.Green;

                if(floor.Y > -1 && 
                   floor.Y < MAP_RESOLUTION.Y && 
                   floor.X > -1 &&
                   floor.X < MAP_RESOLUTION.X)
                {
                    switch (MAP[MAP_RESOLUTION.Y * (int)floor.Y + (int)floor.X])
                    {
                        case 0:
                            color = Color.Green;
                            break;
                        case 1: 
                            color = Color.Blue;
                            break;
                        case 2:
                            color = Color.Red;
                            break;
                        default:
                            color = Color.Azure;
                            break;
                    }

                    Primitives.DrawBox(_spriteBatch, 
                                        new Vector2(x, floorHeight - y) * mult,
                                        color, Vector2.One * mult);
                }
                
            }
        }

        #endregion
        /*
        #region Draw walls
        for (int r = 0; r < GAME_RESOLUTUION.X; r++)
        {
            Color color = (Color.White * 0.5f) * (1 / rayData[r].distance);
            color.A = 255;

            Primitives.DrawTexturedLine(_spriteBatch, wall, (int)lineData[r].texX, 0,
                                new Vector2(lineData[r].lineWidth * r, lineData[r].drawStart), 
                                new Vector2(lineData[r].lineWidth * r, lineData[r].drawEnd), 
                                lineData[r].lineWidth, 
                                color,
                                0);
                                
        }
        
        #endregion
        */
        #region Show Minimap
        
        if (!showMap)
        {
            _spriteBatch.End();

            base.Draw(gameTime);
            return;
        }

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
        
        /*
        for (int r = 0; r < GAME_RESOLUTUION.X; r++)
        {
            Primitives.DrawLine(_spriteBatch, pos * CELLSIZE, (pos + rayData[r].hitPosition) * CELLSIZE, 1f, Color.Red);
        }
        */
        //Draw player and camera plane
        player.Draw(_spriteBatch, pos * CELLSIZE, Color.White, dir, Vector2.One / 10);
        Primitives.DrawLine(_spriteBatch, plane.leftPosition * CELLSIZE, plane.rightPosition * CELLSIZE, 2, Color.Violet);
        #endregion

        _spriteBatch.End();
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

    Vector2 GetScreenPosition(float x, float y)
    {
        float _x = x * 0.5f;
        float _y = y * 0.5f;

        return new Vector2(Math.Abs(-_x / (SCREEN_RESOLUTION.X / 2)),
                           Math.Abs(_y / (SCREEN_RESOLUTION.Y / 2) - 1));
    }
}