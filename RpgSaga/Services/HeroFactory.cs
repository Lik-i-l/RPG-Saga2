using RpgSaga.Models.Enums;
using RpgSaga.Models.Heroes;

namespace RpgSaga.Services;

public class HeroFactory
{
    private readonly IRandomizer _randomizer;
    private readonly string[] _names = {
        "Артур", "Эльдар", "Гэндальф", "Вильямс", "Леголас", "Арагорн",
        "Гимли", "Фродо", "Саурон", "Боромир", "Эовин", "Трандуил"
    };

    public HeroFactory(IRandomizer randomizer)
    {
        _randomizer = randomizer;
    }

    // Фабричный метод для создания одного героя с заданными параметрами
    public Hero CreateHero(HeroType type, string name, int health, int strength)
    {
        return type switch
        {
            HeroType.Knight => new Knight(name, health, strength),
            HeroType.Archer => new Archer(name, health, strength),
            HeroType.Mage => new Mage(name, health, strength),
            _ => throw new ArgumentException("Unknown hero type")
        };
    }

    // Фабричный метод для создания массива случайных героев
    public List<Hero> CreateRandomHeroes(int count)
    {
        var heroes = new List<Hero>();
        var types = Enum.GetValues<HeroType>();

        for (int i = 0; i < count; i++)
        {
            var type = types[_randomizer.Next(0, types.Length)];
            var name = _names[_randomizer.Next(0, _names.Length)];
            var health = _randomizer.Next(50, 131);
            var strength = _randomizer.Next(10, 41);
            
            heroes.Add(CreateHero(type, name, health, strength));
        }
        
        return heroes;
    }
}
