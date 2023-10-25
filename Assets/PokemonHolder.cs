using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonHolder : MonoBehaviour
{
    [SerializeField] List<Sprite> frontSpriteList = new List<Sprite>();
    [SerializeField] List<Sprite> backSpriteList = new List<Sprite>();
    public static Dictionary<string, Attack> attackList = new Dictionary<string, Attack>()
    {
        { "cut", new Attack("cut", "normal", 50, 0.95f, 0, true)},
        { "flamethrower", new Attack("flamethrower", "fire", 95, 1, 0, false) },
        { "swift", new Attack("swift", "normal", 60, 1, 0, false) },
        { "earthquake", new Attack("earthquake", "ground", 100, 1, 0, true) },
        { "thunderbolt", new Attack("thunderbolt", "electric", 90, 1, 0, false) },
        { "drill peck", new Attack("drill peck", "flying", 80, 1, 0, true) },
        { "razor wind", new Attack("razor wind", "normal", 80, 1, 0, false) },
        { "take down", new Attack("take down", "normal", 90, 0.85f, 0.25f, true) },
        { "bite", new Attack("bite", "dark", 60, 1, 0, true) },
        { "teleport", new Attack("teleport", "psychic", 0, 1, 0, false) },
        { "night shade", new Attack("night shade", "ghost", 0, 1, 0, false) },
        { "psychic", new Attack("psychic", "psychic", 90, 1, 0, false) },
        { "self destruct", new Attack("self destruct", "normal", 200, 1, 50, true) },
        { "hydro pump", new Attack("hydro pump", "water", 110, 0.8f, 0, false) },
        { "double edge", new Attack("double edge", "normal", 120, 1, 0.33333f, true) },
        { "slash", new Attack("slash", "normal", 70, 1, 0, true) },
        { "leer", new Attack("leer", "normal", 0, 1, 0, false) },
        { "surf", new Attack("surf", "water", 90, 1, 0, false) },
        { "poison sting", new Attack("poison sting", "poison", 15, 1, 0, true) },
        { "skull bash", new Attack("skull bash", "normal", 130, 1, 0, true) },
        { "water gun", new Attack("water gun", "water", 40, 1, 0, false) },
        { "megahorn", new Attack("megahorn", "bug", 120, 0.85f, 0, true) },
        { "counter", new Attack("counter", "fighting", 0, 1, 0, true) },
        { "rock tomb", new Attack("rock tomb", "rock", 60, 0.95f, 0, true) },
        { "shadow ball", new Attack("shadow ball", "ghost", 80, 1, 0, false) },
        { "calm mind", new Attack("calm mind", "psychic", 0, 1, 0, false) },
        { "reflect", new Attack("reflect", "psychic", 0, 1, 0, false) },
        { "crunch", new Attack("crunch", "dark", 80, 1, 0, true) },
        { "aerial ace", new Attack("aerial ace", "flying", 60, 1, 0, true) },
        { "extreme speed", new Attack("extreme speed", "normal", 80, 1, 0, true) },
        { "overheat", new Attack("overheat", "fire", 130, 0.9f, 0, false) },
        { "iron tail", new Attack("iron tail", "steel", 100, 0.75f, 0, true) },
        { "giga drain", new Attack("giga drain", "grass", 75, 1, 0, false) },
        { "stomp", new Attack("stomp", "normal", 65, 1, 0, true) },
        { "ice beam", new Attack("ice beam", "ice", 90, 1, 0, false) }
    };
    private static Dictionary<string, List<Attack>> movesets = new Dictionary<string, List<Attack>>() 
    {
        {"charizard", new List<Attack>() { attackList["cut"], attackList["flamethrower"], attackList["swift"], attackList["earthquake"]} },
        {"zapdos", new List<Attack>() { attackList["thunderbolt"], attackList["drill peck"], attackList["razor wind"], attackList["swift"]} },
        {"arcanine(1)", new List<Attack>() { attackList["flamethrower"], attackList["take down"], attackList["bite"], attackList["teleport"]} },
        {"gengar", new List<Attack>() { attackList["night shade"], attackList["slash"], attackList["psychic"], attackList["shadow ball"]} },
        {"kabutops", new List<Attack>() { attackList["hydro pump"], attackList["double edge"], attackList["slash"], attackList["leer"]} },
        {"tentacruel", new List<Attack>() { attackList["surf"], attackList["poison sting"], attackList["skull bash"], attackList["water gun"]} },
        {"heracross", new List<Attack>() { attackList["megahorn"], attackList["earthquake"], attackList["counter"], attackList["rock tomb"]} },
        {"alakazam", new List<Attack>() { attackList["psychic"], attackList["shadow ball"], attackList["calm mind"], attackList["reflect"]} },
        {"tyranitar", new List<Attack>() { attackList["crunch"], attackList["earthquake"], attackList["thunderbolt"], attackList["aerial ace"]} },
        {"arcanine(2)", new List<Attack>() { attackList["extreme speed"], attackList["overheat"], attackList["aerial ace"], attackList["iron tail"]} },
        {"exeggutor", new List<Attack>() { attackList["psychic"], attackList["giga drain"], attackList["reflect"], attackList["stomp"]} },
        {"blastoise", new List<Attack>() { attackList["hydro pump"], attackList["ice beam"], attackList["earthquake"], attackList["slash"]} }
    };
    public static Dictionary<string, Pokemon> pokemonList = new Dictionary<string, Pokemon>()
    {
        { "charizard", new Pokemon("charizard", "fire", "flying", 79, 0, 236, 178, 167, 221, 178, 205, movesets["charizard"]) },
        { "zapdos", new Pokemon("zapdos", "electric", "flying", 68, 1, 221, 162, 155, 215, 162, 178, movesets["zapdos"]) },
        { "arcanine(1)", new Pokemon("arcanine", "fire", "none", 70, 2, 227, 198, 151, 182, 151, 174, movesets["arcanine(1)"]) },
        { "gengar", new Pokemon("gengar", "ghost", "poison", 74, 3, 195, 136, 127, 242, 151, 209, movesets["gengar"]) },
        { "kabutops", new Pokemon("kabutops", "rock", "water", 68, 4, 180, 200, 184, 125, 133, 147, movesets["kabutops"]) },
        { "tentacruel", new Pokemon("tentacruel", "water", "poison", 76, 5, 231, 147, 139, 165, 231, 198, movesets["tentacruel"]) },
        { "heracross", new Pokemon("heracross", "bug", "fighting", 72, 6, 219, 227, 148, 92, 180, 163, movesets["heracross"]) },
        { "alakazam", new Pokemon("alakazam", "psychic", "none", 73, 7, 183, 108, 101, 243, 163, 220, movesets["alakazam"]) },
        { "tyranitar", new Pokemon("tyranitar", "rock", "dark", 72, 8, 220, 242, 175, 180, 210, 126, movesets["tyranitar"]) },
        { "arcanine(2)", new Pokemon("arcanine", "fire", "none", 73, 2, 237, 206, 158, 190, 158, 182, movesets["arcanine(2)"]) },
        { "exeggutor", new Pokemon("exeggutor", "grass", "psychic", 73, 9, 193, 94, 158, 126, 102, 94, movesets["exeggutor"]) },
        { "blastoise", new Pokemon("blastoise", "water", "none", 75, 10, 220, 162, 190, 166, 198, 155, movesets["blastoise"]) }
    };
    public Sprite GetSprite(string character, bool isFacingUser = true)
    {
        if (character == "arcanine")
            character = "arcanine(1)";
        if (isFacingUser)
            return frontSpriteList[PokemonHolder.pokemonList[character].spriteIndex];
        return backSpriteList[PokemonHolder.pokemonList[character].spriteIndex];

    }
}
