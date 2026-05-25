using RpgSaga.Models.Abilities;
using RpgSaga.Models.Enums;

namespace RpgSaga.Models.Heroes;

public class Knight : Hero
{
    private readonly List<IAbility> _abilities;
    private readonly Random _random = new();

    public Knight(string name, int health, int strength) : base(name, health, strength)
    {
        _abilities = new List<IAbility>
        {
            new MeleeAttack(),
            new RetributionStrike()
        };
    }

    public override IAbility GetRandomAbility()
    {
        return _abilities[_random.Next(_abilities.Count)];
    }

    public override string GetClassType() => "Рыцарь";

    public override int GetIceArrowsRemainingUses() => 0;
    public override void UseIceArrows() { }
    public override void ResetIceArrowsUses() { }
}
