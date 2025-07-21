using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(fileName = "Class", menuName = "ScriptableObjects/Class")]
public class Class : ScriptableObject
{
    [Header("Class")]
    [SerializeField] private string id;
    //[SerializeField] private Sprite skin;
    [SerializeField] private SpriteLibraryAsset skin;
    [SerializeField] private Stats stats;
    [SerializeField] private PowerUp startingPowerup;
    [SerializeField] private Ability ability;
    [Header("Class Weapon")]
    [SerializeField] private WeaponTopHierarchy weapon;
    [Tooltip("Keep unchanged to get weapon default skin")]
    [SerializeField] private Sprite weaponSkin; 
    [Header("Off-game Parameters")]
    [SerializeField] private int cost;

    public string getId() {
        return id;
    }
    public SpriteLibraryAsset getSkin() {
        return skin;
    }
    public Sprite GetIdleSprite() {
        return skin.GetSprite("Idle","idle_0");
    }
    public Stats getStats() {
        return stats;
    }
    public PowerUp getStartingPowerUp() {
        return startingPowerup;
    }
    public Ability GetAbility() {
        return ability;
    }
    public string GetName() {
        return id;
    }
    public Sprite getWeaponSkin() {
        if(weaponSkin != null)
            return weaponSkin;
        else
            return GetWeaponCore().GetComponent<SpriteRenderer>().sprite;
    }
    public WeaponTopHierarchy GetWeaponTopHierarchy() {
        return weapon;
    }
    public Weapon GetWeaponCore() {
        return weapon.GetComponent<WeaponTopHierarchy>().getWeaponCore();
    }
    public void SetAllItemsTier(int value) {
        //if(GetAbility() != null) GetAbility().SetTier(value);
        //if(GetWeaponCore() != null)
            //GetWeaponCore().SetTier(value);
        if(getStartingPowerUp() != null)
            getStartingPowerUp().SetTier(value);
    }

    public int GetCost() {
        return cost;
    }
}