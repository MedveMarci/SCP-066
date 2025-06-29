using UnityEngine;

namespace Scp066.Interfaces;
public interface IAbility
{
    string Name { get; }
    string Description { get; }
    int KeyId { get; }
    string KeyCode { get; }
    float Cooldown { get; }
    void Register();
    void Unregister();
}