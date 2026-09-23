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
    public Entity tomas;

    protected override void LoadContent()
    {
        MAP = new Map(new int[,]
        {
            {1, 1, 1, 1, 1, 1, 1, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 1, 1, 1, 1, 1, 1, 1 },
        } , 
        32      //Cellsize
        );

        MAP.cells = new Cell[5];

        //Una floor
        MAP.cells[0] = new Cell(CellType.Floor, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/greystone.png"));
        //Ooffr wall
        MAP.cells[1] = new Cell(CellType.Wall, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/redbrick.png"));
        //Una floor
        MAP.cells[2] = new Cell(CellType.Wall, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/una.jpeg"));
        //Tomas wall
        MAP.cells[3] = new Cell(CellType.Wall, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/tomas.jpeg"));
        //Ooffr floor
        MAP.cells[4] = new Cell(CellType.Floor, Color.White, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Content/textures/ooffr.jpeg"));

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
    }

    protected override void Update(GameTime gameTime)
    {
        if (_input.Keyboard.IsButtonJustPressed(Keys.Tab))
            Minimap.Hide = !Minimap.Hide;

        if (_input.Keyboard.IsButtonJustPressed(Keys.Escape))
            Exit();

        glonk.spriteColor = (Color.White * 0.5f) * (1 / Vector2.Distance(player.position, glonk.position));
        glonk.spriteColor.A = 255;

        tomas.spriteColor = (Color.White * 0.5f) * (1 / Vector2.Distance(player.position, tomas.position));
        tomas.spriteColor.A = 255;

        base.Update(gameTime);
    }
}