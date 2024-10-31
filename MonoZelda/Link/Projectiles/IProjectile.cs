using Microsoft.Xna.Framework;

namespace MonoZelda.Link.Projectiles;

public interface IProjectile
{
	void UpdateProjectile();

	void FinishProjectile();

	bool hasFinished();

	Rectangle getCollisionRectangle();
}
