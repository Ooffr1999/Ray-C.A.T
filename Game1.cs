using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Custom;
using RayCat.Data;
using System.Collections.Generic;
using System;
using System.Formats.Asn1;
using System.Diagnostics;

namespace WolfenStein_like;

public class YarrFaceGame : RayCatCore
{   
    public TextRenderer _textDraw;

     TempPlayerDetails p;

    Sector sector;

    protected override void LoadContent()
    {
        MAP = new Map(new int[,]
        {
            {1, 1, 1, 1, 1, 1, 1, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 4, 4, 4, 4, 4, 4, 1 },
            {1, 1, 1, 1, 1, 1, 1, 1 },
        } , 
        32      //Cellsize
        );

        MAP.cells = new Cell[5];

        //Una floor
        MAP.cells[0] = new Cell(CellType.FloorCeil, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/Stone/Stone01.png"));
        //Ooffr wall
        MAP.cells[1] = new Cell(CellType.Wall, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/Brick/Brick01.png"));
        //Stone
        MAP.cells[4] = new Cell(CellType.Floor, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/Stone/Stone01.png"));

        base.LoadContent();

        player.sprite = new Sprite(AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/reddot.png"));

        _textDraw = new TextRenderer(_spriteBatch, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Fonts/square_6x6.png"));
        _textDraw.letterSpacing = 7;
        _textDraw.fontSize = 5;
        _textDraw.SetArea(new Vector2(0, 0), 1000, 200);
        _textDraw.text = "Ray-C.A.T Alpha 0.02";
        _textDraw.textColor = Color.White;
        _textDraw.lineAdjustment = LineAdjustment.Top;

        debugMenu = new DebugMenu(_spriteBatch, _input);

        sector = new Sector(_spriteBatch);
    }
    
    protected override void Initialize()
    {
        base.Initialize();

        p.position = new Vector3(0, 0, 10);
        p.angle = 0;

        Sector sector = new Sector(_spriteBatch);
    }

    protected override void Update(GameTime gameTime)
    {
        if (_input.Keyboard.IsButtonJustPressed(Keys.Tab))
            Minimap.Hide = !Minimap.Hide;

        if (_input.Keyboard.IsButtonJustPressed(Keys.Escape))
            Exit();

        if (_input.Keyboard.IsButtonJustPressed(Keys.F1))
        {
            debugMenu.FlipShowState();
            //player.stop = debugMenu.GetShowState();
        }

        if (_input.Keyboard.IsKeyDown(Keys.Space))
            p.position.Y += _deltaTime * 1;

        if (_input.Keyboard.IsKeyDown(Keys.LeftShift))
            p.position.Y -= _deltaTime * 1;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        //TODO: Add all the textures to one texture-atlas that can be retreived with an xml(or whatever its called) file, to increase performance
        base.Draw(gameTime);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        //TODO: Text adjustment works, but centering lineadjustment doesn't work with even numbers
        _textDraw.Draw();

        sector.DrawBox(_spriteBatch, player, p.position, new Vector2(WindowResolution.X, WindowResolution.Y), _input);
        
        //sector.DrawSectorWall(_spriteBatch, player, new Vector2(7, 3), new Vector2(3, 3), WindowResolution, GameResolution);

        

        _spriteBatch.End();
    }

    struct TempPlayerDetails
    {
        public Vector3 position;
        public float angle;
    }
}