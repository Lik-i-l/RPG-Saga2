using Xunit;
using RpgSaga.Services;
using RpgSaga.Models.Heroes;

namespace RpgSaga.Tests;

public class BattleTests
{
    [Fact]
    public void Fight_OneHeroDies()
    {
        var logger = new Logger();
        var battleManager = new BattleManager(logger);
        
        var hero1 = new Knight("Артур", 100, 30);
        var hero2 = new Knight("Враг", 50, 10);
        
        var winner = battleManager.Fight(hero1, hero2);
        
        Assert.True(winner.IsAlive);
        Assert.Equal("Артур", winner.Name);
    }
}
