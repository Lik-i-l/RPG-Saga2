using RpgSaga.Models.Heroes;

namespace RpgSaga.Services;

public class BattleManager
{
    private readonly Logger _logger;

    public BattleManager(Logger logger)
    {
        _logger = logger;
    }

    public Hero Fight(Hero hero1, Hero hero2)
    {
        _logger.Log($"\n{hero1.Name} ({hero1.GetClassType()}) vs {hero2.Name} ({hero2.GetClassType()})");
        
        int turn = 0;
        while (hero1.IsAlive && hero2.IsAlive)
        {
            // Применяем эффекты в начале хода
            hero1.ApplyEffects();
            hero2.ApplyEffects();
            
            var (attacker, defender) = turn % 2 == 0 ? (hero1, hero2) : (hero2, hero1);
            
            var ability = attacker.GetRandomAbility();
            var result = ability.Execute(attacker, defender);
            _logger.Log(result);
            
            if (!defender.IsAlive)
            {
                _logger.Log($"{defender.Name} погибает");
                break;
            }
            turn++;
        }
        
        return hero1.IsAlive ? hero1 : hero2;
    }

    public Hero Tournament(List<Hero> heroes)
    {
        int round = 1;
        var currentHeroes = heroes.ToList();

        while (currentHeroes.Count > 1)
        {
            _logger.Log($"\n=== Кон {round} ===");
            var winners = new List<Hero>();

            for (int i = 0; i < currentHeroes.Count; i += 2)
            {
                if (i + 1 < currentHeroes.Count)
                {
                    var winner = Fight(currentHeroes[i], currentHeroes[i + 1]);
                    winners.Add(winner);
                }
                else
                {
                    winners.Add(currentHeroes[i]);
                }
            }

            currentHeroes = winners;
            round++;
        }

        _logger.Log($"\n=== ПОБЕДИТЕЛЬ: {currentHeroes[0].Name} ({currentHeroes[0].GetClassType()}) ===");
        return currentHeroes[0];
    }
}
