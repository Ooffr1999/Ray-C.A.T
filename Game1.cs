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
    public Entity glonk;
    public Entity tomas;

    public TextRenderer _textDraw;
    public DebugMenu debugMenu;
    bool showDebugMenu;

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
        //Una floor
        MAP.cells[2] = new Cell(CellType.Wall, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/una.jpeg"));
        //Tomas wall
        MAP.cells[3] = new Cell(CellType.Wall, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/tomas.jpeg"));
        //Ooffr floor
        MAP.cells[4] = new Cell(CellType.FloorCeil, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/Stone/Stone01.png"));

        base.LoadContent();

        player.sprite = new Sprite(AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/teodor.jpg"));
        glonk = new Entity(Services);
        glonk.sprite = new Sprite(AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/pillar.png"));
        glonk.position = new Vector2(4.5f, 4.5f);
        glonk.spriteScale *= 20;

        tomas = new Entity(Services);
        tomas.sprite = new Sprite(AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/tomas.jpeg"));
        tomas.position = new Vector2(7.5f, 1.5f); 
        tomas.spriteScale *= new Vector2(1.5f, 2.5f);

        _textDraw = new TextRenderer(_spriteBatch, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Fonts/square_6x6.png"));
        _textDraw.letterSpacing = 7;
        _textDraw.fontSize = 5;
        _textDraw.SetArea(new Vector2(0, 0), 1000, 200);
        _textDraw.text = "Ray-C.A.T Alpha 0.01";
        _textDraw.textColor = Color.White;
        _textDraw.lineAdjustment = LineAdjustment.Top;

        debugMenu = new DebugMenu(_spriteBatch, _input);
    }

    protected override void Update(GameTime gameTime)
    {
        if (_input.Keyboard.IsButtonJustPressed(Keys.Tab))
        {
            Minimap.Hide = !Minimap.Hide;
            debugMenu.Write("Map key");
        }

        if (_input.Keyboard.IsButtonJustPressed(Keys.Escape))
        {
            debugMenu.Write("Closing game");
            Exit();
        }

        if (_input.Keyboard.IsButtonJustPressed(Keys.F1))
            showDebugMenu = !showDebugMenu;


        glonk.spriteColor = (Color.White * 0.5f) * (1 / Vector2.Distance(player.position, glonk.position));
        glonk.spriteColor.A = 255;

        tomas.spriteColor = (Color.White * 0.5f) * (1 / Vector2.Distance(player.position, tomas.position));
        tomas.spriteColor.A = 255;

        if (showDebugMenu)
            debugMenu.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        if (showDebugMenu)
            debugMenu.Draw();
        
        _textDraw.Draw();

        //TODO: Get text library up and running
        
        //TODO: Text adjustment works, but centering lineadjustment doesn't work with even numbers
        _textDraw.Draw();
        
        _spriteBatch.End();

        //TODO: Fix the infinite redrawing of the floor beyond the confines of the map
        //TODO: Add all the textures to one texture-atlas that can be retreived with an xml(or whatever its called) file, to increase performance
    }

    void Print(Button button)
    {
        button.SetPosition(button.GetPosition() + _input.Mouse.positionDelta.ToVector2());
    }
}