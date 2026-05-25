using RpgSaga.Models.Enums;
using RpgSaga.Models.Heroes;

namespace RpgSaga.Models.Effects;

public class BurningEffect : IEffect
{
    public EffectType Type => EffectType.Burning;
    public int RemainingTurns { get; set; }
    public int DamagePerTurn => 2;
    public ElementType Element => ElementType.Fire;

    public BurningEffect(int duration = 3)
    {
        RemainingTurns = duration;
    }

    public void Apply(Hero target)
    {
        target.TakeDamage(DamagePerTurn, Element);
        RemainingTurns--;
    }

    public IEffect Clone() => new BurningEffect(RemainingTurns);
}
