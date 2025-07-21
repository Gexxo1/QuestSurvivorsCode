using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PowerupTableManager : MonoBehaviour
{
    public static PowerupTableManager instance;
    [Header("Regular Powerups")]
    [SerializeField] private List<PowerUp> puTable; //non deve essere modificata
    [SerializeField] private List<PowerUp> availablePuTable; //runtime init
    [SerializeField] private List<PowerUp> lockedPu; //deve essere inizializzata con il data
    [SerializeField] private Item[] fillerChoice;
    [Header("Mythic Powerups")]
    [SerializeField] private List<PowerUp> mythicsTable; //non deve essere modificata
    [SerializeField] private List<PowerUp> availableMythicsTable;
    [SerializeField] private Item[] fillerMythicChoice;
    [Header("Other")]
    [SerializeField] private PowerUp blankPowerup;
    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("Duplicate PowerupTableManager Instance: " + instance + " Destroying gameobject");
            Destroy(gameObject);
        }
        instance = this;
    }
    public void SetupAllPowerupsTier() {
        for(int i=0; i < puTable.Count; i++) 
            puTable[i].Setup(0);
        for(int i=0; i < mythicsTable.Count; i++) 
            mythicsTable[i].Setup(0);
    }
    public void setupAvailablePowerups() {
        if (SceneManager.GetActiveScene().name != "MainMenu")
            Debug.LogWarning("Setting up available powerups table --> This isn't supposed to be called during the game");
        
        
        //regular powerups
        availablePuTable.Clear();
        for(int i=0; i < puTable.Count; i++) {
            //puTable[i].Setup(0);
            if(!IsPowerupLocked(i))
                availablePuTable.Add(puTable[i]);
        }
        //mythic powerups
        availableMythicsTable.Clear();
        for(int i=0; i < mythicsTable.Count; i++) {
            availableMythicsTable.Add(mythicsTable[i]);
            if(!mythicsTable[i].isMythic)
                Debug.LogWarning("Mythic powerup " + mythicsTable[i].title + " is not correctly marked as mythic");
        }
    }
    public void AddExclusiveToAvailablePowerups(Inventory inv) {
        for(int i=0; i < inv.itemBag.Count; i++) 
            if(IsPowerupLocked(inv.itemBag[i]))
                availablePuTable.Add(inv.itemBag[i]);
    }
    public void RemoveAvailability(string title) {
        int powId = FindAvailablePowerupId(title);
        if(powId >= 0 && powId < availablePuTable.Count)  {
//            Debug.Log("removing pow id from pu table: " + powId);
            availablePuTable.RemoveAt(powId);
        }
        else
            Debug.LogWarning("Trying to remove powerup out of index");
    }
    public void RemoveAvailabilityMythic(string title) {
        int powId = FindAvailableMythicId(title);
        //Debug.Log("powerup id: " + powId);
        if(powId >= 0 && powId < availableMythicsTable.Count) {
            Debug.Log("removing pow id from pu table: " + availableMythicsTable[powId].title);
            availableMythicsTable.RemoveAt(powId);
        } 
        else
            Debug.LogWarning("Trying to remove mythic out of index");
    }
    public void RemoveAvailabilityMythic2(string title) {
        //int powId = findMythicId(title);
        availableMythicsTable.RemoveAll(obj => obj.title != title);
    }
    public PowerUp GetAvailableRandomPowerup(bool isMythicChoice) {
        int max;
        if(!isMythicChoice)
            max = GetAvailablePowerupTableLength();
        else
            max = getMythicsTableLength();
        return GetPowerupById(Random.Range(0,max),isMythicChoice);
    }
    public List<PowerUp> GetAvailableRandomPowerup(bool isMythicChoice, int number) {
        List<PowerUp> puList; 
        if(!isMythicChoice) 
            puList = new List<PowerUp>(GetAvailablePowerupTable());
        else 
            puList = new List<PowerUp>(GetMythicsTable());
        
        List<PowerUp> ret = new();
        for (int i = 0; i < number; i++) {
            if (puList.Count == 0) // Se la lista temporanea è vuota, interrompi il loop
                break;
            int r = Random.Range(0, puList.Count);
            ret.Add(puList[r]); // Aggiungi il powerup estratto alla lista di output
            puList.RemoveAt(r); // Rimuovi il powerup dalla lista temporanea
        }
        return ret;
    }
    public List<Item> GetLvlupmenuItems(bool isMythicChoice, int number) {
        List<Item> items = GetAvailableRandomPowerup(isMythicChoice,number)
            .Cast<Item>()  // Conversione esplicita
            .ToList();
        int fillerNumber = number - items.Count;
        for(int i=0; i < fillerNumber; i++)
            if(GetFillerChoice(i,isMythicChoice) != null)
                items.Add(GetFillerChoice(i,isMythicChoice));
        return items;
    }

    public PowerUp GetPowerupById(int id, bool mythic) {
        if(!mythic)
            return getPowerupById(id);
        else
            return getMythicsById(id);
    }
    public PowerUp getPowerupById(int id) {
        return availablePuTable[id];
    }
    public PowerUp getMythicsById(int id) {
        return availableMythicsTable[id];
    }
    public Item GetFillerChoice(int index, bool mythic) {
        if(!mythic && index < fillerChoice.Length)
            return fillerChoice[index];
        else if(index < fillerMythicChoice.Length)
            return fillerMythicChoice[index];
        return null;
    }
    private int FindAvailablePowerupId(string title) {
        for(int i=0; i < availablePuTable.Count; i++) {
            if(availablePuTable[i].title == title)
                return i;
        }
        //check in caso di errore, il codice se funziona non dovrebbe andare oltre
        if(FindPowerupId(title) > 0) {
            Debug.LogWarning("'findPowerupId' --> Powerup " + title + " is not available, but exists in puTable");
            return -1;
        }

        Debug.LogWarning("'FindPowerupId' --> Powerup Not Found");
        return -2;
    }
    private int FindPowerupId(string title) { //attenzione: capire dove usarla poichè non prende dalla tabella degli available
        //check in caso di errore, il codice se funziona non dovrebbe andare oltre
        for(int i=0; i < puTable.Count; i++) {
            if(puTable[i].title == title) 
                return i;
        }
        Debug.LogWarning("'FindPowerupId' --> Powerup Not Found");
        return -2;
    }
    private int FindAvailableMythicId(string title) {
        for(int i=0; i < availableMythicsTable.Count; i++) {
            if(availableMythicsTable[i].title == title)
                return i;
        }
        //check in caso di errore, il codice se funziona non dovrebbe andare oltre
        for(int i=0; i < mythicsTable.Count; i++) {
            if(mythicsTable[i].title == title) {
                Debug.LogWarning("'findPowerupId' --> Powerup " + title + " is not available, but exists in 'puTable'");
                return -1;
            }
        }
        Debug.LogWarning("'findPowerupId' --> Powerup Not Found in 'total table' and in 'available table'");
        return -2;
    }
    /*
    private bool IsPowerupAvaible(int index) {
        int res = findPowerup(puToFind);
        if(index >= 0)
            return true;
        return false;
    }
    */
    public bool IsPowerupLocked(int j) {
        for(int i=0; i < lockedPu.Count; i++) {
            bool isUnlocked = ObjectiveManager.instance.GetConditionsMetForPowerups(i); 
            if(puTable[j].title == lockedPu[i].title && !isUnlocked)
                return true;
        }
        return false;
    }
    public bool IsPowerupLocked(PowerUp pu) {
        for(int i=0; i < lockedPu.Count; i++) {
            bool isUnlocked = ObjectiveManager.instance.GetConditionsMetForPowerups(i);
            if(pu.title == lockedPu[i].title && !isUnlocked) {
//                Debug.Log(pu.title + " == " + lockedPu[i].title + " && " + !isUnlocked);
                return true;
            }
        }
        return false;
    }
    public List<PowerUp> GetAvailablePowerupTable() { return availablePuTable; }
    public List<PowerUp> GetPowerupTable() { return puTable; }
    public PowerUp GetPowerupTableElement(int index) {
        return puTable[index]; 
    }
    public List<PowerUp> GetMythicsTable() { return availableMythicsTable; }
    public void debugPowerupTable() {
        Debug.Log("Begin----------");
        foreach(PowerUp pu in availablePuTable) 
            Debug.Log(pu);
        Debug.Log("End----------");
    }


    public int GetPowerupTableLength() {
        return puTable.Count;
    }
    public int GetAvailablePowerupTableLength() {
        return availablePuTable.Count;
    }

    public int getMythicsTableLength() {
        return availableMythicsTable.Count;
    }

    public int GetFillerLength() {
        return fillerChoice.Length;
    }
    /*
    public void SetLockedPTable(List<PowerUp> l) {
        lockedPu = l;
    }
    */
    public void SetLockedPTable(List<PowerUp> list) {
        lockedPu.Clear();
        for(int i=0; i < list.Count; i++)
            lockedPu.Add(list[i]);
    }
    public void SetAvailableItems(List<PowerUp> list) {
        availablePuTable.Clear();
        for(int i=0; i < list.Count; i++)
            if(!list[i].isMaxedOut)
                availablePuTable.Add(list[i]);
    }

}
