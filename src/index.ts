// ==================== ENUMS ====================
export enum HeroType {
    Knight = 'Рыцарь',
    Archer = 'Лучник',
    Mage = 'Маг'
}

export enum ElementType {
    None = 'none',
    Fire = 'fire',
    Frost = 'frost',
    Ice = 'ice'
}

export enum EffectType {
    Burning = 'burning',
    Frost = 'frost',
    Ice = 'ice'
}

// ==================== EFFECTS ====================
export interface IEffect {
    type: EffectType;
    remainingTurns: number;
    damagePerTurn: number;
    element: ElementType;
    apply(target: Hero): void;
    clone(): IEffect;
}

export class BurningEffect implements IEffect {
    type: EffectType = EffectType.Burning;
    remainingTurns: number;
    damagePerTurn: number = 2;
    element: ElementType = ElementType.Fire;

    constructor(duration: number = 3) {
        this.remainingTurns = duration;
    }

    apply(target: Hero): void {
        target.takeDamage(this.damagePerTurn, this.element);
        this.remainingTurns--;
    }

    clone(): IEffect {
        return new BurningEffect(this.remainingTurns);
    }
}

export class FrostEffect implements IEffect {
    type: EffectType = EffectType.Frost;
    remainingTurns: number;
    damagePerTurn: number = 1;
    element: ElementType = ElementType.Frost;

    constructor(duration: number = 3) {
        this.remainingTurns = duration;
    }

    apply(target: Hero): void {
        target.takeDamage(this.damagePerTurn, this.element);
        this.remainingTurns--;
    }

    clone(): IEffect {
        return new FrostEffect(this.remainingTurns);
    }
}

export class IceEffect implements IEffect {
    type: EffectType = EffectType.Ice;
    remainingTurns: number;
    damagePerTurn: number = 3;
    element: ElementType = ElementType.Ice;

    constructor(duration: number = 3) {
        this.remainingTurns = duration;
    }

    apply(target: Hero): void {
        target.takeDamage(this.damagePerTurn, this.element);
        this.remainingTurns--;
    }

    clone(): IEffect {
        return new IceEffect(this.remainingTurns);
    }
}

// ==================== ABILITIES ====================
export interface IAbility {
    name: string;
    execute(caster: Hero, target: Hero): string;
    getDamage(): number;
}

export class MeleeAttack implements IAbility {
    name: string = 'Обычная атака';

    execute(caster: Hero, target: Hero): string {
        const damage = caster.strength;
        target.takeDamage(damage, ElementType.None);
        return `${caster.name} наносит урон ${damage} противнику ${target.name}`;
    }

    getDamage(): number {
        return 0;
    }
}

export class RetributionStrike implements IAbility {
    name: string = 'Удар возмездия';

    execute(caster: Hero, target: Hero): string {
        const bonusDamage = Math.floor(caster.strength * 0.3);
        const totalDamage = caster.strength + bonusDamage;
        target.takeDamage(totalDamage, ElementType.None);
        return `${caster.name} использует ${this.name} и наносит урон ${totalDamage} противнику ${target.name}`;
    }

    getDamage(): number {
        return 0;
    }
}

export class FireArrows implements IAbility {
    name: string = 'Огненные стрелы';
    private used: boolean = false;

    execute(caster: Hero, target: Hero): string {
        if (!this.used) {
            this.used = true;
            target.effectManager.addEffect(new BurningEffect(3));
            return `${caster.name} использует ${this.name}! Противник ${target.name} загорелся и будет терять по 2 HP каждый ход.`;
        }
        return `${caster.name} пытается использовать ${this.name}, но способность уже использована!`;
    }

    getDamage(): number {
        return 0;
    }
}

export class IceArrows implements IAbility {
    name: string = 'Ледяные стрелы';
    private damage: number;

    constructor(damage: number = 10) {
        this.damage = damage;
    }

    execute(caster: Hero, target: Hero): string {
        if (target.hasImmunity(ElementType.Ice)) {
            return `${target.name} невосприимчив к ледяным стрелам! Урон не нанесён.`;
        }

        target.takeDamage(this.damage, ElementType.Ice);
        target.effectManager.addEffect(new IceEffect(3));
        
        return `${caster.name} использует ${this.name} и наносит урон ${this.damage} противнику ${target.name}. Противник покрывается льдом на 3 хода!`;
    }

    getDamage(): number {
        return this.damage;
    }
}

export class Charm implements IAbility {
    name: string = 'Заворожение';

    execute(caster: Hero, target: Hero): string {
        if (target instanceof Mage) {
            (target as Mage).setCharmed(true);
        }
        return `${caster.name} использует ${this.name}! Противник ${target.name} пропускает ход.`;
    }

    getDamage(): number {
        return 0;
    }
}

export class Heal implements IAbility {
    name: string = 'Лечение';
    private healAmount: number;

    constructor(healAmount: number = 20) {
        this.healAmount = healAmount;
    }

    execute(caster: Hero, target: Hero): string {
        caster.heal(this.healAmount);
        return `${caster.name} использует ${this.name} и восстанавливает ${this.healAmount} здоровья!`;
    }

    getDamage(): number {
        return 0;
    }
}

export class Cleanse implements IAbility {
    name: string = 'Очищение';

    execute(caster: Hero, target: Hero): string {
        const effectsRemoved = target.effectManager.activeEffects.length;
        target.effectManager.removeAllEffects();
        return `${caster.name} использует ${this.name} и снимает все эффекты с ${target.name} (удалено ${effectsRemoved} эффектов)!`;
    }

    getDamage(): number {
        return 0;
    }
}

// ==================== EFFECT MANAGER ====================
export class EffectManager {
    private _effects: IEffect[] = [];

    get activeEffects(): IEffect[] {
        return [...this._effects];
    }

    addEffect(effect: IEffect): void {
        this._effects.push(effect);
    }

    applyEffects(target: Hero): void {
        for (let i = this._effects.length - 1; i >= 0; i--) {
            const effect = this._effects[i];
            effect.apply(target);
            if (effect.remainingTurns <= 0) {
                this._effects.splice(i, 1);
            }
        }
    }

    removeEffectsByType(type: string): void {
        this._effects = this._effects.filter(e => e.type !== type);
    }

    removeAllEffects(): void {
        this._effects = [];
    }
}

// ==================== HERO ====================
export abstract class Hero {
    private _name: string;
    private _health: number;
    private _strength: number;
    private _effectManager: EffectManager = new EffectManager();
    protected immunities: string[] = [];

    constructor(name: string, health: number, strength: number) {
        this._name = name;
        this._health = health;
        this._strength = strength;
    }

    get name(): string { return this._name; }
    get health(): number { return this._health; }
    get strength(): number { return this._strength; }
    get isAlive(): boolean { return this._health > 0; }
    get effectManager(): EffectManager { return this._effectManager; }

    takeDamage(damage: number, element: ElementType = ElementType.None): void {
        if (this.hasImmunity(element)) {
            console.log(`${this._name} имеет иммунитет к ${element} урону!`);
            return;
        }
        this._health -= damage;
        if (this._health < 0) this._health = 0;
    }

    heal(amount: number): void {
        this._health += amount;
    }

    hasImmunity(element: ElementType): boolean {
        return this.immunities.includes(element);
    }

    applyEffects(): void {
        this._effectManager.applyEffects(this);
    }

    abstract getRandomAbility(): IAbility;
    abstract getClassType(): string;
    abstract getIceArrowsRemainingUses(): number;
    abstract useIceArrows(): void;
    abstract resetIceArrowsUses(): void;
}

// ==================== KNIGHT ====================
export class Knight extends Hero {
    private abilities: IAbility[] = [new MeleeAttack(), new RetributionStrike()];

    getRandomAbility(): IAbility {
        const randomIndex = Math.floor(Math.random() * this.abilities.length);
        return this.abilities[randomIndex];
    }

    getClassType(): string {
        return 'Рыцарь';
    }

    getIceArrowsRemainingUses(): number {
        return 0;
    }

    useIceArrows(): void {}

    resetIceArrowsUses(): void {}
}

// ==================== ARCHER ====================
export class Archer extends Hero {
    private abilities: IAbility[] = [new MeleeAttack(), new FireArrows()];
    private iceArrowsRemaining: number = 2;

    getRandomAbility(): IAbility {
        if (this.iceArrowsRemaining > 0 && Math.random() < 0.3) {
            this.iceArrowsRemaining--;
            return new IceArrows(10);
        }
        const randomIndex = Math.floor(Math.random() * this.abilities.length);
        return this.abilities[randomIndex];
    }

    getClassType(): string {
        return 'Лучник';
    }

    getIceArrowsRemainingUses(): number {
        return this.iceArrowsRemaining;
    }

    useIceArrows(): void {
        if (this.iceArrowsRemaining > 0) {
            this.iceArrowsRemaining--;
        }
    }

    resetIceArrowsUses(): void {
        this.iceArrowsRemaining = 2;
    }
}

// ==================== MAGE ====================
export class Mage extends Hero {
    private abilities: IAbility[] = [new MeleeAttack(), new Charm(), new Heal(), new Cleanse()];
    private iceArrowsRemaining: number = 1;
    private charmed: boolean = false;

    constructor(name: string, health: number, strength: number) {
        super(name, health, strength);
        this.immunities = [ElementType.Frost];
    }

    getRandomAbility(): IAbility {
        if (this.iceArrowsRemaining > 0 && Math.random() < 0.25) {
            this.iceArrowsRemaining--;
            return new IceArrows(12);
        }
        const randomIndex = Math.floor(Math.random() * this.abilities.length);
        return this.abilities[randomIndex];
    }

    isCharmed(): boolean {
        if (this.charmed) {
            this.charmed = false;
            return true;
        }
        return false;
    }

    setCharmed(value: boolean): void {
        this.charmed = value;
    }

    getClassType(): string {
        return 'Маг';
    }

    getIceArrowsRemainingUses(): number {
        return this.iceArrowsRemaining;
    }

    useIceArrows(): void {
        if (this.iceArrowsRemaining > 0) {
            this.iceArrowsRemaining--;
        }
    }

    resetIceArrowsUses(): void {
        this.iceArrowsRemaining = 1;
    }
}

// ==================== LOGGER ====================
export class Logger {
    private logs: string[] = [];

    log(message: string): void {
        console.log(message);
        this.logs.push(message);
    }

    getLogs(): string[] {
        return [...this.logs];
    }

    clear(): void {
        this.logs = [];
    }
}

// ==================== RANDOMIZER ====================
export interface IRandomizer {
    nextInt(min: number, max: number): number;
    nextDouble(): number;
}

export class Randomizer implements IRandomizer {
    nextInt(min: number, max: number): number {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    nextDouble(): number {
        return Math.random();
    }
}

// ==================== HERO FACTORY ====================
export class HeroFactory {
    private names: string[] = [
        'Артур', 'Эльдар', 'Гэндальф', 'Вильямс', 'Леголас', 'Арагорн',
        'Гимли', 'Фродо', 'Саурон', 'Боромир', 'Эовин', 'Трандуил'
    ];

    constructor(private randomizer: IRandomizer) {}

    createHero(type: HeroType, name: string, health: number, strength: number): Hero {
        switch (type) {
            case HeroType.Knight:
                return new Knight(name, health, strength);
            case HeroType.Archer:
                return new Archer(name, health, strength);
            case HeroType.Mage:
                return new Mage(name, health, strength);
            default:
                throw new Error(`Unknown hero type: ${type}`);
        }
    }

    createRandomHeroes(count: number): Hero[] {
        const heroes: Hero[] = [];
        const types = [HeroType.Knight, HeroType.Archer, HeroType.Mage];

        for (let i = 0; i < count; i++) {
            const type = types[this.randomizer.nextInt(0, types.length - 1)];
            const name = this.names[this.randomizer.nextInt(0, this.names.length - 1)];
            const health = this.randomizer.nextInt(50, 130);
            const strength = this.randomizer.nextInt(10, 40);
            heroes.push(this.createHero(type, name, health, strength));
        }

        return heroes;
    }
}

// ==================== HERO BUILDER ====================
export class HeroBuilder {
    private type: HeroType = HeroType.Knight;
    private name: string = 'Герой';
    private health: number = 80;
    private strength: number = 20;

    setType(type: HeroType): this {
        this.type = type;
        return this;
    }

    setName(name: string): this {
        this.name = name;
        return this;
    }

    setHealth(health: number): this {
        this.health = health;
        return this;
    }

    setStrength(strength: number): this {
        this.strength = strength;
        return this;
    }

    build(): Hero {
        switch (this.type) {
            case HeroType.Knight:
                return new Knight(this.name, this.health, this.strength);
            case HeroType.Archer:
                return new Archer(this.name, this.health, this.strength);
            case HeroType.Mage:
                return new Mage(this.name, this.health, this.strength);
            default:
                throw new Error(`Unknown hero type: ${this.type}`);
        }
    }
}

// ==================== GAME ====================
export class Game {
    private heroes: Hero[];
    private logger: Logger;

    constructor(heroes: Hero[], logger: Logger) {
        this.heroes = heroes;
        this.logger = logger;
    }

    private fight(hero1: Hero, hero2: Hero): Hero {
        this.logger.log(`\n(${hero1.getClassType()}) ${hero1.name} vs (${hero2.getClassType()}) ${hero2.name}`);
        
        let turn = 0;
        while (hero1.isAlive && hero2.isAlive) {
            const attacker = turn % 2 === 0 ? hero1 : hero2;
            const defender = turn % 2 === 0 ? hero2 : hero1;

            attacker.applyEffects();
            defender.applyEffects();

            if (defender instanceof Mage && (defender as Mage).isCharmed()) {
                this.logger.log(`${defender.name} заворожён и пропускает ход!`);
                turn++;
                continue;
            }

            const useSpecial = Math.random() < 0.3;
            let result: string;
            
            if (useSpecial) {
                result = attacker.getRandomAbility().execute(attacker, defender);
            } else {
                const damage = attacker.strength;
                defender.takeDamage(damage);
                result = `${attacker.name} наносит урон ${damage} противнику ${defender.name}`;
            }
            
            this.logger.log(result);

            if (!defender.isAlive) {
                this.logger.log(`${defender.name} погибает`);
                break;
            }
            turn++;
        }
        
        return hero1.isAlive ? hero1 : hero2;
    }

    start(): Hero {
        let round = 1;
        let currentHeroes = [...this.heroes];

        while (currentHeroes.length > 1) {
            this.logger.log(`\n=== Кон ${round} ===`);
            const winners: Hero[] = [];

            for (let i = 0; i < currentHeroes.length; i += 2) {
                if (i + 1 < currentHeroes.length) {
                    const winner = this.fight(currentHeroes[i], currentHeroes[i + 1]);
                    winners.push(winner);
                } else {
                    winners.push(currentHeroes[i]);
                }
            }

            currentHeroes = winners;
            round++;
        }

        this.logger.log(`\n=== ПОБЕДИТЕЛЬ: (${currentHeroes[0].getClassType()}) ${currentHeroes[0].name} ===`);
        return currentHeroes[0];
    }
}

// ==================== MAIN ====================
import * as readline from 'readline';

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

const logger = new Logger();
const randomizer = new Randomizer();
const factory = new HeroFactory(randomizer);

console.log('=== RPG SAGA 2 ===\n');
console.log('1. Случайный турнир');
console.log('2. Создать своего героя');
console.log('3. Выход');

rl.question('Выберите вариант: ', (choice) => {
    if (choice === '1') {
        rl.question('Введите количество игроков (чётное число): ', (countStr) => {
            const count = parseInt(countStr) || 6;
            const heroes = factory.createRandomHeroes(count);
            
            console.log('\n=== УЧАСТНИКИ ===');
            heroes.forEach(hero => {
                console.log(`${hero.name} (${hero.getClassType()}) - Здоровье: ${hero.health}, Сила: ${hero.strength}`);
            });
            
            const game = new Game(heroes, logger);
            game.start();
            
            console.log('\n=== ЛОГИ БИТВЫ ===');
            logger.getLogs().forEach(log => console.log(log));
            
            rl.close();
        });
    } else if (choice === '2') {
        const builder = new HeroBuilder();
        
        rl.question('Введите имя героя: ', (name) => {
            builder.setName(name);
            
            rl.question('Выберите класс (1 - Рыцарь, 2 - Лучник, 3 - Маг): ', (classChoice) => {
                switch (classChoice) {
                    case '1': builder.setType(HeroType.Knight); break;
                    case '2': builder.setType(HeroType.Archer); break;
                    case '3': builder.setType(HeroType.Mage); break;
                    default: builder.setType(HeroType.Knight);
                }
                
                rl.question('Введите здоровье (50-130): ', (healthStr) => {
                    builder.setHealth(parseInt(healthStr) || 80);
                    
                    rl.question('Введите силу (10-40): ', (strengthStr) => {
                        builder.setStrength(parseInt(strengthStr) || 20);
                        
                        const hero = builder.build();
                        console.log(`\nВаш герой: ${hero.name} (${hero.getClassType()}) - Здоровье: ${hero.health}, Сила: ${hero.strength}`);
                        
                        const opponents = factory.createRandomHeroes(2);
                        console.log('\n=== ПРОТИВНИКИ ===');
                        opponents.forEach(opp => {
                            console.log(`${opp.name} (${opp.getClassType()}) - Здоровье: ${opp.health}, Сила: ${opp.strength}`);
                        });
                        
                        const allHeroes = [hero, ...opponents];
                        const game = new Game(allHeroes, logger);
                        game.start();
                        
                        console.log('\n=== ЛОГИ БИТВЫ ===');
                        logger.getLogs().forEach(log => console.log(log));
                        
                        rl.close();
                    });
                });
            });
        });
    } else {
        rl.close();
    }
});
