using RpgSaga.Models.Heroes;
using RpgSaga.Models.Effects;
using RpgSaga.Models.Enums;

namespace RpgSaga.Models.Abilities;

public class FrostArrows : IAbility
{
    public string Name => "Ледяные стрелы";
    private readonly int _damage;

    public FrostArrows(int damage = 10)
    {
        _damage = damage;
    }

    public string Execute(Hero caster, Hero target)
    {
        // Проверка иммунитета
        if (target.HasImmunity(ElementType.Frost))
        {
            return $"{target.Name} невосприимчив к ледяным стрелам! Урон не нанесён.";
        }

        target.TakeDamage(_damage, ElementType.Frost);
        
        // Добавляем эффект мороза
        target.EffectManager.AddEffect(new FrostEffect(3));
        
        return $"{caster.Name} использует {Name} и наносит урон {_damage} противнику {target.Name}. Противник замёрз на 3 хода!";
    }

    public int GetDamage() => _damage;
}
