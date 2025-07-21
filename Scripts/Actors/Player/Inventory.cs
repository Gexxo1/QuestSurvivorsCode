using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour 
{
    public List<WeaponTopHierarchy> weaponBag;
    //public int weaponCarryLimit = 1;
    public List<PowerUp> itemBag;
    private const int powerupCarryLimit = 8;
    public List<PowerUp> mythicBag;
    public List<GameObject> consumablesBag; //to implement: possibilità di tenere da parte pozioni
    private Ability currAbility;
    [SerializeField] private DashBase dashBaseAbility;
    public int currWpnIndex = 0;
    public float swapCd;
    [SerializeField] GameObject wgo; //weapon
    public GameObject igo; //powerup & mythic powerup
    [SerializeField] GameObject pgo; //powerup
    [SerializeField] GameObject mgo; //mythic powerup
    [SerializeField] GameObject ago; //ability
    private float lastPressed;
    [SerializeField] private Magnet magnet;
    [Header("Indicators")]
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private GameObject indicatorParent;
    [SerializeField] private float chargeIndicatorsDistanceX = 1.5f;
    void Awake()
    {
        InitWeaps();
        InitItems();
        InitMythic();
    }
    private void InitWeaps() {
        int wpnCount = wgo.transform.childCount;
        weaponBag = new List<WeaponTopHierarchy>();
        for(int i = 0; i < wpnCount; i++)  {
            WeaponTopHierarchy wpn = wgo.transform.GetChild(i).GetComponent<WeaponTopHierarchy>();
            weaponBag.Add(wpn);
            weaponBag[i].gameObject.SetActive(false);
        }        

        if(weaponBag.Count > 0)
            weaponBag[0].gameObject.SetActive(true);
    }
    private void InitItems() { //To do: cambiare pure i metodi sotto, sono utilizzati dalla UI, potrebbero creare problemi
        //int itmCount = pgo.transform.childCount;
        int pwrCount = pgo.transform.childCount;
        itemBag = new List<PowerUp>();
        for(int i=0; i < pwrCount; i++) {
            PowerUp powerup = pgo.transform.GetChild(i).GetComponent<PowerUp>();
            itemBag.Add(powerup);
        }
    }

    private void InitMythic() { 
        int mythCount = mgo.transform.childCount;
        mythicBag = new List<PowerUp>();
        for(int i=0; i < mythCount; i++) {
            PowerUp powerup = mgo.transform.GetChild(i).GetComponent<PowerUp>();
            mythicBag.Add(powerup);
        }
    }
    private ThrowProjectile throwProjectile;
    public void AddWeaponSize(Vector3 sizeIncrease) {
        //Debug.Log("sizeIncrease: " + sizeIncrease);
        getCurrWeapon().transform.localScale += sizeIncrease;
        //getCurrWeapon().extraProjScale += sizeIncrease;
        igo.transform.localScale += sizeIncrease/2; //incrementa tutti i powerup
        if(throwProjectile == null)
            InitThrowProjectile();
        //if(throwProjectile != null) throwProjectile.stats.weaponSize += sizeIncrease.magnitude;
        if(getCurrWeapon().TryGetComponent(out Bow bow))
            bow.IncreaseVisualArrowsSize(sizeIncrease);
    }
    public void SetWeaponSize(Vector3 size) {
//        Debug.Log("Weapon size set: " + size);
        getCurrWeapon().transform.localScale = size;
        //getCurrWeapon().extraProjScale = size;
        igo.transform.localScale = size/2; //metà rispetto al val originale
        if(throwProjectile == null)
            InitThrowProjectile();
        //if(throwProjectile != null) throwProjectile.stats.weaponSize = size.magnitude;
        if(getCurrWeapon().TryGetComponent(out Bow bow))
            bow.SetVisualArrowsSize(size);
    }
    private void InitThrowProjectile() {
        if(ago.transform.GetComponentInChildren<ThrowProjectile>() != null)
            throwProjectile = ago.transform.GetComponentInChildren<ThrowProjectile>();
    }
    public void SwapWeapon() {
        if (weaponBag.Count <= 1)
            return;

        //Debug.Log("Swapping weapon [" + GetWeaponByIndex(getCurrWeaponIndex()) + "] -> [" + GetWeaponByIndex((getCurrWeaponIndex() + 1) % weaponBag.Count) + "]");
        int prevIndex = currWpnIndex;
        float prevRotation = getCurrRootWeapon().getWpnOrigin().transform.rotation.z;
        getCurrRootWeapon().gameObject.SetActive(false);
        currWpnIndex = (currWpnIndex + 1) % weaponBag.Count;
        getCurrRootWeapon().gameObject.SetActive(true);
        /* non funziona perchè l'animator sovrascrive la rotazione
        if(getCurrRootWeapon().getWeaponCore() is MeleeWeapon currMw && GetWeaponByIndex(prevIndex) is MeleeWeapon prevMw) {
            Debug.Log(prevMw.IsFirstSwing() + " | curr: " + getCurrRootWeapon().getWpnOrigin().transform.rotation + " prev: " + prevRotation);
            currMw.SetSwing(prevMw.IsFirstSwing());
            //getCurrRootWeapon().getWpnOrigin().transform.rotation.z = prevRotation;
            getCurrRootWeapon().getWpnOrigin().transform.rotation = Quaternion.Euler(0, 0, prevRotation);
            //Debug.DrawLine(getCurrRootWeapon().getWpnOrigin().transform.position, getCurrRootWeapon().getWpnOrigin().transform.position + getCurrRootWeapon().getWpnOrigin().transform.up, Color.red, 2f);
        }
        */
        if(getCurrRootWeapon().getWeaponCore() is MeleeWeapon currMw && GetWeaponByIndex(prevIndex) is MeleeWeapon prevMw) 
            if(prevMw.IsFirstSwing() != currMw.IsFirstSwing())
                currMw.StartSwingAnimation("SkipSwing");
        
        getCurrWeapon().SetLastShot();
        
        MenuManager.instance.UpdateWeaponHUD(getCurrWeaponSprite());
    }
    public WeaponTopHierarchy getCurrRootWeapon() { //Ottieni la root di un'arma
        if (weaponBag.Count == 0)
                return null;
        return weaponBag[currWpnIndex];
    }
    public ProjectileWeapon getCurrWeapon() {  //Ottieni il "core" dell'arma
        if (weaponBag.Count == 0) {
            Debug.LogError("'getCurrWeapon()' --> no weapon available");
            return null;
        }
        return weaponBag[currWpnIndex].getWeaponCore();
    }
    public ProjectileWeapon GetWeaponByIndex(int index) {
        if(index >= weaponBag.Count)
            return null;
        return weaponBag[index].getWeaponCore();
    }
    public WeaponTopHierarchy GetRootWeaponByIndex(int index) {
        if(index >= weaponBag.Count)
            return null;
        return weaponBag[index];
    }
    public void AddStatsToMainWeapon(WeaponStats add) {
        //getCurrWeapon().UpdateWeaponStats(add);
        UpdateAllWeaponsStats(add);
    }
    public void AddStatsToMainWeapon(BaseWeaponStats add) {
        //getCurrWeapon().UpdateWeaponStats(add);
        UpdateAllWeaponsStats(add);
    }
    public void RemoveStatsToMainWeapon(WeaponStats sub) {
        //getCurrWeapon().UpdateWeaponStats(sub,false);
        UpdateAllWeaponsStats(sub,false);
    }
    public void RemoveStatsToMainWeapon(BaseWeaponStats sub) {
        //getCurrWeapon().UpdateWeaponStats(sub,false);
        UpdateAllWeaponsStats(sub,false);
    }
    public void UpdateAllWeaponsStats(WeaponStats add, bool isAdded = true) {
        foreach(WeaponTopHierarchy wb in weaponBag) {
//            Debug.Log("Updating weapon stats: " + wb);
            wb.getWeaponCore().UpdateWeaponStats(add, isAdded);
        }
    }
    public void UpdateAllWeaponsStats(BaseWeaponStats add, bool isAdded = true) {
        foreach(WeaponTopHierarchy wb in weaponBag) {
//            Debug.Log("Updating weapon stats: " + wb);
            wb.getWeaponCore().UpdateWeaponStats(add, isAdded);
        }
    }
    public void AddStatsToPowerupWeapons(WeaponStats add) {
        foreach(Item itm in itemBag) {
            if(itm.TryGetComponent(out WeaponPowerup wpn)) {
                if(itm.TryGetComponent(out ProjectileWeaponPowerup pwpn))
                    pwpn.AddToStats(add);
                else
                    wpn.AddToStats(add);
            }
        }
    }
    public Sprite getCurrWeaponSprite() {  //Ottieni il "core" dell'arma
        if(getCurrWeapon().TryGetComponent(out SpriteRenderer sr))
            return sr.sprite;
        
        Debug.LogError("'getCurrWeaponSprite()' --> weapon has no sprite");
        return null;
    }

    public List<PowerUp> GetAllPowerups() {
        return itemBag;
    }
    public PowerUp getItemByIndex(int i) {
        if(i >= itemBag.Count)
            return null;
        return itemBag[i];
    }

    public PowerUp getMythicByIndex(int i) {
        if(i >= mythicBag.Count)
            return null;
        return mythicBag[i];
    }

    public void AddItemToInventory(GameObject item, bool isPickup = true) {
        if(item.TryGetComponent(out PowerUp pu))
            if(!pu.isMythic)
                AddPowerupToInventory(pu);
            else
                AddMythicPowerupToInventory(pu);
        else if(item.TryGetComponent(out WeaponTopHierarchy wth))
            AddWeaponToInventory(wth);
        else if(item.TryGetComponent(out Collectable c))
            if(isPickup)
                c.OnPickup();
            else
                c.OnCollect();
        else
            Debug.Log("addToInventory--> " + item + " is not an item. Failed adding to inventory");
    }
    /*
        il powerup passa per un ciclo iniziale per controllare se già ne esiste uno con lo stesso nome, 
        se già esiste lo upgradiamo e torniamo
        altrimenti, istanziamo il nuovo GO e lo aggiungiamo alla lista
    */
    public void AddPowerupToInventory(PowerUp powerup) {
//        Debug.Log("aggiunto '" + powerup + "' all'inventario");
        //Case 1: powerup is already there
        for(int i=0; i < itemBag.Count; i++) {
            if(itemBag[i].title == powerup.title) {
//                Debug.Log(itemBag[i].title + " == " + powerup.title);
                itemBag[i].tryUpgradePowerup();
                return;
            }  
        }
        //pu.SetMythic(isMythic);
        //Case 2: powerup is new 
        //Case 2.1: inventory is full
        if(itemBag.Count >= powerupCarryLimit) {
            Debug.Log("Inventory is full -> Not adding powerup + " + powerup.GetTitle() + " to inventory");
            return;
        }
        //Case 2.2: inventory is not full
        PowerUp pu = Instantiate(powerup, pgo.transform);
        itemBag.Add(pu);
        pu.Setup(0);
        if(itemBag.Count == MenuManager.instance.GetPowerupCellsCount())
            PowerupTableManager.instance.SetAvailableItems(itemBag);

        ObjectiveManager.instance.GottenPowerupsObjectiveCheck(itemBag);
    }
    public void AddMythicPowerupToInventory(PowerUp powerup) {
        if(mgo.transform.childCount >= 1) {
            if(mythicBag[0].title == powerup.title)
                mythicBag[0].tryUpgradePowerup();
            return;
        }
        MenuManager.instance.ShowMythicRightTab();
        PowerUp pu = Instantiate(powerup, mgo.transform);
        mythicBag.Add(pu);
        pu.Setup(0);
        PowerupTableManager.instance.RemoveAvailabilityMythic2(powerup.title);
    }
    /*
    public void AddNewTieredPowerupToInventory(PowerUp powerup) { 
        //ATTENZIONE: VA UTILIZZATO SOLO SU POWERUP APPENA AGGIUNTI CON TIER NON ZERO, E CON NON-MYTHIC
        //if(!powerup.isMythic) {
        int times = powerup.tier;
        Debug.Log("times " + times + " pu --> " + powerup.title);
        while(times >= 0) {
//                Debug.Log("times " + times);
            AddPowerupToInventory(powerup);
            times--;
        }
        if(itemBag.Count == MenuManager.instance.GetPowerupCellsCount())
            PowerupTableManager.instance.SetAvailableItems(itemBag);
    }
    */
    public void AddNewTieredPowerupToInventory(PowerUp powerup) { 
        AddPowerupToInventory(powerup);
        if(itemBag.Count == MenuManager.instance.GetPowerupCellsCount())
            PowerupTableManager.instance.SetAvailableItems(itemBag);
    }

    public void AddAbilityToInventory(Ability ability) {
        currAbility = Instantiate(ability, ago.transform);
        if(currAbility.TryGetComponent(out ChargedProjectile cp))
            InstantiateIndicators(cp.GetMaxTickNumber());
    }
    #region Indicators Methods
    private List<GameObject> indicators; private List<SpriteRenderer> indicatorsSpriteRenderer;
    public void InstantiateIndicators(int n) { //Instantiate n indicators
        indicators = new List<GameObject>();
        for(int i=0; i < n; i++) {
            indicators.Add(Instantiate(indicatorPrefab, indicatorParent.transform));
            Vector3 newPosition = indicators[i].transform.position;
            newPosition.x = i * chargeIndicatorsDistanceX;
            indicators[i].transform.position = newPosition;
        }
        indicatorsSpriteRenderer = new List<SpriteRenderer>();
        for(int i=0; i < indicators.Count; i++) 
            indicatorsSpriteRenderer.Add(indicators[i].GetComponent<SpriteRenderer>());
        
        DisableAllIndicators();
    }
    public void SetIndicatorActive(int index, bool active) {
        if(index >= indicators.Count) {
            Debug.LogWarning("Indicator Index out of range [" + index + ">=" + indicators.Count + "]");
            return;
        }
        indicatorsSpriteRenderer[index].enabled = active;
    }
    public void DisableAllIndicators() {
        foreach(SpriteRenderer indicator in indicatorsSpriteRenderer) 
            indicator.enabled = false;
    }

    #endregion
    public PowerUp SearchPowerupByTitle(string title, bool mythic) {
        PowerUp pu;
        if(!mythic)
            pu = SearchNormalPowerupByTitle(title);
        else
            pu = SearchMythicPowerupByTitle(title);
        return pu;
    }
    public PowerUp SearchNormalPowerupByTitle(string title) {
        for(int i=0; i < itemBag.Count; i++) 
            if(itemBag[i].title == title) 
                return itemBag[i];
        Debug.LogError("Powerup not found");
        return null;
    }
    public PowerUp SearchMythicPowerupByTitle(string title) {
        for(int i=0; i < mythicBag.Count; i++) 
            if(mythicBag[i].title == title) 
                return mythicBag[i];
        Debug.LogError("Mythic not found");
        return null;
    }

    public bool IsPowerupInInventory(PowerUp pu) {
        if(!pu.isMythic)
            return IsNormalPowerupInInventory(pu);
        else
            return IsMythicPowerupInInventory(pu);
    }

    private bool IsNormalPowerupInInventory(PowerUp pu) {
        for(int i=0; i < itemBag.Count; i++) {
//            Debug.Log(itemBag[i] + " == " + pu);
            if(itemBag[i].title == pu.title) 
                return true;
        }
//        Debug.Log("false");
        return false;
    }

    private bool IsMythicPowerupInInventory(PowerUp pu) {
        for(int i=0; i < mythicBag.Count; i++) {
//            Debug.Log(itemBag[i] + " == " + pu);
            if(mythicBag[i].title == pu.title) 
                return true;
        }
//        Debug.Log("false");
        return false;
    }


    public void AddWeaponToInventory(WeaponTopHierarchy wpn) {
        weaponBag.Add(Instantiate(wpn, wgo.transform));
        if(weaponBag.Count == 1)
            weaponBag[0].gameObject.SetActive(true);
        else
            weaponBag[^1].gameObject.SetActive(false);
    }

    public void replaceWeapon(WeaponTopHierarchy newWeapon) {
        Debug.Log(weaponBag[currWpnIndex]);
        WeaponTopHierarchy previousWeapon = weaponBag[currWpnIndex];
        weaponBag[currWpnIndex] = newWeapon;

        convertToPickup(previousWeapon, previousWeapon.getPickup());
    }

    public void convertToPickup(WeaponTopHierarchy w, Pickup p) {
        Debug.Log(p);
        Instantiate(p.gameObject);
        Destroy(w.gameObject);
    }
    //simple getter and setters
    public int getWeaponCount() {
        return weaponBag.Count;
    }

    public int getItemCount() {
        return itemBag.Count;
    }
    public int getMythicCount() {
        return mythicBag.Count;
    }

    public int getCurrWeaponIndex() {
        return currWpnIndex;
    }

    public PowerUp[] getItemsBag() {
        return itemBag.ToArray();
    }

    //print
    public void printItemBag() {
        for(int i=0; i < itemBag.Count; i++) 
            Debug.Log(itemBag[i]);
    }

    public void printWeapon() { //UNTESTED
        Debug.Log("----Weapon information----");
        for(int i=0; i < getWeaponCount(); i++) {
            Debug.Log("count: " + getWeaponCount());
            Debug.Log("index: " + i);
            Debug.Log("weaponbag[index] " + weaponBag[i]);
            Debug.Log("weapon core: " + weaponBag[i].getWeaponCore());
        }
    }

    public Ability GetAbility() {
        return currAbility;
    }

    public DashBase GetDash() {
        return dashBaseAbility;
    }

    public Sprite GetAbiltySprite() {  //Ottieni il "core" dell'arma
        if(GetAbility().TryGetComponent(out SpriteRenderer sr))
            return sr.sprite;
        
        Debug.LogError("'GetAbiltySprite()' --> has no sprite");
        return null;
    }

    public Magnet GetMagnet() {
        return magnet;
    }

}
