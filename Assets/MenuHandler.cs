using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] BattleHandler handler;
    [SerializeField] PokemonHolder pokemonHolder;
    [SerializeField] Image trueBox;
    [SerializeField] RectTransform selector;


    [Header("Battlefield References")]

    [SerializeField] SpriteRenderer playerPokemon;
    [SerializeField] SpriteRenderer rivalPokemon;

    [Header("Pokemon Status")]

    [SerializeField] List<Sprite> healthSprites;
    [Header("Player Status")]

    [SerializeField] Text playerPokemonName;
    [SerializeField] Text playerPokemonLevel;
    [SerializeField] Text playerPokemonHealthText;
    [SerializeField] Image playerPokemonHealthBar;

    [Header("Enemy Status")]
    [SerializeField] Text rivalPokemonName;
    [SerializeField] Text rivalPokemonLevel;
    [SerializeField] Image rivalPokemonHealthBar;


    [Header("Display Text")]
    [SerializeField] GameObject mainParent;
    [SerializeField] Sprite mainImage;
    [SerializeField] Text mainText;

    [Header("Menu")]
    [SerializeField] GameObject menuParent;
    [SerializeField] Sprite menuImage;
    [SerializeField] Text menuText;
    [SerializeField] List<Vector3> menuLocations;

    [Header("Attack Select")]
    [SerializeField] GameObject attackParent;
    [SerializeField] Sprite attackImage;
    [SerializeField] Image attackTypeIcon;
    [SerializeField] List<Vector3> attackLocations;
    [SerializeField] List<Sprite> attackTypes;
    [SerializeField] List<Text> attackText;

    [Header("Switch")]
    [SerializeField] GameObject switchParent;
    [SerializeField] Sprite selImage;
    [SerializeField] Sprite notSelImage;
    [SerializeField] Sprite cancelSel;
    [SerializeField] Sprite notCancelSel;
    [SerializeField] Image cancelButton;
    [SerializeField] List<SwitchIcons> switchIcons;


    public List<string> queue;
    public bool forcedSwitch = false;
    Text currentTextBox;
    int healingPotionsRemaining = 3;
    int selectorIDX = 0;
    string currentPage = "main";
    bool getSpaceForSkip = false;
    bool isRivalTurn = false;
    bool isAttacking = false;


    IEnumerator HandleText()
    {
        string selfBox;
        bool skipLine = false;
        Text textBoxInUse;
        while (true)
        {
            if (isRivalTurn)
                StartRivalTurn();
            if (currentPage == "main" && queue.Count == 0)
                GoToPage("menu");
            if (forcedSwitch && queue.Count == 0)
            {
                GoToPage("switch");
            }
            yield return new WaitUntil(() => (queue.Count > 0 && currentPage != "attack") || isRivalTurn);
            if (queue.Count == 0)
                continue;
            if (queue[0].Length == 0)
            {
                queue.RemoveAt(0);
                continue;
            }
            textBoxInUse = currentTextBox;
            selfBox = currentPage;
            skipLine = false;
            getSpaceForSkip = false;
            //selfAudio.clip = ding;
            textBoxInUse.text = "";
            foreach (char letter in queue[0])
            {
                if (getSpaceForSkip && currentPage == "main") skipLine = true;
                if (skipLine) break;
                textBoxInUse.text += letter;
                yield return new WaitForSeconds(0.075f);
            }
            if (skipLine)
            {
                textBoxInUse.text = queue[0];
                yield return new WaitUntil(() => !Input.GetKey(KeyCode.Space) || currentPage == "attack");
            }
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || currentPage == "attack");
            yield return new WaitUntil(() => !Input.GetKey(KeyCode.Space) || currentPage == "attack");
            queue.RemoveAt(0);
            textBoxInUse.text = "";
            //selfAudio.Play();
        }
    }
    public void GoToPage(string page)
    {
        currentPage = page;
        mainParent.SetActive(false);
        menuParent.SetActive(false);
        attackParent.SetActive(false);
        switchParent.SetActive(false);
        selector.gameObject.SetActive(false);
        if (page == "main") {
            mainParent.SetActive(true);

            currentTextBox = mainText;
            trueBox.sprite = mainImage;

            return;
        } else if (page == "switch")
        {
            bool gameOver = true;
            foreach (Pokemon pk in handler.playerPokemon)
            {
                if (pk.health > 0)
                    gameOver = false;
            }
            if (gameOver)
            {
                EndBattle("rival");
            }
            switchParent.SetActive(true);
            UpdateSwitchMenu();
        }
        selector.gameObject.SetActive(true);
        selectorIDX = 0;
        if (page == "menu") {
            menuParent.SetActive(true);

            currentTextBox = menuText;
            trueBox.sprite = menuImage;
            currentTextBox.text = "What will " + handler.activePokemonPlayer.name + " do?";
        } else if (page == "attack") {
            attackParent.SetActive(true);

            currentTextBox = null;
            trueBox.sprite = attackImage;

            for (int i = 0; i < attackText.Count; i++)
            {
                attackText[i].text = handler.activePokemonPlayer.moves[i].attackName;
            }
        }
        UpdateSelector();

    }
    public void SetHealthBar(Pokemon pokemon, Image bar, Text text = null)
    {
        int index = (int)49 - 49 * pokemon.health / pokemon.healthStat;
        if (index < 0)
            index = 0;
        else if (index > 48)
            index = 48;
        bar.sprite = healthSprites[index];
        if (text != null)
            text.text = pokemon.health + "/" + pokemon.healthStat;
    }
    void UpdateSelector()
    {
        if (currentPage == "attack")
        {
            selector.anchoredPosition = attackLocations[selectorIDX];
            attackTypeIcon.sprite = attackTypes[BattleHandler.typeLookup[handler.activePokemonPlayer.moves[selectorIDX].attackType]];
        } else if (currentPage == "menu")
            selector.anchoredPosition = menuLocations[selectorIDX];
        else if (currentPage == "switch")
        {
            for (int i = 0; i < 5; i++)
            {
                if (selectorIDX == i)
                    switchIcons[i + 1].selfImage.sprite = selImage;
                else
                    switchIcons[i + 1].selfImage.sprite = notSelImage;

            }
            if (selectorIDX == 5)
                cancelButton.sprite = cancelSel;
            else
                cancelButton.sprite = notCancelSel;
        }

    }
    public void UpdateSwitchMenu()
    {
        foreach (SwitchIcons icon in switchIcons)
        {
            icon.UpdateSelf();
        }
    }
    void UpdatePokemon()
    {
        SetHealthBar(handler.activePokemonPlayer, playerPokemonHealthBar, playerPokemonHealthText);
        SetHealthBar(handler.activePokemonRival, rivalPokemonHealthBar);


        playerPokemon.sprite = pokemonHolder.GetSprite(handler.activePokemonPlayer.name, isFacingUser: false);
        rivalPokemon.sprite = pokemonHolder.GetSprite(handler.activePokemonRival.name);

        playerPokemonName.text = handler.activePokemonPlayer.name;
        rivalPokemonName.text = handler.activePokemonRival.name;

        playerPokemonLevel.text = "Lv." + handler.activePokemonPlayer.level;
        rivalPokemonLevel.text = "Lv." + handler.activePokemonRival.level;
    }
    void StartRivalTurn()
    {
        if (isAttacking)
            handler.DoTurn(handler.activePokemonPlayer.moves[selectorIDX]);
        else
        {
            handler.DoRivalTurn();
            if (handler.activePokemonPlayer.health <= 0)
                forcedSwitch = true;
        }
        isRivalTurn = false;
        isAttacking = false;
        UpdatePokemon();
    }
    void HandleSpaceInput()
    {
        getSpaceForSkip = true;
        if (currentPage == "menu")
        {
            switch (selectorIDX)
            {
                case 0:
                    GoToPage("attack");
                    break;
                case 1:
                    if (healingPotionsRemaining > 0)
                    {
                        healingPotionsRemaining--;
                        handler.activePokemonPlayer.health = Mathf.Clamp(handler.activePokemonPlayer.health + 125, 0, handler.activePokemonPlayer.healthStat);
                        queue.Add("trainer RED used healing potion " + healingPotionsRemaining + " remaining");
                        GoToPage("main");
                        isRivalTurn = true;
                        return;
                    }
                    queue.Add("no potions left");
                    GoToPage("menu");
                    break;
                case 2:
                    GoToPage("switch");
                    break;
                case 3:
                    queue.Add("You can't run now!");
                    GoToPage("main");
                    isRivalTurn = true;
                    break;
            }
        }
        else if (currentPage == "attack")
        {
            GoToPage("main");
            isRivalTurn = true;
            isAttacking = true;
        } else if (currentPage == "switch")
        {
            if (selectorIDX == 5)
            {
                if (!forcedSwitch)
                    GoToPage("menu");
                return;
            }
            if (handler.playerPokemon[selectorIDX + 1].health <= 0)
                return;
            queue.Add("thats enough " + handler.activePokemonPlayer.name);
            switchIcons[0].SwitchWith(selectorIDX + 1);
            queue.Add("go " + handler.activePokemonPlayer.name);
            UpdatePokemon();
            GoToPage("main");
            isRivalTurn = !forcedSwitch;
            forcedSwitch = false;
        }
    }
    public void EndBattle(string winner)
    {
        if (winner == "player")
            SceneManager.LoadScene("ending");
        else
            SceneManager.LoadScene("fail");
    }
    private void Start()
    {
        GoToPage("main");
        UpdatePokemon();
        queue.Add("Champion red would like to battle");
        StartCoroutine(HandleText());

    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && ((currentPage == "switch" && !forcedSwitch) || currentPage == "attack"))
            GoToPage("menu");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleSpaceInput();
        }
        if (currentPage == "switch")
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (selectorIDX > 0)
                    selectorIDX--;
                else
                    selectorIDX = 5;
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (selectorIDX < 5)
                    selectorIDX++;
                else
                    selectorIDX = 0;
            }
            UpdateSelector();
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectorIDX = (selectorIDX + 2) % 4;
            UpdateSelector();
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectorIDX = (selectorIDX + 1) % 4;
            UpdateSelector();
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectorIDX = selectorIDX - 1;
            if (selectorIDX < 0) selectorIDX = 3;
            UpdateSelector();
        }

    }
}
