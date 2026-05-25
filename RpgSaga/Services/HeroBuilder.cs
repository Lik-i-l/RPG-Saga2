using RpgSaga.Models.Enums;
using RpgSaga.Models.Heroes;

namespace RpgSaga.Services;

public class HeroBuilder
{
    private HeroType _type;
    private string _name = "Unknown";
    private int _health = 80;
    private int _strength = 20;

    public HeroBuilder SetType(HeroType type)
    {
        _type = type;
        return this;
    }

    public HeroBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public HeroBuilder SetHealth(int health)
    {
        _health = health;
        return this;
    }

    public HeroBuilder SetStrength(int strength)
    {
        _strength = strength;
        return this;
    }

    public Hero Build()
    {
        return _type switch
        {
            HeroType.Knight => new Knight(_name, _health, _strength),
            HeroType.Archer => new Archer(_name, _health, _strength),
            HeroType.Mage => new Mage(_name, _health, _strength),
            _ => throw new ArgumentException("Unknown hero type")
        };
    }
}
