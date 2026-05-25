using RpgSaga.Models.Heroes;

namespace RpgSaga.Models.Abilities;

public class Heal : IAbility
{
    public string Name => "Лечение";
    private readonly int _healAmount;

    public Heal(int healAmount = 20)
    {
        _healAmount = healAmount;
    }

    public string Execute(Hero caster, Hero target)
    {
        caster.Heal(_healAmount);
        return $"{caster.Name} использует {Name} и восстанавливает {_healAmount} здоровья!";
    }

    public int GetDamage() => 0;
}
