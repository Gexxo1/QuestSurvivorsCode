using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : Fighter
{
    [Header("Player")]

    [HideInInspector] public WeaponStats globalWpnStats;
    private Class pClass;
    private Vector3 mousePosition;
    [SerializeField] public Inventory inventory;
    [Header("Experience")]
    [SerializeField] private int maxLevel;
    [SerializeField] public BaseWeaponStats mainWeaponUpgr;
    [HideInInspector] public long exp = 0;
    [HideInInspector] public long[] expTable;
    [HideInInspector] public int currLvl;
    private bool blockXpGain = false; //when is max lvl or when cheat activates it
    //private bool blockXpGain = false;
    [SerializeField] private Material redFlash;
    [HideInInspector] public int currArmor = 0;
    [SerializeField] private float armorRegenBaseTick = 10.0f; //how much seconds it takes to regen armor
    [SerializeField] private int baseArmorRegenAmount = 1; //how much armor regens per tick
    protected override void Awake() {
        base.Awake();
        GenerateExpTable();
    }
    public override void Movement(Vector3 direction, float modifier) {
        base.Movement(direction,modifier);

        mousePosition = Input.mousePosition;
        if(mousePosition.x > Screen.width / 2.0f)
            sprite.flipX = false;
        else
            sprite.flipX = true;
        //Debug.Log("Curr mouse Pos: " + mousePosition);
    }
    private const float playerHitRecovery = 0.25f;
    private int damageTaken = 0;
    public override void getHit(Damage dmg, float hitCd) {
        if(IsUnhittable()) return;
        if(CooldownCheck(hitCd + playerHitRecovery)) {
            if(dmg.playSound) AudioManager.instance.PlaySingleSound("Hit",true,0.3f);
            if(damageTaken > 0) damageTaken += dmg.amount;
        }
        base.getHit(dmg,hitCd + playerHitRecovery);
    }
    protected override void OnPostHit() {
        GameManager.instance.OnHitpointChange();
    }
    public int GetDamageTaken() { return damageTaken; }
    private float armorTimer;
    public float GetArmorRegenTick() { 
        return stats.GetArmorRegenReduction(armorRegenBaseTick);
    }
    protected override void Update() {
        armorTimer += Time.deltaTime;
        if(armorTimer >= GetArmorRegenTick()) {
            currArmor = Mathf.Min(currArmor + baseArmorRegenAmount, stats.armor);
            if(currArmor > stats.armor)
                currArmor = stats.armor;
            armorTimer = 0;
            GameManager.instance.OnHitpointChange();
        }
        base.Update();
    }
    protected override void FixedUpdate() {
        if(stopMoving)
            return;
        //qua gestiamo la variabile "moving" dell'animator
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if(animator != null) 
            animator.SetFloat("Moving", Mathf.Abs(x) + Mathf.Abs(y));

        Movement(new Vector3(x,y,0),stats.moveSpeed);
    }
    public void OnLevelUp(long overflow) {
        exp = overflow;
        currLvl++;
        
        MenuManager.instance.UpdateCurrLvlHUD(currLvl);
        GameManager.instance.OnExperienceChange();
        //Debug.Log(currLvl + " " + (currLvl+1) % 10);
        //if((currLvl+1) % 10 != 0)
        inventory.AddStatsToMainWeapon(mainWeaponUpgr);
        MenuManager.instance.ShowLevelUpMenu(false);
        //else MenuManager.instance.ShowLevelUpMenu(true);

        ObjectiveManager.instance.PlayerLevelObjectiveCheck(GetVisualLevel());
    }
    public void GrantXp(int givenXP) {
        if(blockXpGain)
            return;
        int fixedXp = (int)(givenXP * stats.getFixedXpPercent());
        exp += fixedXp;
        while(exp >= expTable[currLvl]) {
            long overflow;
            if(currLvl+1 != expTable.Length) {
                overflow = exp - expTable[currLvl];
            }
            else { //è al livello max
                exp = expTable[currLvl];
                blockXpGain = true;
                GameManager.instance.OnExperienceChange();
                return;
            }
            
            OnLevelUp(overflow);
        }
        GameManager.instance.OnExperienceChange();
    }
    public void SetLevel(int level) {
        for(int i=0; i < level; i++)
            OnLevelUp(0);
        currLvl = level;
    }

    public int GetVisualLevel() { return currLvl+1; }
    public int GetActualLevel() { return currLvl; }
    protected override void Death(HitSource deathSource){
        if(stats.resCount == 0)
            MenuManager.instance.ShowGameoverScreen(true);
        else if(stats.resCount > 0)
            Resurrection();
    }

    private void Resurrection() {
        currHP = stats.healthpoint;
        stats.resCount--;
    }
    
    public IEnumerator invincibilityFrame(float invTime)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invTime);
        isInvulnerable = false;
        yield return null;
    }

    //Class section
    public override void InstantiateStats(Stats stat) {
        base.InstantiateStats(stat);
        currArmor = stats.armor;
        globalWpnStats = ScriptableObject.CreateInstance<WeaponStats>();
        globalWpnStats.name = "Global Weapon Stats";
    }

    public void InstantiateClass(Class c) {
        pClass = c;
        //Debug.Log("instantiate class " + pClass.getStartingPowerUp().tier);
        //weapon
        inventory.AddWeaponToInventory(pClass.GetWeaponTopHierarchy());
        //ability
        if(pClass.GetAbility() != null) inventory.AddAbilityToInventory(pClass.GetAbility());
        //stats
        Stats pStats = Instantiate(pClass.getStats()); //stats from class
        StatsUp upStats = UpgradeShopManager.instance.GetAllStatsSum(); //upgrade shop stats
        Stats sum = pStats.AddToStats(upStats); //base player stats + upgrade shop stats;
        InstantiateStats(sum);
        //powerup
        if(pClass.getStartingPowerUp() != null) inventory.AddNewTieredPowerupToInventory(pClass.getStartingPowerUp());
        //skin
        if(pClass.getSkin() != null) GetComponent<SpriteLibrary>().spriteLibraryAsset = pClass.getSkin();
    }
    //Stats section
    public void AddStats(Stats add) {
        stats.AddToStats(add);
        if(stats.healthpoint <= 0)
            stats.healthpoint = 1;
        if(add.healthpoint > 0)
            currHP += add.healthpoint;
        currArmor += add.armor;
        currMp += add.manapoint;
        AdjustAnimatorMovSpd();
        GameManager.instance.OnHitpointChange(false);
        GameManager.instance.OnManapointChange(false);
        //MenuManager.instance.UpdateCharInfoUI();
        ObjectiveManager.instance.StatsObjectiveCheck(stats);
    }
    public void RemoveStats(Stats sub) {
        stats.SubToStats(sub);
        if(stats.healthpoint <= 0)
            stats.healthpoint = 1;
        currMp += sub.manapoint;

        GameManager.instance.OnHitpointChange(false);
        GameManager.instance.OnManapointChange(false);
    }
    public void AddToGlobalWpnStats(WeaponStats add) {
        globalWpnStats.AddToStats(add);
        inventory.AddStatsToMainWeapon(add);
        inventory.AddStatsToPowerupWeapons(add);
    }
    private void GenerateExpTable() {
        expTable = new long[maxLevel];
        int start = 100; float modifer = 1.2f;
        expTable[0] = start;
        for(int i=1; i < maxLevel; i++) {
            expTable[i] = (int)(expTable[i-1] * modifer);
        }
    }

    public override IEnumerator FlashEffect() {
        ChangeToRedFlash();
        yield return new WaitForSeconds(flashDuration);
        RevertSpriteToNormal();
        yield return null;
    }

    private void ChangeToRedFlash() { sprite.material = redFlash; }

    public override void Heal(int amount) {
        base.Heal(amount);
        GameManager.instance.OnHitpointChange();
        GameManager.instance.ShowText("+" + amount + " hp", 18, Color.green, transform.position, Vector3.up * 30, 1.0f);
    }
    public override void RestoreMana(int amount) {
        base.RestoreMana(amount);
        GameManager.instance.OnManapointChange();
        GameManager.instance.ShowText("+" + amount + " mp", 18, Color.blue, transform.position, Vector3.up * 30, 1.0f);
    }

    public void ActivateGodMode(bool active) {
        isInvulnerable = active;
        inventory.GetAbility().SetFreeCast(active);
        GiveTestStats(active);
    }
    private void GiveTestStats(bool give) {
        if(give)
            stats = TestManager.instance.GetTestClass().getStats();
        else
            stats = originalStat;
    }
    public void BlockXpGain(bool active) {
        blockXpGain = active;
    }

    protected override int ArmorCalculation(int damageReceived) {
        if (currArmor > 0) {
            int damageToArmor = Mathf.Min(damageReceived, currArmor); // Danno assorbito dall'armatura
            currArmor -= damageToArmor; // Riduci l'armatura
            damageReceived -= damageToArmor; // Riduci il danno ricevuto
        }
        return damageReceived;
    }
}   
