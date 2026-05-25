using RpgSaga.Models.Heroes;

namespace RpgSaga.Models.Abilities;

public interface IAbility
{
    string Name { get; }
    string Execute(Hero caster, Hero target);
    int GetDamage();
}
