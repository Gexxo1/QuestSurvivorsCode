using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region serialized fields
    public static MenuManager instance;
    //Various Menus
    [SerializeField] private Animator charInfoAnimator;
    [SerializeField] private Animator InvAnimator;
    [SerializeField] private Animator mythAnimator;
    private bool showI = false;
    //Pause utilities
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject DarkenBackground;
    public static bool isGamePaused = false;
    [Header("Player Info Tab")]
    [SerializeField] private Text playerName;
    [SerializeField] private Text className;
    [SerializeField] private Text currLvl;
    [SerializeField] private Image charaPortrait;

    [Header("Player Primary Stats Tab")]
    [SerializeField] private Text hpTxt;
    [SerializeField] private Text mpTxt;
    [SerializeField] private Text strTxt;
    [SerializeField] private Text dexTxt;
    [SerializeField] private Text intlTxt;
    [SerializeField] private Text lckTxt;
    [Header("Player Secondary Stats Tab")]
    [SerializeField] private Text spdTxt;
    [SerializeField] private Text atkSpdTxt;
    [SerializeField] private Text critRateTxt;
    [SerializeField] private Text critDmgTxt;

    [Header("Player Inventory")]
    [SerializeField] private Inventory playerInventory;
    //[SerializeField] private Image currWeaponImg;
    //[SerializeField] private Image currAbilityImg;
    //[SerializeField] private GameObject playerItems;
    [SerializeField] private GameObject itemTab;
    [SerializeField] private GameObject mythicTab;
    //[SerializeField] private Image abilityCdImg;
    //powerup
    private GameObject[] itemCells; 
    private int cellsCount; //ATTENZIONE: non sono gli oggetti stessi, ma le i contenitori di tali!
    private Text[] tierCells; 
    //mythic
    private GameObject[] itemMythicCells; 
    private int mythicCellsCount; //ATTENZIONE: non sono gli oggetti stessi, ma le i contenitori di tali!
    private Text[] mythicTierCells; 
    [Header("Player HUD")]
    //hp
    [SerializeField] private RectTransform hpBar;
    [SerializeField] private RectTransform hpBarTopPlayer;
    [SerializeField] private Text hpText;
    //mp
    [SerializeField] private RectTransform mpBar;
    [SerializeField] private Text mpText;
    //armor
    [SerializeField] private GameObject topHierarchyArmorBar;
    [SerializeField] private RectTransform armorBar;
    [SerializeField] private Text armorText;
    //coin
    [SerializeField] private Text coinCounter;
    //xp
    [SerializeField] private RectTransform xpBar;
    [SerializeField] private Text xpText;
    [SerializeField] private Text currLevelText;
    [SerializeField] private Image magnetActiveIndicator;
    //bottom frames
    [SerializeField] private Image weaponFrame;
    [SerializeField] private Image abilityFrame;
    [SerializeField] private Text abilityCost;
    [SerializeField] private Image dashFrame;
    [SerializeField] private Image stackFrame;
    //boss healthbar
    [SerializeField] private GameObject bossHpBarGO;
    [SerializeField] private RectTransform bossHpBar;
    [SerializeField] private Text bossHpText;
    [SerializeField] private TextMeshProUGUI bossNameText;
    [Header("Level Up Menu")]
    [SerializeField] private GameObject levelUpMenu;
    [SerializeField] private Button[] btnOption;
    [SerializeField] private Text[] lvlUpOptionText;

    [SerializeField] private Image[] lvlUpOptionSprite;
    [SerializeField] private Text[] lvlUpOptionDesc;
    [SerializeField] private Image[] lvlUpOptionFrame;
    [SerializeField] private Text[] choiceState;
    [SerializeField] private Sprite powerupFrame;
    [SerializeField] private Sprite mythicFrame;
    [SerializeField] private GameObject rerollBtn;
    [SerializeField] private Text rerollCounter;
    [Header("In game counters")]
    [SerializeField] private Text waveTimer;
    [SerializeField] private Text score;
    [Header("Settings Tab")]
    [SerializeField] private GameObject settingsTab;
    [Header("Fullscreen")]
    [SerializeField] private GameObject fullscreenDarkenBackground;
    [SerializeField] private GameObject tutorialScreen;
    //[SerializeField] private Text fullscreenScore;
    [SerializeField] private GameObject waveCompleteScreen;
    [SerializeField] private GameObject gameCompleteScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject mainMenuBtn;
    [SerializeField] private GameObject resultsScreen;
    [Header("Animators")]
    [SerializeField] public Animator animator;
    //[SerializeField] private Animator fullscreenAnimator;
    [Header("Buff Section")]
    [SerializeField] private GameObject buffParent;
    [SerializeField] private GameObject buffTemplate;
    [Header("Results Screen")]
    [SerializeField] private Text resultsTextConstantTemplate;
    [SerializeField] private Text resultsTextVariableTemplate;
    //[Header("Other")]
    #endregion
    //Internal parameters
    private bool isSettingsShown = false;
    private bool isTutorialShown = false;
    //Item[] powerupChoice; //farla diventare una lista
    private Player player;
    
    private void Awake() {
        if(instance != null) 
            Debug.Log("MenuManager ha un'istanza duplicata: " + instance);
        instance = this;
        //Debug.Log(currWeaponHUD.GetComponent<Image>().sprite);
        //Debug.Log(playerWeapons.GetComponent<Inventory>().getCurrWeaponGO().GetComponent<SpriteRenderer>().sprite);
        
        itemTabInit();
        mythicTabInit();
        SetupFrames();
        //powerupChoice = new Item[3];
    }
    
    private void itemTabInit() {
        cellsCount = itemTab.transform.childCount;
        itemCells = new GameObject[cellsCount];
        tierCells = new Text[cellsCount];
        for(int i = 0; i < cellsCount; i++) {
            itemCells[i] = itemTab.transform.GetChild(i).GetChild(0).gameObject;
            //Debug.Log("itemCells[i] " + itemCells[i]);
            if(itemTab.transform.GetChild(i).GetChild(1).gameObject.TryGetComponent<Text>(out Text levelText)) {
                tierCells[i] = levelText;
                //Debug.Log("tierCells[i] " + tierCells[i]);
                tierCells[i].gameObject.SetActive(false);
            }
            else
                Debug.Log("level text not found");
        }
    }
    private void mythicTabInit() {
        mythicCellsCount = mythicTab.transform.childCount;
        itemMythicCells = new GameObject[mythicCellsCount];
        mythicTierCells = new Text[mythicCellsCount];

        for(int i = 0; i < mythicCellsCount; i++) {
            itemMythicCells[i] = mythicTab.transform.GetChild(i).GetChild(0).gameObject;
            if(mythicTab.transform.GetChild(i).GetChild(1).gameObject.TryGetComponent(out Text levelText)) {
                mythicTierCells[i] = levelText;
                mythicTierCells[i].gameObject.SetActive(false);
            }
            else
                Debug.Log("level text not found");
        }
    }
    private PowerupTableManager ptm;
    private void Start() {
        player = GameManager.instance.player;
        ptm = PowerupTableManager.instance;
        UpdateCurrLvlHUD(player.currLvl);
        UpdateItemHUD();
        ShowInventoryStart();
        pauseTutorialText = tutorialScreen.transform.Find("escTxt").GetComponent<Text>();
        ShowTutorialMenu();
    }
    void Update()
    {
        ShowCharacterInfo();
        //ShowInventory();
        ShowPause();
    }
    void ShowCharacterInfo() {
        /*
        if(Input.GetKeyDown(KeyCode.C)) {
            showC = !showC;
            charInfoAnimator.SetBool("show", showC);
            if(showC) {
                UpdateCharInfoUI();
            }
        }
        */
    }
    void ShowInventory() {
        if(Input.GetKeyDown(KeyCode.I)) {
            showI = !showI;
            InvAnimator.SetBool("show",showI);
        }
    }
    void ShowInventoryStart() {
        InvAnimator.SetBool("show",true);
    }
    public void ShowMythicRightTab() {
        mythAnimator.SetBool("show",true);
    }
    private bool PauseInput() {
        #if UNITY_WEBGL
            return Input.GetKeyDown(KeyCode.Backslash) || Input.GetKeyDown(KeyCode.Tilde) || Input.GetKeyDown(KeyCode.Escape);
        #else
            return Input.GetKeyDown(KeyCode.Escape);
        #endif
    }
    void ShowPause() {
        if(PauseInput() && !animator.GetBool("showLvlup")) {
            if(isTutorialShown || isSettingsShown || waveCompleteScreen.activeInHierarchy || gameOverScreen.activeInHierarchy)
                return;
                
            if(isGamePaused) 
                HidePauseMenu();
            else
                ShowPauseMenu();
            //PauseAnimator.SetBool("show",showP);
        }
    }
    public void HidePauseMenu() {
        Resume();
        fullscreenDarkenBackground.SetActive(false);
        PauseMenu.SetActive(false);
    }
    private bool hideQuitBtn = false;
    public void HideQuitBtn(bool b) { hideQuitBtn = b; }
    private Button quitBtn;
    public void ShowPauseMenu() {
        Pause();
        fullscreenDarkenBackground.SetActive(true);
        quitBtn = PauseMenu.transform.Find("QuitButton").GetComponent<Button>();
        if(quitBtn != null && hideQuitBtn) quitBtn.interactable = false;
        PauseMenu.SetActive(true);
    }  
    public void Resume() { // viene anche richiamata dal bottone
        player.setAttackBlocked(false);
        Time.timeScale = 1;
        isGamePaused = !isGamePaused;
    }
    public void Pause() {
        player.setAttackBlocked(true);
        Time.timeScale = 0;
        isGamePaused = !isGamePaused;
    }
    public void setPlayerAttack(bool b) { //chiamato quando stai sopra il menu
        Weapon w = GameManager.instance.inv.getCurrWeapon();
        if(w == null)
            return;
        w.SetBlock(b);
    }
    public void UpdateCharInfoUI() {
        Player p = GameManager.instance.player;
        Stats s = p.stats;
        //Top tab
        playerName.text = "Alberto"; //TO CHANGE
        className.text = "Knight"; //TO CHANGE
        currLvl.text = p.currLvl.ToString();
        charaPortrait.sprite = p.gameObject.GetComponent<SpriteRenderer>().sprite;
        //Bot tab 1
        hpTxt.text = s.healthpoint.ToString();
        mpTxt.text = s.manapoint.ToString();
        //strTxt.text = s.damage.ToString();
        //strTxt.text = s.strength.ToString();
        //dexTxt.text = s.dexterity.ToString();
        //intlTxt.text = s.intelligence.ToString();
        lckTxt.text = s.luckPercent.ToString();
        //Bot tab 2
        spdTxt.text = s.moveSpeed.ToString();
        critRateTxt.text = s.critRatePercent.ToString() + "%";
        critDmgTxt.text = s.critDamagePercent.ToString() + "%";

    }
    public void UpdateWeaponHUD() {
        wpnSprite.sprite = playerInventory.getCurrWeaponSprite();
    }
    public void UpdateWeaponHUD(Sprite spr) {
        wpnSprite.sprite = spr;
    }
    public void UpdateAbilityHUD() {
        abilitySprite.sprite = playerInventory.GetAbiltySprite();
        UpdateAbilityManaCost();
        if(playerInventory.GetAbility() is ProjectileExecuteStack eps) {
            stackFrame.gameObject.SetActive(true);
            stackCounterTxt = stackFrame.transform.Find("Counter").GetComponent<Text>();
            UpdateStackCounter(eps.GetStackCounter());
        }
    }
    public void UpdateAbilityManaCost() {
        abilityCost.text = playerInventory.GetAbility().GetManaCost() + "";
    }
    public void UpdateStackCounter(int stack) {
        stackCounterTxt.text = stack + "";
    }
    public void UpdateItemHUD() {
        for(int i=0; i < itemCells.Length && i < playerInventory.getItemCount(); i++) {
            Sprite itemSprite = playerInventory.getItemByIndex(i).GetComponent<SpriteRenderer>().sprite;
            if(itemSprite != null) {
                itemCells[i].GetComponent<Image>().sprite = itemSprite;
                itemCells[i].GetComponent<Image>().color = new Color32(255,255,255,255);
//                Debug.Log("Update item hud");
            }
            else {
                itemCells[i].gameObject.SetActive(false);
                Debug.Log("Warning: Item sprite is null");
            }
        }
    }
    
    public void UpdateItemTierHUD() {
        for(int i=0; i < itemCells.Length && i < playerInventory.getItemCount(); i++) {
//            Debug.Log(playerInventory.getItemByIndex(i).title + " " + (playerInventory.getItemByIndex(i).tier+1));
            tierCells[i].text =  Utility.ToRoman(playerInventory.getItemByIndex(i).tier+1) + "";
            tierCells[i].gameObject.SetActive(true);
        }
    }

    public void UpdateMythicHUD() {
        for(int i=0; i < itemMythicCells.Length && i < playerInventory.getMythicCount(); i++) {
            Sprite itemSprite = playerInventory.getMythicByIndex(i).GetComponent<SpriteRenderer>().sprite;
            if(itemSprite != null) {
                itemMythicCells[i].GetComponent<Image>().sprite = itemSprite;
                itemMythicCells[i].GetComponent<Image>().color = new Color32(255,255,255,255);
//                Debug.Log("Update item hud");
            }
            else {
                itemMythicCells[i].SetActive(false);
                Debug.Log("Warning: Item sprite is null");
            }
        }
    }
    public void UpdateMythicTierHUD() {
        for(int i=0; i < itemMythicCells.Length && i < playerInventory.getMythicCount(); i++) {
            mythicTierCells[i].text =  Utility.ToRoman(playerInventory.getMythicByIndex(i).tier+1) + "";
            mythicTierCells[i].gameObject.SetActive(true);
        }
    }
    public void UpdateHpHUD(float ratio, string text) {
        hpBar.localScale = new Vector3(ratio,1,1);
        hpBarTopPlayer.localScale = new Vector3(ratio,1,1);
        hpText.text = text;
    }
    public void UpdateMpHUD(float ratio, string text) {
        mpBar.localScale = new Vector3(ratio,1,1);
        mpText.text = text;
    }
    public void UpdateArmorHUD(float ratio, string text) {
        armorBar.localScale = new Vector3(ratio,1,1);
        armorText.text = text;
    }
    public void SetActiveArmorHUD(bool b) {
        if(topHierarchyArmorBar.activeInHierarchy != b)
            topHierarchyArmorBar.SetActive(b);
    }
    public void UpdateCoinHUD(int gold) {
        coinCounter.text = gold + "";
    }
    public void UpdateXpHUD(float ratio, string text) {
        xpBar.localScale = new Vector3(ratio,1,1);
        xpText.text = text;
    }
    public void UpdateCurrLvlHUD(int lvl) {
        string text = "Lvl." + (lvl+1);
        currLevelText.text = text;
    }
    private Text pauseTutorialText;
    public void ShowTutorialMenu() {
        
        isTutorialShown = !isTutorialShown;
        tutorialScreen.SetActive(isTutorialShown);
        fullscreenDarkenBackground.SetActive(isTutorialShown);
        #if UNITY_WEBGL
            pauseTutorialText.text = "\"Esc\" or \"Backslash\" or \"Tilde\" = Pause";
        #endif
        PauseMenu.SetActive(false);
        
        if(isTutorialShown)
            Pause();
        else 
            Resume();
    }
    public void ShowMainMenuBtn(bool b) {
        mainMenuBtn.SetActive(b);
    }

    public void ShowWavecompleteMenu(bool isShown) {
        
        if(animator.GetBool("showLvlup"))
            animator.SetBool("showLvlup",false);
        end = true;
        waveCompleteScreen.SetActive(isShown);
        fullscreenDarkenBackground.SetActive(isShown);
        ShowMainMenuBtn(isShown);
        if(isShown) 
            Pause();
        else
            Resume();
        
        //fullscreenAnimator.SetTrigger("showWaveComplete");
    }

    public void ShowGameoverScreen(bool isShown) {
        if(animator.GetBool("showLvlup"))
            animator.SetBool("showLvlup",false);
        gameOverScreen.SetActive(isShown);
        fullscreenDarkenBackground.SetActive(isShown);
        ShowMainMenuBtn(isShown);
        if(isShown) 
            Pause();
        else
            Resume();

        //ShowResultsScreen(isShown);
    }
    private void ShowResultsScreen(bool isShown) {
        ResultsData data = GameManager.instance.GetRunResultsData();
        Dictionary<string, (string, string)> resultsData = data.GetResultsDictionary();
        //Dictionary: <key, (nome visualizzato, valore effettivo)>
        foreach (var pair in resultsData) {
            Text constantText = Instantiate(resultsTextConstantTemplate,resultsTextConstantTemplate.transform.parent);
            Text variableText = Instantiate(resultsTextVariableTemplate,resultsTextVariableTemplate.transform.parent);
            constantText.text = pair.Value.Item1 + ": ";
            variableText.text = pair.Value.Item2;
        }

        resultsScreen.SetActive(isShown);
    } 
    public void HideFullscreenMenus() {
        //fullscreenAnimator.SetTrigger("hide");
    }

    int lvlupDebt = 0; 
    int mythDebt = 0;
    [SerializeField] List<Item> powerupChoice;
    private const bool showDetailed = true;
    public void ShowLevelUpMenu(bool isMythicChoice = false) {
        //Debug.Log(isMythicChoice);
        if(end)
            return;
        if(animator.GetBool("showLvlup")) {
            if(!isMythicChoice)
                lvlupDebt++;
            else
                mythDebt++;
            return;
        }
        animator.SetBool("showLvlup",true);
        bool isRerollActive = !isMythicChoice && GameManager.instance.player.stats.rerollCount > 0;
        rerollBtn.SetActive(isRerollActive);
        if(isRerollActive) UpdateRerollText();
        
        powerupChoice = ptm.GetLvlupmenuItems(isMythicChoice,3);
        //Understand if powerup is new or needs tier to be added after the name
        for(int i=0; i < powerupChoice.Count; i++) {
            lvlUpOptionText[i].text = powerupChoice[i].title;
            lvlUpOptionSprite[i].sprite = powerupChoice[i].GetSprite();
            if(!isMythicChoice) 
                lvlUpOptionFrame[i].sprite = powerupFrame;
            else 
                lvlUpOptionFrame[i].sprite = mythicFrame;
            //lvlUpOptionDesc[i].text = powerupChoice[i].GetDescription();
            PowerupSetupDescription(i,isMythicChoice);
        }
        int n = powerupChoice.Count;
        if(n < 3) {
            btnOption[2].gameObject.SetActive(false);
            if(n < 2)
                btnOption[1].gameObject.SetActive(false);
        }
        else if(!btnOption[1].gameObject.activeInHierarchy || !btnOption[2].gameObject.activeInHierarchy) {
            if(!btnOption[1].gameObject.activeInHierarchy)
                btnOption[1].gameObject.SetActive(true);
            if(!btnOption[2].gameObject.activeInHierarchy)
                btnOption[2].gameObject.SetActive(true);
        }
    }

    private void PowerupSetupDescription(int index, bool isMythic = false) {
        if (powerupChoice[index] is PowerUp pu) { //case 1: is powerup
            if(playerInventory.IsPowerupInInventory(pu)) {
                powerupChoice[index] = playerInventory.SearchPowerupByTitle(pu.title,isMythic);
                pu = (PowerUp)powerupChoice[index];
                lvlUpOptionText[index].text += " " + pu.GetTierRoman(1);
                choiceState[index].text = "";
                lvlUpOptionDesc[index].text = pu.GetUpgrDesc();
            }
            else {
                choiceState[index].text = "New!";
                if(showDetailed)
                    lvlUpOptionDesc[index].text = pu.GetDescription();
            }
        }
        else { //case 2: is item (potion, gold, etc)
            choiceState[index].text = "Item";
            lvlUpOptionDesc[index].text = powerupChoice[index].GetDescription();
        } 
    }

    public void applyPowerupChoice(int choice) {
        switch(choice) {
            case 1:
                if(powerupChoice[0] != null)
                    playerInventory.AddItemToInventory(powerupChoice[0].gameObject,false);
            break;
            case 2:
                if(powerupChoice[1] != null)
                    playerInventory.AddItemToInventory(powerupChoice[1].gameObject,false);
            break;
            case 3:
                if(powerupChoice[2] != null)
                    playerInventory.AddItemToInventory(powerupChoice[2].gameObject,false);
            break;
            case -1: //reroll
                ShowLevelUpMenu(false);
                GameManager.instance.player.stats.rerollCount--;
//                Debug.Log("Reroll count: " + GameManager.istance.player.stats.rerollCount);
            break;
            default:
                Debug.LogError("Choice must be between 1 and 3 (included extremes) or -1 (for rerolling)");
            break;
        }
        animator.SetBool("showLvlup",false);
        if(lvlupDebt > 0) {
            lvlupDebt--;  
            ShowLevelUpMenu(false); 
        } else if(mythDebt > 0) {
            mythDebt--;  
            ShowLevelUpMenu(true); 
        }
        else
            Resume();
    }
    private bool end = false;
    
    /*
    public void ShowGamecompleteMenu(bool isShown) {
        end = true;
        gameCompleteScreen.SetActive(isShown);
        DarkenBackground.SetActive(isShown);
        //updateFullscreenScore();
        //fullscreenScore.gameObject.SetActive(isShown);
        if(isShown)
            Pause();
        else
            Resume();
    }
    */

    public void ShowSettingsMenu(bool isShown) {
        isSettingsShown = isShown;
        fullscreenDarkenBackground.SetActive(isShown);
        CanvasGroup cg = settingsTab.GetComponent<CanvasGroup>();
        if(isShown) 
            cg.alpha = 1;
        else
            cg.alpha = 0; 
        cg.interactable = isShown;
        cg.blocksRaycasts = isShown;
        //btnSave.interactable = isShown;
        //btnClose.interactable = isShown;
        //DarkenBackground.SetActive(isShown);
        if(isShown)
            Pause();
        else
            Resume();
    }
    public void activateTimer(bool b) {
        waveTimer.gameObject.SetActive(b);
    }
    public void UpdateTimer(float time) {
        waveTimer.text = "Time: " + (int)time;
    }

    public void UpdateKillsUI(int k) {
        score.text = k + "";
    }
    /*
    public void SetAbilityFill(float value) {
        abilityCdImg.fillAmount = value;
    }
    public void UpdateAbilityFill(float cd) {  //! MUST BE PUT IN Update() !
        abilityCdImg.fillAmount += 1 / cd * Time.deltaTime;
    }
    */
    [Header("Objectives")]
    [SerializeField] private Text objUnlockedDesc;
    [SerializeField] private Image objImg;
    [SerializeField] private float objectiveTimeShown;
    public void ShowObjectiveCompleted(int i) {
        Objective obj = ObjectiveManager.instance.objectives[i];

        //objDesc.text = obj.requirementsDesc; //descrizione obiettivo, nella notifica non serve, però potrebbe servire nel menù
        objUnlockedDesc.text = obj.GetTitle(); //cosa l'obiettivo ha sbloccato
        objImg.sprite = obj.GetSprite();

        //StartCoroutine(ObjectiveShown());
        animator.SetTrigger("showObj");
    }

    private IEnumerator ObjectiveShown() {
        animator.SetBool("showObj",true);
        Debug.Log("Mostra " + objectiveTimeShown);
        yield return new WaitForSecondsRealtime(objectiveTimeShown);
        Debug.Log("Togli");
        animator.SetBool("showObj",false);
        yield return null;
    }
    
    public int GetPowerupCellsCount() {
        return cellsCount;
    }
    //magnet
    private Image magnetCdImg;
    private Image wpnCdImg,wpnSprite;
    private Image abilityCdImg,abilitySprite,dashCdImg;
    private Text stackCounterTxt;
    private Coroutine magnetFillCoroutine;
    private Coroutine wpnFillCoroutine,abilityFillCoroutine,dashFillCoroutine;
    private void SetupFrames() {
        SetupFrameSprites();
        SetupCdIndicators();
        stackFrame.gameObject.SetActive(false);
        ActivateBossHpBar(false);
    }
    private void SetupFrameSprites() {
        wpnSprite = weaponFrame.transform.Find("Sprite").GetComponent<Image>();
        abilitySprite = abilityFrame.transform.Find("Sprite").GetComponent<Image>();
    }
    private void SetupCdIndicators() {
        magnetActiveIndicator.gameObject.SetActive(false);
        magnetCdImg = magnetActiveIndicator.transform.Find("BlackCdImg").GetComponent<Image>();
        wpnCdImg = weaponFrame.transform.Find("BlackCdImg").GetComponent<Image>();
        abilityCdImg = abilityFrame.transform.Find("BlackCdImg").GetComponent<Image>();
        dashCdImg = dashFrame.transform.Find("BlackCdImg").GetComponent<Image>();
    }
    public void ShowMagnetIndicator(float duration) {
        if(magnetFillCoroutine != null) {
            StopCoroutine(magnetFillCoroutine);
            magnetFillCoroutine = null;
            SetMagnetFill(0);
        }
        magnetActiveIndicator.gameObject.SetActive(true);
        magnetFillCoroutine = StartCoroutine(UpdateMagnetFillCoroutine(duration));
    }
    private void SetMagnetFill(float value) { magnetCdImg.fillAmount = value; }
    private IEnumerator UpdateMagnetFillCoroutine(float duration) {
        while (magnetCdImg.fillAmount < 1) {
            magnetCdImg.fillAmount += 1 / duration * Time.deltaTime;
            yield return null;
        }
        magnetActiveIndicator.gameObject.SetActive(false);
    }

    //generic cd method
    public void ShowWeaponIndicatorFill(float duration) {
        wpnFillCoroutine = ShowGenericIndicatorFill(wpnFillCoroutine,wpnCdImg,duration);
    }
    public void ShowAbilityIndicatorFill(float duration) {
        abilityFillCoroutine = ShowGenericIndicatorFill(abilityFillCoroutine,abilityCdImg,duration);
    }
    public void ShowDashIndicatorFill(float duration) {
        dashFillCoroutine = ShowGenericIndicatorFill(dashFillCoroutine,dashCdImg,duration);
    }
    //peak programming
    public Coroutine ShowGenericIndicatorFill(Coroutine coroutine, Image cdImg, float duration) {
        if(coroutine != null) {
            StopCoroutine(coroutine);
            return ShowGenericIndicatorFill(null, cdImg, duration);
        }
        SetCdFill(cdImg,1);
        return StartCoroutine(UpdateCdFillCoroutine(cdImg, duration));
        //Debug.Log("ShowWeaponIndicatorFill " + wpnFillCoroutine);
    }
    private void SetCdFill(Image img, float value) { img.fillAmount = value; }
    private IEnumerator UpdateCdFillCoroutine(Image img, float duration) {
        while (img.fillAmount > 0) {
            img.fillAmount -= 1 / duration * Time.deltaTime;
//            Debug.Log("FillAmount: " + img.fillAmount);
            yield return null;
        }
        img.fillAmount = 0;
    }
    
    public void UpdateRerollText() {
        rerollCounter.text = "Rerolls: " + player.stats.rerollCount;
    }
    [SerializeField] List<GameObject> buffGOs = new();
    /*
    private void UpdateBuffsUI() {
        foreach (var buffPair in buffs) {
            GameObject buffGO = Instantiate(buffTemplate,buffParent.transform);
            buffGO.transform.Find("Image").GetComponent<Image>().sprite = buffPair.Value;
        }
    }
    */
    public void AddBuffsToUI(string id, Sprite sprite) {
        //if already present
        foreach (GameObject buffGO in buffGOs) {
            if(buffGO.name == id) {
                buffGO.SetActive(true);
                return;
            }
        }
        //if not present
        GameObject go = Instantiate(buffTemplate,buffParent.transform);
        go.name = id;
        go.transform.Find("Image").GetComponent<Image>().sprite = sprite;
        go.SetActive(true);
        buffGOs.Add(go);
    }
    public void RemoveBuffFromUI(string name) {
        foreach (GameObject buffGO in buffGOs) {
            if(buffGO.name == name) {
                buffGO.SetActive(false);
                return;
            }
        }
    }
    [Header("Stats Display")]
    public List<StatsDisplay> instStatsDisplay;
    /*
    public void UpdateStatsDisplayUI() {
        Stats pstats = player.stats;
        List<double> statList = pstats.GetStatsList(1);
        List<string> statStrList = pstats.GetStatsStringList();

        instStatsDisplay = new List<StatsDisplay>();
        for(int i=0; i < statList.Count; i++) {
            GameObject go = Instantiate(statDisplayTemplate,statDisplayTemplate.transform.parent);
            string statName = statStrList[i];
            double statValue = statList[i];
            go.transform.Find("QuadStatText").GetComponent<Text>().text = statName;
            go.transform.Find("StatValue").GetComponent<Text>().text = statValue + "";
            //go.transform.Find("StatSprite").GetComponent<Image>().sprite = pstats.GetStatSprite(i);
            instStatsDisplay.Add(new StatsDisplay(statName,statValue,null));
        }
    }
    */

    //boss hp bar methods
    public void ActivateBossHpBar(bool active, float maxHP = -1, string name = "") {
        if(bossHpBarGO.activeInHierarchy == active) return;
        bossHpBarGO.SetActive(active);
        bossNameText.gameObject.SetActive(active);
        if(active) {
            bossNameText.text = name;
            UpdateBossHpHUD(maxHP,maxHP);
        }
    }
    public void UpdateBossHpHUD(float currHP, float maxHP) {
        if(!bossHpBar.gameObject.activeInHierarchy) 
            Debug.LogWarning("Trying to update unactive boss bar");
        
        float hpRatio = currHP / (float)maxHP;
        string hpTxt = currHP + "/" + maxHP;
        string hpPercent = (int)(hpRatio * 100) + "%";

        bossHpBar.localScale = new Vector3(hpRatio,1,1);
        bossHpText.text = hpTxt + " (" + hpPercent + ")";
    }
}

[Serializable]
public class StatsDisplay {
    public GameObject statGO;
    public string statID;
    public double value = -1; //set at runtime
    public Sprite sprite;
}

