using RpgSaga.Models.Effects;
using RpgSaga.Models.Enums;

namespace RpgSaga.Models.Status;

public class EffectManager
{
    private readonly List<IEffect> _effects = new();

    public IReadOnlyList<IEffect> ActiveEffects => _effects.AsReadOnly();

    public void AddEffect(IEffect effect)
    {
        _effects.Add(effect);
    }

    public void ApplyEffects(Hero target)
    {
        for (int i = _effects.Count - 1; i >= 0; i--)
        {
            var effect = _effects[i];
            effect.Apply(target);
            if (effect.RemainingTurns <= 0)
            {
                _effects.RemoveAt(i);
            }
        }
    }

    public void RemoveEffectsByType(EffectType type)
    {
        _effects.RemoveAll(e => e.Type == type);
    }

    public void RemoveAllEffects()
    {
        _effects.Clear();
    }
}
