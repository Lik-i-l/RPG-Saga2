using Xunit;
using RpgSaga.Models.Heroes;
using RpgSaga.Models.Abilities;

namespace RpgSaga.Tests;

public class AbilityTests
{
    [Fact]
    public void RetributionStrike_DealsBonusDamage()
    {
        var knight = new Knight("Артур", 100, 20);
        var target = new Knight("Враг", 100, 10);
        
        var ability = new RetributionStrike();
        ability.Execute(knight, target);
        
        int expectedHealth = 100 - (20 + (int)(20 * 0.3));
        Assert.Equal(expectedHealth, target.Health);
    }

    [Fact]
    public void Heal_DoesNotGoesBelowZero()
    {
        var mage = new Mage("Гэндальф", 10, 10);
        var ability = new Heal(30);
        ability.Execute(mage, mage);
        
        Assert.Equal(40, mage.Health);
    }
}
