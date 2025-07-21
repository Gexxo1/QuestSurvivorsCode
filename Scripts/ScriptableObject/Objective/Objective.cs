using UnityEngine;

//[CreateAssetMenu(fileName = "Objective", menuName = "ScriptableObjects/Objectives/Objective")]
public abstract class Objective : ScriptableObject
{
    //WARNING: 1.ID must be unique 2. Do not change ID
    //if number 2 happens --> the progress of the objective will be reset
    //string objId = "not set";
    [Header("DO NOT CHANGE THIS ID")]
    [SerializeField] protected string id = "not set"; //should put a number there
    private string requirementsDesc = "not set"; //this is shown in main menu, it's automatically initialized in code (not in inspector)
    private Sprite image;
    public PowerUp unlockedPowerup;
    public Class unlockedClass;
    public string GetUnlockDesc() {
        string r;
        string u;

        if(GetReqDesc() != "not set")
            r = 
            //"Requirements:\n" +
            GetReqDesc() + "\n";
        else
            r = "";
        
        if(unlockedPowerup != null && unlockedClass != null)
            u = "Unlocked:\n";
        else
            u = "";

        u += GetWhatUnlocks();
        
        return r + u;
        
        //return r;
    }
    private string GetUnlockType(bool prefPu = true) {
        if(unlockedPowerup && unlockedClass) {
            if(prefPu)
                return "Powerup";
            else
                return "Class";
        }
        if(unlockedPowerup != null)
            return "Powerup";
        if(unlockedClass != null)
            return "Class";
        return "";
    }
    public string GetWhatUnlocks() {
        string s = "Unlocks:\n ";
        if(unlockedClass != null)
            s += "Class '" + unlockedClass.getId() + "'\n";
        if(unlockedPowerup != null)
            s += "Powerup '" + unlockedPowerup.title + "'\n";
        return s;
    }
    public Sprite GetSprite() {
        Sprite sprite = GetUnlockedClassSprite();
        if(sprite != null)
            return sprite;
        sprite = GetUnlockedPowerupSprite();
        if(sprite != null)
            return sprite;
        return image;
    } 
    public Sprite GetUnlockedPowerupSprite() {
        if(unlockedPowerup != null)
            if(unlockedPowerup.gameObject.TryGetComponent(out SpriteRenderer sr))
                return sr.sprite;
        return null;
    }
    public Sprite GetUnlockedClassSprite() {
        if(unlockedClass != null)
            if(unlockedClass.GetIdleSprite() != null)
                return unlockedClass.GetIdleSprite();
        return null;
    }
    public virtual string GetReqDesc() { 
        return requirementsDesc; 
    }
    public virtual string GetTitle() {
        if (int.TryParse(id, out int idNumber)) 
            return Utility.ToRoman(idNumber + 1);
        
        return id;
    }
    //alternativa se non basta un parametro, passare una struct
    //WARNING: Used only for condition checking, not for getting the value, use objectivemanger for that instead
    public abstract bool IsConditionTrue(int value);

    public virtual string GetId() {
        return "obj_";
    }
}
