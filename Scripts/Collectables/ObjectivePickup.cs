using UnityEngine;

public class ObjectivePickup : Pickup {
    //[SerializeField] private string objectiveToUnlockID;
    //protected new PowerUp itemPrefab => (PowerUp)base.itemPrefab;
    public override void OnPickup() {   
        if(collected)
            return;
        ObjectiveManager.instance.PickupObjectiveCheck(itemPrefab);
        base.OnPickup();
    }

}
