using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

public static class MapBuilder
{

    public static void DrawEdgelines(SpriteBatch _batch, List<MapSectorVertice> _mapVertices)
    {
        if (_mapVertices.Count() < 2)
            return;

        for (int i = 1; i < _mapVertices.Count(); i++)
        {
            Primitives.DrawLine(_batch, _mapVertices[i - 1].GetPosition(), _mapVertices[i].GetPosition(), Color.White);
        }

        Primitives.DrawLine(_batch, _mapVertices[0].GetPosition(), _mapVertices.Last().GetPosition(), Color.White);
    }
}