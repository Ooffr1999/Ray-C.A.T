using Microsoft.Xna.Framework;
using Custom;
using RayCat.Data;
using RayCat.Draw;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using RayCat.Logic;
using System;

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

    //public RaycastCalculate _calculateMap;
    
    //Game Window Values
    public Vector2i GameResolution = new Vector2i(320, 240);
    public Vector2i WindowResolution = new Vector2i(1920, 1080);
    
    //public int FOV = 90;
    
    public Player player;  

    public float _deltaTime;    

    public bool showMap = false;

    public Minimap Minimap;

    public DebugMenu debugMenu;

    //TODO: For øyeblikket fjerner jeg alt som har å gjøre med raycasting fra Raycat, til fordel for den nye matrixløsningen
    //Det betyr at jeg senere reimplementerer den med forbedringene denne andre løsningen byr på, slik at jeg kan optimalisere
    //Raycasting blir dermed en instilling i frameworket. Type når man starter et prosjekt i Unity og man kan velge mellom 
    //2D og 3D. Raycat får en slik instilling, men i stedet blir det om prosjektet skal være Raycast eller Matrix. 
    //Kanskje jeg etterhvert til og med kan implementere true 3D, likt som i Quake, men det tør jeg ikke tenke på.

    public RayCatCore()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;

        Window.IsBorderless = true;
    }

    //Initialize the most important toold
    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = WindowResolution.X;        //Set window size
        _graphics.PreferredBackBufferHeight = WindowResolution.Y;       //Set window height
        _graphics.ApplyChanges();                                       //Apply the changes
                                                                        //TODO: Add a modular way to set these things in game
        IsMouseVisible = true;

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
        player.position = new Vector3(2, 0, 2);                         //Set initial position
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
        //_calculateMap = new RaycastCalculate(player, FOV, MAP, GameResolution, WindowResolution);

        //_calculateMap.Floor();
        //_calculateMap.Walls();
        
        //for (int i = 0; i < Services.GetService<List<Entity>>().Count; i++)
            //Services.GetService<List<Entity>>()[i].Update(_deltaTime);

        debugMenu.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        GraphicsDevice.Clear(Color.DarkBlue);

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, samplerState: SamplerState.PointClamp);

        //TODO: Legg inn tegnefunksjon for Matrix her

        _spriteBatch.End();

        _spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState:SamplerState.PointClamp);
        
        for (int i = 0; i < Services.GetService<List<Entity>>().Count; i++)
            Services.GetService<List<Entity>>()[i].DrawToScreen(_spriteBatch, player, WindowResolution);
                                    
        _spriteBatch.End();

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        Minimap.DrawMap();                              //Draws the minimap
        Minimap.DrawPlayer(player, 90);                 //Draws player onto the minimap
        
        debugMenu.Draw();
        debugMenu.ClearDraw();

        _spriteBatch.End();
    }
}