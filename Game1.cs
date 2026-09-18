using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Custom;
using RayCat.Data;
using System.Collections.Generic;
using System;

namespace WolfenStein_like;

public class YarrFaceGame : RayCatCore
{   
    public Entity glonk;

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        MAP = new Map(new int[,]
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

        base.LoadContent();

        player.sprite = new Sprite(AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/teodor.jpg"));
        glonk = new Entity(Services);
        glonk.sprite = new Sprite();
        glonk.sprite.texture = AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/tomas.jpeg");
        glonk.position = new Vector2(4.5f, 4.5f);
    }

    protected override void Update(GameTime gameTime)
    {
        if (_input.Keyboard.IsButtonJustPressed(Keys.Tab))
            Minimap.Hide = !Minimap.Hide;

        if (_input.Keyboard.IsButtonJustPressed(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    //TODO: Make a better, modular implementation of DRAW that can add stuff over the current method
    public override void tempDraw()
    {
        Vector2 dirrr = player.position - glonk.position;

        Console.WriteLine(dirrr);

        glonk.Draw(_spriteBatch, 
                    glonk.position * 32, 
                    Color.White, 
                    glonk.direction, 
                    Vector2.One / 20);
        /*
        glonk.Draw(_spriteBatch, 
                    new Vector2((WindowResolution.X / 2) - (player.forward.Y * 6.28f) * Vector2.Distance(player.position, glonk.position) * 100, WindowResolution.Y / 2), 
                    Color.White, 
                    glonk.direction, 
                    Vector2.One / 20);
        */
    }
}