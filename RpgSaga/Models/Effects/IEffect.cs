using RpgSaga.Models.Enums;
using RpgSaga.Models.Heroes;

namespace RpgSaga.Models.Effects;

public interface IEffect
{
    EffectType Type { get; }
    int RemainingTurns { get; set; }
    int DamagePerTurn { get; }
    ElementType Element { get; }
    void Apply(Hero target);
    IEffect Clone();
}
