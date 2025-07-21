using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Riceve il riferimento della classe da MainMenuManager e lo manda al GameManager
public class CharacterClassManager : MonoBehaviour
{
    [SerializeField] public Class[] classes;
    //[HideInInspector] public bool[] isClassOwned;
    //Upgrade
    [SerializeField] public int upgradeCost; //il costo è uguale per tutte le classi
    public int[] classesTier; //only display | -1 -> not unlocked 0 -> null 1 -> tier 0 -> 2 -> tier1 etc.
    [SerializeField] public int maxTierLevel = 3;
    //other
    public static CharacterClassManager instance;
    private Class choosenClass;
    private int currentTier;
    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Debug.LogError("Found another class manager in scene");
        
        //isClassOwned = new bool[classes.Length];
        //IsClassOwnedInit();
        classesTier = new int[classes.Length];
    }
    /*
    private void IsClassOwnedInit() {
        isClassOwned = new bool[classes.Length];
        for(int i=0; i < classes.Length)

    }*/

    public void SetPlayerClass(int index) {
        if(index == -69) {
            Debug.Log("Test class selected");
            choosenClass = TestManager.instance.GetTestClass();
            currentTier = 0;
            return;
        }
        if(classes[index] == null)
            Debug.LogError("Choosen class is null");
        choosenClass = classes[index];
        currentTier = classesTier[index]-1;
//        Debug.Log("current tier set: " + currentTier);
    }
    
    public void SetClassesTier(List<int> list) {
        //dimensione lista
        if(list.Count == 0)
            list = GetClassesTierToList();
        for(int i=0; i < list.Count; i++) {
            classesTier[i] = list[i];
            if(classesTier[i] == 0 && classes[i].GetCost() == 0) //sblocca automaticamente la classe se non ha costo
                classesTier[i] = 1;
        }
    }
    public List<int> GetClassesTierToList() {
        List<int> list = new();
        for(int i=0; i < classesTier.Length; i++)
            list.Add(classesTier[i]);

        return list;
    }
    public void SetClassesTier(SerializableDictionary<string, int> table) {
        for(int i=0; i < classes.Length; i++) {
            string key = classes[i].getId();
            if(table.ContainsKey(key))
                classesTier[i] = table[key];
            else
                classesTier[i] = 0;
            if(classesTier[i] == 0 && classes[i].GetCost() == 0) //sblocca automaticamente la classe se non ha costo
                classesTier[i] = 1;
        }
    }
    public SerializableDictionary<string,int> GetClassesTierToDictionary() {
        SerializableDictionary<string,int> table = new();
        for(int i=0; i < classes.Length; i++)
            table.Add(classes[i].getId(),classesTier[i]);
        return table;
    }

    public void SetPlayerClass(Class c) {
        choosenClass = c;
        currentTier = 0;
    }

    //Il parametro viene passato dal gamemanger (passa il prefab di "blankplayer")
    public void UpdatePlayerClass(Player player) {
        //player.inventory.addItemToInventory()
//        Debug.Log("Set Class Tier" + currentTier);
        //Class newClass = choosenClass;
        //newClass.SetAllItemsTier(currentTier);
        player.InstantiateClass(choosenClass);
        MenuManager.instance.UpdateWeaponHUD();
        MenuManager.instance.UpdateAbilityHUD();
        MenuManager.instance.UpdateItemHUD();
        MenuManager.instance.UpdateItemTierHUD();
    }

    public void UnlockClass(int index) {
        //Debug.Log(classesTier[index]);
        classesTier[index] = 1;//isClassOwned[index] = true;
    }
    public void UnlockAll() { //for testing purposes only
        for(int i=0; i < classes.Length; i++) 
            UnlockClass(i);
    }
    public void UnlockClassById(string id) {
        UnlockClass(GetClassIndexById(id));
    }
    public int GetClassIndexById(string id) {
        for(int i=0; i < classes.Length; i++) {
            if(classes[i].getId() == id) {
                return i;
            }
        }
        Debug.Log("Warning! Following class id put in parameter not found: " + id);
        return -1;
    }

    public void UpgradeClass(int index) {
        if(!IsClassMaxLevel(index)) {
            classesTier[index]++;
            SetPlayerClass(index);
        }
    }

    public bool IsClassMaxLevel(int index) {
        return classesTier[index]-1 >= maxTierLevel - 1;
    }

    public bool IsClassOwned(int index) {
        return classesTier[index]-1 >= 0;
    }

    public CharClassProperty GetClassProperty(int index) {
        if(IsClassOwned(index)) //if(isClassOwned[index]) 
            return CharClassProperty.unlocked;
        else if (classes[index].GetCost() != -1) 
            return CharClassProperty.buyable;
        else
            return CharClassProperty.objLocked;
        
    }
}
