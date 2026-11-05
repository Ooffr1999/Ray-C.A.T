using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class MapSectorVertice : Button
{
    public Sector parent;

    public MapSectorVertice(SpriteBatch _batch, InputManager _input) : base(_batch, _input)
    {
        SetWidth(10);
        SetHeight(10);
        SetPosition(new Vector2(100, 100));
        SetLabel("");

        SetEvent(SetMove);
        SetButtonEventType(ButtonEvent.hold);
    }

    public override void Update()
    {
        base.Update();
        //SetPosition(input.Mouse.position.ToVector2() - new Vector2(GetWidth() / 2, GetHeight() / 2));
    }

    public void SetMove()
    {
        if (input.Keyboard.IsKeyDown(Keys.Space))
        {
            foreach(MapSectorVertice v in parent.vertices)
            {
                v.color = Color.Yellow;
                v.SetPosition(new Vector2(GetPosition().X + input.Mouse.XDelta, GetPosition().Y + input.Mouse.YDelta));
            }
        }

        else SetPosition(new Vector2(GetPosition().X + input.Mouse.XDelta, GetPosition().Y + input.Mouse.YDelta));
    }

    public override void Draw()
    {
        base.Draw();
    }
}