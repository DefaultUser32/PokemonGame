using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    [SerializeField] MenuHandler handler;
    public static float[,] typeChart =
   {  /* 0   , 1   , 2   , 3   , 4   , 5   , 6   , 7   , 8   , 9   , 10  , 11  , 12   , 13   , 14    */ 
        { 1   , 1   , 1   , 1   , 1   , 1   , 1   , 1   , 1   , 1   , 1   , 1   , 0.5f, 0   , 1   },/*0*/
        { 1   , 0.5f, 0.5f, 2   , 1   , 2   , 1   , 1   , 1   , 1   , 1   , 2   , 0.5f, 1   , 1   },/*1*/
        { 0   , 2   , 0.5f, 0.5f, 1   , 1   , 1   , 1   , 2   , 1   , 1   , 1   , 2   , 1   , 1   },/*2*/
        { 1   , 0.5f, 2   , 0.5f, 1   , 1   , 1   , 0.5f, 2   , 0.5f, 1   , 0.5f, 2   , 1   , 1   },/*3*/
        { 1   , 1   , 2   , 0.5f, 0.5f, 1   , 1   , 1   , 0   , 2   , 1   , 1   , 1   , 1   , 1   },/*4*/
        { 1   , 0.5f, 0.5f, 2   , 1   , 0.5f, 1   , 1   , 2   , 2   , 1   , 1   , 1   , 1   , 1   },/*5*/
        { 2   , 1   , 1   , 1   , 1   , 2   , 1   , 0.5f, 1   , 0.5f, 0.5f, 0.5f, 2   , 0   , 2   },/*6*/
        { 1   , 1   , 1   , 2   , 1   , 1   , 1   , 0.5f, 0.5f, 1   , 1   , 1   , 0.5f, 0.5f, 1   },/*7*/
        { 1   , 2   , 1   , 0.5f, 2   , 1   , 1   , 2   , 1   , 0   , 1   , 0.5f, 2   , 1   , 1   },/*8*/
        { 1   , 1   , 1   , 2   , 0.5f, 1   , 2   , 1   , 1   , 1   , 1   , 2   , 0.5f, 1   , 1   },/*9*/
        { 1   , 1   , 1   , 1   , 1   , 1   , 2   , 2   , 1   , 1   , 0.5f, 1   , 1   , 1   , 0   },/*10*/
        { 1   , 0.5f, 1   , 2   , 1   , 1   , 0.5f, 0.5f, 1   , 0.5f, 2   , 1   , 1   , 0.5f, 2   },/*11*/
        { 1   , 2   , 1   , 1   , 1   , 2   , 0.5f, 1   , 0.5f, 2   , 1   , 2   , 1   , 1   , 1   },/*12*/
        { 0   , 1   , 1   , 1   , 1   , 1   , 1   , 1   , 1   , 1   , 2   , 1   , 1   , 2   , 0.5f},/*13*/
        { 1   , 1   , 1   , 1   , 1   , 1   , 0.5f, 1   , 1   , 1   , 2   , 1   , 1   , 2   , 0.5f},/*14*/
    };
    public static Dictionary<string, int> typeLookup = new Dictionary<string, int>
    {
        { "normal", 0 },
        { "fire", 1 },
        { "water", 2 },
        { "grass", 3 },
        { "electric", 4 },
        { "ice", 5 },
        { "fighting", 6 },
        { "poison", 7 },
        { "ground", 8 },
        { "flying", 9 },
        { "psychic", 10 },
        { "bug", 11 },
        { "rock", 12 },
        { "ghost", 13 },
        { "dark", 14 },
    };
    public List<Pokemon> playerPokemon = new List<Pokemon>() 
    {
        PokemonHolder.pokemonList["charizard"].Copy(),
        PokemonHolder.pokemonList["zapdos"].Copy(),
        PokemonHolder.pokemonList["arcanine(1)"].Copy(),
        PokemonHolder.pokemonList["gengar"].Copy(),
        PokemonHolder.pokemonList["kabutops"].Copy(),
        PokemonHolder.pokemonList["tentacruel"].Copy()
    };
    public List<Pokemon> rivalPokemon = new List<Pokemon>()
    {
        PokemonHolder.pokemonList["blastoise"],
        PokemonHolder.pokemonList["alakazam"],
        PokemonHolder.pokemonList["heracross"],
        PokemonHolder.pokemonList["arcanine(2)"],
        PokemonHolder.pokemonList["exeggutor"],
        PokemonHolder.pokemonList["tyranitar"]
    };
    public Pokemon activePokemonPlayer;
    public Pokemon activePokemonRival;
    private void Start()
    {
        activePokemonPlayer = playerPokemon[0];
        activePokemonRival = rivalPokemon[0];
    }
    public float GetAdvantage(string attackType = null, string targetType = null, int attackIndex = -1, int targetIndex = -1)
    {
        if (attackType == "none" || targetType == "none")
            return 1;
        int attack;
        int target;
        if (attackIndex != -1)
            attack = attackIndex;
        else
            attack = typeLookup[attackType];
        if (targetIndex != -1)
            target = targetIndex;
        else
            target = typeLookup[targetType];

        return typeChart[attack, target];
    }
    public void DoTurn(Attack attack)
    {
        if (activePokemonPlayer.speed > activePokemonRival.speed)
        {
            handler.queue.Add(activePokemonPlayer.name + " used " + attack.attackName);
            handler.queue.Add(DoAttack(ref activePokemonPlayer, ref activePokemonRival, attack));
            if (activePokemonRival.health > 0)
                DoRivalTurn();
            else
                ChangeRivalPokemon();
            if (activePokemonPlayer.health < 0)
            {
                handler.forcedSwitch = true;
            }
        } else
        {
            DoRivalTurn();
            if (activePokemonPlayer.health < 0)
            {
                handler.forcedSwitch = true;
                return;
            }
            handler.queue.Add(activePokemonPlayer.name + " used " + attack.attackName);
            handler.queue.Add(DoAttack(ref activePokemonPlayer, ref activePokemonRival, attack));
            if (activePokemonRival.health <= 0)
                ChangeRivalPokemon();
        }
    }
    private void ChangeRivalPokemon()
    {
        handler.queue.Add("thats enough " + activePokemonRival.name);
        List<Pokemon> pokemonRemaining = new();
        foreach (Pokemon pk in rivalPokemon)
        {
            if (pk.health > 0)
                pokemonRemaining.Add(pk);
        }
        if (pokemonRemaining.Count == 0)
        {
            handler.EndBattle("player");
            return;
        }
        activePokemonRival = pokemonRemaining[UnityEngine.Random.Range(0, pokemonRemaining.Count)];
        handler.queue.Add("go " + activePokemonRival.name + "!");
    }
    public string DoAttack(ref Pokemon attacker, ref Pokemon victem, Attack attack)
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) > attack.accuracy)
            return "but it missed";

        float baseDamamge = attack.power;
        if (baseDamamge < 5)
            baseDamamge = 95;

        float advantage = GetAdvantage(attackType: attack.attackType, targetType: victem.type1) * GetAdvantage(attackType: attack.attackType, targetType: victem.type2);
        float damage = (attacker.level * 2 / 5) + 2;
        damage *= baseDamamge;
        if (attack.isPhysical) // Physical/Special
            damage *= attacker.physicalAttack / victem.physicalDefense;
        else
            damage *= attacker.specialAttack / victem.specialDefense;
        damage /= 50;

        damage += 2;

        if (attack.attackType == attacker.type1 || attack.attackType == attacker.type2) // STAB
            damage *= 1.5f;

        damage *= advantage * UnityEngine.Random.Range(0.85f, 1);

        victem.health -= (int) math.floor(damage);
        attacker.health -= (int)math.floor(attack.recoil * damage);

        rivalPokemon[0] = activePokemonRival;
        playerPokemon[0] = activePokemonPlayer;

        if (attack.power < 5)
            return "but i didnt program it, so it does damage";

        if (math.floor(advantage) < 0.9f)
            return "But it was not very effective";
        if (math.floor(advantage) > 1.1f)
            return "It was super effective";
        return "";
    }
    public void DoRivalTurn()
    {
        Attack chosen = activePokemonRival.moves[UnityEngine.Random.Range(0, 3)];
        handler.queue.Add(activePokemonRival.name + " used " + chosen.attackName);
        handler.queue.Add(DoAttack(ref activePokemonRival, ref activePokemonPlayer, activePokemonRival.moves[0]));
    }
}
