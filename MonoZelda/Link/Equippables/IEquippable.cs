using System.Security.AccessControl;

namespace MonoZelda.Link;

public interface IEquippable
{
    void Use(params object[] args);
}
