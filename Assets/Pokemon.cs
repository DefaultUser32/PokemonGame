using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Pokemon
{
    public string name;
    public string type1;
    public string type2;
    public int level;
    public int spriteIndex;

    public int healthStat;
    public int physicalAttack;
    public int physicalDefense;
    public int specialAttack;
    public int specialDefense;
    public int speed;

    public int health;
    public List<Attack> moves;

    public Pokemon(string _name, string _type1, string _type2, int _level, int _spriteIndex, int _healthStat, int _physicalAttack, int _physicalDefense, int _specialAttack, int _specialDefense, int _speed, List<Attack> _moves) {
        name = _name;
        type1= _type1;
        type2 = _type2;
        level = _level;
        spriteIndex = _spriteIndex;

        healthStat = _healthStat;
        physicalAttack= _physicalAttack;
        physicalDefense = _physicalDefense;
        specialAttack = _specialAttack;
        specialDefense= _specialDefense;
        speed= _speed;

        health = healthStat;

        moves= _moves;
    }
    public Pokemon Copy()
    {
        return (Pokemon) this.MemberwiseClone();
    }
}
