using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Custom;
using RayCat.Data;
using System.Collections.Generic;
using System;
using System.Formats.Asn1;
using System.Diagnostics;
using System.Linq;
using System.Data;
using System.Runtime.ExceptionServices;

namespace WolfenStein_like;

public class YarrFaceGame : RayCatCore
{   
    public TextRenderer _versionHeader;
    TextRenderer _mapMode;
    bool _mapModeOn;

    Camera cam;
    SectorBuilder sectorBuilder;
    List<Sector> sectors;

    Vector2 cursorPosition;

    Vector3[] vertices;

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

        _versionHeader = new TextRenderer(_spriteBatch, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Fonts/square_6x6.png"));
        _versionHeader.letterSpacing = 7;
        _versionHeader.fontSize = 5;
        _versionHeader.SetArea(new Vector2(0, 0), 1000, 200);
        _versionHeader.text = "Ray-C.A.T Alpha 0.02";
        _versionHeader.textColor = Color.White;
        _versionHeader.lineAdjustment = LineAdjustment.Top;

        _mapMode = new TextRenderer(_spriteBatch, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Fonts/square_6x6.png"));
        _mapMode.letterSpacing = 5;
        _mapMode.fontSize = 3;
        _mapMode.SetArea(new Vector2(1000, 0), 1000, 200);
        _mapMode.text = "Map mode on";

        debugMenu = new DebugMenu(_spriteBatch, _input);

        sectors = new List<Sector>();
    }
    
    protected override void Initialize()
    {
        base.Initialize();

        cam = new Camera();
        sectorBuilder = new SectorBuilder(_spriteBatch, cam);
        vertices = new Vector3[0];
    }

    protected override void Update(GameTime gameTime)
    {
        if (_input.Keyboard.IsButtonJustPressed(Keys.Tab))
        {
           _mapModeOn = !_mapModeOn;
            cam.FreezeMovement(_mapModeOn);
            cam.FreezeRotation(_mapModeOn);
        }

        if (_input.Keyboard.IsButtonJustPressed(Keys.Escape))
            Exit();

        if (_input.Keyboard.IsButtonJustPressed(Keys.F1))
        {
            debugMenu.FlipShowState();
            //player.stop = debugMenu.GetShowState();
        }

        cursorPosition = new Vector2(_input.Mouse.X - WindowResolution.X / 2,
                                            _input.Mouse.Y - WindowResolution.Y / 2);

        cam.Update(_input, _deltaTime);
        
        if (_input.Keyboard.IsButtonJustPressed(Keys.K) && _mapModeOn)
        {
            Sector sector = new Sector();
            List<MapSectorVertice> vertice = new List<MapSectorVertice>();

            MapSectorVertice _v1 = new MapSectorVertice(_spriteBatch, _input);
            _v1.SetPosition(new Vector2(_input.Mouse.position.X - 50, _input.Mouse.position.Y - 50));
            vertice.Add(_v1);

            MapSectorVertice _v2 = new MapSectorVertice(_spriteBatch, _input);
            _v2.SetPosition(new Vector2(_input.Mouse.position.X + 50, _input.Mouse.position.Y - 50));
            vertice.Add(_v2);

            MapSectorVertice _v3 = new MapSectorVertice(_spriteBatch, _input);
            _v3.SetPosition(new Vector2(_input.Mouse.position.X + 50, _input.Mouse.position.Y + 50));
            vertice.Add(_v3);

            MapSectorVertice _v4 = new MapSectorVertice(_spriteBatch, _input);
            _v4.SetPosition(new Vector2(_input.Mouse.position.X - 50, _input.Mouse.position.Y + 50));
            vertice.Add(_v4);  
            
            sector.vertices = vertice;
            sector.height = 10;

            sectors.Add(sector);

            foreach(MapSectorVertice m in sectors.Last().vertices)
            {
                m.parent = sectors.Last();   
            }
        }

        foreach(Sector s in sectors)
        {
            foreach(MapSectorVertice m in s.vertices)
            {
                m.Update();
            }
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        //TODO: Add all the textures to one texture-atlas that can be retreived with an xml(or whatever its called) file, to increase performance
        base.Draw(gameTime);

        Matrix worldMatrix = Matrix.CreateWorld(cam.getCameraTarget(), cam.getForward(), Vector3.Up);

        if (vertices.Length > 1)
            sectorBuilder.RenderSector(vertices, 10);

        for (int s = 0; s < sectors.Count(); s++)
        {
            sectorBuilder.RenderSector(sectors[s]);
        }

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        //TODO: Text adjustment works, but centering lineadjustment doesn't work with even numbers
        _versionHeader.Draw();

        if (_mapModeOn)
        {
            _mapMode.Draw();

            Primitives.DrawBox(_spriteBatch, new Vector2(cam.getPosition().X, cam.getPosition().Z) * 10, Color.White, Vector2.One * 10);
            
            if (sectors.Count > 0)
            for (int s = 0; s < sectors.Count; s++)
            {
                for (int i = 0; i < sectors[s].vertices.Count; i++)
                {
                    sectors[s].vertices[i].Draw();
                    MapBuilder.DrawEdgelines(_spriteBatch, sectors[s].vertices);
                }
            }
        }

        debugMenu.Write(cursorPosition.ToString());

        _spriteBatch.End();
    }
}