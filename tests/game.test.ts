import { 
    Knight, Archer, Mage, Logger, Game, HeroType, HeroFactory, Randomizer,
    MeleeAttack, RetributionStrike, FireArrows, IceArrows, Charm, Heal, Cleanse,
    BurningEffect, FrostEffect, IceEffect
} from '../src/index';

describe('Knight', () => {
    test('should take damage correctly', () => {
        const knight = new Knight('Артур', 100, 20);
        knight.takeDamage(30);
        expect(knight.health).toBe(70);
    });

    test('should not go below zero', () => {
        const knight = new Knight('Артур', 100, 20);
        knight.takeDamage(150);
        expect(knight.health).toBe(0);
        expect(knight.isAlive).toBe(false);
    });

    test('should have correct class type', () => {
        const knight = new Knight('Артур', 100, 20);
        expect(knight.getClassType()).toBe('Рыцарь');
    });
});

describe('Archer', () => {
    test('should have 2 ice arrows uses', () => {
        const archer = new Archer('Леголас', 80, 15);
        expect(archer.getIceArrowsRemainingUses()).toBe(2);
    });

    test('should decrease ice arrows when used', () => {
        const archer = new Archer('Леголас', 80, 15);
        archer.useIceArrows();
        expect(archer.getIceArrowsRemainingUses()).toBe(1);
    });

    test('should have correct class type', () => {
        const archer = new Archer('Леголас', 80, 15);
        expect(archer.getClassType()).toBe('Лучник');
    });
});

describe('Mage', () => {
    test('should have immunity to frost', () => {
        const mage = new Mage('Гэндальф', 70, 12);
        const healthBefore = mage.health;
        mage.takeDamage(10, 'frost' as any);
        expect(mage.health).toBe(healthBefore);
    });

    test('should have 1 ice arrow use', () => {
        const mage = new Mage('Гэндальф', 70, 12);
        expect(mage.getIceArrowsRemainingUses()).toBe(1);
    });

    test('should have correct class type', () => {
        const mage = new Mage('Гэндальф', 70, 12);
        expect(mage.getClassType()).toBe('Маг');
    });
});

describe('Game', () => {
    test('should have a winner', () => {
        const logger = new Logger();
        const heroes = [
            new Knight('Артур', 100, 25),
            new Archer('Леголас', 80, 15)
        ];
        const game = new Game(heroes, logger);
        const winner = game.start();
        expect(winner).toBeDefined();
        expect(winner.isAlive).toBe(true);
    });
});

describe('HeroFactory', () => {
    test('should create random heroes', () => {
        const randomizer = new Randomizer();
        const factory = new HeroFactory(randomizer);
        const heroes = factory.createRandomHeroes(4);
        expect(heroes.length).toBe(4);
        heroes.forEach(hero => {
            expect(hero.name).toBeDefined();
            expect(hero.health).toBeGreaterThan(0);
        });
    });
});

describe('Abilities', () => {
    test('MeleeAttack deals damage', () => {
        const knight = new Knight('Артур', 100, 20);
        const target = new Knight('Враг', 100, 10);
        const attack = new MeleeAttack();
        attack.execute(knight, target);
        expect(target.health).toBe(80);
    });

    test('RetributionStrike deals bonus damage', () => {
        const knight = new Knight('Артур', 100, 20);
        const target = new Knight('Враг', 100, 10);
        const attack = new RetributionStrike();
        attack.execute(knight, target);
        expect(target.health).toBe(100 - (20 + Math.floor(20 * 0.3)));
    });

    test('FireArrows adds burning effect', () => {
        const archer = new Archer('Леголас', 80, 15);
        const target = new Knight('Враг', 100, 10);
        const arrows = new FireArrows();
        arrows.execute(archer, target);
        expect(target.effectManager.activeEffects.length).toBeGreaterThan(0);
    });
});
