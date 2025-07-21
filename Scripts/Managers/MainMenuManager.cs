using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour, IDataPersistence
{
    public static MainMenuManager instance;
    [SerializeField] private Canvas mainMenuCanvas;
    private DataPersistenceManager dataManager;
    [SerializeField] private GameObject scoreTab;
    [Header("Map Selection")]
    [SerializeField] private Canvas mapSelectionCanvas;
    [SerializeField] private GameObject mapElements;

    [Header("---Character Selection---")]
    [Header("Char Selection - Top")]
    [SerializeField] private Canvas characterSelectionCanvas;
    [SerializeField] private GameObject charBtn;
    //[HideInInspector] private GameObject[] characters;
    [SerializeField] private GameObject classSelectionElementTemplate;
    [SerializeField] private Text classNameText;
    [Header("Char Selection - Bottom")]
    [SerializeField] private GameObject characterSelectionBottom;
    /*
    [SerializeField] private GameObject leftPanel;
    [SerializeField] private GameObject rightPanel;
    [SerializeField] private GameObject centerPanel;
    */
    [Header("Left Panel")]
    //[SerializeField] private GameObject leftPanel;
    [SerializeField] private Image WpnImg;
    [SerializeField] private Image ProjImg;
    [SerializeField] private Text WpnName;
    [SerializeField] private Text WpnDesc; 
    [Header("Right Panel")]
    //[SerializeField] private GameObject rightPanel;
    [SerializeField] private Image PwrImg;
    [SerializeField] private Text PwrName;
    [SerializeField] private Text PwrDesc; 
    [Header("Center Panel")]
    //[SerializeField] private GameObject centerPanel;
    [SerializeField] private Text SkillTitle;
    [SerializeField] private Image SkillImg;
    [SerializeField] private Text SkillName;
    [SerializeField] private Text SkillDesc; 
    [Header("Clickable Buttons")]
    [SerializeField] private GameObject startBtn;
    [SerializeField] private GameObject buyBtn;
    [SerializeField] private GameObject lockBtn;
    [SerializeField] private GameObject upgrBtn;
    [SerializeField] private GameObject maxLvlBtn;
    [SerializeField] private Text upgrBtnCostTxt;
    [SerializeField] private Text upgrBtnTxt;
    [SerializeField] private Text goldCost;
    [Header("Collection Info Section")]
    [SerializeField] private Text collectionTitle;
    [SerializeField] private GameObject collectionTemplate;
    [SerializeField] private GameObject collectionBottomInfoPanel;
    [Header("Enemy Info Section")]
    [SerializeField] private Text enemyTitle;
    [SerializeField] private GameObject enemyTemplate;
    [SerializeField] private GameObject enemyBottomInfoPanel;
    [SerializeField] private GameObject enemyLayoutParent;
    [Header("Powerup Info Section")]
    [SerializeField] private GameObject powerupParent;
    [Header("Mythics Info Section")]
    [SerializeField] private GameObject mythicParent;
    [Header("Objectives Info Section")]
    [SerializeField] private Button objectiveTemplate;
    [SerializeField] private GameObject objectiveParent;
    [SerializeField] private GameObject objBotFrame;
    [SerializeField] private Text botObjTitle;
    [SerializeField] private Image botObjImg;
    [SerializeField] private Image botObjImg2;
    [SerializeField] private Text botObjReq;
    [SerializeField] private Text botObjUnlock;
    [SerializeField] private Text botObjIsObtained;
    [Header("Global Info")]
    [SerializeField] private GameObject goldDataFrame;
    private Text goldDataCounter;
    [SerializeField] private Text versionInfo;
    private Class[] classes;
    //instances ref
    private CharacterClassManager classInstance;
    private ObjectiveManager objInstance;
    private int goldData;
    [Header("Settings Initialization")]
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Text masterPercentText;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Text bgmPercentText;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Text sfxPercentText;
    [SerializeField] private Toggle fullscreenCheckmark;
    [SerializeField] private Toggle floatingTextCheckmark;
    [SerializeField] private Toggle enemyHpBarCheckmark;
    [Header("Map Selection Things")]
    private int currMapSelectedIndex = -1;
    [SerializeField] private GameObject mapSelectionAdditionalOptions;
    [SerializeField] private GameObject mapSelectionTestModeCheckmark;
    [SerializeField] private GameObject mapSelectionConfirmBtn;
    [SerializeField] private Button mapSelectionElementTemplate;
    [Header("Other")]
    [SerializeField] private GameObject downloadableVersionBanner;
    private void Awake() {
        if(instance != null) 
            Debug.Log(gameObject.name + " ha un'istanza duplicata: " + instance);
        instance = this;

        goldDataCounter = goldDataFrame.transform.GetChild(0).GetComponent<Text>();
        SetGoldFrameActive(false);
        #if !UNITY_WEBGL
            downloadableVersionBanner.SetActive(true);
        #else
            downloadableVersionBanner.SetActive(false);
        #endif
    }
    private void Start() {
        //Game data
        SettingsManager.instance.SetupSettingsManager(resolutionDropdown,bgmSlider,sfxSlider,masterSlider,bgmPercentText,sfxPercentText,masterPercentText,fullscreenCheckmark,floatingTextCheckmark,enemyHpBarCheckmark);
        classInstance = CharacterClassManager.instance;
        objInstance = ObjectiveManager.instance;
        GameObject dataManagerGO = GameObject.Find("DataPersistenceManager");
        if(dataManagerGO != null) {
            dataManager = dataManagerGO.GetComponent<DataPersistenceManager>();
            dataManager.LoadGameSetup();
        }
        else
            Debug.Log("No data manager found. No file loaded");
        
        UpdateVersionInfo();
    }
    private void UpdateVersionInfo() {
        versionInfo.text = "Version: " + Application.version;
    }
    //MapSelection
    private bool isEndlessMode = false; private bool isTestMode; 
    public void SetEndlessMode(bool flag) { isEndlessMode = flag; Debug.Log("Endless mode: " + isEndlessMode); } 
    public void SetTestMode(bool flag) { isTestMode = flag; Debug.Log("Test mode: " + isTestMode); }
    
    public void SelectMap(int index) { //usato dai bottoni della mappa
        currMapSelectedIndex = index;
        //MapManager.instance.LoadMap(index,isEndlessMode);
        mapSelectionConfirmBtn.SetActive(true);
        //Debug.Log("Map " + index + " selected");
        bool flag = MapManager.instance.IsMapCleared(index);
        mapSelectionAdditionalOptions.SetActive(flag);
    }
    public void LoadMap() {
        if(currMapSelectedIndex != -1) {
            MapManager.instance.LoadMap(currMapSelectedIndex,isEndlessMode,isTestMode);
            //SaveGame();
        }
        else 
            Debug.LogWarning("No map selected");
        
    }
    public void OpenMapSelection() {
        characterSelectionCanvas.gameObject.SetActive(false);
        mapSelectionCanvas.gameObject.SetActive(true);
        mapSelectionAdditionalOptions.SetActive(false);
        mapSelectionTestModeCheckmark.SetActive(TestManager.instance.IsTestModeOn);
        InitializeMaps();
    }
    private Button[] mapsGo;
    private void InitializeMaps() {
        //mapNames = MapManager.instance.GetSceneNames();
        int n = MapManager.instance.GetMapCount();
        mapSelectionElementTemplate.gameObject.SetActive(false);
        if(mapsGo != null) {
            for(int i = 0; i < n; i++) {
                mapsGo[i].gameObject.SetActive(false);
                Destroy(mapsGo[i]);
            }
        }
        mapsGo = new Button[n];
        for(int i = 0; i < n; i++)  {
            mapsGo[i] = Instantiate(mapSelectionElementTemplate, mapElements.transform, false);
            mapsGo[i].gameObject.SetActive(true);
            Text desc = mapsGo[i].transform.Find("MapDescFrame").transform.Find("MapDescription").GetComponent<Text>();
            
            GameObject iconGo = mapsGo[i].transform.Find("MapIcon").gameObject;
            Image img = iconGo.transform.Find("Image").GetComponent<Image>();
            //Text title = iconGo.transform.Find("MapName").GetComponent<Text>();
            Text title = mapsGo[i].transform.Find("MapDescFrame").transform.Find("MapName").GetComponent<Text>();

            if(MapManager.instance.IsUnlocked(i)) {
                title.text = MapManager.instance.GetSceneTitle(i) + "";
                img.sprite = MapManager.instance.GetScenePreview(i);
                desc.text = MapManager.instance.GetSceneDescription(i);
                mapsGo[i].interactable = true;
                int tempIndex = i;
                mapsGo[i].onClick.AddListener(() => SelectMap(tempIndex));
            }
            else {
                desc.text = "???";
                title.text = "???";
                img.color = Color.black;
                mapsGo[i].interactable = false;
            }
        }
    }
    //ClassSelection
    public void OpenClassSelection() {
        mainMenuCanvas.gameObject.SetActive(false);
        SetGoldFrameActive(true);
        characterSelectionCanvas.gameObject.SetActive(true);
        UpdateGoldDataCounter();
        SetupClassesUI();
    }
    int tempSelClassIndex;
    public void SelectClass(int index) { //usato dai bottoni della scelta della classe
        UpdateClassBottom(index);
        classInstance.SetPlayerClass(index);
        ChangeBtnCheck(index);
        tempSelClassIndex = index;
        //UpdateUpgradeClassBtn(index);
    }
    private void SetupClassesUI() { //schermata selezione classi
        classes = classInstance.classes;
        UpdateClassTop();
        UpdateClassBottom(-1);
    }

    private void UpdateGoldDataCounter() {
        //Debug.Log("updating gold data counter " + goldData);
        goldDataCounter.text = "x" + goldData;
    }

    private void UpdateClassTop() {
        classSelectionElements ??= new GameObject[classes.Length];
        for(int i = 0; i < classes.Length; i++) {
            UpdateClassSelectionElement(i);
        }
    }
    GameObject[] classSelectionElements;
    private GameObject UpdateClassSelectionElement(int index) {
        GameObject go;
        if(classSelectionElements[index] == null)
            go = Instantiate(classSelectionElementTemplate, charBtn.transform, false);
        else
            go = classSelectionElements[index];
        
        Text txt = go.transform.Find("ClassName").GetComponent<Text>();
        Image cs = go.transform.Find("CharacterSprite").GetComponent<Image>();
        txt.text = classes[index].getId();
        cs.sprite = classes[index].GetIdleSprite();

        go.name = "ClassSelectionElement_" + index;
        go.SetActive(true);

        Button btn = go.GetComponent<Button>();
        btn.onClick.AddListener(() => SelectClass(index));
        ChangeToBlackIfNotUnlocked(index,cs);

        classSelectionElements[index] = go;

        return go;
    }
    private void ChangeToBlackIfNotUnlocked(int index, Image sprite) {
        if(classInstance.GetClassProperty(index) != CharClassProperty.unlocked) 
            sprite.color = Color.black;
        else if(sprite.color == Color.black)  //nel caso nel quale è sbloccata e il colore è nero lo fa ritornare bianco
            sprite.color = Color.white;
    }
    

    private void ChangeBtnCheck(int index) {
        CharClassProperty currProp = classInstance.GetClassProperty(index);
        bool u = currProp == CharClassProperty.unlocked;
        bool b = currProp == CharClassProperty.buyable;
        bool l = currProp == CharClassProperty.objLocked;
        //Debug.Log("u ["+u+"] b [" + b + "] l" + l + "]");
        ShowButtons(u,b,l);
        if(b) goldCost.text = classInstance.classes[index].GetCost() + "G";
    }
    private void ShowButtons(bool startVisiblity, bool buyVisibility, bool lockVisibility) {
        startBtn.SetActive(startVisiblity);
        buyBtn.SetActive(buyVisibility);
        lockBtn.SetActive(lockVisibility);

        //if(buyVisibility) goldCost.text = classInstance.classes[tempSelClassIndex].GetCost() + "G"; è meglio averlo qua però non funziona
    }
    private string prevText;
    public void UpdateClassBottom(int index) {
        bool flag;
        if(index == -1)
            flag = false;
        else
            flag = true;

        characterSelectionBottom.SetActive(flag);
        
        if(!flag) {
            if(prevText != null)
                classNameText.text = prevText;
            lockBtn.SetActive(false);
            return;
        }
        //int tier = classInstance.classesTier[index]; //to do: il tier di class instance è diverso da quello attuale del powerup, fare in modo che venga inizializzato in classmanager
        //Text & Img setup
        //Weapon
        Weapon wpn = classes[index].GetWeaponTopHierarchy().getWeaponCore();
        if(wpn != null) {
            WpnImg.sprite = classes[index].getWeaponSkin();
            if(wpn is ProjectileWeapon projwpn) {
                ProjImg.sprite = projwpn.GetWeaponProjectileSprite();
//                Debug.Log("Weapon Size: " + projwpn.GetOriginalWeaponStats().weaponSize);
                ProjImg.transform.localScale = projwpn.GetOriginalWeaponStats().weaponSize * Vector3.one;
                //ProjImg.transform.localScale = projwpn.GetBulletPrefab().baseScale;

                //RectTransform rectTransform = ProjImg.GetComponent<RectTransform>();
                //float wpnSize = projwpn.GetOriginalWeaponStats().weaponSize;
                //rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x * wpnSize, rectTransform.sizeDelta.y * wpnSize);
                //rectTransform.localScale = new Vector3(wpnSize, wpnSize, 1);
            }
            else
                ProjImg.gameObject.SetActive(false);
            ChangeToBlackIfNotUnlocked(index,WpnImg);
            if(ProjImg != null) ChangeToBlackIfNotUnlocked(index,ProjImg);
            WpnName.text = wpn.title;
            //WpnDesc.text = wpn.description;
            WpnDesc.text = wpn.GetWeaponBaseStatsDetails();
        }
        //Ability
        Ability abi = classes[index].GetAbility();
        if(abi != null) {
            SkillImg.sprite = abi.GetSpriteRenderer().sprite;
            ChangeToBlackIfNotUnlocked(index,SkillImg);
            SkillTitle.text = "Skill [" + abi.GetAbilityType() + "]";
            SkillName.text = abi.GetTitle();
            SkillDesc.text = abi.GetDescription();
        }
        //Powerup
        PowerUp  pwu = classes[index].getStartingPowerUp();
        if(pwu != null) {
            PwrImg.sprite = pwu.GetComponent<SpriteRenderer>().sprite;
            ChangeToBlackIfNotUnlocked(index,PwrImg);
            PwrName.text = pwu.title; //+ " " + Utility.ToRoman(tier); pwu.tier = tier-1;
            PwrDesc.text = pwu.GetDescription();
        }
        prevText ??= classNameText.text;
        classNameText.text = classes[index].GetName();
    }   

    public void BuyClass() {
        int cost = classInstance.classes[tempSelClassIndex].GetCost();
        if(goldData >= cost) {
            classInstance.UnlockClass(tempSelClassIndex);
            classInstance.SetPlayerClass(tempSelClassIndex);
            AddGold(-cost);
            UpdateClassTop();
            UpdateClassBottom(tempSelClassIndex);
            ShowButtons(true,false,false);
            //UpdateUpgradeClassBtn(tempSelClassIndex);
            //SaveGame();
            AudioManager.instance.PlaySingleSound("Buy",false);
        }
        else {
            AudioManager.instance.PlaySingleSound("Youcant",false);
        }
    }

    public void UpgradeClass() {
        if(classInstance.IsClassMaxLevel(tempSelClassIndex)){
            Debug.Log("Upgrade failed. Class is maxed");
            return;
        }
            
        int classTier = classInstance.classesTier[tempSelClassIndex]-1;
        int cost = classInstance.upgradeCost;
        if(goldData >= cost) {
            classInstance.UpgradeClass(tempSelClassIndex);
            AddGold(-cost);
            //SaveGame();
            UpdateClassBottom(tempSelClassIndex);
            UpdateGoldDataCounter();
            //UpdateUpgradeClassBtn(tempSelClassIndex);
        }
        else
            Debug.Log("Insufficient Gold");
    }

    public void UpdateUpgradeClassBtn(int index) {
        if(classInstance.IsClassOwned(index)) {//if(classInstance.isClassOwned[index]) {
            upgrBtn.SetActive(true);
            int classTier = classInstance.classesTier[index]-1;
            upgrBtnCostTxt.text = classInstance.upgradeCost + "";

            if(classInstance.IsClassMaxLevel(index)) {
                upgrBtn.SetActive(false);
                maxLvlBtn.SetActive(true);
            }
            else {
                upgrBtn.SetActive(true);
                maxLvlBtn.SetActive(false);
            }
        }
        else {
            upgrBtn.SetActive(false);
            maxLvlBtn.SetActive(false);
        }
            
    }
    //se tornando indietro
    public void ResetClassSelection() {
        tempSelClassIndex = 0;
        startBtn.SetActive(false);
        upgrBtn.SetActive(false);
        buyBtn.SetActive(false);
        maxLvlBtn.SetActive(false);
    }
    //Collection Table
    private Image collectionBottomImage;
    private Text collectionBottomDesc;
    private Text collectionBottomTitle;
    //Cells
    private GameObject[] powerupCells;
    private GameObject[] mythicCells;
    public void OpenCollectionTable(string stype) {
        //Debug.Log("Open Collection Table --> " + stype + " " + powerupCells + " " + mythicCells);
        Item[] table = null;
        GameObject parent = null;
        string goName = "DefaultName_";
        GameObject[] cells = null;
        ItemType type = ItemType.Powerup;
        switch(stype) {
            case "powerup":
                powerupParent.SetActive(true);
                mythicParent.SetActive(false);
                collectionTitle.text = "Powerups Collection";
                if(powerupCells != null)
                    return;
                type = ItemType.Powerup;
                table = PowerupTableManager.instance.GetPowerupTable().ToArray();
                powerupCells = new GameObject[table.Length];
                parent = powerupParent;
                goName = "PowerupCell_";
                cells = powerupCells;
                break;
            case "mythic":
                mythicParent.SetActive(true);
                powerupParent.SetActive(false);
                collectionTitle.text = "Mythics Collection";
                if(mythicCells != null)
                    return;
                type = ItemType.Mythic;
                table = PowerupTableManager.instance.GetMythicsTable().ToArray();
                mythicCells = new GameObject[table.Length];
                parent = mythicParent;
                goName = "MythicCell_";
                cells = mythicCells;
                
                break;
            case "ability":
                Debug.Log("ability not implemented yet");
                return;
                //break;
            case "weapon":
                Debug.Log("weapon not implemented yet");
                return;
                //break;
            default :
                Debug.LogWarning("Unexpected ItemType");
                return;
        }
        
        for(int i=0; i < table.Length; i++) {
//            Debug.Log(table[i]);
            cells[i] = Instantiate(collectionTemplate, parent.transform, false);
            cells[i].name = goName + i.ToString();
            cells[i].SetActive(true);
            Item itm = table[i];
            Image img = cells[i].transform.Find("Image").GetComponent<Image>();
            TextMeshProUGUI txt = cells[i].transform.Find("Text").GetComponent<TextMeshProUGUI>();

            int currentIndex = i;
            cells[i].GetComponent<Button>().onClick.AddListener(() => SelectionCollectionElement(currentIndex,type));
            if(itm is PowerUp pu) {
                if(!PowerupTableManager.instance.IsPowerupLocked(pu)) {
                    img.sprite = pu.GetComponent<SpriteRenderer>().sprite;
                    img.gameObject.SetActive(true);
                    txt.gameObject.SetActive(false);
                }
                else {
                    //img.color = Color.black;
                    img.gameObject.SetActive(false);
                    txt.gameObject.SetActive(true);
                }
            }
        }
        collectionTemplate.SetActive(false);
        //setup for lower panel
        collectionBottomImage = collectionBottomInfoPanel.transform.Find("Image").GetComponent<Image>();
        collectionBottomDesc = collectionBottomInfoPanel.transform.Find("Description").GetComponent<Text>();
        collectionBottomTitle = collectionBottomInfoPanel.transform.Find("Title").GetComponent<Text>();
        BottomCollectionInfoSetActive(false);
    }
    Item currentItem;
    private void SelectionCollectionElement(int index, ItemType type) {
        currentItem = null;

        switch(type) {
            case ItemType.Powerup:
                currentItem = PowerupTableManager.instance.GetPowerupTable()[index];
                break;
            case ItemType.Mythic:
                currentItem = PowerupTableManager.instance.GetMythicsTable()[index];
                break;
            case ItemType.Ability:
                Debug.Log("abilities ecchime");
                break;
            case ItemType.Weapon:
                Debug.Log("weapons ecchime");
                break;
            default :
                Debug.LogWarning("Unexpected ItemType");
                return;
        }
        
        RefreshCollectionDescription();
        BottomCollectionInfoSetActive(true);
    }
    Enemy currentEnemy;
    private void SelectionEnemyElement(int index) {
        currentEnemy = enemyTable[index];
        Debug.Log("Selected Enemy: " + currentEnemy);
        //RefreshCollectionDescription();
        //BottomCollectionInfoSetActive(true);
    }
    public void BottomCollectionInfoSetActive(bool active) {
        collectionBottomImage.gameObject.SetActive(active);
        collectionBottomDesc.gameObject.SetActive(active);
        collectionBottomTitle.gameObject.SetActive(active);
    }
    private void RefreshCollectionDescription() {
        if(currentItem == null)
            return;
        collectionBottomImage.sprite = currentItem.GetComponent<SpriteRenderer>().sprite;
        if(currentItem is PowerUp pu) {
            if(!PowerupTableManager.instance.IsPowerupLocked(pu)) {
                collectionBottomDesc.text = GetDescription(pu);
                collectionBottomTitle.text = pu.GetTitle();
                collectionBottomImage.color = Color.white;
            }
            else {
                collectionBottomImage.color = Color.black;
                collectionBottomDesc.text = "???";
                collectionBottomTitle.text = "???";
            }
        }
    }
    private enum DescriptionType {
        Base,
        Upgrade,
    }
    DescriptionType descType;
    public void SetDescriptionType(string type) {
        switch(type) {
            case "base":
                descType = DescriptionType.Base;
                break;
            case "upgrade":
                descType = DescriptionType.Upgrade;
                break;
            default:
                descType = DescriptionType.Base;
                Debug.LogWarning("Unexpected DescriptionType, setting it to base");
                return;
        }
        RefreshCollectionDescription();
    }
    private string GetDescription(Item itm) {
        switch(descType) {
            case DescriptionType.Base: return itm.GetDescription();
            case DescriptionType.Upgrade:
                if(itm is TieredItem ti) return ti.GetUpgrDesc();
                else return itm.GetDescription();
            default:
                Debug.LogWarning("Unexpected DescriptionType");
                return itm.GetDescription();
        }
    }
    //Objectives info
    private Button[] objCells;
    private GameObject objBottomPanel;
    public void OpenObjectiveInfo() {
        if(objCells != null)
            return;
        Objective[] objectives = ObjectiveManager.instance.objectives;
        int n = objectives.Length;
        objCells = new Button[n];

        objBottomPanel = objBotFrame.transform.GetChild(0).gameObject;

        for(int i=0; i < n; i++) {
            objCells[i] = Instantiate(objectiveTemplate, objectiveParent.transform, false);
            objCells[i].name = "ObjectiveCell_" + i.ToString();
            objCells[i].gameObject.SetActive(true);
            Objective obj = objectives[i];
            Image img = objCells[i].transform.Find("Framed Image").GetChild(0).GetComponent<Image>();
            Text txt = objCells[i].transform.Find("Description").GetComponent<Text>();
            Toggle mrk = objCells[i].transform.Find("Checkmark").GetComponent<Toggle>();

            img.sprite = obj.GetSprite();
            if(obj.unlockedClass != null) {
                img.type = Image.Type.Filled;
                img.fillMethod = Image.FillMethod.Vertical;
                img.fillAmount = 0.55f;
                img.fillOrigin = 1;

                RectTransform rt = img.GetComponent<RectTransform>();
                                                //-17.6f
                rt.offsetMin = new Vector2(-4, -20); // left, bottom
                rt.offsetMax = new Vector2(4, 0); // -right, top
            }
            
            txt.text = obj.GetReqDesc();

            bool isObjDone = ObjectiveManager.instance.isObjectiveDone(i);
            mrk.isOn = isObjDone;
            
            //if(isObjDone)  img.color = Color.white;
            //else img.color = Color.black;
            //Debug.Log("Objective [" + i + "] = " + objCells[i].name);
            int currentIndex = i; //THIS IS NECESSARY !!!!!!!!!!!! xd
            objCells[i].onClick.AddListener(() => SelectObjectiveElement(currentIndex));
        }
        objectiveTemplate.gameObject.SetActive(false);
        objBottomPanel.SetActive(false);
    }

    public void SelectObjectiveElement(int index) {
        Objective obj = ObjectiveManager.instance.objectives[index];
        botObjTitle.text = obj.GetTitle();
        if(obj.GetUnlockedClassSprite() != null && obj.GetUnlockedPowerupSprite() != null) {
            botObjImg.sprite = obj.GetUnlockedClassSprite();
            botObjImg2.sprite = obj.GetUnlockedPowerupSprite();
            botObjImg2.gameObject.SetActive(true);
        }
        else {
            botObjImg.sprite = obj.GetSprite();
            botObjImg2.gameObject.SetActive(false);
        }
        botObjReq.text = "Requirements: " + obj.GetReqDesc();
        botObjUnlock.text = obj.GetWhatUnlocks();

        if(ObjectiveManager.instance.isObjectiveDone(index))
            botObjIsObtained.text = "Obtained";
        else
            botObjIsObtained.text = "Unobtained";

        if(!objBottomPanel.activeSelf)
            objBottomPanel.SetActive(true);
    }
    List<Enemy> enemyTable;
    public void OpenBestiary() {
        //Debug.Log("Open Collection Table --> " + stype + " " + powerupCells + " " + mythicCells);
        enemyTable = MapManager.instance.GetAllEnemiesList();
        string goName = "DefaultName_";
        GameObject[] cells = new GameObject[enemyTable.Count];

        for(int i=0; i < enemyTable.Count; i++) {
            Debug.Log(enemyTemplate);
            cells[i] = Instantiate(enemyTemplate, enemyLayoutParent.transform, false);
            cells[i].name = goName + i.ToString();
            cells[i].SetActive(true);
            Enemy enemy = enemyTable.ElementAt(i);
            Image img = cells[i].transform.Find("Image").GetComponent<Image>();
            TextMeshProUGUI txt = cells[i].transform.Find("Text").GetComponent<TextMeshProUGUI>();

            int currentIndex = i;
            cells[i].GetComponent<Button>().onClick.AddListener(() => SelectionEnemyElement(currentIndex));
            
            if(MapManager.instance.IsUnlocked(MapManager.instance.GetIndexByMapData(MapManager.instance.GetMapDataByEnemy(enemy)))) {
                img.sprite = enemy.GetComponent<SpriteRenderer>().sprite;
                img.gameObject.SetActive(true);
                txt.gameObject.SetActive(false);
            }
            else {
                //img.color = Color.black;
                img.gameObject.SetActive(false);
                txt.gameObject.SetActive(true);
            }
            
        }
        collectionTemplate.SetActive(false);
        //setup for lower panel
        collectionBottomImage = collectionBottomInfoPanel.transform.Find("Image").GetComponent<Image>();
        collectionBottomDesc = collectionBottomInfoPanel.transform.Find("Description").GetComponent<Text>();
        collectionBottomTitle = collectionBottomInfoPanel.transform.Find("Title").GetComponent<Text>();
        BottomCollectionInfoSetActive(false);
    }

    public void AddGold(int amount) {
        goldData += amount;
        SaveGame();
        LoadGame();
        UpdateGoldDataCounter();
    }
    public int GetGoldData() { return goldData; }
    public void SetGoldFrameActive(bool flag) {
        goldDataFrame.SetActive(flag);
        if(flag) UpdateGoldDataCounter();
    }
    //Used by main menu "quit" button
    public void Quit() {
        SaveGame();
        //SaveSettings();
        Application.Quit();
    }
    public void LoadData(GameData data) {
        classInstance.SetClassesTier(data.classes);//classInstance.classesTier = data.classesTier;
        goldData = data.gold;

        //classInstance.SetClassOwned(data.ownedClasses);//classInstance.isClassOwned = data.ownedClasses;
        objInstance.SetObjDone(data.objectives);
        //MapManager.instance.SetUnlockedList(data.mapUnlocked);
        MapManager.instance.SetClearedMaps(data.maps);
        PowerupTableManager.instance.SetLockedPTable(objInstance.GetUnlockedPowerupsList());
        PowerupTableManager.instance.setupAvailablePowerups();
    }

    public void SaveData(ref GameData data) {
        data.classes = classInstance.GetClassesTierToDictionary();
        data.gold = goldData;
        //data.ownedClasses = classInstance.GetClassOwnedToList();
        data.objectives = objInstance.GetObjDone();
        data.maps = MapManager.instance.GetClearedMaps();
        //data.mapUnlocked = MapManager.instance.GetUnlockedList();
    }

    //Incapsulazione Data
    public void SaveGame() {
        Debug.Log("--MainMenuManager Save Game--");
        if(dataManager != null)
            dataManager.SaveGame();
        else
            Debug.LogWarning("Data Manager is null. Failed to Save Game");
    }
    public void LoadGame() {
        Debug.Log("--MainMenuManager Load Game--");
        if(dataManager != null)
            dataManager.LoadGame();
        else
            Debug.LogWarning("Data Manager is null. Failed to Load Game");
    }
}
