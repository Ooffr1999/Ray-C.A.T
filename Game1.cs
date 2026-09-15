using System;
using System.ComponentModel.DataAnnotations;
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
    
    EMath.Vector2i GAME_RESOLUTUION = new EMath.Vector2i(320, 240);
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
    Texture2D teo;

    //TEMP VALUES REMOVE LATER
    int[,] floorCheckTest;
    Vector2[,] floorTexturePosition;
    float[,] floorPointDistance;

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

        floorCheckTest = new int[GAME_RESOLUTUION.X, GAME_RESOLUTUION.Y];
        floorPointDistance = new float[GAME_RESOLUTUION.X, GAME_RESOLUTUION.Y];
        floorTexturePosition = new Vector2[GAME_RESOLUTUION.X, GAME_RESOLUTUION.Y];
        

        input = new InputManager();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        teo = AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/teodor.jpg");
        player = new Sprite(teo);
        //wall = AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/redbrick.png");
        wall = AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/teodor.jpg");
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
            CameraPlane plane = new CameraPlane(pos, dir, 0.25f, FOV);

            rayData[r] = Ray.WallCast(pos, startCastDir + FOVincrement * r, MAP, MAP_RESOLUTION);

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
        }
        #endregion
        
        #region Floorcast
        for (int y = 0; y < GAME_RESOLUTUION.Y / 2; y++)
        {
            int p = y - GAME_RESOLUTUION.Y / 2;
            float posZ = height * GAME_RESOLUTUION.Y;
            float rowDistance = Math.Abs(posZ / p);
                
            for (int x = 0; x < GAME_RESOLUTUION.X; x++)
            {
                //Get ray data for floor computations
                Ray.Raydata data = Ray.Cast(pos, 
                                            startCastDir + (FOVincrement * x), 
                                            rowDistance, 
                                            MAP, MAP_RESOLUTION);

                #region Get floor coordinates
                floorCheckTest[x, y] = data.hitData;
                floorPointDistance[x, y] = data.distance;
                #endregion

                #region Get floor Textures Coordinates
                Vector2 floorCheckCell = new Vector2(EMath.FloorToInt(data.hitPosition.X), 
                                                     EMath.FloorToInt(data.hitPosition.Y));

                Vector2 texturePosition = data.hitPosition - floorCheckCell;

                floorTexturePosition[x, y] = new Vector2((int)wall.Width * (data.hitPosition.X - floorCheckCell.X),
                                                        (int)wall.Height * (data.hitPosition.Y - floorCheckCell.Y));
                #endregion
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
        {
            Exit();
            showMap = !showMap;
        }

        if (input.Keyboard.IsButtonJustReleased(Keys.Tab))

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
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        
        //Draw floor and ceiling
        for (int y = 0; y < GAME_RESOLUTUION.Y / 2; y++)
        {
            for (int x = 0; x < GAME_RESOLUTUION.X; x++)
            {        
                //Get distance shade   
                Color floorShade = (Color.White * 0.5f) * (1 / floorPointDistance[x, y]);
                floorShade.A = 255;

                //Texture floor
                Primitives.DrawTexturedBox(_spriteBatch, 
                                                wall, 
                                                new EMath.Vector2i((int)floorTexturePosition[x, y].X, (int)floorTexturePosition[x, y].Y), 
                                                new EMath.Vector2i(1, 1), 
                                                new Vector2(x * 6f,
                                                SCREEN_RESOLUTION.Y - y * 4.5f),
                                                Vector2.One * 0.5f,
                                                Vector2.One * 6,
                                                floorShade);
                
                //Texture ceiling
                Primitives.DrawTexturedBox(_spriteBatch, 
                                                wall, 
                                                new EMath.Vector2i((int)floorTexturePosition[x, y].X, (int)floorTexturePosition[x, y].Y), 
                                                new EMath.Vector2i(1, 1), 
                                                new Vector2(x * 6f,
                                                0 + y * 4.5f),
                                                Vector2.One * 0.5f,
                                                Vector2.One * 6,
                                                floorShade);
            }
        }
        
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
        
        
        for (int r = 0; r < GAME_RESOLUTUION.X; r++)
        {
            Primitives.DrawLine(_spriteBatch, pos * CELLSIZE, (pos + rayData[r].hitPosition) * CELLSIZE, 1f, Color.Red);
        }
        
        //Draw player and camera plane
        player.Draw(_spriteBatch, pos * CELLSIZE, Color.White, dir, Vector2.One / 10);

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

    Vector2 rotateAround(Vector2 center, Vector2 point, float angle)
    {
        float angleInRadians = MathHelper.ToRadians(angle);

        float cos = (float)Math.Cos(angleInRadians);
        float sin = (float)Math.Sin(angleInRadians);
        
        Vector2 direction = point - center;
        
        float rotatedX = direction.X * cos - direction.Y * sin;
        float rotatedY = direction.X * sin + direction.Y * cos;
        
        return new Vector2(rotatedX, rotatedY) + center;
    }
}