using RpgSaga.Models.Enums;
using RpgSaga.Models.Heroes;

namespace RpgSaga.Models.Abilities;

public class RetributionStrike : IAbility
{
    public string Name => "Удар возмездия";

    public string Execute(Hero caster, Hero target)
    {
        int bonusDamage = (int)(caster.Strength * 0.3);
        int totalDamage = caster.Strength + bonusDamage;
        target.TakeDamage(totalDamage, ElementType.None);
        return $"{caster.Name} использует {Name} и наносит урон {totalDamage} противнику {target.Name}";
    }

    public int GetDamage() => 0;
}
