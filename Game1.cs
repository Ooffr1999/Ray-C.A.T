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

    Camera cam;
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

        //sector = new Sector(_spriteBatch, cam);
    }
    
    protected override void Initialize()
    {
        base.Initialize();

        cam = new Camera();
        sector = new Sector(_spriteBatch, cam);
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

        cam.Update(_input, _deltaTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        //TODO: Add all the textures to one texture-atlas that can be retreived with an xml(or whatever its called) file, to increase performance
        base.Draw(gameTime);

        Matrix worldMatrix = Matrix.CreateWorld(cam.getCameraTarget(), cam.getForward(), Vector3.Up);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        //TODO: Text adjustment works, but centering lineadjustment doesn't work with even numbers
        _textDraw.Draw();

        debugMenu.Write(_input.Mouse.position.ToString());

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 10),
            new Vector3(10, 0, 0),
            new Vector3(10, 0, -10),
            new Vector3(-10, 0, -10),
            new Vector3(-10, 0, 0),
            new Vector3(0, 0, 10)
        };

        sector.RenderSector(vertices, 10);

        _spriteBatch.End();
    }
}