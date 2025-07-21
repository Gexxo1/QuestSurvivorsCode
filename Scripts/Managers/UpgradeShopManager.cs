using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopManager : MonoBehaviour, IDataPersistence
{
    public static UpgradeShopManager instance;
    [SerializeField] private OffGameUpgrades[] originalTable;
    private OffGameUpgrades[] table;
    //UI stuff
    [Header("UI Stuff")]
    [SerializeField] private Button upgrBtn;
    [SerializeField] private Text goldCounter;
    [SerializeField] private GameObject parentLayout;
    [SerializeField] private GameObject template;
    [SerializeField] private GameObject bottomInfoPanel;
    [SerializeField] private GameObject buyBtn;
    [SerializeField] private GameObject maxBtn;
    private Text goldCost;
    private Image bottomImage;
    private Text bottomTitle;
    private Text bottomDesc;
    //data
    //private int goldData;
    private int[] upgradesTiers;
    MainMenuManager mmInst;
    private void Awake() {
        if(instance != null) 
            Debug.Log(gameObject.name + " ha un'istanza duplicata: " + instance);
        instance = this;
    }
    private void Start() {
        upgradesTiers = new int[table.Length];
        for(int i=0; i < table.Length; i++)
            upgradesTiers[i] = table[i].GetTier();
        mmInst = MainMenuManager.instance;
    }
    /*
    private void OffgameUpgradesInstance() {
        table = new OffGameUpgrades[originalTable.Length];
        for(int i=0; i < originalTable.Length; i++) 
            table[i] = Instantiate(originalTable[i]);
    }
    */
    private bool isInitialized = false;
    struct UpgradeCell {
        public GameObject go;
        public GameObject[] tierCells;
    }
    UpgradeCell[] upgrElem;
    public void OpenCollectionTable() {
        if(isInitialized) return;

        mmInst.SetGoldFrameActive(true);
        isInitialized = true;
        upgrElem = new UpgradeCell[table.Length];
        parentLayout.SetActive(true);
        //collectionTitle.text = "Powerups Collection";
        for(int i=0; i < table.Length; i++) {
            upgrElem[i].go = InstatiateTemplate(i);
            upgrElem[i].go.name = "UpgradeItem" + i.ToString();
            upgrElem[i].go.SetActive(true);
            OffGameUpgrades upgrData = table[i];
            Image img = upgrElem[i].go.transform.Find("Image").GetComponent<Image>();
            TextMeshProUGUI txt = upgrElem[i].go.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            Text title = upgrElem[i].go.transform.Find("Title").GetComponent<Text>();
            
            int currentIndex = i;
            upgrElem[i].go.GetComponent<Button>().onClick.AddListener(() => SelectionCollectionElement(currentIndex));
            title.text = upgrData.GetName();
            img.sprite = upgrData.GetIcon();
            img.gameObject.SetActive(true);
            txt.gameObject.SetActive(false);
        }
        SetupInfo();
    }
    //[SerializeField] private GameObject[] upgrCells; //TODO: move remove serialize field
    public GameObject InstatiateTemplate(int index) {
        GameObject tmp = Instantiate(template, parentLayout.transform, false);
        Transform cellLayout = tmp.transform.Find("CellsLayout");
        GameObject tierCellTemplate = cellLayout.Find("CellTierTemplate").gameObject;
        //Text title = cellLayout.Find("Title").gameObject;
        upgrElem[index].tierCells = new GameObject[table[index].GetMaxTier()];
        for(int i=0; i < table[index].GetMaxTier(); i++) {
            upgrElem[index].tierCells[i] = Instantiate(tierCellTemplate, cellLayout.transform, false);
            //cell.SetActive(true);
        }
        tierCellTemplate.SetActive(false);
        return tmp;
    }
    private int selectedIndex;
    private void SelectionCollectionElement(int index) {
        selectedIndex = index;
        OffGameUpgrades upgr = table[index];
        bottomInfoPanel.SetActive(true);
        buyBtn.SetActive(true);
        bool isMax = upgr.IsMaxTier();
        upgradesTiers[index] = upgr.GetTier(); 
        UpdateInfo(upgr);
        SetActiveBuyBtnSelection(!isMax);
    }
    [SerializeField] private Color notUnlocked;
    [SerializeField] private Color unlocked;
    private void UpdateInfo(OffGameUpgrades upgr) {
        if(!upgr.IsMaxTier())
            goldCost.text = upgr.GetCost().ToString() + "G";
        bottomImage.sprite = upgr.GetIcon();
        bottomTitle.text = upgr.GetName();
        bottomDesc.text = upgr.GetUpgradeDescription();
    }
    private void UpdateAllTierCells() {
        for(int i=0; i < table.Length; i++)
            UpdateTierCell(i);
    }
    private void UpdateTierCell(int index) {
        for(int i=0; i < table[index].GetMaxTier(); i++)
            upgrElem[index].tierCells[i].GetComponentInChildren<RawImage>().color = i < upgradesTiers[index] ? unlocked : notUnlocked;
    }
    private void ResetTierCells() {
        for(int i=0; i < table.Length; i++) {
            for(int j=0; j < table[i].GetMaxTier(); j++)
                upgrElem[i].tierCells[j].GetComponentInChildren<RawImage>().color = notUnlocked;
            table[i].SetTier(0);
        }
    }
    public void UpdateGoldDataCounter() {
        goldCounter.text = "x" + mmInst.GetGoldData().ToString();
    }
    private void SetupInfo() {
        bottomImage = bottomInfoPanel.transform.Find("Image").GetComponent<Image>();
        bottomTitle = bottomInfoPanel.transform.Find("Title").GetComponent<Text>();
        bottomDesc = bottomInfoPanel.transform.Find("Description").GetComponent<Text>();
        goldCost = buyBtn.transform.Find("UpgrCostText").GetComponent<Text>();

        buyBtn.SetActive(false);
        maxBtn.SetActive(false);
        template.SetActive(false);
        bottomInfoPanel.SetActive(false);

        UpdateAllTierCells();
    }

    public void Upgrade() {
        int index = selectedIndex;
        if(table[index].IsMaxTier()) {
            Debug.Log("Upgrade " + table[index] + " is Maxed Out");
            return;
        }
        int cost = table[index].GetCost();  
        if(mmInst.GetGoldData() >= cost) {
            table[index].Upgrade();
            if(table[index].IsMaxTier()) 
                SetActiveBuyBtnSelection(false);
            mmInst.AddGold(-cost);
            upgradesTiers[index]++;
            UpdateInfo(table[index]);
            //UpdateSelectedTierCell();
            UpdateTierCell(selectedIndex);
            AudioManager.instance.PlaySingleSound("Buy", false);
        }
        else {
            AudioManager.instance.PlaySingleSound("Youcant", false);
        }
    }
    public void Refund() {
        //reset
        mmInst.AddGold(GetAllGoldCost());
        ResetTierCells();
        //UpdateInfo(table[selectedIndex]);
        ResetSelection();
        
    }

    private void ResetSelection() {
        bottomInfoPanel.SetActive(false);
        upgrBtn.gameObject.SetActive(false);
        buyBtn.SetActive(false);
        maxBtn.SetActive(false);
        Debug.Log("Reset Selection " + gameObject.name);
        if(ButtonSelectionIndicator.selectedButton != null)
            ButtonSelectionIndicator.selectedButton.Deselect();
    }

    private int GetAllGoldCost() {
        int sum = 0;
        for(int i=0; i < table.Length; i++) 
            sum += table[i].GetCostSum();
        return sum;
    }

    private void SetActiveBuyBtnSelection(bool flag) {
        buyBtn.SetActive(flag);
        maxBtn.SetActive(!flag);
    }

    public StatsUp GetAllStatsSum() {
        StatsUp sum = ScriptableObject.CreateInstance<StatsUp>();
        foreach(OffGameUpgrades upgr in table) {
            for(int i=0; i < upgr.GetTier(); i++)
                sum.AddToStats(upgr.GetStatUpgrade());
        }
        return sum;
    }
    private void OffgameUpgradesInstance(SerializableDictionary<string, int> upgrades) {
        table = new OffGameUpgrades[originalTable.Length];
        bool flag = upgrades.Count == 0;
        for (int i = 0; i < originalTable.Length; i++) {
            table[i] = Instantiate(originalTable[i]);
            if(flag) continue;
            string tableKey = originalTable[i].GetTitle();
            if (upgrades.ContainsKey(tableKey)) {
                int tier = upgrades[tableKey];
                table[i].SetTier(tier);
            }
        }
    }
    public SerializableDictionary<string,int> GetUpgrades() {
        SerializableDictionary<string,int> sd = new();
        foreach(OffGameUpgrades up in table)
            sd.Add(up.GetTitle(), up.GetTier());
        return sd;
    }
    private void Save() { MainMenuManager.instance.SaveGame(); }
    private void Load() { MainMenuManager.instance.LoadGame(); }
    public void LoadData(GameData data) {
        //goldData = data.gold;
        OffgameUpgradesInstance(data.upgrades);
    }

    public void SaveData(ref GameData data) {
        //data.gold = goldData;
        data.upgrades = GetUpgrades();
    }
}
