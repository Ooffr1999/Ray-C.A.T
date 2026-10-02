using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Button : GUIObject
{
    String text;
    Action action;
    TextRenderer textRenderer;

    ButtonEvent buttonEvent;

    bool holding;
    bool active;

    public Button(InputManager _input, SpriteBatch _batch)
    {
        text = "Label";
        base.input = _input;  
        textRenderer = new TextRenderer(_batch, AssetLoader.LoadTexture2D("/home/ooffr/Documents/C#/WolfenStein-like/Fonts/square_6x6.png"));     
        SetArea(base.GetPosition(), base.GetHeight(), base.GetWidth());
        textRenderer.SetArea(base.GetPosition(), base.GetWidth(), base.GetHeight());
        textRenderer.textAdjustment = TextAdjustment.Middle;
        buttonEvent = ButtonEvent.mouseDown;
    }

    public override void Update()
    {
        switch(buttonEvent)
        {
            case ButtonEvent.mouseDown:
            {
                if (GetMouseClick())
                {
                    color = clickColor;

                    if (action != null)
                        action();            
                }
                else if(IsMouseInConfines())
                    color = hoverColor;
                else color = standardColor;
                break;
            }

            case ButtonEvent.hold:
            {
                if (input.Mouse.IsButtonDown(MouseInput.MouseButton.left) && IsMouseInConfines())
                    holding = true;

                if (input.Mouse.IsButtonDown(MouseInput.MouseButton.left) && holding)
                {
                    if (action != null)
                    action(); 

                    color = clickColor;
                }
                else if (input.Mouse.IsButtonJustReleased(MouseInput.MouseButton.left) && holding)
                    {
                        holding = false;
                    }
                else if (IsMouseInConfines())                
                    color = hoverColor;
                else color = standardColor;
                break;
            }
            
            case ButtonEvent.buttonUp:
            {
                if (input.Mouse.IsButtonJustReleased(MouseInput.MouseButton.left) && IsMouseInConfines())
                {
                    color = clickColor;
                    
                    if (action != null)
                        action();  
                }
                else if (IsMouseInConfines())                
                    color = hoverColor;
                else color = standardColor;
                break;
            }
            case ButtonEvent.activate:
            {
                if (input.Mouse.IsButtonJustPressed(MouseInput.MouseButton.left) && IsMouseInConfines())
                {
                    active = true;
                }
                break;
            }
        }
        
        if (buttonEvent == ButtonEvent.activate && active)
        {
            color = clickColor;
            
            if (action != null)
                action();  
        }

        base.Update();
    }

    public override void Draw(SpriteBatch _batch)
    {
        base.Draw(_batch);
        textRenderer.GetArea();
        textRenderer.text = text;
        textRenderer.textColor = Color.Black;
        textRenderer.Draw();
    }

    public void SetEvent(Action _action)
    {
        action = _action;
    }

    public void SetLabel(String _text)
    {
        text = _text;
    }

    public void SetButtonEventType(ButtonEvent _buttonEvent)
    {
        buttonEvent = _buttonEvent;
    }

    public void SetFontSize(float size)
    {
        textRenderer.fontSize = size;
    }

    public void SetLetterSpace(float letterSpacing)
    {
        textRenderer.letterSpacing = letterSpacing;
    }

    public override void SetPosition(Vector2 _position)
    {
        base.SetPosition(_position);

        if (textRenderer != null)
            textRenderer.SetArea(base.GetPosition(), base.GetWidth(), base.GetHeight());
    }

    public override void SetWidth(int _Width)
    {
        base.SetWidth(_Width);
        
        if (textRenderer != null)
            textRenderer.SetArea(base.GetPosition(), base.GetWidth(), base.GetHeight());
    }

    public override void SetHeight(int _Height)
    {
        base.SetHeight(_Height);
        
        if (textRenderer != null)
            textRenderer.SetArea(base.GetPosition(), base.GetWidth(), base.GetHeight());
    }

    public void SetLabelTextAdjustment(TextAdjustment _textAdjustment)
    {
        textRenderer.textAdjustment = _textAdjustment;
    }

    public void Deactivate()
    {
        active = false;
    }
}

public enum ButtonEvent
{
    hold, mouseDown, buttonUp, activate
}