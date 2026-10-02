using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class TextureAtlas
{
    public Texture2D texture {get; set;}
    Dictionary<string, TextureRegion> _regions;

    public TextureAtlas()
    {
        _regions = new Dictionary<string, TextureRegion>();   
    }    

    public TextureAtlas(Texture2D _texture)
    {
        texture = _texture;
        _regions = new Dictionary<string, TextureRegion>();
    }

    public void AddRegion(string name, int x, int y, int width, int height)
    {
        TextureRegion region = new TextureRegion(texture, x, y, width, height);
        _regions.Add(name, region);
    }

    public TextureRegion GetRegion(string name)
    {
        return _regions[name];
    }

    public bool RemoveRegion(string name)
    {
        return _regions.Remove(name);
    }

    public void ClearRegion()
    {
        _regions.Clear();
    }

    public static TextureAtlas FromBitMap (Texture2D _texture, int width, int height)
    {
        TextureAtlas atlas = new TextureAtlas(_texture);

        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-=()[]{}<>/¤¨#%!?.,'^@&$ æØø";
        char[] _alphabet = alphabet.ToCharArray();

        int a = 0;

        for (int y = 0; y < height; y++)
        {                                                                                                                                       
            for (int x = 0; x < width; x++)
            {
                atlas.AddRegion(_alphabet[a].ToString(), 
                                (atlas.texture.Width / width) * x, 
                                (atlas.texture.Height / height) * y, 
                                (atlas.texture.Width / width), 
                                (atlas.texture.Height / height));
                a++;
            }
        }

        return atlas;
    }
}