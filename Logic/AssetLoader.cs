using System.IO;
using Microsoft.Xna.Framework.Graphics;
using WolfenStein_like;

public static class AssetLoader
{
    public static Texture2D LoadTexture2D(string fileName)
    {
        Texture2D texture;

        FileStream filestream = new FileStream(fileName, FileMode.Open);
        texture = Texture2D.FromStream(RayCatCore._graphics.GraphicsDevice, filestream);
        filestream.Dispose();

        return texture;
    }
}