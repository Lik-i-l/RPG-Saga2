using Xunit;
using RpgSaga.Models.Heroes;
using RpgSaga.Models.Enums;

namespace RpgSaga.Tests;

public class HeroTests
{
    [Fact]
    public void Knight_TakesDamage_HealthDecreases()
    {
        var knight = new Knight("Артур", 100, 20);
        knight.TakeDamage(30);
        Assert.Equal(70, knight.Health);
    }

    [Fact]
    public void Archer_IceArrows_LimitedUses()
    {
        var archer = new Archer("Леголас", 80, 15);
        Assert.Equal(2, archer.GetIceArrowsRemainingUses());
        
        archer.UseIceArrows();
        Assert.Equal(1, archer.GetIceArrowsRemainingUses());
        
        archer.UseIceArrows();
        Assert.Equal(0, archer.GetIceArrowsRemainingUses());
    }

    [Fact]
    public void Mage_FrostImmunity()
    {
        var mage = new Mage("Гэндальф", 70, 12);
        var healthBefore = mage.Health;
        
        mage.TakeDamage(10, ElementType.Frost);
        
        Assert.Equal(healthBefore, mage.Health);
    }

    [Fact]
    public void Hero_Heal_RestoresHealth()
    {
        var knight = new Knight("Артур", 50, 20);
        knight.Heal(30);
        Assert.Equal(80, knight.Health);
    }
}
