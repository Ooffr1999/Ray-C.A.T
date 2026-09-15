using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RayCatCore;

public class Core : Game
{
    internal static Core s_Instance;

    public static Core Instance => s_Instance;

    public static GraphicsDeviceManager Graphics {get; set;}

    public static GraphicsDevice graphicsDevice {get; set;}

    public static SpriteBatch _batch {get; set;}

    public static ContentManager manager {get; set;}

    public Core(string title, int width, int height, bool IsFullScreen)
    {
        if (s_Instance != null)
        {
            throw new InvalidOperationException($"Only a single Core instance can be created");
        }

        s_Instance = this;

        Graphics = new GraphicsDeviceManager(this);

        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferWidth = height;

        Graphics.IsFullScreen = IsFullScreen;

        Graphics.ApplyChanges();

        Window.Title = title;

        Content = base.Content;

        Content.RootDirectory = "Content";

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        graphicsDevice = base.GraphicsDevice;

        _batch = new SpriteBatch(GraphicsDevice);
    }
}