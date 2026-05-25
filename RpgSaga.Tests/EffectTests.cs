using Xunit;
using RpgSaga.Models.Effects;
using RpgSaga.Models.Heroes;

namespace RpgSaga.Tests;

public class EffectTests
{
    [Fact]
    public void BurningEffect_AppliesDamageOverTime()
    {
        var knight = new Knight("Артур", 100, 20);
        var effect = new BurningEffect(3);
        
        effect.Apply(knight);
        Assert.Equal(98, knight.Health);
        
        effect.Apply(knight);
        Assert.Equal(96, knight.Health);
        
        effect.Apply(knight);
        Assert.Equal(94, knight.Health);
    }
}
