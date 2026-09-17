using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Custom;
using System.Collections.Generic;

namespace WolfenStein_like;

public class Game1 : Game
{
    public static GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    Vector2i GAME_RESOLUTION = new Vector2i(320, 240);
    Vector2i SCREEN_RESOLUTION = new Vector2i(1920, 1080);

    Map MAP = new Map(new int[,]
        {
            {1, 2, 1, 2, 1, 2, 1, 1 },
            {1, 0, 0, 1, 0, 1, 4, 2 },
            {2, 0, 4, 1, 0, 1, 0, 1 },
            {1, 0, 0, 1, 4, 1, 0, 2 },
            {2, 0, 0, 0, 4, 0, 0, 3 },
            {1, 0, 4, 1, 0, 1, 0, 2 },
            {2, 1, 1, 1, 0, 1, 4, 1 },
            {1, 0, 0, 0, 0, 1, 1, 1 },
            {1, 0, 0, 0, 0, 1, 0, 1 },
            {1, 0, 0, 0, 0, 1, 0, 1 },
            {1, 0, 0, 0, 0, 1, 0, 1 },
            {1, 0, 0, 0, 0, 1, 0, 1 },
            {1, 1, 1, 1, 1, 1, 1, 1 },
            {1, 1, 1, 1, 1, 1, 1, 1 },
            {1, 1, 1, 1, 1, 1, 1, 1 },
            {1, 1, 1, 1, 1, 1, 1, 1 },
            {1, 1, 1, 1, 1, 1, 1, 1 },
        } , 
        32      //Cellsize
        );


    const int FOV = 90;

    Player player = new Player();
    PlayerInput playerInput;
    
    bool showMap = false;

    //TODO: Implement raydata and linedata into the CalculateFloorWallsCeiling class
    Ray.Raydata[] rayData;
    Ray.LineData[] lineData;

    InputManager input;

    //TODO: Implement the three values below into the CalculateFloorWallsCeiling class
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
        Window.Title = "Ray-C.A.T";
        Window.AllowUserResizing = true;

        _graphics.PreferredBackBufferWidth = SCREEN_RESOLUTION.X;
        _graphics.PreferredBackBufferHeight = SCREEN_RESOLUTION.Y;
        //_graphics.IsFullScreen = true;
        
        _graphics.ApplyChanges();

        rayData = new Ray.Raydata[GAME_RESOLUTION.X];
        lineData = new Ray.LineData[GAME_RESOLUTION.X];

        floorCheckTest = new int[GAME_RESOLUTION.X, GAME_RESOLUTION.Y];
        floorPointDistance = new float[GAME_RESOLUTION.X, GAME_RESOLUTION.Y];
        floorTexturePosition = new Vector2[GAME_RESOLUTION.X, GAME_RESOLUTION.Y];

        input = new InputManager();
        
        player = new Player();
        player.position = new Vector2(2, 2);
        player.direction = 90;
        player.height = 0.5f;

        playerInput = new PlayerInput(input, 1, 2.5f);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        player.sprite = new Sprite(AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/teodor.jpg"));

        MAP.cells = new Cell[5];

        //Una floor
        MAP.cells[0] = new Cell(CellType.Floor, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/una.jpeg"));
        //Ooffr wall
        MAP.cells[1] = new Cell(CellType.Wall, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/ooffr.jpeg"));
        //Una floor
        MAP.cells[2] = new Cell(CellType.Wall, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/una.jpeg"));
        //Tomas wall
        MAP.cells[3] = new Cell(CellType.Wall, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/tomas.jpeg"));
        //Ooffr floor
        MAP.cells[4] = new Cell(CellType.Floor, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/ooffr.jpeg"));
    }

    protected override void Update(GameTime gameTime)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        input.Update(gameTime);
        player.Update();

        //Render floor and walls
        CalculateMap calculateMap = new CalculateMap(player, FOV, MAP, GAME_RESOLUTION, SCREEN_RESOLUTION);
        
        calculateMap.Floor(player, MAP, floorCheckTest, floorPointDistance, floorTexturePosition);
        calculateMap.Walls(player, MAP, rayData, lineData);
        
        if (input.Keyboard.IsButtonJustPressed(Keys.Tab))
            showMap = !showMap;

        if (input.Keyboard.IsButtonJustPressed(Keys.Escape))
            Exit();
    
        playerInput.Move(player, delta, MAP);
        playerInput.Turn(player, delta, SCREEN_RESOLUTION.X);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        
        Render render = new Render(_spriteBatch);

        render.lightStrength = 1f;

        render.Floors(MAP, GAME_RESOLUTION, SCREEN_RESOLUTION, floorCheckTest, floorTexturePosition, floorPointDistance);
        render.Walls(MAP, rayData, lineData, GAME_RESOLUTION.X);
        
        #region Show Minimap
        
        if (!showMap)
        {
            _spriteBatch.End();

            base.Draw(gameTime);
            return;
        }

        for (int y = 0; y < MAP.resolution.Y; y++)
        {
            for (int x = 0; x < MAP.resolution.X; x++)
            {
                Color col = new Color();
                col = Color.White;

                switch(MAP.data[x, y])
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
                Primitives.DrawBox(_spriteBatch, (new Vector2(x, y) * MAP.cellSize), col, Vector2.One * (MAP.cellSize - 1));
            }
        }
        
        
        for (int r = 0; r < GAME_RESOLUTION.X; r++)
        {
            Primitives.DrawLine(_spriteBatch, player.position * MAP.cellSize, (player.position + rayData[r].hitPosition) * MAP.cellSize, 1f, Color.Red);
        }
        
        //Draw player and camera plane
        player.Draw(_spriteBatch, player.position * MAP.cellSize, Color.White, player.direction, Vector2.One / 10);

        #endregion

        _spriteBatch.End();
    }
}