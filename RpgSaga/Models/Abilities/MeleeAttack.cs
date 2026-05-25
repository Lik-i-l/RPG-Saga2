using RpgSaga.Models.Enums;
using RpgSaga.Models.Heroes;

namespace RpgSaga.Models.Abilities;

public class MeleeAttack : IAbility
{
    public string Name => "Обычная атака";

    public string Execute(Hero caster, Hero target)
    {
        int damage = caster.Strength;
        target.TakeDamage(damage, ElementType.None);
        return $"{caster.Name} наносит урон {damage} противнику {target.Name}";
    }

    public int GetDamage() => 0;
}
