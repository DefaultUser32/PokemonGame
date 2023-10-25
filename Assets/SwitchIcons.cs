using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchIcons : MonoBehaviour
{
    [SerializeField] BattleHandler battleHandler;
    [SerializeField] MenuHandler menuHandler;
    public int index;
    public Image selfIcon;
    public Text nameText;
    public Text levelText;
    public Text healthText;
    public Image healthBar;
    public Image selfImage;

    Pokemon self;
    private void Start()
    {
        selfImage = GetComponent<Image>();
    }
    public void UpdateSelf()
    {
        self = battleHandler.playerPokemon[index];
        nameText.text = self.name;
        levelText.text = "." + self.level;
        menuHandler.SetHealthBar(self, healthBar, healthText);
    }
    public void SwitchWith(int swapIndex)
    {
        Pokemon temp = self;
        battleHandler.playerPokemon[index] = battleHandler.playerPokemon[swapIndex];
        battleHandler.playerPokemon[swapIndex] = temp;
        battleHandler.activePokemonPlayer = battleHandler.playerPokemon[0];
        menuHandler.UpdateSwitchMenu();
    }
}
