using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Link.Projectiles;

public interface IProjectile
{
	public Vector2 ProjectilePosition { get; set; }

	void Setup();

	void Update();

	void FinishProjectile();

	bool hasFinished();

	Rectangle getCollisionRectangle();
}
