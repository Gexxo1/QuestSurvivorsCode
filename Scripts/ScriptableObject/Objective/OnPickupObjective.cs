using UnityEngine;

[CreateAssetMenu(fileName = "OnPickupObjective", menuName = "ScriptableObjects/Objectives/PickupObjective")]
public class OnPickupObjective : Objective {
    //[SerializeField] private Item itemToPick;
    //initializing item is not needed because it directly uses the unlocked powerup from inherited class
    [SerializeField] private MapData foundInMap;
    public override bool IsConditionTrue(int value) {
        Debug.LogWarning("Not needed to be called");
        return false;
    }
    public override string GetReqDesc() {
        return "Collect [" + unlockedPowerup.GetTitle() + "] in map: " + foundInMap.GetTitle();
    }
    public override string GetTitle() {
        return "Item Collection " + base.GetTitle();
    }
    public override string GetId() { return base.GetId() + "itemcollect_" + id; }
    //public override string GetId() { return base.GetId() + "goldcollect_" + goldTreshold; }
    public Item GetItem() { return unlockedPowerup; }
    public MapData GetMapData() { return foundInMap; }
}
