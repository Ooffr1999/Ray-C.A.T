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

    public TextRenderer _textDraw;
    public DebugMenu debugMenu;

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
        MAP.cells[4] = new Cell(CellType.FloorCeil, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/Stone/Stone01.png"));

        base.LoadContent();

        player.sprite = new Sprite(AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/reddot.png"));
        
        glonk = new Entity(Services);
        glonk.sprite = new Sprite(AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/pillar.png"));
        glonk.position = new Vector2(4.5f, 4.5f);
        glonk.spriteScale *= 20;

        _textDraw = new TextRenderer(_spriteBatch, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Fonts/square_6x6.png"));
        _textDraw.letterSpacing = 7;
        _textDraw.fontSize = 5;
        _textDraw.SetArea(new Vector2(0, 0), 1000, 200);
        _textDraw.text = "Ray-C.A.T Alpha 0.02";
        _textDraw.textColor = Color.White;
        _textDraw.lineAdjustment = LineAdjustment.Top;

        debugMenu = new DebugMenu(_spriteBatch, _input);
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
            player.stop = debugMenu.GetShowState();
        }

        glonk.spriteColor = (Color.White * 0.5f) * (1 / Vector2.Distance(player.position, glonk.position));
        glonk.spriteColor.A = 255;

        debugMenu.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        //TODO: Text adjustment works, but centering lineadjustment doesn't work with even numbers
        _textDraw.Draw();
        
        debugMenu.Draw();
        
        debugMenu.ClearDraw();
        debugMenu.Write(glonk.screenPosition.ToString());
        debugMenu.Write("|");
        debugMenu.Write("Angle to player: " + glonk.angleToPlayer.ToString());
        debugMenu.Write("|");
        debugMenu.Write(player.direction.ToString());

        _spriteBatch.End();

        //TODO: Fix the infinite redrawing of the floor beyond the confines of the map
        //TODO: Add all the textures to one texture-atlas that can be retreived with an xml(or whatever its called) file, to increase performance
    }
}