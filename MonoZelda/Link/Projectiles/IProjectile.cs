namespace MonoZelda.Link.Projectiles;

public interface IProjectile
{
	void UpdateProjectile();

	bool hasFinished();

	bool reachedDistance();
}
