using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Attack
{
    public string attackName;
    public string attackType;
    public int power;
    public float accuracy;
    public float recoil;
    public bool isPhysical;
    public Attack(string _attackName, string _attackType, int _power, float _accuracy, float _recoil, bool _isPhysical)
    {   
        attackName = _attackName;
        attackType = _attackType;
        power = _power;
        accuracy = _accuracy;
        recoil = _recoil;
        isPhysical = _isPhysical;
    }
}
