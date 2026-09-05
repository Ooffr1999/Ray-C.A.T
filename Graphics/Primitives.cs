using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WolfenStein_like;

public static class Primitives{
    
    public static Texture2D blank;
    
    static Primitives()
    {
        blank = AssetLoader.LoadTexture2D("C:/Users/chris/Documents/C#/Ray-C.A.T/Content/textures/Debug1x1.png");
    }

    public static void DrawBox(SpriteBatch batch, Vector2 location, Color color, float rotation, Vector2 scale, float layer)
    {
        //Texture2D texture = getEmptyTexture(batch);

        Vector2 origin = new Vector2(blank.Width / 2, blank.Height / 2);

        batch.Draw(blank, location, null, color, rotation, origin, scale, SpriteEffects.None, layer);
    }

    public static void DrawBox(SpriteBatch batch, Vector2 location, Color color, Vector2 scale)
    {
        DrawBox(batch, location, color, 0.0f, scale, 0.0f);
    }

    #region Draw Untextured Line

    //The DrawLine function with the most parameters. The other functions deviate from this one. 
    public static void DrawLine(SpriteBatch batch, Vector2 start, Vector2 end, float thickness, Color color, float layer)
    {
        float distance = Vector2.Distance(start, end);                                                         //Gets the distance from start to end. 
        Vector2 direction = end - start;                                                                       //Gets the direction from start to end.
        float angle = (float)Math.Atan2(direction.Y, direction.X);                                             //Sets the angle to the direction above


        batch.Draw(                                                                                             //Makes a square that points to end with the distance between, creating a line                                                                                             
            blank, 
            start, 
            null, 
            color, 
            angle, 
            Vector2.Zero, 
            new Vector2(distance, thickness), 
            SpriteEffects.None, 
            layer
        );
    }

    //Simpler drawlines
    public static void DrawLine(SpriteBatch batch, Vector2 start, Vector2 end, Color color, float layer)        
    {
        DrawLine(batch, start, end, 1, color, layer);
    }

    public static void DrawLine(SpriteBatch batch, Vector2 start, Vector2 end, float thickness, Color color)
    {
        DrawLine(batch, start, end, thickness, color, 0.0f);
    }

    public static void DrawLine(SpriteBatch batch, Vector2 start, Vector2 end, Color color)
    {
        DrawLine(batch, start, end, 1, color, 0.0f);
    }
    #endregion

    #region Draw Textured Line
    //Draws a textured line with the same code as above.
    public static void DrawTexturedLine(SpriteBatch batch, Texture2D texture, int x, int y, Vector2 start, Vector2 end, float thicknessModifier, Color color, float layer)
    {
        double _lineThickness = 1f / texture.Height;
        float lineThickness = (float)_lineThickness;
        
        float distance = Vector2.Distance(start, end);
        Vector2 direction = end - start;
        float angle = (float)Math.Atan2(direction.Y, direction.X);
        
        TextureRegion line = new TextureRegion(texture, x, y, (int)Math.Clamp(thicknessModifier, 0, texture.Width), texture.Height);
        
        
        
        line.Draw(batch, 
                    start, 
                    color, 
                    0, 
                    Vector2.Zero, 
                    new Vector2(1, (float)(_lineThickness * distance)),
                    SpriteEffects.None, 
                    0.0f);
        
    }
    #endregion
}