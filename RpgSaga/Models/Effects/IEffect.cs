using RpgSaga.Models.Enums;

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
