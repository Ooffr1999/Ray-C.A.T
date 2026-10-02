using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TextRenderer
{
    public Vector2 offset {get; set;}
    public float fontSize {get; set;}
    public float letterSpacing {get; set;}
    public float lineSpacing {get; set;}
    Rectangle textSpace {get; set;}
    public Color textColor;
    public string text;
    public TextAdjustment textAdjustment;
    public LineAdjustment lineAdjustment;

    SpriteBatch batch;
    TextureAtlas alphabet;

    float letterWidth;
    float letterHeight;

    public TextRenderer(SpriteBatch _batch, Texture2D bitmap)
    {
        batch = _batch;
        alphabet = TextureAtlas.FromBitMap(bitmap, 13, 7);
        fontSize = 2;
        letterSpacing = 4;
        lineSpacing = 0;
        textColor = Color.White;
        letterWidth = bitmap.Width / 13;
        letterHeight = bitmap.Height / 7;
        textAdjustment = TextAdjustment.Left;
        lineAdjustment = LineAdjustment.Top;
    }

    public void DrawBoundaryBox()
    {
        Primitives.DrawLine(batch, new Vector2(textSpace.X, textSpace.Y), new Vector2(textSpace.X + textSpace.Width, textSpace.Y), Color.Green);
        Primitives.DrawLine(batch, new Vector2(textSpace.X + textSpace.Width, textSpace.Y), new Vector2(textSpace.X + textSpace.Width, textSpace.Y + textSpace.Height), Color.Green);
        Primitives.DrawLine(batch, new Vector2(textSpace.X + textSpace.Width, textSpace.Y + textSpace.Height), new Vector2(textSpace.X, textSpace.Y + textSpace.Height), Color.Green);
        Primitives.DrawLine(batch, new Vector2(textSpace.X, textSpace.Y + textSpace.Height), new Vector2(textSpace.X, textSpace.Y), Color.Green);
    }

    public void SetArea(Vector2 position, int Width, int Height)
    {
        textSpace = new Rectangle((int)position.X, (int)position.Y, Width, Height);
    }

    public Rectangle GetArea()
    {
        return textSpace;
    }

    public void Draw()
    {
        float letterAreal = letterSpacing * fontSize;
        float lineAreal = lineSpacing * fontSize;
        float lettersThatCanFitInX = textSpace.Width / letterAreal;
        float lettersThatCanFitInY = textSpace.Height / letterAreal;
        
        String[] lines = text.Split('|');
        
        for (int y = 0; y < lines.Length; y++)
        {
            for (int i = 0; i < lines[y].Length; i++)
            {
                float letterAdjustmentX = 0;
                float letterAdjustmentY = 0;
                
                char[] _text = lines[y].ToCharArray();
                
                switch(textAdjustment)
                {
                    case TextAdjustment.Left:
                        letterAdjustmentX = i * letterAreal + textSpace.X;
                        break;
                    case TextAdjustment.Middle:
                        letterAdjustmentX = (lettersThatCanFitInX - _text.Length) / 2 * letterAreal + (i * letterAreal + textSpace.X);
                        break;
                    case TextAdjustment.Right: 
                        letterAdjustmentX = (lettersThatCanFitInX - _text.Length) * letterAreal + (i * letterAreal + textSpace.X);
                        break;
                }

                switch(lineAdjustment)
                {
                    case LineAdjustment.Top:
                        letterAdjustmentY = textSpace.Y + letterAreal * y;
                        break;

                    case LineAdjustment.Center:
                        letterAdjustmentY = textSpace.Y + ((float)Math.Floor(lettersThatCanFitInY / 2) - (float)Math.Floor(lines.Length / 2f) + y) * letterAreal;
                        break;

                    case LineAdjustment.Bottom:
                        letterAdjustmentY = textSpace.Y + (lettersThatCanFitInY - lines.Length + y) * letterAreal;
                        break;
                }

                alphabet.
                GetRegion(
                    _text[i].ToString()).Draw(
                        batch, 
                        new Vector2(letterAdjustmentX, 
                                    letterAdjustmentY + lineSpacing) +
                                    offset, 
                        textColor,
                        0,
                        Vector2.Zero,
                        Vector2.One * fontSize,
                        SpriteEffects.None);
            }
        }
    }
}

public enum TextAdjustment {Left, Middle, Right}
public enum LineAdjustment {Top, Center, Bottom}