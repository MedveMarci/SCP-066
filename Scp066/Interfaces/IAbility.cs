using Exiled.API.Features;

namespace Scp066.Interfaces;

public interface IAbility
{
    string Name { get; }
    string Description { get; }
    float Cooldown { get; }
    void Register();
    void Unregister();
}