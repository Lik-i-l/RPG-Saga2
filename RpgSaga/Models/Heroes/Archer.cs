using RpgSaga.Models.Abilities;
using RpgSaga.Models.Enums;

namespace RpgSaga.Models.Heroes;

public class Archer : Hero
{
    private readonly List<IAbility> _abilities;
    private readonly Random _random = new();
    private int _iceArrowsRemainingUses = 2; // Лучник может использовать ледяные стрелы 2 раза за бой

    public Archer(string name, int health, int strength) : base(name, health, strength)
    {
        _abilities = new List<IAbility>
        {
            new MeleeAttack(),
            new FrostArrows(10),
            new IceArrows(15)
        };
    }

    public override IAbility GetRandomAbility()
    {
        if (_iceArrowsRemainingUses > 0 && _random.Next(3) == 0) // Шанс использовать ледяные стрелы
        {
            _iceArrowsRemainingUses--;
            return new IceArrows(15);
        }
        return _abilities[_random.Next(_abilities.Count)];
    }

    public override string GetClassType() => "Лучник";

    public override int GetIceArrowsRemainingUses() => _iceArrowsRemainingUses;
    public override void UseIceArrows()
    {
        if (_iceArrowsRemainingUses > 0)
            _iceArrowsRemainingUses--;
    }
    public override void ResetIceArrowsUses() => _iceArrowsRemainingUses = 2;
}
