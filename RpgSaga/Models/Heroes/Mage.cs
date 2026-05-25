using RpgSaga.Models.Abilities;
using RpgSaga.Models.Enums;

namespace RpgSaga.Models.Heroes;

public class Mage : Hero
{
    private readonly List<IAbility> _abilities;
    private readonly Random _random = new();
    private int _iceArrowsRemainingUses = 1; // Маг может использовать ледяные стрелы 1 раз за бой

    public Mage(string name, int health, int strength) : base(name, health, strength)
    {
        Immunities = ElementType.Frost; // Маг невосприимчив к ледяным стрелам
        
        _abilities = new List<IAbility>
        {
            new MeleeAttack(),
            new Heal(20),
            new Cleanse(),
            new IceArrows(12)
        };
    }

    public override IAbility GetRandomAbility()
    {
        if (_iceArrowsRemainingUses > 0 && _random.Next(4) == 0)
        {
            _iceArrowsRemainingUses--;
            return new IceArrows(12);
        }
        return _abilities[_random.Next(_abilities.Count)];
    }

    public override string GetClassType() => "Маг";

    public override int GetIceArrowsRemainingUses() => _iceArrowsRemainingUses;
    public override void UseIceArrows()
    {
        if (_iceArrowsRemainingUses > 0)
            _iceArrowsRemainingUses--;
    }
    public override void ResetIceArrowsUses() => _iceArrowsRemainingUses = 1;
}
