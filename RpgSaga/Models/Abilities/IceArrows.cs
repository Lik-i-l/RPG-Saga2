using RpgSaga.Models.Heroes;
using RpgSaga.Models.Effects;
using RpgSaga.Models.Enums;

namespace RpgSaga.Models.Abilities;

public class IceArrows : IAbility
{
    public string Name => "Ледяные стрелы (усиленные)";
    private readonly int _damage;

    public IceArrows(int damage = 15)
    {
        _damage = damage;
    }

    public string Execute(Hero caster, Hero target)
    {
        if (target.HasImmunity(ElementType.Ice))
        {
            return $"{target.Name} невосприимчив к усиленным ледяным стрелам! Урон не нанесён.";
        }

        target.TakeDamage(_damage, ElementType.Ice);
        target.EffectManager.AddEffect(new IceEffect(3));
        
        return $"{caster.Name} использует {Name} и наносит урон {_damage} противнику {target.Name}. Противник покрывается льдом на 3 хода!";
    }

    public int GetDamage() => _damage;
}
