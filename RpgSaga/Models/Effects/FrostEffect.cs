using RpgSaga.Models.Enums;
using RpgSaga.Models.Heroes;

namespace RpgSaga.Models.Effects;

public class FrostEffect : IEffect
{
    public EffectType Type => EffectType.Frost;
    public int RemainingTurns { get; set; }
    public int DamagePerTurn => 1;
    public ElementType Element => ElementType.Frost;

    public FrostEffect(int duration = 3)
    {
        RemainingTurns = duration;
    }

    public void Apply(Hero target)
    {
        target.TakeDamage(DamagePerTurn, Element);
        RemainingTurns--;
    }

    public IEffect Clone() => new FrostEffect(RemainingTurns);
}
