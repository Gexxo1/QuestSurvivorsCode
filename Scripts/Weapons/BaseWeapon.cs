using UnityEngine;
//Classe di intermezzo, non viene implementata nei gameobject
public abstract class BaseWeapon : Item
{
    [Header("Weapon")]
    [SerializeField] private BaseWeaponStats originalStat;
    [HideInInspector] public BaseWeaponStats stats;
    //Upgrade
    [HideInInspector] public SpriteRenderer spriteRenderer;
    //Possessore dell'arma
    [HideInInspector] public Fighter wielder;
    protected Player player;
    public string animTriggerName = "none";
    protected virtual void Start()
    {
        wielder = GetComponentInParent<Fighter>();
//        Debug.Log("wielder getcomponentinparent: " + wielder);
        player = GameManager.instance.player;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if(originalStat == null) {
            Debug.LogWarning("wielder's [" + wielder + "] weapon [" + title +  "] original Stat is null --> [" + originalStat + "]");
            return;
        }
        if(originalStat != null)
            stats = Instantiate(originalStat);
    }
    protected virtual void Update()
    {

    }
    public void SetBlock(bool b) {
        wielder.setAttackBlocked(b);
    }
}