using RpgSaga.Models.Heroes;
using RpgSaga.Models.Enums;

namespace RpgSaga.Models.Abilities;

public class Cleanse : IAbility
{
    public string Name => "Очищение";

    public string Execute(Hero caster, Hero target)
    {
        var effectsRemoved = target.EffectManager.ActiveEffects.Count;
        target.EffectManager.RemoveAllEffects();
        return $"{caster.Name} использует {Name} и снимает все эффекты с {target.Name} (удалено {effectsRemoved} эффектов)!";
    }

    public int GetDamage() => 0;
}
