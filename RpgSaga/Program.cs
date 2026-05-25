using RpgSaga.Services;

Console.WriteLine("=== RPG SAGA 2 ===\n");
Console.WriteLine("1. Случайный турнир");
Console.WriteLine("2. Создать своего героя");
Console.Write("Выберите вариант: ");
var choice = Console.ReadLine();

var logger = new Logger();
var randomizer = new Randomizer();
var factory = new HeroFactory(randomizer);
var battleManager = new BattleManager(logger);

if (choice == "1")
{
    Console.Write("Введите количество игроков (чётное число): ");
    var count = int.Parse(Console.ReadLine() ?? "6");
    
    var heroes = factory.CreateRandomHeroes(count);
    
    Console.WriteLine("\n=== УЧАСТНИКИ ===");
    foreach (var hero in heroes)
    {
        Console.WriteLine($"{hero.Name} ({hero.GetClassType()}) - Здоровье: {hero.Health}, Сила: {hero.Strength}");
    }
    
    var winner = battleManager.Tournament(heroes);
    
    Console.WriteLine($"\nПобедитель: {winner.Name} ({winner.GetClassType()})!");
}
else if (choice == "2")
{
    var builder = new HeroBuilder();
    
    Console.Write("Введите имя героя: ");
    builder.SetName(Console.ReadLine() ?? "Герой");
    
    Console.WriteLine("Выберите класс: 1 - Рыцарь, 2 - Лучник, 3 - Маг");
    var classChoice = Console.ReadLine();
    builder.SetType(classChoice switch
    {
        "1" => Models.Enums.HeroType.Knight,
        "2" => Models.Enums.HeroType.Archer,
        "3" => Models.Enums.HeroType.Mage,
        _ => Models.Enums.HeroType.Knight
    });
    
    Console.Write("Введите здоровье (50-130): ");
    builder.SetHealth(int.Parse(Console.ReadLine() ?? "80"));
    
    Console.Write("Введите силу (10-40): ");
    builder.SetStrength(int.Parse(Console.ReadLine() ?? "20"));
    
    var hero = builder.Build();
    
    Console.WriteLine($"\nВаш герой: {hero.Name} ({hero.GetClassType()}) - Здоровье: {hero.Health}, Сила: {hero.Strength}");
    
    // Создаём противников
    var opponents = factory.CreateRandomHeroes(2);
    
    Console.WriteLine("\n=== ПРОТИВНИКИ ===");
    foreach (var opp in opponents)
    {
        Console.WriteLine($"{opp.Name} ({opp.GetClassType()}) - Здоровье: {opp.Health}, Сила: {opp.Strength}");
    }
    
    var allHeroes = new List<Models.Heroes.Hero> { hero };
    allHeroes.AddRange(opponents);
    
    var winner = battleManager.Tournament(allHeroes);
    Console.WriteLine($"\nРезультат: {(winner == hero ? "ВЫ ПОБЕДИЛИ!" : "ВЫ ПРОИГРАЛИ...")}");
}

Console.WriteLine("\n=== ПОЛНЫЕ ЛОГИ ===");
foreach (var log in logger.GetLogs())
{
    Console.WriteLine(log);
}
