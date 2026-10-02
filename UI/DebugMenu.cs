using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class DebugMenu
{
    public TextRenderer debugText;
    public Button DebugMenuHeader;

    SpriteBatch batch;
    InputManager input;

    Vector2 position;
    int Width;
    int Height;

    public DebugMenu() {}

    public DebugMenu(SpriteBatch _batch, InputManager _input)
    {   
        debugText = new TextRenderer(_batch, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Fonts/square_6x6.png"));
        debugText.SetArea(new Vector2(100, 100), 1000, 500);
        debugText.fontSize = 3;
        debugText.letterSpacing = 5;
        debugText.lineAdjustment = LineAdjustment.Bottom;
        debugText.text = "Debug console";

        DebugMenuHeader = new Button(_input, _batch);
        DebugMenuHeader.SetPosition(new Vector2(debugText.GetArea().X, debugText.GetArea().Y));
        DebugMenuHeader.SetButtonEventType(ButtonEvent.hold);
        DebugMenuHeader.SetWidth(debugText.GetArea().Width);
        DebugMenuHeader.SetHeight(50);
        DebugMenuHeader.SetFontSize(4);
        DebugMenuHeader.SetLetterSpace(6);
        DebugMenuHeader.SetLabelTextAdjustment(TextAdjustment.Left);
        DebugMenuHeader.SetLabel("Debug Console");
        DebugMenuHeader.SetEvent(MoveDebugMenu);

        SetPosition(new Vector2(920, 450));
        SetWidth(1000);
        SetHeight(500);
        
        batch = _batch;
        input = _input;
    }

    public void Update()
    {
        DebugMenuHeader.Update();
    }

    void MoveDebugMenu()
    {
        SetPosition(GetPosition() + input.Mouse.positionDelta.ToVector2());
    }

    public void Write(string text)
    {
        debugText.text += "|" + text;
    }

    public void Draw()
    {   
        debugText.DrawBoundaryBox();
        debugText.Draw();

        DebugMenuHeader.Draw(batch);
    }

    public void SetPosition(Vector2 _position)
    {
        position = _position;

        if (debugText != null)
            debugText.SetArea(position, Width, Height);

        if (DebugMenuHeader != null)
            DebugMenuHeader.SetPosition(_position);
    }

    public Vector2 GetPosition()
    {
        return position;
    }

    public void SetWidth(int _Width)
    {
        Width = _Width;

        if (debugText != null)
            debugText.SetArea(position, Width, Height);
    }

    public void SetHeight(int _Height)
    {
        Height = _Height;

        if (debugText != null)
            debugText.SetArea(position, Width, Height);
    }
}