using RpgSaga.Models.Abilities;
using RpgSaga.Models.Enums;
using RpgSaga.Models.Status;

namespace RpgSaga.Models.Heroes;

public abstract class Hero
{
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int Strength { get; private set; }
    public bool IsAlive => Health > 0;
    
    private readonly EffectManager _effectManager = new();
    public EffectManager EffectManager => _effectManager;
    
    protected ElementType Immunities { get; set; } = ElementType.None;
    
    protected Hero(string name, int health, int strength)
    {
        Name = name;
        Health = health;
        Strength = strength;
    }

    public void TakeDamage(int damage, ElementType element = ElementType.None)
    {
        if (HasImmunity(element))
        {
            Console.WriteLine($"{Name} имеет иммунитет к {element} урону!");
            return;
        }
        
        Health -= damage;
        if (Health < 0) Health = 0;
    }

    public void Heal(int amount)
    {
        Health += amount;
    }

    public bool HasImmunity(ElementType element)
    {
        if (element == ElementType.None) return false;
        return Immunities.HasFlag(element);
    }

    public void ApplyEffects()
    {
        EffectManager.ApplyEffects(this);
    }

    public abstract IAbility GetRandomAbility();
    public abstract string GetClassType();
    public abstract int GetIceArrowsRemainingUses();
    public abstract void UseIceArrows();
    public abstract void ResetIceArrowsUses();
}
