using RpgSaga.Models.Enums;

namespace RpgSaga.Models.Effects;

public class IceEffect : IEffect
{
    public EffectType Type => EffectType.Ice;
    public int RemainingTurns { get; set; }
    public int DamagePerTurn => 3;
    public ElementType Element => ElementType.Ice;

    public IceEffect(int duration = 3)
    {
        RemainingTurns = duration;
    }

    public void Apply(Hero target)
    {
        target.TakeDamage(DamagePerTurn, Element);
        RemainingTurns--;
    }

    public IEffect Clone() => new IceEffect(RemainingTurns);
}
