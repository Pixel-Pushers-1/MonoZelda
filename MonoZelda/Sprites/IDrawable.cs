using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IDrawable
{
	public bool Enabled { get; set; }
	public void Draw(SpriteBatch spriteBatch);
}
