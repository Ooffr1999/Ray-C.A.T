using Microsoft.Xna.Framework;
using Custom;
using RayCat.Data;
using RayCat.Draw;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

//The RayCat Engine core. 
//This is the file that runs the game. It is designed to be a sandbox that files inherriting will build upon.
//In a loose sense, the following code is the Game Engine. 
//It must therefore be neutral and there musn't be a necessity to manipulate the following files in order to get a base playable game.

public class RayCatCore : Game
{
    public static GraphicsDeviceManager _graphics;
    public SpriteBatch _spriteBatch;
    List<Entity> _entities = new List<Entity>();
    public static InputManager _input;

    public Map MAP;

    //TODO: Add threading to the calculation and drawing

    public Calculate _calculateMap;
    
    //Game Window Values
    public Vector2i GameResolution = new Vector2i(320, 240);
    public Vector2i WindowResolution = new Vector2i(1920, 1080);
    
    public int FOV = 90;
    
    public Player player;  

    public float _deltaTime;    

    public bool showMap = false;

    public Minimap Minimap;

    public RayCatCore()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;

        //Window.IsBorderless = true;
    }

    //Initialize the most important toold
    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = WindowResolution.X;        //Set window size
        _graphics.PreferredBackBufferHeight = WindowResolution.Y;       //Set window height
        _graphics.ApplyChanges();                                       //Apply the changes
                                                                        //TODO: Add a modular way to set these things in game

        _input = new InputManager();                                    //Create the inputmanager that drives button clicks.
        Services.AddService(typeof(List<Entity>), _entities);           //Add a list of entities to the GameServiceManager

        base.Initialize();                                              //Load additional framework things at the end.
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        
        _spriteBatch = new SpriteBatch(GraphicsDevice);                 //Create the spritebatch
        Services.AddService(typeof(SpriteBatch), _spriteBatch);         //Add spritebatch to the Servicemanager

        Services.AddService(typeof(Map), MAP);                          //Add the game map to the ServiceManager

        player = new Player(Services, MAP);                             //Create the player
        player.position = new Vector2(2, 2);                            //Set initial position
        player.direction = 90;                                          //Set initial direction
        player.height = 0.5f;
        player.speed = 1;
        player.runModifier = 3;

        Minimap = new Minimap(_spriteBatch, MAP);
    }

    protected override void Update(GameTime gameTime)
    {
        _deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _input.Update(gameTime);

        //Calculate Map
        _calculateMap = new Calculate(player, FOV, MAP, GameResolution, WindowResolution);
        
        _calculateMap.Floor();
        _calculateMap.Walls();
        
        for (int i = 0; i < Services.GetService<List<Entity>>().Count; i++)
            Services.GetService<List<Entity>>()[i].Update(_deltaTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        Render render = new Render(Services, _calculateMap);

        render.Floors();
        render.Walls();

        Minimap.DrawMap();                              //Draws the minimap
        Minimap.DrawPlayer(player, 90);                 //Draws player onto the minimap

        for (int i = 0; i < Services.GetService<List<Entity>>().Count; i++)
            Services.GetService<List<Entity>>()[i].DrawToScreen(_spriteBatch, player, WindowResolution);

        _spriteBatch.End();
    }
}