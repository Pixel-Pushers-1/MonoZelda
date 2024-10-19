using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoZelda.Sprites;

internal static class SpriteDrawer
{
    private class DictComparator : IComparer<Drawable>
    {
        public int Compare(Drawable x, Drawable y)
        {
            if (x.priority < y.priority)
                return -1;
            if (x.priority == y.priority)
                return 0;
            return 1;
        }
    }

    private class Drawable {
        public IDrawable iDrawable;
        public int priority;
        public bool isGizmo;
    }

    public static bool DrawGizmos { get; set; } = false;

    private static readonly List<Drawable> drawables = new();
    private static readonly DictComparator comparator = new();

    public static void RegisterDrawable(IDrawable iDrawable, int priority, bool isGizmo = false)
    {
        Drawable newDrawable = new() {
            iDrawable = iDrawable,
            priority = priority,
            isGizmo = isGizmo
        };
        drawables.Add(newDrawable);
        drawables.Sort(comparator);
        //optimization: only sort list after all initial SpriteDicts are added
    }

    public static void UnregisterDrawable(IDrawable iDrawable)
    {
        drawables.RemoveAll(x => x.iDrawable == iDrawable);
    }

    public static void Reset()
    {
        drawables.Clear();
    }

    public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach (Drawable drawable in drawables)
        { 
            if (DrawGizmos || !drawable.isGizmo) {
                drawable.iDrawable.Draw(spriteBatch, gameTime);
            }
        }
    }
}

